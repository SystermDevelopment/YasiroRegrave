using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;

namespace YasiroRegrave.Pages
{
    public class UserListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public UserListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<User> Users { get; set; } = new List<User>();
        public void OnGet()
        {
            GetPage();
        }
        private void GetPage()
        {
            var userList = _context.Users
                //.Where(u => u.DeleteFlag == 0)
                .Select(u => new User
                {
                    Index = u.Index,
                    Id = u.Id,
                    Name = u.Name,
                    VenderIndex = u.VenderIndex,
                    VenderName = u.Vendor.Name,
                    Password = u.Password
                })
                .ToList();
            Users = userList;
        }
        public class User
        {
            public int Index { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public int VenderIndex {  get; set; }
            public string VenderName { get; set; }
            public string Password { get; set; }
        }
    }
}
