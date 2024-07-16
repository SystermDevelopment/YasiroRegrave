using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace YasiroRegrave.Pages
{
    public class ReserveModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReserveModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int CemeteryIndex { get; private set; } = 0;
        [BindProperty]
        public int ReserveMode { get; set; } = (int)Config.ReserveType.見学予約;
        public string ReserveName { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0016)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string LastName { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0017)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string FirstName { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0018)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        [RegularExpression(@"^[\u3040-\u309Fー]+$", ErrorMessage = Message.M_E0035)]
        public string LastNameKana { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0019)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        [RegularExpression(@"^[\u3040-\u309Fー]+$", ErrorMessage = Message.M_E0035)]
        public string FirstNameKana { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0020)]
        [StringLength(7, ErrorMessage = Message.M_E0011)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = Message.M_E0034)]
        public string PostalCode { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0021)]
        [StringLength(20, ErrorMessage = Message.M_E0011)]
        public string Prefecture { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0022)]
        [StringLength(30, ErrorMessage = Message.M_E0011)]
        public string City { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0023)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string Address { get; set; } = "";

        [BindProperty]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string Building { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0024)]
        [StringLength(11, ErrorMessage = Message.M_E0011)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = Message.M_E0034)]
        public string Phone { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0025)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string Email { get; set; } = "";

        [BindProperty]
        [StringLength(500, ErrorMessage = Message.M_E0011)]
        public string Inquiry { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0027)]
        public string Date1 { get; set; } = "";
        [BindProperty]
        public string Time1 { get; set; } = "";
        [BindProperty]
        public string Date2 { get; set; } = "";
        [BindProperty]
        public string Time2 { get; set; } = "";
        [BindProperty]
        public string Date3 { get; set; } = "";
        [BindProperty]
        public string Time3 { get; set; } = "";

        [BindProperty]
        public bool IsContactByPhone { get; set; } = false;
        [BindProperty]
        public bool IsContactByEmail { get; set; } = false;
        [BindProperty]
        public List<string> SelectCheckBox { get; set; } = new List<string>();

        [BindProperty]
        public string Subscription { get; set; } = "";

        [BindProperty]
        public string CemeteryName { get; set; } = "";
        public List<DateOnly>? RegularHolidays { get; set; } = new List<DateOnly>();

        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }

        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public IActionResult OnGet(int? index, int mode)
        {
            if (index.HasValue)
            {
                CemeteryIndex = index ?? 0;
                ReserveMode = mode;
                ReserveName = mode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();
                GetPage();
            }
            else if ((TempData["CemeteryIndex"] is int tempCemeteryIndex) && (TempData["ReserveMode"] is int tempReserveMode))
            {
                CemeteryIndex = tempCemeteryIndex;
                CemeteryName = TempData["CemeteryName"] as string ?? "";
                ReserveMode = tempReserveMode;
                ReserveName = TempData["ReserveName"] as string ?? "";
                LastName = TempData["LastName"] as string　?? "";
                FirstName = TempData["FirstName"] as string ?? "";
                LastNameKana = TempData["LastNameKana"] as string ?? "";
                FirstNameKana = TempData["FirstNameKana"] as string ?? "";
                PostalCode = TempData["PostalCode"] as string ?? "";
                Prefecture = TempData["Prefecture"] as string ?? "";
                City = TempData["City"] as string ?? "";
                Address = TempData["Address"] as string ?? "";
                Building = TempData["Building"] as string ?? "";
                Phone = TempData["Phone"] as string ?? "";
                Email = TempData["Email"] as string ?? "";
                Date1 = TempData["Date1"] as string ?? "";
                Time1 = TempData["Time1"] as string ?? "";
                Date2 = TempData["Date2"] as string ?? "";
                Time2 = TempData["Time2"] as string ?? "";
                Date3 = TempData["Date3"] as string ?? "";
                Time3 = TempData["Time3"] as string ?? "";
                Inquiry = TempData["Inquiry"] as string ?? "";
                IsContactByPhone = (TempData["IsContactByPhone"] as string) == "1";
                IsContactByEmail = (TempData["IsContactByEmail"] as string) == "1";
                Subscription = TempData["Subscription"] as string ?? "";
                GetPage();
            }

            if (CemeteryName.IsNullOrEmpty())
            {
                return RedirectToPage("/PlotDetails");
            }
            return Page();
        }

        /// <summary>
        /// OnPost処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost(int? index, int mode)
        {
            if (index.HasValue)
            {
                CemeteryIndex = index ?? 0;
                ReserveMode = mode;
                ReserveName = mode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();
            }

            if (!IsContactByPhone && !IsContactByEmail)
            {
                ModelState.AddModelError("SelectCheckBox", Message.M_E0026);
                GetPage();
                return Page();
            }

            // バリデーションエラーを除外
            ModelState.Remove("Building");
            ModelState.Remove("Date2");
            ModelState.Remove("Time2");
            ModelState.Remove("Date3");
            ModelState.Remove("Time3");
            ModelState.Remove("Inquiry");
            if (ReserveMode != (int)Config.ReserveType.見学予約)
            {
                ModelState.Remove("Date1");
                ModelState.Remove("Time1");
            }

            // バリデーションエラーチェック
            if (!ModelState.IsValid)
            {
                GetPage();
                return Page();
            }
            TempData["CemeteryIndex"] = CemeteryIndex;
            TempData["CemeteryName"] = CemeteryName;
            TempData["ReserveMode"] = ReserveMode;
            TempData["ReserveName"] = ReserveName;
            TempData["LastName"] = LastName;
            TempData["FirstName"] = FirstName;
            TempData["LastNameKana"] = LastNameKana;
            TempData["FirstNameKana"] = FirstNameKana;
            TempData["PostalCode"] = PostalCode;
            TempData["Prefecture"] = Prefecture;
            TempData["City"] = City;
            TempData["Address"] = Address;
            TempData["Building"] = Building;
            TempData["Phone"] = Phone;
            TempData["Email"] = Email;
            TempData["Date1"] = Date1;
            TempData["Time1"] = Time1;
            TempData["Date2"] = Date2;
            TempData["Time2"] = Time2;
            TempData["Date3"] = Date3;
            TempData["Time3"] = Time3;
            TempData["Inquiry"] = Inquiry;
            TempData["IsContactByPhone"] = IsContactByPhone ? "1" : "0";
            TempData["IsContactByEmail"] = IsContactByEmail ? "1" : "0";
            TempData["Subscription"] = Subscription;

            return RedirectToPage("ReserveConfirm");
        }
        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void GetPage()
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId != null)
            {
                LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
                ViewData["LoggedInUser"] = LoggedInUser;
            }

            var reienData = _context.Cemeteries
                .Include(s => s.Section)
                    .ThenInclude(s => s.Area)
                    .ThenInclude(a => a.Reien)
                .Where(ci => ci.CemeteryIndex == CemeteryIndex && ci.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.Reien.DeleteFlag == (int)Config.DeleteType.未削除)
                .Select(ci => new
                {
                    ReienCode = ci.Section.Area.Reien.ReienCode,
                    ReienIndex = ci.Section.Area.Reien.ReienIndex,
                    CemeteryName = Utils.SectionCode2Name(ci.Section.SectionCode) + " " + Utils.CemeteryCode2Name(ci.CemeteryCode),
                })
                .FirstOrDefault();

            if (reienData != null)
            {
                CemeteryName = reienData.CemeteryName;
                RegularHolidays = _context.Calenders.Where(c => c.ReienIndex == reienData.ReienIndex).Select(c=>c.RegularHoliday).ToList();
            }
        }
    }
}
