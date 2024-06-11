using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.UserListModel;

namespace YasiroRegrave.Pages

{

    public class UserEditModel : PageModel
    {
        [BindProperty]
        [StringLength(20, ErrorMessage = Message.M_E0010)]
        [Required(ErrorMessage = Message.M_E0003)]

        public string Id { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0008)]
        public int Authority { get; set; } = 1;

        [BindProperty]
        public List<int> SelectedReiens { get; set; } = new List<int>();

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
        public int? SelectVenderIndex { get; set; } 
        //[BindProperty]
        public int? Index { get; set; }

        public List<Reien> reiens { get; set; } = new List<Reien>();

        private readonly ApplicationDbContext _context;
        public UserEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PageUser> Users { get; set; } = new List<PageUser>();
        public void OnGet(int? index)
        {
            reiens = _context.Reiens
                .Where(r => r.DeleteFlag == (int)Config.DeleteType.–¢íœ)
                .ToList();
            Index = index;
            if (index.HasValue)
            {
                var user = _context.Users
                    .Where(u => u.UserIndex == Index && u.DeleteFlag == (int)Config.DeleteType.–¢íœ)
                    .FirstOrDefault();
                if (user != null)
                {
                    Id = user.Id;
                    Authority = user.Authority;
                    Name = user.Name;
                    Password = user.Password;
                    SelectedReiens = _context.ReienInfos
                        .Where(ri => ri.Users.UserIndex == user.UserIndex)
                        .Select(ri => ri.Reiens.ReienIndex)
                        .ToList();
                    SelectVenderIndex = user.Vender.Index;
                }
            }
            Venders = _context.Venders
            .Where(v => v.DeleteFlag == (int)Config.DeleteType.–¢íœ)
            .ToList();

        }
        public IActionResult OnPost(int? index)
        {
            if (!ModelState.IsValid)
            {
                if(index!=null)
                {
                    var user = _context.Users
                        .Where(u => u.UserIndex == index && u.DeleteFlag == (int)Config.DeleteType.–¢íœ)
                        .FirstOrDefault();
                    if (user != null)
                    {
                        SelectVenderIndex = user.Vender.Index;
                    }
                }
                reiens = _context.Reiens
                    .Where(r => r.DeleteFlag == (int)Config.DeleteType.–¢íœ)
                    .ToList();
                Venders = _context.Venders
                    .Where(v => v.DeleteFlag == (int)Config.DeleteType.–¢íœ) 
                    .ToList();
                return Page();
            }
            try
            {
                reiens = _context.Reiens
                    .Where(r => r.DeleteFlag == (int)Config.DeleteType.–¢íœ)
                    .ToList();

                if (index == null)
                {
                    var forignVender = _context.Venders
                        .Where(u => u.Index == SelectVenderIndex)
                        .FirstOrDefault();

                    if (forignVender == null)
                    {
                        throw new InvalidOperationException();
                    }
                    //INSERT
                    var newUser = new YasiroRegrave.Model.User
                    {
                        Id=Id,
                        Authority = Authority,
                        Name = Name,
                        Password = Password,    
                        CreateDate = DateTime.UtcNow,
                        //CreateUser = LoginId,
                        DeleteFlag = (int)Config.DeleteType.–¢íœ,
                        Vender = forignVender,
                        VenderIndex = SelectVenderIndex ??0,

                    };
                    _context.Users.Add(newUser);
                    foreach (var reienIndex in SelectedReiens)
                    {
                        if (!_context.ReienInfos.Any(ri => ri.Users.UserIndex == newUser.UserIndex && ri.Reiens.ReienIndex == reienIndex))
                        {
                            _context.ReienInfos.Add(new ReienInfo
                            {
                                Users = newUser,
                                Reiens = _context.Reiens.Find(reienIndex)
                            });
                        }
                    }
                    _context.SaveChanges();
                }
                else
                {
                    var existingUser = _context.Users.FirstOrDefault(v => v.DeleteFlag == 0 && v.UserIndex == index.Value);
                    if (existingUser != null)
                    {
                        // UPDATE
                        existingUser.Id = Id;
                        existingUser.Authority = Authority;
                        existingUser.Name = Name;
                        existingUser.Password = Password;
                        existingUser.UpdateDate = DateTime.UtcNow;
                        existingUser.VenderIndex = SelectVenderIndex ?? 0;
                        //existingVender.UpdateUser = LoginId,
                        var existingReienInfos = _context.ReienInfos.Where(ri => ri.Users.UserIndex == existingUser.UserIndex).ToList();
                        _context.ReienInfos.RemoveRange(existingReienInfos);

                        foreach (var reienIndex in SelectedReiens)
                        {
                            _context.ReienInfos.Add(new ReienInfo
                            {
                                Users = existingUser,
                                Reiens = _context.Reiens.Find(reienIndex)
                            });
                        }
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
