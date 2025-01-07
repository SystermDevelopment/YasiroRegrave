using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages
{
    public class PlotAreaModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PlotAreaModel(ApplicationDbContext context)
        {
            _context = context;
        }

        //public List<SectionData> SectionDatas { get; private set; } = new List<SectionData>();
        public List<AreaData> AreaDatas { get; private set; } = new List<AreaData>();

        public int ReienIndex { get; private set; }
        public string ReienCode { get; private set; } = "";
        public string ReienName { get; private set; } = "";
        public int AreaIndex { get; private set; }
        public string AreaCode { get; private set; } = "";
        public string AreaName { get; private set; } = "";

        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }
        public string hrefBack { get; private set; } = "https://www.yasiro.co.jp/reien/ikoma/";
        public string strBack { get; private set; } = "ホームページ";

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
        /// ログアウト処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void GetPage()
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId != null)
            {
                LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
                ViewData["LoggedInUser"] = LoggedInUser;

                // 前画面の戻り先
                var user = _context.Users.FirstOrDefault(u => u.UserIndex == LoginId);
                if (user.Authority == (int)Config.AuthorityType.管理者)
                {
                    hrefBack = "/UserList";
                    strBack = "管理画面";
                }
                else if (user.Authority == (int)Config.AuthorityType.担当者 && user.VenderIndex == 0)
                {
                    hrefBack = "/CemeteryInfoList";
                    strBack = "管理画面";
                }
                else
                {
                    hrefBack = "/Index";
                    strBack = "ログイン画面";
                }
            }

            AreaDatas = _context.Areas
                .OrderBy(area => area.AreaIndex)
                .Select(a => new AreaData
                {
                    AreaIndex = a.AreaIndex,
                    AreaCode = a.AreaCode,
                    AreaName = a.AreaName
                })
                .ToList();

            // 霊園、エリア情報の取得（大阪生駒霊園、第１期、固定とする）
            ReienIndex = _context.Reiens.FirstOrDefault(r => r.ReienName == "大阪生駒霊園" && r.DeleteFlag == (int)Config.DeleteType.未削除)?.ReienIndex ?? 0;
            AreaIndex = _context.Areas.FirstOrDefault(a => a.AreaName == "第１期" && a.DeleteFlag == (int)Config.DeleteType.未削除)?.AreaIndex ?? 0;
            // 霊園、エリア情報の取得
            ReienCode = _context.Reiens.FirstOrDefault(r => r.ReienIndex == ReienIndex && r.DeleteFlag == (int)Config.DeleteType.未削除)?.ReienCode ?? "";
            ReienName = _context.Reiens.FirstOrDefault(r => r.ReienIndex == ReienIndex && r.DeleteFlag == (int)Config.DeleteType.未削除)?.ReienName ?? "";
            AreaCode = _context.Areas.FirstOrDefault(a => a.AreaIndex == AreaIndex && a.DeleteFlag == (int)Config.DeleteType.未削除)?.AreaCode ?? "";
            AreaName = _context.Areas.FirstOrDefault(a => a.AreaIndex == AreaIndex && a.DeleteFlag == (int)Config.DeleteType.未削除)?.AreaName ?? "";

            //// 区画情報の取得
            //SectionDatas = _context.Sections
            //                .Where(s => s.AreaIndex == AreaIndex && s.DeleteFlag == (int)Config.DeleteType.未削除)
            //                .Select(s => new SectionData
            //                {
            //                    SectionIndex = s.SectionIndex,
            //                    SectionCode = s.SectionCode,
            //                    SectionName = Utils.SectionCode2Name(s.SectionCode),
            //                })
            //                .ToList();

            //// 区画ごとに取得
            //foreach (var section in SectionDatas)
            //{
            //    // 空き墓所情報の取得
            //    section.NoReserveCount = _context.CemeteryInfos
            //                                .Count(c => c.Cemetery.SectionIndex == section.SectionIndex
            //                                    && c.Cemetery.DeleteFlag == (int)Config.DeleteType.未削除
            //                                    && c.DeleteFlag == (int)Config.DeleteType.未削除
            //                                    && c.ReleaseStatus == (int)Config.ReleaseStatusType.販売中
            //                                    && c.SectionStatus == (int)Config.SectionStatusType.空
            //                                );
            //}
            return;
        }


        public class AreaData
        {
            public int AreaIndex { get; set; } = 0;
            public string AreaCode { get; set; } = string.Empty;
            public string AreaName { get; set; } = string.Empty;
        }

        //public class SectionData
        //{
        //    public int SectionIndex { get; set; } = 0;
        //    public string SectionCode { get; set; } = string.Empty;
        //    public string SectionName { get; set; } = string.Empty;
        //    public int NoReserveCount { get; set; } = 0;
        //}
        //public class Coordinate
        //{
        //    public int X { get; set; }
        //    public int Y { get; set; }
        //}
    }
}
