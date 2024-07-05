using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages
{
    public class ReserveComplateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReserveComplateModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }

        public void OnGet()
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId != null)
            {
                LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
                ViewData["LoggedInUser"] = LoggedInUser;
            }
        }
    }
}
