using global::YasiroRegrave.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YasiroRegrave.Pages
{
    public class ReienListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReienListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Reien> Reiens { get; set; } = new List<Reien>();
        public void OnGet()
        {
            GetPage();
        }
        private void GetPage()
        {
            var reienList = _context.Reiens
                .Where(r => r.DeleteFlag == 0)
                .Select(r => new Reien
                {
                    Index = r.Index,
                    Code = r.ReienCode,
                    ReienName = r.ReienName,
                    MailAddress = r.MailAddress,

                })
                .ToList();
            Reiens = reienList;
        }
        public IActionResult OnPost(int index)
        {
            var reienDelete = _context.Reiens.FirstOrDefault(r => r.Index == index);
            if (reienDelete != null)
            {
                //DELITE
                reienDelete.DeleteFlag = 1;
                //reienDelete.UpdateDate = DateTime.Now;
                //reienDelete.UpdateUser = LoginId;

                _context.SaveChanges();
            }
            return RedirectToPage("/ReienList");
        }

        public class Reien
        {
            public int Index { get; set; }
            public string Code { get; set; }
            public string ReienName { get; set; }
            public string MailAddress { get; set; }

        }
    }
}
