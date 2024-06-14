using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;


namespace YasiroRegrave.Pages
{

    public class Reserve1Model : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Reserve1Model(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0016)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string LastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0017)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string FirstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0018)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string LastNameKana { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0019)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]
        public string FirstNameKana { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0020)]
        public string PostalCode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0021)]
        public string Prefecture { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0022)]
        public string City { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0023)]
        public string Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0024)]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0025)]
        public string Email { get; set; }

        [BindProperty]
        public List<string> SelectCheckBox { get; set; } = new List<string>();

        [BindProperty]
        public int CemeteryIndex { get; private set; } = 0;

        [BindProperty]
        public int ReserveMode { get; set; } = (int)Config.ReserveType.見学予約;

        [BindProperty]
        public string SectionName { get; set; } = "";

        [BindProperty]
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
                ReserveMode = mode;
                CemeteryIndex = index ?? 0;
                GetPage();
            }
            return;
        }

        /// <summary>
        /// OnPost処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost()
        {
            if (SelectCheckBox.Count == 0)
            {
                ModelState.AddModelError("SelectCheckBox", Message.M_E0026);
            }

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
            var reienData = _context.Cemeteries
                .Include(s => s.Section)
                    .ThenInclude(s => s.Area)
                    .ThenInclude(a => a.Reien)
                .Where(ci => ci.CemeteryIndex == ci.CemeteryIndex && ci.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.Reien.DeleteFlag == (int)Config.DeleteType.未削除)
                .Select(ci => new
                {
                    ReienCode = ci.Section.Area.Reien.ReienCode,
                    ReienIndex = ci.Section.Area.Reien.ReienIndex
                })
                .FirstOrDefault();

            if (reienData != null)
            {
                RegularHolidays = _context.Calenders.Where(c => c.ReienIndex == reienData.ReienIndex).Select(c=>c.RegularHoliday).ToList();
            }
        }
    }
}
