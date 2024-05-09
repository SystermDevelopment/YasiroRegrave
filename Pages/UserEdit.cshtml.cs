using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;

namespace YasiroRegrave.Pages



{
    public class UserEditModel : PageModel
    {
        [BindProperty]
        public string Id { get; set; } = string.Empty;

        [BindProperty]
        public string Name { get; set; } = string.Empty;

        public List<string> VenderNames { get; set; } = new List<string>();

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        //[BindProperty]
        public int SelectedVender = 0;

        private readonly ApplicationDbContext _context;
        public UserEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<User> Users { get; set; } = new List<User>();
        public void OnGet(int? index)
        {
            if (index.HasValue)
            {
                var user = _context.Users
                    .Where(u => u.DeleteFlag == 0 && u.Index == index.Value)
                    .FirstOrDefault();
                if (user != null)
                {
                    Id = user.Id;
                    Name = user.Name;
                    Password = user.Password;
                }
            }
            VenderNames = _context.Venders
                .Where(v => v.DeleteFlag == 0)
                .Select(v => v.Name)
                .ToList();
        }
        public class User
        {
            public int Index { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public int VendorIndex { get; set; }
            public string VenderName { get; set; }
            public string Password { get; set; }
        }
    }
}
