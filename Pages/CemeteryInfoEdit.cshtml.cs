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

        //[BindProperty]
        public int? CemeteryInfoIndex { get; set; }

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
                // データベースから墓所情報を取得
                var cemeteryinfo = _context.CemeteryInfos
                    .Where(ci => ci.DeleteFlag == 0 && ci.CemeteryInfoIndex == index.Value)
                    .Select(ci => new PageCemeteryInfo
                    {
                        CemeteryInfoIndex = ci.CemeteryInfoIndex,
                        ReienName = ci.Cemetery.Section.Area.Reien.ReienName,
                        AreaName = ci.Cemetery.Section.Area.AreaName,
                        SectionName = ci.Cemetery.Section.SectionName,
                        CemeteryName = ci.Cemetery.CemeteryName,
                        Image1Fname = ci.Image1Fname,
                        Image2Fname = ci.Image2Fname
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
                }
            }
        }
        public IActionResult OnPost(int? index)
        {
            try
            {
                if (index == null)
                {
                    var newCemeteryInfo = new Models.CemeteryInfo
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
        public class PageCemeteryInfo
        {
            public int CemeteryInfoIndex { get; set; }
            public string ReienName { get; set; }
            public string AreaName { get; set; }
            public string SectionName { get; set; }
            public string CemeteryName { get; set; }
            public string Image1Fname { get; set; }
            public string Image2Fname { get; set; }

        }
    }
}
