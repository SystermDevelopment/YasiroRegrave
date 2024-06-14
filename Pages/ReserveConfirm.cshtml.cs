using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;


namespace YasiroRegrave.Pages
{
    public class ReserveConfirmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReserveConfirmModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ReserveData ReserveInfo { get; private set; } = new ReserveData();
        public int CemeteryIndex { get; private set; }
        public int ReserveMode { get; set; }

        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet(int? index, int mode)
        {
            ReserveMode = (int)Config.ReserveType.見学予約;
            if (index.HasValue)
            {
                CemeteryIndex = index ?? 0;
                ReserveMode = mode;
                GetPage();
            }
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
            ReserveInfo.SectionNumber = "親鸞E区 07列-03";
            ReserveInfo.LastName = "生駒";
            ReserveInfo.FirstName = "太郎";
            ReserveInfo.LastNameKana = "いこま";
            ReserveInfo.FirstNameKana = "たろう";
            ReserveInfo.PostalCode = "5750014";
            ReserveInfo.Prefecture = "大阪府";
            ReserveInfo.City = "四條畷市";
            ReserveInfo.Address = "上田原";
            ReserveInfo.Building = "1366番地";
            ReserveInfo.Phone = "0120753948";
            ReserveInfo.Email = "yoyaku@yashiro.co.jp";
            ReserveInfo.Date1 = "2024/07/01";
            ReserveInfo.Time1 = "11:00";
            ReserveInfo.Date2 = "2024/07/02";
            ReserveInfo.Time2 = "12:00";
            ReserveInfo.Date3 = "2024/07/03";
            ReserveInfo.Time3 = "13:00";
            ReserveInfo.ContactPhone = 1;
            ReserveInfo.ContactEmail = 1;
            ReserveInfo.Subscribe = 1;
            return;
        }

        public class ReserveData
        {
            public string SectionNumber { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastNameKana { get; set; } = string.Empty;
            public string FirstNameKana { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            public string Prefecture { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string Building { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Date1 { get; set; } = string.Empty;
            public string Time1 { get; set; } = string.Empty;
            public string Date2 { get; set; } = string.Empty;
            public string Time2 { get; set; } = string.Empty;
            public string Date3 { get; set; } = string.Empty;
            public string Time3 { get; set; } = string.Empty;
            public int ContactPhone { get; set; }
            public int ContactEmail { get; set; }
            public int Subscribe { get; set; }
        }
    }
}
