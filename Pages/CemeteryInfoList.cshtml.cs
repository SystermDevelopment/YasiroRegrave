using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Configuration;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

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
        public List<int> SelectedReiens { get; set; } = new List<int>();
        public string? LoginId { get; private set; }
        public int Authority = (int)Config.AuthorityType.担当者;
        public IActionResult OnGet()
        {
            LoginId = HttpContext.Session.GetString("LoginId");
            if (string.IsNullOrEmpty(LoginId))
            {
                return RedirectToPage("/Index");
            }
            GetPage();
            return Page();
        }
        private void GetPage()
        {
            var existingUser = _context.Users.FirstOrDefault(v => v.DeleteFlag == 0 && v.Id == LoginId);
            if (existingUser != null)
            {
                Authority = existingUser.Authority;
                SelectedReiens = _context.ReienInfos
                 .Where(ri => ri.Users.UserIndex == existingUser.UserIndex)
                 .Select(ri => ri.Reiens.ReienIndex)
                 .ToList();
            }
            var cemeteryinfoList = _context.CemeteryInfos
            .Where(ci => ci.DeleteFlag == 0 && SelectedReiens.Contains(ci.Cemetery.Section.Area.ReienIndex))
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
