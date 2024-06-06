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
        public string ReienCode { get; private set; } = "";
        public string ReienName { get; private set; } = "";
        public int AreaIndex { get; private set; }
        public string AreaCode { get; private set; } = "";
        public string AreaName { get; private set; } = "";


        /// <summary>
        /// OnGet����
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet()
        {
            GetPage(); 
            return;
        }
        /// <summary>
        /// OnPost����
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost()
        {
            GetPage();
            return Page();
        }
        /// <summary>
        /// ��ʐ�������
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void GetPage()
        {
            // �쉀�A�G���A���̎擾
            ReienIndex = 0;
            AreaIndex = 0;
            ReienCode = _context.Reiens.FirstOrDefault(r => r.Index == ReienIndex && r.DeleteFlag == (int)Config.DeleteType.���폜)?.ReienCode ?? "";
            ReienName = _context.Reiens.FirstOrDefault(r => r.Index == ReienIndex && r.DeleteFlag == (int)Config.DeleteType.���폜)?.ReienName ?? "";
            AreaCode = _context.Areas.FirstOrDefault(a => a.AreaIndex == AreaIndex && a.DeleteFlag == (int)Config.DeleteType.���폜)?.AreaCode ?? "";
            AreaName = _context.Areas.FirstOrDefault(a => a.AreaIndex == AreaIndex && a.DeleteFlag == (int)Config.DeleteType.���폜)?.AreaName ?? "";

            // �����̎擾
            SectionDatas = _context.Sections
                            .Where(s => s.AreaIndex == AreaIndex && s.DeleteFlag == (int)Config.DeleteType.���폜)
                            .Select(s => new SectionData
                            {
                                SectionIndex = s.SectionIndex,
                                SectionCode = s.SectionCode,
                                SectionName = Utils.SectionCode2Name(s.SectionCode),
                            })
                            .ToList();

            // ��悲�ƂɎ擾
            foreach (var section in SectionDatas)
            {
                // �󂫕揊���̎擾
                section.NoReserveCount = _context.CemeteryInfos
                                            .Count(c => c.Cemetery.SectionIndex == section.SectionIndex
                                                && c.ReleaseStatus == (int)Config.ReleaseStatusType.�̔���
                                                && c.SectionStatus == (int)Config.SectionStatusType.��
                                            );
            }
            return;
        }


        public class SectionData
        {
            public int SectionIndex { get; set; } = 0;
            public string SectionCode { get; set; } = string.Empty;
            public string SectionName { get; set; } = string.Empty;
            public int NoReserveCount { get; set; } = 0;
        }
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
