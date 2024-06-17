using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace YasiroRegrave.Pages
{
    public class ReserveConfirmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReserveConfirmModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string SectionNumber { get; set; } = "";
        [BindProperty]
        public int CemeteryIndex { get; private set; } = 0;
        [BindProperty]
        public int ReserveMode { get; set; } = (int)Config.ReserveType.見学予約;
        [BindProperty]
        public string ReserveName { get; set; } = "";

        [BindProperty]
        public string LastName { get; set; } = "";
        [BindProperty]
        public string FirstName { get; set; } = "";
        [BindProperty]
        public string LastNameKana { get; set; } = "";
        [BindProperty]
        public string FirstNameKana { get; set; } = "";
        [BindProperty]
        public string PostalCode { get; set; } = "";
        [BindProperty]
        public string Prefecture { get; set; } = "";
        [BindProperty]
        public string City { get; set; } = "";
        [BindProperty]
        public string Address { get; set; } = "";
        [BindProperty]
        public string Building { get; set; } = "";
        [BindProperty]
        public string Phone { get; set; } = "";
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string Inquiry { get; set; } = "";
        [BindProperty]
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
        public string IsContactByPhone { get; set; } = "";
        [BindProperty]
        public string IsContactByEmail { get; set; } = "";
        [BindProperty]
        public string Subscription { get; set; } = "";


        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet()
        {
            if (!int.TryParse(TempData["CemeteryIndex"]?.ToString(), out int index)) { index = -1; }
            if (!int.TryParse(TempData["ReserveMode"]?.ToString(), out int mode)) { mode = -1; }
            CemeteryIndex = index;
            ReserveMode = mode;
            ReserveName = mode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();

            GetPage();
            return;
        }

        /// <summary>
        /// OnPost処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost()
        {
            if (!int.TryParse(TempData["CemeteryIndex"]?.ToString(), out int index)) { index = -1; }
            if (!int.TryParse(TempData["ReserveMode"]?.ToString(), out int mode)) { mode = -1; }
            CemeteryIndex = index;
            ReserveMode = mode;
            ReserveName = mode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();

            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage();
        }

        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void GetPage()
        {
            // 区画番号
            var cemeteryData = _context.Cemeteries
                .Where(ci => ci.CemeteryIndex == CemeteryIndex && ci.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.Reien.DeleteFlag == (int)Config.DeleteType.未削除)
                .Select(ci => new
                {
                    SectionNumber = Utils.SectionCode2Name(ci.Section.SectionCode) + " " + Utils.CemeteryCode2Name(ci.CemeteryCode),
                })
                .FirstOrDefault();

            if (cemeteryData != null)
            {
                SectionNumber = cemeteryData.SectionNumber ?? "";
            }

            // 入力項目
            LastName = TempData["LastName"]?.ToString() ?? "";
            FirstName = TempData["FirstName"]?.ToString() ?? "";
            LastNameKana = TempData["LastNameKana"]?.ToString() ?? "";
            FirstNameKana = TempData["FirstNameKana"]?.ToString() ?? "";
            PostalCode = TempData["PostalCode"]?.ToString() ?? "";
            Prefecture = TempData["Prefecture"]?.ToString() ?? "";
            City = TempData["City"]?.ToString() ?? "";
            Address = TempData["Address"]?.ToString() ?? "";
            Building = TempData["Building"]?.ToString() ?? "";
            Phone = TempData["Phone"]?.ToString() ?? "";
            Email = TempData["Email"]?.ToString() ?? "";
            Date1 = (TempData["Date1"]?.ToString() ?? "").Replace(" 0:00:00", "");
            Time1 = TempData["Time1"]?.ToString() ?? "";
            Date2 = (TempData["Date2"]?.ToString() ?? "").Replace(" 0:00:00", "");
            Time2 = TempData["Time2"]?.ToString() ?? "";
            Date3 = (TempData["Date3"]?.ToString() ?? "").Replace(" 0:00:00", "");
            Time3 = TempData["Time3"]?.ToString() ?? "";
            Inquiry = TempData["Inquiry"]?.ToString() ?? "";
            IsContactByPhone = TempData["IsContactByPhone"]?.ToString() ?? "";
            IsContactByEmail = TempData["IsContactByEmail"]?.ToString() ?? "";
            Subscription = TempData["Subscription"]?.ToString() ?? "";


            //// テストータ
            SectionNumber = "親鸞E区 07列-03";
            LastName = "生駒";
            FirstName = "太郎";
            LastNameKana = "いこま";
            FirstNameKana = "たろう";
            PostalCode = "5750014";
            Prefecture = "大阪府";
            City = "四條畷市";
            Address = "上田原";
            Building = "1366番地";
            Phone = "0120753948";
            Email = "yoyaku@yashiro.co.jp";
            Date1 = "2024/07/01";
            Time1 = "11:00";
            Date2 = "2024/07/02";
            Time2 = "12:00";
            Date3 = "2024/07/03";
            Time3 = "13:00";
            //Inquiry = "ああああああああああ";
            IsContactByPhone = "1";
            IsContactByEmail = "1";
            Subscription = "1";

            return;
        }
    }
}
