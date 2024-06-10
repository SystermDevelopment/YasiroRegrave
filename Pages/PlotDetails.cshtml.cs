using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

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
        public List<CemeteryData> CemeteryDatas { get; private set; } = new List<CemeteryData>();

        public string ReienCode { get; private set; } = "";
        public string AreaCode { get; private set; } = "";
        public int SectionIndex { get; private set; } = 0;
        public string SectionCode { get; private set; } = "";
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
                .Where(s => s.SectionIndex == SectionIndex && s.DeleteFlag == (int)Config.DeleteType.未削除)
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
                    SectionName = Utils.SectionCode2Name(s.SectionCode),
                })
                .FirstOrDefault();

            ReienCode = SectionDatas?.ReienCode ?? "";
            AreaCode = SectionDatas?.AreaCode ?? "";
            SectionCode = SectionDatas?.SectionCode ?? "";
            SectionName = SectionDatas?.SectionName ?? "";

            if (SectionDatas != null)
            {
                // 墓所情報の取得
                CemeteryDatas = _context.CemeteryInfos
                    .Where(c => c.Cemetery.SectionIndex == SectionDatas.SectionIndex
                            && c.Cemetery.DeleteFlag == (int)Config.DeleteType.未削除
                            && c.DeleteFlag == (int)Config.DeleteType.未削除
                            && c.ReleaseStatus == (int)Config.ReleaseStatusType.販売中
                            && c.SectionStatus == (int)Config.SectionStatusType.空
                            )
                    .Select(c => new CemeteryData
                    {
                        CemeteryIndex = c.CemeteryIndex,
                        CemeteryCode = c.Cemetery.CemeteryCode,
                        CemeteryName = Utils.SectionCode2Name(c.Cemetery.CemeteryCode),
                        CemeteryDisp = Utils.CemeteryCode2Disp(c.Cemetery.CemeteryCode),
                        SectionStatus = c.SectionStatus ?? 0,
                        AreaValue = c.AreaValue ?? "",
                        UsageFee = Utils.StringToInt(c.UsageFee),
                        ManagementFee = Utils.StringToInt(c.ManagementFee),
                        StoneFee = Utils.StringToInt(c.StoneFee),
                        SetPrice = Utils.StringToInt(c.SetPrice),
                        TotalPrice = Utils.StringToInt(c.UsageFee) + Utils.StringToInt(c.ManagementFee) + Utils.StringToInt(c.SetPrice),
                    })
                    .ToList();
            }
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
        public class CemeteryData
        {
            public int CemeteryIndex { get; set; } = 0;
            public string CemeteryCode { get; set; } = string.Empty;
            public string CemeteryName { get; set; } = string.Empty;
            public string CemeteryDisp { get; set; } = string.Empty;
            public int SectionStatus { get; set; } = 0;
            public string AreaValue { get; set; } = string.Empty;
            public int UsageFee { get; set; } = 0;
            public int ManagementFee { get; set; } = 0;
            public int StoneFee { get; set; } = 0;
            public int SetPrice { get; set; } = 0;
            public int TotalPrice { get; set; } = 0;
        }
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
