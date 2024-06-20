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
        public string LastNameKana { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0019)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string FirstNameKana { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0020)]
        public string PostalCode { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0021)]
        public string Prefecture { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0022)]
        public string City { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0023)]
        public string Address { get; set; } = "";

        [BindProperty]
        public string Building { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0024)]
        public string Phone { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0025)]
        public string Email { get; set; } = "";

        [BindProperty]
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
        

        public string CemeteryName { get; set; } = "";
        public List<DateOnly>? RegularHolidays { get; set; } = new List<DateOnly>();


        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet(int? index, int mode)
        {
            if (index.HasValue)
            {
                CemeteryIndex = index ?? 0;
                ReserveMode = mode;
                ReserveName = mode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();
                GetPage();
            }
            return;
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
                return Page();
            }

            TempData["CemeteryIndex"] = CemeteryIndex;
            TempData["ReserveMode"] = ReserveMode;
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
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void GetPage()
        {
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
