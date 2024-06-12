using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string? LoginId { get; private set; }
        public int FilterVender { get; set; } = -1;

        /// <summary>
        /// OnGetèàóù
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            LoginId = HttpContext.Session.GetString("LoginId");
            if (string.IsNullOrEmpty(LoginId))
            {
                return RedirectToPage("/Index");
            }
            GetPage();
            return Page();
        }
        /// <summary>
        /// OnPostèàóù
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost(int index)
        {
            LoginId = HttpContext.Session.GetString("LoginId");
            if (string.IsNullOrEmpty(LoginId))
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
                    userDelete.DeleteFlag = 1;
                    userDelete.UpdateDate = DateTime.Now;
                    //userDelete.UpdateUser = LoginId;
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
        /// âÊñ ê∂ê¨èàóù
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        private void GetPage()
        {
            var userList = _context.Users
                .Where(u => u.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                .Select(u => new UserData
                {
                    Index = u.UserIndex,
                    Id = u.Id,
                    Authority = u.Authority,
                    Name = u.Name,
                    VenderIndex = u.VenderIndex,
                    VenderName = u.Vender.Name,
                    Password = u.Password
                })
                .ToList();
            Users = userList;

            // åüçıã@î\
            if (FilterVender != -1)
            {
                Users = Users
                    .Where(u => u.VenderIndex == FilterVender)
                    .ToList();
            }

            var venderList = _context.Venders
                .Where(v => v.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                .Select(v => new VenderData
                {
                    VenderIndex = v.VenderIndex,
                    VenderName = v.Name,
                })
                .ToList();
            Venders = venderList;
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
        }
        public class VenderData
        {
            public int VenderIndex { get; set; }
            public string VenderName { get; set; } = string.Empty;
        }
    }
}
