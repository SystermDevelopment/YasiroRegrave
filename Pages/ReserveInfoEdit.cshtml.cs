//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.ComponentModel.DataAnnotations;
//using System.Diagnostics.SymbolStore;
//using YasiroRegrave.Data;
//using YasiroRegrave.Model;
//using YasiroRegrave.Pages.common;

//namespace YasiroRegrave.Pages

//{
//    public class ReserveInfoEditModel : PageModel
//    {
//        [BindProperty]

//        public string Last_Name { get; set; } = string.Empty;

//        [BindProperty]

//        public string First_Name { get; set; } = string.Empty;

//        [BindProperty]
//        public string Last_Name_Yomi { get; set; } = string.Empty;
//        [BindProperty]

//        public string Zip_Code { get; set; } = string.Empty;
//        [BindProperty]

//        public string Adress { get; set; } = string.Empty;
//        [BindProperty]
//        public string Telephone_Number { get; set; } = string.Empty;

//        [BindProperty]
//        public string E_Mail { get; set; } = string.Empty;

//        [BindProperty]
//        public string Question { get; set; } = string.Empty;




        ////[BindProperty]
        //public int? ReserveIndex { get; set; }

        //public int SelectedVender = 0;

        //private readonly ApplicationDbContext _context;
        //public ReserveInfoEditModel(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        //public List<PageReserveInfo> Users { get; set; } = new List<PageReserveInfo>();
        //public void OnGet(int? index)
        //{
        //    ReserveIndex = ReserveIndex;
        //    if (index.HasValue)
        //    {
        //        var user = _context.Reserve_Infos
        //            .Where(u => u.DeleteFlag == 0 && u.Index == index.Value)
        //            .FirstOrDefault();
        //        if (user != null)
        //        {
        //            Id = user.Id;
        //            Name = user.Name;
        //            Password = user.Password;
        //        }
        //    }
            //VenderNames = _context.Venders
            //    .Where(v => v.DeleteFlag == 0)
            //    .Select(v => v.Name)
            //    .ToList();
        //}
        //public IActionResult OnPost(int? index)
        //{
        //    try
        //    {
        //        if (index == null)
        //        {
        //            // ššššTDB.VenderID‰¼‘Î‰žšššš
        //            var forignVender = _context.Venders.FirstOrDefault();
        //            if (forignVender == null)
        //            {
        //                throw new InvalidOperationException();
        //            }
        //            //INSERT
        //            var newUser = new User
        //            {
        //                Id = Id,
        //                Name = Name,
        //                Password = Password,
        //                CreateDate = DateTime.UtcNow,
        //                //CreateUser = LoginId,
        //                DeleteFlag = 0,
        //                Vendor = forignVender,



        //            };
        //            _context.Users.Add(newUser);
        //            _context.SaveChanges();
        //        }
        //        else
        //        {
        //            var existingUser = _context.Users.Where(v => v.DeleteFlag == 0 && v.Index == index.Value).FirstOrDefault();
        //            if (existingUser != null)
        //            {
        //                // UPDATE
        //                existingUser.Id = Id;
        //                existingUser.Name = Name;
        //                existingUser.Password = Password;
        //                existingUser.UpdateDate = DateTime.UtcNow;
        //                //existingVender.UpdateUser = LoginId,

        //                _context.SaveChanges();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return Page();
        //    }
        //    return RedirectToPage("/UserList");
        //}
//        public class PageReserveInfo
//        {
//            public int Index { get; set; }
//            public string Id { get; set; }
//            public string Name { get; set; }
//            public int VendorIndex { get; set; }
//            public string VenderName { get; set; }
//            public string Password { get; set; }
//        }
//    }
//}
