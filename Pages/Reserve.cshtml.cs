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

    public class Index1Model : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Index1Model(ApplicationDbContext context)
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
        public List<string> SelectCheckBox { get; set; } = new List<string>();

        [BindProperty]
        public int CemeteryIndex { get; private set; } = 0;

        [BindProperty]
        public int ReserveMode { get; set; } = (int)Config.ReserveType.å©äwó\ñÒ;

        [BindProperty]
        public string SectionName { get; set; } = "";

        /// <summary>
        /// OnGetèàóù
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
        public void GetPage()
        {
            var existingCemetery = _context.Cemeteries.FirstOrDefault(r => r.DeleteFlag == 0 && r.CemeteryIndex == CemeteryIndex);
            if (existingCemetery != null)
            {
                var existingSection = _context.Sections.FirstOrDefault(r => r.DeleteFlag == 0 && r.SectionIndex == existingCemetery.SectionIndex);
                if (existingSection != null)
                {
                    SectionName = Utils.SectionCode2Name(existingSection.SectionCode) + "-" + Utils.CemeteryCode2Name(existingCemetery.CemeteryCode);
                    return;
                }
            }
        }
    }
}
