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

        private string? ReienCode { get; set; }
        private string? AreaCode { get; set; }
        private string? SectionCode { get; set; }
        private string? CemeteryCode { get; set; }

        private readonly ApplicationDbContext _context;
        public CemeteryInfoEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PageCemeteryInfo> CemeteryInfos { get; set; } = new List<PageCemeteryInfo>();
        public void OnGet(int? index)
        {
            CemeteryInfoIndex = index;
            if (index.HasValue)
            {
                GetPage(index);
            }
        }
        public IActionResult OnPost(int? index)
        {
            GetPage(index);
            try
            {
                if (Image1 != null)
                {
                    var filePath = Path.Combine(Config.DataFilesRegravePath, "");
                    var fileExtension1 = Path.GetExtension(Image1.FileName);
                    var imgPath = $"{filePath}\\{ReienCode}\\{AreaCode}\\{SectionCode}-{CemeteryCode}-1{fileExtension1}";
                    using (var stream = System.IO.File.Create(imgPath))
                    {
                        Image1.CopyTo(stream);
                    }
                    Image1Fname = Image1.FileName;
                }

                if (Image2 != null)
                {
                    var filePath = Path.Combine(Config.DataFilesRegravePath, "");
                    var fileExtension1 = Path.GetExtension(Image2.FileName);
                    var imgPath = $"{filePath}\\{ReienCode}\\{AreaCode}\\{SectionCode}-{CemeteryCode}-2{fileExtension1}";
                    using (var stream = System.IO.File.Create(imgPath))
                    {
                        Image2.CopyTo(stream);
                    }
                    Image2Fname = Image2.FileName;
                }
                if (index == null)
                {
                    var newCemeteryInfo = new CemeteryInfo
                    {

                    };
                    _context.CemeteryInfos.Add(newCemeteryInfo);
                    _context.SaveChanges();
                }
                else
                {
                    var existingCemeteryinfo = _context.CemeteryInfos.
                        Where(ci => ci.DeleteFlag == 0 && ci.CemeteryInfoIndex == index.Value).
                        FirstOrDefault();

                    if (existingCemeteryinfo != null)
                    {
                        // UPDATE
                        existingCemeteryinfo.Image1Fname = Image1Fname;
                        existingCemeteryinfo.Image2Fname = Image2Fname;
                        existingCemeteryinfo.UpdateDate = DateTime.UtcNow;
                        //existingVender.UpdateUser = LoginId,

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
            // データベースから墓所情報を取得
            var cemeteryinfo = _context.CemeteryInfos
                .Where(ci => ci.DeleteFlag == 0 && ci.CemeteryInfoIndex == index)
                .Select(ci => new PageCemeteryInfo
                {
                    CemeteryInfoIndex = ci.CemeteryInfoIndex,
                    ReienName = ci.Cemetery.Section.Area.Reien.ReienName,
                    AreaName = ci.Cemetery.Section.Area.AreaName,
                    SectionName = ci.Cemetery.Section.SectionName,
                    CemeteryName = ci.Cemetery.CemeteryName,
                    Image1Fname = ci.Image1Fname,
                    Image2Fname = ci.Image2Fname,
                    ReienCode = ci.Cemetery.Section.Area.Reien.ReienCode,
                    AreaCode = ci.Cemetery.Section.Area.AreaCode,
                    SectionCode = ci.Cemetery.Section.SectionCode,
                    CemeteryCode = ci.Cemetery.CemeteryCode
                })
                .FirstOrDefault();

            // 墓所情報を各プロパティに設定
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
                    Image1FnameURL = $"https://localhost:7147/api/Files/GraveImg?r={ReienCode}&a={AreaCode}&k={SectionCode}-{CemeteryCode}&sel=1";
                }
                if (Image2Fname != "")
                {
                    Image2FnameURL = $"https://localhost:7147/api/Files/GraveImg?r={ReienCode}&a={AreaCode}&k={SectionCode}-{CemeteryCode}&sel=2";
                }
            }
        }

        public class PageCemeteryInfo
        {
            public int CemeteryInfoIndex { get; set; }
            public string ReienName { get; set; }
            public string AreaName { get; set; }
            public string SectionName { get; set; }
            public string CemeteryName { get; set; }
            public string Image1Fname { get; set; }
            public string Image2Fname { get; set; }
            public string ReienCode { get; set; }
            public string AreaCode { get; set; }
            public string SectionCode { get; set; }
            public string CemeteryCode { get;set; }
        }
    }
}
