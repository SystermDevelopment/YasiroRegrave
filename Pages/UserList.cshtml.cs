using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages
{
    public class UserListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public UserListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<UserData> Users { get; set; } = new List<UserData>();
        public List<VenderData> Venders { get; set; } = new List<VenderData>();

        [BindProperty]
        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }
        public int FilterVender { get; set; } = -1;
        public int YsrMngCnt { get; set; } = 0;

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

            var action = Request.Form["Action"].ToString();
            if (action == "Delete")
            {
                var userDelete = _context.Users.FirstOrDefault(u => u.UserIndex == index);
                if (userDelete != null)
                {
                    //DELITE
                    userDelete.DeleteFlag = (int)Config.DeleteType.削除;
                    userDelete.UpdateDate = DateTime.Now;
                    userDelete.UpdateUser = LoginId;
                    _context.SaveChanges();
                }
                return RedirectToPage("/UserList");
            }
            else if (action == "Search")
            {
                if (!int.TryParse(Request.Form["FilterVender"], out int temp))
                {
                    temp = -1;
                }
                FilterVender = temp;
                GetPage();
            }
            return Page();
        }

        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        private void GetPage()
        {
            var userList = _context.Users
                .Include(u => u.Vender)
                .Where(u => u.DeleteFlag == (int)Config.DeleteType.未削除)
                .OrderBy(u => u.VenderIndex)
                .ThenBy(u => u.Id)
                .Select(u => new UserData
                {
                    Index = u.UserIndex,
                    Id = u.Id,
                    Authority = u.Authority,
                    Name = u.Name,
                    VenderIndex = u.VenderIndex,
                    VenderName = u.Vender.Name,
                    Password = u.Password,
                })
                .ToList();
            Users = userList;

            // 検索機能
            if (FilterVender != -1)
            {
                Users = Users
                    .Where(u => u.VenderIndex == FilterVender)
                    .ToList();
            }

            var venderList = _context.Venders
                .Where(v => v.DeleteFlag == (int)Config.DeleteType.未削除)
                .OrderBy(v => v.VenderIndex)
                .Select(v => new VenderData
                {
                    VenderIndex = v.VenderIndex,
                    VenderName = v.Name,
                })
                .ToList();
            Venders = venderList;
            LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
            // ヤシロ管理者数
            YsrMngCnt = _context.Users
                .Where(u => u.DeleteFlag == (int)Config.DeleteType.未削除
                    && u.Authority == (int)Config.AuthorityType.管理者
                    && u.VenderIndex == 0)  // ヤシロ
                .Count();
            return;
        }

        public class UserData
        {
            public int Index { get; set; }
            public string Id { get; set; } = string.Empty;
            public int Authority { get; set; }
            public string Name { get; set; } = string.Empty;
            public int VenderIndex { get; set; }
            public string VenderName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public VenderData Vender { get; set; } = new VenderData();  
        }
        public class VenderData
        {
            public int VenderIndex { get; set; }
            public string VenderName { get; set; } = string.Empty;
        }
    }
}
