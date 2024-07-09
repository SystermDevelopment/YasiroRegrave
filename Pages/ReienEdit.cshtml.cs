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
    public class ReienEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReienEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0006)]
        [StringLength(500, ErrorMessage = Message.M_E0011)]

        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0005)]
        [StringLength(10, ErrorMessage = Message.M_E0009)]

        public string ReienCode { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = Message.M_E0002)]
        [StringLength(500, ErrorMessage = Message.M_E0013)]

        public string MailAddress { get; set; } = string.Empty;

        //[BindProperty]
        public int? Index { get; set; }

        public int SelectedReien = 0;

        public List<PageReien> Reiens { get; set; } = new List<PageReien>();

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
                var reien = _context.Reiens
                    .Where(r => r.ReienIndex == Index && r.DeleteFlag == (int)Config.DeleteType.未削除)
                    .FirstOrDefault();
                if (reien != null)
                {
                    ReienCode = reien.ReienCode;
                    Name = reien.ReienName;
                    MailAddress = reien.MailAddress ?? "";
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
            // メールアドレスチェック
            if (!string.IsNullOrEmpty(MailAddress))
            {
                string[] addresses = MailAddress.Split(',');
                if (addresses.Any(address => !Utils.IsValidMailAddress(address)))
                {
                    ModelState.AddModelError("MailAddress", Message.M_E0028);
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (index == null)
                {
                    // INSERT
                    //var newReien = new Reien
                    //{
                    //    ReienCode = ReienCode,
                    //    ReienName = Name,
                    //    MailAddress = MailAddress,
                    //    CreateDate = DateTime.Now,
                    //    CreateUser = LoginId,
                    //    DeleteFlag = (int)Config.DeleteType.未削除,
                    //};
                    //_context.Reiens.Add(newReien);
                    //_context.SaveChanges();
                }
                else
                {
                    var existingReien = _context.Reiens
                        .Where(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.ReienIndex == index.Value)
                        .FirstOrDefault();
                    if (existingReien != null)
                    {
                        // UPDATE
                        existingReien.ReienCode = ReienCode;
                        existingReien.ReienName = Name;
                        existingReien.MailAddress = MailAddress;
                        existingReien.UpdateDate = DateTime.Now;
                        existingReien.UpdateUser = LoginId;
                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/ReienList");
        }


        public class PageReien
        {
            public int Index { get; set; }
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string MailAddress { get; set; } = string.Empty;
        }
    }
}
