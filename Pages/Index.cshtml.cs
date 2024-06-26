using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

            var user = _context.Users
                .Include(u => u.Vender)
                .FirstOrDefault(u => u.Id == LoginId && u.Password == Password);
            if (user != null && LoginId != null && user.Id == LoginId && user.Password == Password)
            {
                HttpContext.Session.SetInt32("LoginId", user.UserIndex);
                if (user.Authority == (int)Config.AuthorityType.管理者)
                {
                    return RedirectToPage("/UserList");
                }
                else if (user.Authority == (int)Config.AuthorityType.担当者 && user.Vender.VenderIndex == 0)
                {
                    return RedirectToPage("/CemeteryInfoList");
                }
                else
                {
                    return RedirectToPage("/PlotSelection");
                }
            }
            ShowConfirm = true;
            return Page();
        }
    }
}
