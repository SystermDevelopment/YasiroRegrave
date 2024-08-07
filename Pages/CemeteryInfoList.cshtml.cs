using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Configuration;
using System.Security.Cryptography;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.UserListModel;

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
        public List<ReienData> Reiens { get; set; } = new List<ReienData>();
        public List<AreaData> Areas { get; set; } = new List<AreaData>();
        public List<SectionData> Sections { get; set; } = new List<SectionData>();
        public int FilterReien { get; set; } = -1;
        public int FilterArea { get; set; } = -1;
        public int FilterSection { get; set; } = -1;
        public int FilterImage { get; set; } = -1;
        public int FilterPrice { get; set; } = -1;
        public int FilterRelease { get; set; } = -1;

        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }
        public int Authority = (int)Config.AuthorityType.担当者;


        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public IActionResult OnGet(int? FilterReien, int? FilterArea, int? FilterSection, int? FilterImage, int? FilterPrice, int? FilterRelease)
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }
            if (FilterReien.HasValue) this.FilterReien = FilterReien.Value;
            if (FilterArea.HasValue) this.FilterArea = FilterArea.Value;
            if (FilterSection.HasValue) this.FilterSection = FilterSection.Value;
            if (FilterImage.HasValue) this.FilterImage = FilterImage.Value;
            if (FilterPrice.HasValue) this.FilterPrice = FilterPrice.Value;
            if (FilterRelease.HasValue) this.FilterRelease = FilterRelease.Value;
            var checkVender = _context.Users.FirstOrDefault(u => u.UserIndex == LoginId && u.DeleteFlag == (int)Config.DeleteType.未削除)?.VenderIndex;
            if (checkVender != 0)
            {
                return RedirectToPage("/Index");
            }

            GetPage();
            return Page();
        }

        /// <summary>
        /// OnPost処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public IActionResult OnPost(int index)
        {
            LoginId = HttpContext.Session.GetInt32("LoginId");
            if (LoginId == null)
            {
                return RedirectToPage("/Index");
            }

            var action = Request.Form["Action"].ToString();
            if (action == "Search")
            {
                if (!int.TryParse(Request.Form["FilterReien"], out int reien)) { reien = -1; }
                if (!int.TryParse(Request.Form["FilterArea"], out int area)) { area = -1; }
                if (!int.TryParse(Request.Form["FilterSection"], out int sect)) { sect = -1; }
                if (!int.TryParse(Request.Form["FilterImage"], out int img)) { img = -1; }
                if (!int.TryParse(Request.Form["FilterPrice"], out int price)) {  price = -1; }
                if (!int.TryParse(Request.Form["FilterRelease"], out int rls)) { rls = -1; }
                FilterReien = reien;
                FilterArea = area;
                FilterSection = sect;
                FilterImage = img;
                FilterPrice = price;
                FilterRelease = rls;
                HttpContext.Session.SetInt32("FilterReien", FilterReien);
                HttpContext.Session.SetInt32("FilterArea", FilterArea);
                HttpContext.Session.SetInt32("FilterSection", FilterSection);
                HttpContext.Session.SetInt32("FilterImage", FilterImage);
                HttpContext.Session.SetInt32("FilterPrice", FilterPrice);
                HttpContext.Session.SetInt32("FilterRelease", FilterRelease);
                GetPage();
            }
            return Page();
        }

        /// <summary>
        /// 画面生成処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        private void GetPage()
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.DeleteFlag == (int)Config.DeleteType.未削除 && u.UserIndex == LoginId);
            if (existingUser != null)
            {
                Authority = existingUser.Authority;
                SelectedReiens = _context.ReienInfos
                 .Where(ri => ri.Users.UserIndex == existingUser.UserIndex)
                 .Select(ri => ri.Reiens.ReienIndex)
                 .ToList();
            }
            var cemeteryinfoList = _context.CemeteryInfos
            .Where(ci => ci.DeleteFlag == (int)Config.DeleteType.未削除 && SelectedReiens.Contains(ci.Cemetery.Section.Area.ReienIndex))
            .Where(ci => ci.SectionStatus == (int)Config.SectionStatusType.空)
            .OrderBy(ci => ci.Cemetery.Section.Area.Reien.ReienCode)
            .ThenBy(ci => ci.Cemetery.Section.Area.AreaCode)
            .ThenBy(ci => ci.Cemetery.Section.SectionCode)
            .ThenBy(ci => ci.Cemetery.CemeteryCode)
            .Select(ci => new CemeteryInfo
            {
                CemeteryInfoIndex = ci.CemeteryInfoIndex,
                CemeteryIndex = ci.CemeteryIndex,
                CemeteryCode = ci.Cemetery.CemeteryCode,
                CemeteryName = Utils.CemeteryCode2Name(ci.Cemetery.CemeteryCode),
                SectionIndex = ci.Cemetery.Section.SectionIndex,
                SectionCode = ci.Cemetery.Section.SectionCode,
                SectionName = Utils.SectionCode2Name(ci.Cemetery.Section.SectionCode),
                AreaIndex = ci.Cemetery.Section.Area.AreaIndex,
                AreaCode = ci.Cemetery.Section.Area.AreaCode,
                AreaName = ci.Cemetery.Section.Area.AreaName,
                ReienIndex = ci.Cemetery.Section.Area.ReienIndex,
                ReienCode = ci.Cemetery.Section.Area.Reien.ReienCode,
                ReienName = ci.Cemetery.Section.Area.Reien.ReienName,
                Image1Fname = ci.Image1Fname,
                Image2Fname = ci.Image2Fname,
                ReleaseStatus = ci.ReleaseStatus ?? 0,
                ReleaseName = ci.ReleaseStatus == (int)Config.ReleaseStatusType.販売中 ? Config.ReleaseStatusType.販売中.ToString() : Config.ReleaseStatusType.準備中.ToString(),
                TotalPrice = Utils.StringToInt(ci.UsageFee) + Utils.StringToInt(ci.ManagementFee) + Utils.StringToInt(ci.SetPrice) + Utils.StringToInt(ci.StoneFee),
            })
            .ToList();
            CemeteryInfos = cemeteryinfoList;

            // 検索機能
            if (FilterReien != -1)
            {
                CemeteryInfos = CemeteryInfos
                    .Where(c => c.ReienIndex == FilterReien)
                    .ToList();
            }
            if (FilterArea != -1)
            {
                CemeteryInfos = CemeteryInfos
                    .Where(c => c.AreaIndex == FilterArea)
                    .ToList();
            }
            if (FilterSection != -1)
            {
                CemeteryInfos = CemeteryInfos
                    .Where(c => c.SectionIndex == FilterSection)
                    .ToList();
            }
            if (FilterImage != -1)
            {
                if(FilterImage == 0)
                {
                    CemeteryInfos = CemeteryInfos
                        .Where(c => string.IsNullOrEmpty(c.Image1Fname) || string.IsNullOrEmpty(c.Image2Fname))
                        .ToList();
                }
                else
                {
                    CemeteryInfos = CemeteryInfos
                        .Where(c => !string.IsNullOrEmpty(c.Image1Fname) && !string.IsNullOrEmpty(c.Image2Fname))
                        .ToList();
                }
            }
            if (FilterPrice != -1)
            {
                if(FilterPrice == 0)
                {
                    CemeteryInfos = CemeteryInfos
                    .Where(c => c.TotalPrice == 0)
                    .ToList();
                }
                else
                {
                    CemeteryInfos = CemeteryInfos
                    .Where(c => c.TotalPrice > 0)
                    .ToList();
                }
            }
            if (FilterRelease != -1)
            {
                CemeteryInfos = CemeteryInfos
                    .Where(c => c.ReleaseStatus == FilterRelease)
                    .ToList();
            }

            // 検索条件
            var reienList = _context.Reiens
                    .Where(r => r.DeleteFlag == (int)Config.DeleteType.未削除)
                    .OrderBy(r => r.ReienCode)
                    .Select(r => new ReienData
                    {
                        ReienIndex = r.ReienIndex,
                        ReienName = r.ReienName,
                    })
                    .ToList();
            Reiens = reienList;

            var areaList = _context.Areas
                    .Where(a => a.DeleteFlag == (int)Config.DeleteType.未削除)
                    .OrderBy(a => a.Reien.ReienCode)
                    .ThenBy(a => a.AreaCode)
                    .Select(a => new AreaData
                    {
                        ReienIndex = a.ReienIndex,
                        AreaIndex = a.AreaIndex,
                        AreaName = a.AreaName,
                    })
                    .ToList();
            Areas = areaList;

            var sectionList = _context.Sections
                    .Where(s => s.DeleteFlag == (int)Config.DeleteType.未削除)
                    .OrderBy(s => s.Area.Reien.ReienCode)
                    .ThenBy(s => s.Area.AreaCode)
                    .ThenBy(s => s.SectionCode)
                    .Select(s => new SectionData
                    {
                        AreaIndex = s.AreaIndex,
                        SectionIndex = s.SectionIndex,
                        SectionName = Utils.SectionCode2Name(s.SectionCode),
                    })
                    .ToList();
            Sections = sectionList;
            LoggedInUser = Utils.GetLoggedInUser(_context, LoginId);
            return;
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
        /// QR画像処理
        /// </summary>
        /// <param</param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> OnPostGenerateQRCodeAsync(int sectionIndex, int index)
        {
            var code = _context.CemeteryInfos
                    .Where(c => c.CemeteryInfoIndex == index)
                    .Select(c => c.Cemetery.Section.SectionCode + "-" + c.Cemetery.CemeteryCode).FirstOrDefault() ?? "";

            var request = HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string data = $"{baseUrl}/PlotDetails?Index={sectionIndex}&CemeteryInfoIndex={index}";
            string qrCodeUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=300x300&data={Uri.EscapeDataString(data)}";

            using (HttpClient client = new HttpClient())
            {
                byte[] qrCodeImage = await client.GetByteArrayAsync(qrCodeUrl);
                return File(qrCodeImage, "image/png", "QR_" + code + ".png");
            }
        }

        public class CemeteryInfo
        {
            public int CemeteryInfoIndex { get; set; }
            public int ReienIndex { get; set; }
            public string ReienCode { get; set; } = string.Empty;
            public string ReienName { get; set; } = string.Empty;
            public string AreaCode { get; set; } = string.Empty;
            public int AreaIndex { get; set; }
            public string AreaName { get; set; } = string.Empty;
            public string SectionCode { get; set; } = string.Empty;
            public int SectionIndex { get; set; }
            public string SectionName { get; set; } = string.Empty;
            public string CemeteryCode { get; set; } = string.Empty;
            public int CemeteryIndex { get; set; }
            public string CemeteryName { get; set; } = string.Empty;
            public string? Image1Fname { get; set; } = string.Empty;
            public string? Image2Fname { get; set; } = string.Empty;
            public int ReleaseStatus { get; set; }
            public string ReleaseName { get; set; } = string.Empty;
            public int TotalPrice { get; set; } = 0;
        }
        public class ReienData
        {
            public int ReienIndex { get; set; }
            public string ReienName { get; set; } = string.Empty;
        }
        public class AreaData
        {
            public int ReienIndex { get; set; }
            public int AreaIndex { get; set; }
            public string AreaName { get; set; } = string.Empty;
        }
        public class SectionData
        {
            public int AreaIndex { get; set; }
            public int SectionIndex { get; set; }
            public string SectionName { get; set; } = string.Empty;
        }
    }
}
