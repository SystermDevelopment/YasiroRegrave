using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.common.Config;


namespace YasiroRegrave.Pages
{
    public class VenderEditModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = Message.M_E0007)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]

        public string VenderName { get; set; } = string.Empty;

        public List<string> VenderNames { get; set; } = new List<string>();

        public int? Index { get; set; }
        
        public Config.DeleteType DeleteFlag { get; set; }
    

        //[BindProperty]
        public int SelectedVender = 0;

        private readonly ApplicationDbContext _context;
        public VenderEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PageVender> Venders { get; set; } = new List<PageVender>();

        public void OnGet(int? index)
        {
            Index = index;
            if (index.HasValue)
            {
                var vender = _context.Venders
                    .Where(v => v.Index == Index && v.DeleteFlag == (int)Config.DeleteType.–¢íœ)

                    .FirstOrDefault();
                if (vender != null)
                {
                    VenderName = vender.Name;
                }
            }
        }

        public IActionResult OnPost(int? index)
        {
            try
            {
                if (index == null)
                {
                    //INSERT
                    var newVender = new Vender
                    {
                        Name = VenderName,
                        CreateDate = DateTime.UtcNow,
                        //CreateUser = LoginId,
                        DeleteFlag = (int)Config.DeleteType.–¢íœ,


                    };
                    _context.Venders.Add(newVender);
                    _context.SaveChanges();
                }
                else
                {
                    var existingVender = _context.Venders.Where(v => v.DeleteFlag == 0 && v.Index == index.Value).FirstOrDefault();
                    if (existingVender != null)
                    {
                        // UPDATE
                        existingVender.Name = VenderName;
                        existingVender.UpdateDate = DateTime.UtcNow;
                        //existingVender.UpdateUser = LoginId,

                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/VenderList");
        }






    

        public class PageVender
        {
            public int Index { get; set; }
            public string Name { get; set; }
        }
    }
}
