using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;

namespace YasiroRegrave.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}
        [BindProperty]
        public string? LoginId { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet()
        {
            Page();
        }
        public IActionResult OnPost()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId==LoginId
                                                       && u.Password==Password);
            if (user != null)
            {
                return RedirectToPage("/PlotSelection");
            }
            else
            {
                return Page();
            }
        }
    }
}