using global::YasiroRegrave.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Pages.common;

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

        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }

        /// <summary>
        /// OnGet処理
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
            var checkAuthority = _context.Users.FirstOrDefault(u => u.UserIndex == LoginId && u.DeleteFlag == (int)Config.DeleteType.未削除)?.Authority;
            if (checkAuthority != (int)Config.AuthorityType.管理者)
            {
                return RedirectToPage("/Index");
            }

            GetPage();
            return Page();
        }
        
        /// <summary>
        /// OnPost処理
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
            
            var reienDelete = _context.Reiens.FirstOrDefault(r => r.ReienIndex == index);
            if (reienDelete != null)
            {
                //DELITE
                reienDelete.DeleteFlag = (int)Config.DeleteType.削除;
                reienDelete.UpdateDate = DateTime.Now;
                reienDelete.UpdateUser = LoginId;
                _context.SaveChanges();
            }
            return RedirectToPage("/ReienList");
        }

        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        private void GetPage()
        {
            var reienList = _context.Reiens
                .Where(r => r.DeleteFlag == (int)Config.DeleteType.未削除)
                .OrderBy(r => r.ReienCode)
                .Select(r => new Reien
                {
                    Index = r.ReienIndex,
                    Code = r.ReienCode,
                    ReienName = r.ReienName,
                    MailAddress = r.MailAddress ?? "",
                })
                .ToList();
            Reiens = reienList;
            LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
            return;
        }


        public class Reien
        {
            public int Index { get; set; }
            public string Code { get; set; } = string.Empty;
            public string ReienName { get; set; } = string.Empty;
            public string MailAddress { get; set; } = string.Empty;
        }
    }
}
