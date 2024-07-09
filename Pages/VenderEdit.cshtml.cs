using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.common.Config;
using static YasiroRegrave.Pages.UserListModel;


namespace YasiroRegrave.Pages
{
    public class VenderEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public VenderEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0007)]
        [StringLength(100, ErrorMessage = Message.M_E0011)]

        public string VenderName { get; set; } = string.Empty;

        public List<string> VenderNames { get; set; } = new List<string>();

        public int? Index { get; set; }

        public List<UserData> Users { get; set; } = new List<UserData>();

        public Config.DeleteType DeleteFlag { get; set; }
    
        //[BindProperty]
        public int SelectedVender = 0;

        public List<PageVender> Venders { get; set; } = new List<PageVender>();

        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }

        /// <summary>
        /// OnGet処理
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
            LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
            var checkAuthority = _context.Users.FirstOrDefault(u => u.UserIndex == LoginId && u.DeleteFlag == (int)Config.DeleteType.未削除)?.Authority;
            if (checkAuthority != (int)Config.AuthorityType.管理者)
            {
                return RedirectToPage("/Index");
            }

            Index = index;
            if (index.HasValue)
            {
                var vender = _context.Venders
                    .Where(v => v.VenderIndex == Index && v.DeleteFlag == (int)Config.DeleteType.未削除)
                    .FirstOrDefault();
                if (vender != null)
                {
                    VenderName = vender.Name;
                }
            }
            return Page();
        }

        /// <summary>
        /// OnPost処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost(int? index)
        {
            Index = index;
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }
            LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (index == null)
                {
                    //INSERT
                    var newVender = new Vender
                    {
                        Name = VenderName,
                        CreateDate = DateTime.Now,
                        CreateUser = LoginId,
                        DeleteFlag = (int)Config.DeleteType.未削除,
                    };
                    _context.Venders.Add(newVender);
                    _context.SaveChanges();
                }
                else
                {
                    var existingVender = _context.Venders.Where(v => v.DeleteFlag == (int)Config.DeleteType.未削除 && v.VenderIndex == index.Value).FirstOrDefault();
                    if (existingVender != null)
                    {
                        // UPDATE
                        existingVender.Name = VenderName;
                        existingVender.UpdateDate = DateTime.Now;
                        existingVender.UpdateUser = LoginId;
                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/VenderList");
        }
    

        public class PageVender
        {
            public int Index { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
