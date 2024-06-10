using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;

namespace YasiroRegrave.Pages
{

        public class CemeteryInfoListModel : PageModel
        {
            private readonly ApplicationDbContext _context;
            public CemeteryInfoListModel(ApplicationDbContext context)
            {
                _context = context;
            }
            public List<CemeteryInfo> CemeteryInfos { get; set; } = new List<CemeteryInfo>();
            public void OnGet()
            {
                GetPage();
            }
            private void GetPage()
            {

            var cemeteryinfoList = _context.CemeteryInfos
                .Where(ci => ci.DeleteFlag == 0)
                .Select(ci => new CemeteryInfo
                {
                    CemeteryInfoIndex = ci.CemeteryInfoIndex,
                    CemeteryIndex = ci.CemeteryIndex,
                    CemeteryCode = ci.Cemetery.CemeteryCode,
                    CemeteryName = ci.Cemetery.CemeteryName,
                    SectionIndex = ci.Cemetery.Section.SectionIndex,
                    SectionCode = ci.Cemetery.Section.SectionCode,
                    SectionName = ci.Cemetery.Section.SectionName,
                    AreaIndex = ci.Cemetery.Section.Area.AreaIndex,
                    AreaCode = ci.Cemetery.Section.Area.AreaCode,
                    AreaName = ci.Cemetery.Section.Area.AreaName,
                    ReienIndex = ci.Cemetery.Section.Area.ReienIndex,
                    ReienCode = ci.Cemetery.Section.Area.Reien.ReienCode,
                    ReienName = ci.Cemetery.Section.Area.Reien.ReienName,
                    Image1Fname = ci.Image1Fname,
                    Image2Fname = ci.Image2Fname,
                })
                    .ToList();
            CemeteryInfos = cemeteryinfoList;
            }

            public class CemeteryInfo
            {
                public int CemeteryInfoIndex { get; set; }
                public int ReienIndex { get; set; }
                public string ReienCode { get; set; }
                public string ReienName { get; set; }
                public string AreaCode { get; set; }
                public int AreaIndex { get; set; }
                public string AreaName { get; set; }
                public string SectionCode { get; set; }
                public int SectionIndex { get; set; }
                public string SectionName { get; set; }
                public string CemeteryCode { get; set; }
                public int CemeteryIndex { get; set; }
                public string CemeteryName { get; set; }
                public string? Image1Fname { get; set; }
                public string? Image2Fname { get; set; }

            }

        }
    }
