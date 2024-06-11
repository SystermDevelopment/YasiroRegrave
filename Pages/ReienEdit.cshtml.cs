using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages
{ 
    public class ReienEditModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = Message.M_E0006)]
        [StringLength(500, ErrorMessage = Message.M_E0011)]

        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0005)]
        [StringLength(10, ErrorMessage = Message.M_E0009)]

        public string ReienCode { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0002)]
        [StringLength(500, ErrorMessage = Message.M_E0013)]

        public string MailAddress { get; set; } = string.Empty;

        //[BindProperty]
        public int? Index { get; set; }

        public int SelectedReien = 0;

        private readonly ApplicationDbContext _context;
        public ReienEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PageReien> Reiens { get; set; } = new List<PageReien>();
        public void OnGet(int? index)
        {
            Index = index;
            if (index.HasValue)
            {
                var reien = _context.Reiens
            .Where(r => r.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                    .FirstOrDefault();
                if (reien != null)
                {
                    ReienCode = reien.ReienCode;
                    Name = reien.ReienName;
                    MailAddress = reien.MailAddress;
                }
            }
        }
        public IActionResult OnPost(int? index)
        {
            try
            {
                if (index == null)
                {
                    var newReien = new Reien
                    {
                        ReienCode = ReienCode,
                        ReienName = Name,
                        MailAddress = MailAddress,
                        CreateDate = DateTime.UtcNow,
                        //CreateUser = LoginId,
                        DeleteFlag = (int)Config.DeleteType.ñ¢çÌèú,
                        //Vendor = forignVender,



                    };
                    _context.Reiens.Add(newReien);
                    _context.SaveChanges();
                }
                else
                {
                    var existingReien = _context.Reiens
                        .Where(r => r.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú && r.ReienIndex == index.Value)
                        .FirstOrDefault();
                    if (existingReien != null)
                    {
                        // UPDATE
                        existingReien.ReienCode = ReienCode;
                        existingReien.ReienName = Name;
                        existingReien.MailAddress = MailAddress;
                        existingReien.UpdateDate = DateTime.UtcNow;
                        //existingVender.UpdateUser = LoginId,

                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/ReienList");
        }
        public class PageReien
        {
            public int Index { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string MailAddress { get; set; }
        }
    }
}
