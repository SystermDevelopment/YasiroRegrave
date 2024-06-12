using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;


namespace YasiroRegrave.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0014)]

        public string? LoginId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0004)]
        public string? Password { get; set; }

        public bool ShowConfirm { get; set; } = false;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == LoginId && u.Password == Password);
            if (user != null && LoginId != null)
            {
                HttpContext.Session.SetString("LoginId", LoginId);
                if (user.Authority == (int)Config.AuthorityType.ä«óùé“)
                {
                    return RedirectToPage("/UserList");
                }
                else if (user.Authority == (int)Config.AuthorityType.íSìñé“)
                {
                    return RedirectToPage("/CemeteryInfoList");
                }
            }
            ShowConfirm = true;
            return Page();
        }
    }
}
