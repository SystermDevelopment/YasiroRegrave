using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Login_Page.Pages
{
    public class PlotSelectionModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            return Page();
        }

    }
}
