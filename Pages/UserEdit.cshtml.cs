using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages

{

    public class UserEditModel : PageModel
    {
        [BindProperty]
        [StringLength(20, ErrorMessage = Message.M_E0010)]
        [Required(ErrorMessage = Message.M_E0003)]

        public string Id { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0001)]
        [StringLength(100, ErrorMessage = Message.M_E0002)]

        public string Name { get; set; } = string.Empty;

        public List<Vender> Venders{ get; set; } = new List<Vender>();

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0004)]

        public string Password { get; set; } = string.Empty;
        [BindProperty]
        [Required(ErrorMessage = Message.M_E0008)]
        public int SelectVenderIndex { get; set; }
        //[BindProperty]
        public int? Index { get; set; }

        private readonly ApplicationDbContext _context;
        public UserEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PageUser> Users { get; set; } = new List<PageUser>();
        public void OnGet(int? index)
        {
            Index = index;
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
            Venders = _context.Venders
                .Where(v => v.DeleteFlag == 0)
                .ToList();
            
        }
        public IActionResult OnPost(int? index)
        {
            if (!ModelState.IsValid)
            {
                Venders = _context.Venders.Where(v => v.DeleteFlag == 0).ToList();
                return Page();
            }
            try
            {
                if (index == null)
                {
                    // ššššTDB.VenderID‰¼‘Î‰žšššš
                var forignVender = _context.Venders
                    .Where(u => u.Index == SelectVenderIndex)
                    .FirstOrDefault();

                if (forignVender == null)
                {
                    throw new InvalidOperationException();
                }
                    //INSERT
                    var newUser = new User
                    {
                        Id=Id,
                        Name = Name,
                        Password = Password,    
                        CreateDate = DateTime.UtcNow,
                        //CreateUser = LoginId,
                        DeleteFlag = 0,
                        Vender = forignVender,
                        VenderIndex = SelectVenderIndex,

                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                }
                else
                {
                    var existingUser = _context.Users.FirstOrDefault(v => v.DeleteFlag == 0 && v.Index == index.Value);
                    if (existingUser != null)
                    {
                        // UPDATE
                        existingUser.Id = Id;
                        existingUser.Name = Name;
                        existingUser.Password = Password;
                        existingUser.UpdateDate = DateTime.UtcNow;
                        existingUser.VenderIndex = SelectVenderIndex;
                        //existingVender.UpdateUser = LoginId,

                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/UserList");
        }

        public class PageUser
        {
            public int Index { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public int VenderIndex { get; set; }
            public string VenderName { get; set; }
            public string Password { get; set; }
        }
    }
}
