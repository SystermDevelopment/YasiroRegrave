using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.common.Config;

namespace Login_Page.Pages
{
    public class PlotSelectionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PlotSelectionModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SectionData> SectionDatas { get; private set; } = new List<SectionData>();

        public int ReienIndex { get; private set; }
        public string ReienName { get; private set; } = "";
        public int AreaIndex { get; private set; }
        public string AreaName { get; private set; } = "";


        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet()
        {
            GetPage(); 
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
            // 霊園、エリア情報の取得
            ReienIndex = 0;
            AreaIndex = 0;
            ReienName = _context.Reiens.FirstOrDefault(r => r.Index == ReienIndex)?.ReienName ?? "";
            AreaName = _context.Areas.FirstOrDefault(a => a.AreaIndex == AreaIndex)?.AreaName ?? "";

            // 区画情報の取得
            SectionDatas = _context.Sections
                            .Where(s => s.AreaIndex == AreaIndex)
                            .Select(s => new SectionData
                            {
                                SectionIndex = s.SectionIndex,
                                SectionCode = s.SectionCode,
                                SectionName = s.SectionName,
                            })
                            .ToList();

            // 区画ごとに取得
            foreach (var section in SectionDatas)
            {
                // 空き墓所情報の取得
                section.NoReserveCount = _context.CemeteryInfos
                                            .Count(c => c.Cemetery.SectionIndex == section.SectionIndex
                                                && c.ReleaseStatus == (int)Config.ReleaseStatusType.販売中
                                                && c.SectionStatus == (int)Config.SectionStatusType.空
                                            );

                // 区画座標の取得
                var coords = _context.SectionCoords
                    .Where(sc => sc.SectionIndex == section.SectionIndex)
                    .Select(sc => new Coordinate
                    {
                        X = sc.X,
                        Y = sc.Y,
                    })
                    .ToList();

                if (coords.Count > 0)
                {
                    section.Coordinates.AddRange(coords);
                }
            }
            return;
        }


        public class SectionData
        {
            public int SectionIndex { get; set; } = 0;
            public string SectionCode { get; set; } = string.Empty;
            public string SectionName { get; set; } = string.Empty;
            public int NoReserveCount { get; set; } = 0;
            public List<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
        }
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
