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

        public string Name { get; set; } = string.Empty;

        [BindProperty]

        public string Code { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = Message.M_E0003)]
        [StringLength(100, ErrorMessage = Message.M_E0002)]

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
                    .Where(r => r.DeleteFlag == 0 && r.Index == index.Value)
                    .FirstOrDefault();
                if (reien != null)
                {
                    Code = reien.Code;
                    Name = reien.Name;
                    MailAddress = reien.MailAddress;
                    
                }
            }
            //VenderNames = _context.Venders
            //    .Where(v => v.DeleteFlag == 0)
            //    .Select(v => v.Name)
            //    .ToList();
        }
        public IActionResult OnPost(int? index)
        {
            try
            {
                if (index == null)
                {
                    //// ššššTDB.VenderID‰¼‘Î‰žšššš
                    //var forignVender = _context.Venders.FirstOrDefault();
                    //if (forignVender == null)
                    //{
                    //    throw new InvalidOperationException();
                    //}
                    //INSERT
                    var newReien = new Reien
                    {
                        Code = Code,
                        Name = Name,
                        MailAddress = MailAddress,
                        CreateDate = DateTime.UtcNow,
                        //CreateUser = LoginId,
                        DeleteFlag = 0,
                        //Vendor = forignVender,



                    };
                    _context.Reiens.Add(newReien);
                    _context.SaveChanges();
                }
                else
                {
                    var existingReien = _context.Reiens.Where(r => r.DeleteFlag == 0 && r.Index == index.Value).FirstOrDefault();
                    if (existingReien != null)
                    {
                        // UPDATE
                        existingReien.Code = Code;
                        existingReien.Name = Name;
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
