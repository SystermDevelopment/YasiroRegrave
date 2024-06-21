using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages
{
    public class UserEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public UserEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
        [Required(ErrorMessage = Message.M_E0002)]
        [StringLength(100, ErrorMessage = Message.M_E0013)]

        public string MailAddress { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0008)]
        public int? SelectVenderIndex { get; set; } 

        //[BindProperty]
        public int? Index { get; set; }

        public List<Reien> reiens { get; set; } = new List<Reien>();

        public List<PageUser> Users { get; set; } = new List<PageUser>();

        public int? LoginId { get; private set; }


        /// <summary>
        /// OnGetèàóù
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public IActionResult OnGet(int? index)
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }
            var checkAuthority = _context.Users.FirstOrDefault(u => u.UserIndex == LoginId && u.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)?.Authority;
            if (checkAuthority != (int)Config.AuthorityType.ä«óùé“)
            {
                return RedirectToPage("/Index");
            }

            reiens = _context.Reiens
                .Where(r => r.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                .ToList();
            Index = index;
            if (index.HasValue)
            {
                var user = _context.Users
                    .Where(u => u.UserIndex == Index && u.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                    .FirstOrDefault();
                if (user != null)
                {
                    Id = user.Id;
                    Authority = user.Authority;
                    Name = user.Name;
                    Password = user.Password;
                    MailAddress = user.MailAddress;
                    SelectedReiens = _context.ReienInfos
                        .Where(ri => ri.Users.UserIndex == user.UserIndex)
                        .Select(ri => ri.Reiens.ReienIndex)
                        .ToList();
                    SelectVenderIndex = user.Vender.VenderIndex;
                }
            }
            Venders = _context.Venders
            .Where(v => v.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
            .ToList();

            return Page();
        }

        /// <summary>
        /// OnPostèàóù
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost(int? index)
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }

            if (!ModelState.IsValid)
            {
                if(index!=null)
                {
                    var user = _context.Users
                        .Where(u => u.UserIndex == index && u.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                        .FirstOrDefault();
                    if (user != null)
                    {
                        SelectVenderIndex = user.Vender.VenderIndex;
                    }
                }
                reiens = _context.Reiens
                    .Where(r => r.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                    .ToList();
                Venders = _context.Venders
                    .Where(v => v.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú) 
                    .ToList();
                return Page();
            }
            try
            {
                reiens = _context.Reiens
                    .Where(r => r.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú)
                    .ToList();

                if (index == null)
                {
                    var forignVender = _context.Venders
                        .Where(u => u.VenderIndex == SelectVenderIndex)
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
                        MailAddress = MailAddress,
                        CreateDate = DateTime.Now,
                        CreateUser = LoginId,
                        DeleteFlag = (int)Config.DeleteType.ñ¢çÌèú,
                        Vender = forignVender,
                        VenderIndex = SelectVenderIndex ?? 0,

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
                    var existingUser = _context.Users.FirstOrDefault(v => v.DeleteFlag == (int)Config.DeleteType.ñ¢çÌèú && v.UserIndex == index.Value);
                    if (existingUser != null)
                    {
                        // UPDATE
                        existingUser.Id = Id;
                        existingUser.Authority = Authority;
                        existingUser.Name = Name;
                        existingUser.Password = Password;
                        existingUser.MailAddress = MailAddress;
                        existingUser.UpdateDate = DateTime.Now;
                        existingUser.UpdateUser = LoginId;
                        existingUser.VenderIndex = SelectVenderIndex ?? 0;
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
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public int VenderIndex { get; set; }
            public string VenderName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
