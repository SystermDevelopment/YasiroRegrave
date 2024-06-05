using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;
using static Login_Page.Pages.PlotSelectionModel;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.PlotDetailsModel;

namespace YasiroRegrave.Pages
{
    public class PlotDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PlotDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public SectionData? SectionDatas { get; private set; } = new SectionData();

        public int SectionIndex { get; private set; } = 0;
        public string SectionName { get; private set; } = "";


        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet(int? index)
        {
            if (index.HasValue)
            {
                SectionIndex = index ?? 0;
                GetPage();
            }
            return;
        }
        /// <summary>
        /// OnPost処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost()
        {
            GetPage();
            return Page();
        }
        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void GetPage()
        {
            // 霊園、エリア、区画情報の取得
            SectionDatas = _context.Sections
                .Where(s => s.SectionIndex == SectionIndex)
                .Select(s => new SectionData
                {
                    ReienIndex = s.Area.Reien.Index,
                    ReienCode = s.Area.Reien.ReienCode,
                    ReienName = s.Area.Reien.ReienName,
                    AreaIndex = s.Area.AreaIndex,
                    AreaCode = s.Area.AreaCode,
                    AreaName = s.Area.AreaName,
                    SectionIndex = s.SectionIndex,
                    SectionCode = s.SectionCode,
                    SectionName = s.SectionName,
                })
                .FirstOrDefault();

            SectionName = SectionDatas?.SectionName ?? "";
            return;
        }


        public class SectionData
        {
            public int ReienIndex { get; set; } = 0;
            public string ReienCode { get; set; } = string.Empty;
            public string ReienName { get; set; } = string.Empty;
            public int AreaIndex { get; set; } = 0;
            public string AreaCode { get; set; } = string.Empty;
            public string AreaName { get; set; } = string.Empty;
            public int SectionIndex { get; set; } = 0;
            public string SectionCode { get; set; } = string.Empty;
            public string SectionName { get; set; } = string.Empty;
        }
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
