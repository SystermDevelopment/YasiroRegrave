using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

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
        
        public int? LoginId { get; private set; }


        /// <summary>
        /// OnGet����
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }
            var checkAuthority = _context.Users.FirstOrDefault(u => u.UserIndex == LoginId && u.DeleteFlag == (int)Config.DeleteType.���폜)?.Authority;
            if (checkAuthority != (int)Config.AuthorityType.�Ǘ���)
            {
                return RedirectToPage("/Index");
            }

            GetPage();
            return Page();
        }

        /// <summary>
        /// OnPost����
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost(int index)
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }
            
            var venderDelete = _context.Venders.FirstOrDefault(v => v.VenderIndex == index);
            if (venderDelete != null)
            {
                //DELITE
                venderDelete.DeleteFlag = (int)Config.DeleteType.�폜;
                venderDelete.UpdateDate = DateTime.Now;
                venderDelete.UpdateUser = LoginId;
                _context.SaveChanges();
            }
            return RedirectToPage("/VenderList");
        }

        /// <summary>
        /// ��ʐ�������
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        private void GetPage()
        {
            var venderList = _context.Venders
                .Where(v => v.DeleteFlag == (int)Config.DeleteType.���폜)
                .Select(v => new Vender
                {
                    Index = v.VenderIndex,
                    Name = v.Name,
                    Member = _context.Users.Count(u => u.VenderIndex == v.VenderIndex && u.DeleteFlag == (int)Config.DeleteType.���폜),
                })
                .OrderBy(v => v.Index)
                .ToList();
            Venders = venderList;

            return;
        }


        public class Vender
        {
            public int Index { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Member { get; set; }
        }
    }
}
