using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.ReienListModel;

namespace YasiroRegrave.Pages
{
    public class CemeteryInfoEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CemeteryInfoEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ReienName { get; set; }
        [BindProperty]
        public string SectionName { get; set; }
        [BindProperty]
        public string AreaName { get; set; }
        [BindProperty]
        public string CemeteryName { get; set; }
        [BindProperty]
        public string Image1Fname { get; set; }
        [BindProperty]
        public string Image2Fname { get; set; }

        [BindProperty]
        public string Image1FnameURL { get; set; }
        [BindProperty]
        public string Image2FnameURL { get; set; }
        [BindProperty]
        public IFormFile Image1 { get; set; }
        [BindProperty]
        public IFormFile Image2 { get; set; }

        [BindProperty]
        public int? CemeteryInfoIndex { get; set; }

        [BindProperty]
        public bool Image1Deleted { get; set; } = false;
        [BindProperty]
        public bool Image2Deleted { get; set; } = false;
        public int Authority = (int)Config.AuthorityType.担当者;

        private string? ReienCode { get; set; }
        private string? AreaCode { get; set; }
        private string? SectionCode { get; set; }
        private string? CemeteryCode { get; set; }

        public List<PageCemeteryInfo> CemeteryInfos { get; set; } = new List<PageCemeteryInfo>();
        public int? LoginId { get; private set; }


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

            CemeteryInfoIndex = index;
            if (index.HasValue)
            {
                GetPage(index);
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
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }

            GetPage(index);
            try
            {
                var filePath = Path.Combine(Config.DataFilesRegravePath, "");

                // Process Image1
                if (Image1Deleted && !string.IsNullOrEmpty(Image1Fname))
                {
                    var imgPath = $"{filePath}\\{ReienCode}\\{AreaCode}\\{SectionCode}-{CemeteryCode}-1{Path.GetExtension(Image1Fname)}";
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    Image1Fname = null;
                }
                else if (Image1 != null && !Image1Deleted) // Ensure that the image is not deleted
                {
                    var fileExtension1 = Path.GetExtension(Image1.FileName);
                    var imgPath = $"{filePath}\\{ReienCode}\\{AreaCode}\\{SectionCode}-{CemeteryCode}-1{fileExtension1}";
                    using (var stream = System.IO.File.Create(imgPath))
                    {
                        Image1.CopyTo(stream);
                    }
                    Image1Fname = Image1.FileName;
                }

                // Process Image2
                if (Image2Deleted && !string.IsNullOrEmpty(Image2Fname))
                {
                    var imgPath = $"{filePath}\\{ReienCode}\\{AreaCode}\\{SectionCode}-{CemeteryCode}-2{Path.GetExtension(Image2Fname)}";
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    Image2Fname = null;
                }
                else if (Image2 != null && !Image2Deleted) // Ensure that the image is not deleted
                {
                    var fileExtension2 = Path.GetExtension(Image2.FileName);
                    var imgPath = $"{filePath}\\{ReienCode}\\{AreaCode}\\{SectionCode}-{CemeteryCode}-2{fileExtension2}";

                    using (var stream = System.IO.File.Create(imgPath))
                    {
                        Image2.CopyTo(stream);
                    }
                    Image2Fname = Image2.FileName;
                }

                if (index == null)
                {
                    /* Nothing */
                }
                else
                {
                    var existingCemeteryinfo = _context.CemeteryInfos
                        .Where(ci => ci.DeleteFlag == (int)Config.DeleteType.未削除 && ci.CemeteryInfoIndex == index.Value)
                        .FirstOrDefault();
                    if (existingCemeteryinfo != null)
                    {
                        // UPDATE
                        existingCemeteryinfo.Image1Fname = Image1Fname;
                        existingCemeteryinfo.Image2Fname = Image2Fname;
                        existingCemeteryinfo.UpdateDate = DateTime.Now;
                        existingCemeteryinfo.UpdateUser = LoginId;
                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/CemeteryInfoList");
        }
        private void GetPage(int? index)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.DeleteFlag == (int)Config.DeleteType.未削除 && u.UserIndex == LoginId);
            if (existingUser != null)
            {
                Authority = existingUser.Authority;
            }

            var cemeteryinfo = _context.CemeteryInfos
            .Where(ci => ci.DeleteFlag == (int)Config.DeleteType.未削除 && ci.CemeteryInfoIndex == index)
            .Select(ci => new PageCemeteryInfo
            {
                CemeteryInfoIndex = ci.CemeteryInfoIndex,
                ReienName = ci.Cemetery.Section.Area.Reien.ReienName,
                AreaName = ci.Cemetery.Section.Area.AreaName,
                SectionName = ci.Cemetery.Section.SectionName,
                CemeteryName = ci.Cemetery.CemeteryName,
                Image1Fname = ci.Image1Fname ?? "",
                Image2Fname = ci.Image2Fname ?? "",
                ReienCode = ci.Cemetery.Section.Area.Reien.ReienCode,
                AreaCode = ci.Cemetery.Section.Area.AreaCode,
                SectionCode = ci.Cemetery.Section.SectionCode,
                CemeteryCode = ci.Cemetery.CemeteryCode
            })
            .FirstOrDefault();


            if (cemeteryinfo != null)
            {
                ReienName = cemeteryinfo.ReienName;
                AreaName = cemeteryinfo.AreaName;
                SectionName = cemeteryinfo.SectionName;
                CemeteryName = cemeteryinfo.CemeteryName;
                Image1Fname = cemeteryinfo.Image1Fname;
                Image2Fname = cemeteryinfo.Image2Fname;
                ReienCode = cemeteryinfo.ReienCode;
                AreaCode = cemeteryinfo.AreaCode;
                SectionCode = cemeteryinfo.SectionCode;
                CemeteryCode = cemeteryinfo.CemeteryCode;
                if (Image1Fname != "")
                {
                    Image1FnameURL = $"/api/Files/GraveImg?r={ReienCode}&a={AreaCode}&k={SectionCode}-{CemeteryCode}&sel=1";
                }
                if (Image2Fname != "")
                {
                    Image2FnameURL = $"/api/Files/GraveImg?r={ReienCode}&a={AreaCode}&k={SectionCode}-{CemeteryCode}&sel=2";
                }
            }
            return;
        }


        public IActionResult OnPostDeleteImage1()
        {
            Image1Deleted = true;
            Image1Fname = null;
            Image1FnameURL = null;
            return Page();
        }

        public IActionResult OnPostDeleteImage2()
        {
            Image2Deleted = true;
            Image2Fname = null;
            Image2FnameURL = null;
            return Page();
        }


        public class PageCemeteryInfo
        {
            public int CemeteryInfoIndex { get; set; }
            public string ReienName { get; set; } = string.Empty;
            public string AreaName { get; set; } = string.Empty;
            public string SectionName { get; set; } = string.Empty;
            public string CemeteryName { get; set; } = string.Empty;
            public string Image1Fname { get; set; } = string.Empty;
            public string Image2Fname { get; set; } = string.Empty;
            public string ReienCode { get; set; } = string.Empty;
            public string AreaCode { get; set; } = string.Empty;
            public string SectionCode { get; set; } = string.Empty;
            public string CemeteryCode { get; set; } = string.Empty;
        }
    }
}
