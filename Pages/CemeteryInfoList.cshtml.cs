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
                    .Where(c => c.DeleteFlag == 0)
                    .Select(c => new CemeteryInfo
                    {
                        CemeteryInfoIndex = c.CemeteryInfoIndex,
                        CemeteryIndex = c.CemeteryIndex,
                        //CemeteryCode = c.Cemetery.CemeteryCode,
                        //SectionCode = c.Section.SectionCode,
                        //AreaCode = c.Area.Areacode,
                        //ReienCode = c.Reien.ReienCode,
                    })
                    .ToList();
            CemeteryInfos = cemeteryinfoList;
            }
            public IActionResult OnPost(int index)
            {
                var CemeteryInfoDelete = _context.CemeteryInfos.FirstOrDefault(c => c.CemeteryInfoIndex == index);
                if (CemeteryInfoDelete != null)
                {
                    //DELITE
                    CemeteryInfoDelete.DeleteFlag = 1;
                    CemeteryInfoDelete.UpdateDate = DateTime.Now;
                  //CemeteryInfoDelete.UpdateUser = LoginId
                _context.SaveChanges();
                }
                return RedirectToPage("/CemeteryInfoList");
            }

            public class CemeteryInfo
            {
                public int CemeteryInfoIndex { get; set; }
                public int CemeteryIndex { get; set; }
                public string ReienCode { get; set; }
                public string AreaCode { get; set; }
                public string SectionCode { get; set; }
                public string CemeteryCode { get; set; }

                public string? Image1Fname { get; set; }
                public string? Image2Fname { get; set; }
            }
        }
    }
