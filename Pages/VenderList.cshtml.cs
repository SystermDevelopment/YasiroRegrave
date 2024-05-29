using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;

namespace YasiroRegrave.Pages
{
    public class VenderListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public VenderListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Vender> Venders { get; set; } = new List<Vender>();
        public void OnGet()
        {
            GetPage();
        }
        private void GetPage()
        {
            var venderList = _context.Venders
                .Where(v => v.DeleteFlag == 0)
                .Select(v => new Vender
                {
                    Index = v.Index,
                    Name = v.Name,
                })
                .ToList();
            Venders = venderList;
        }
        public IActionResult OnPost(int index)
        {
            var venderDelete = _context.Venders.FirstOrDefault(v => v.Index == index);
            if (venderDelete != null)
            {
                //DELITE
                venderDelete.DeleteFlag = 1;
                _context.SaveChanges();
            }
            return RedirectToPage("/VenderList");
        }


        public class Vender
        {
            public int Index { get; set; }
            public string Name { get; set; }
        }
    }
}
