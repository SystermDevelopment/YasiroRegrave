using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetGraveInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GetGraveInfoController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult SetGraveInfo([FromBody] GetGraveInfoRequest request)
        {
            var start = request.start_date;
            var end = request.end_date;

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(start))
            {
                startDate = DateTime.Parse(start);
            }

            if (!string.IsNullOrEmpty(end))
            {
                endDate = DateTime.Parse(end);
            }

            var reserveInfos = _context.ReserveInfos
                .Include(r => r.CemeteryInfo)
                .ThenInclude(c => c.Cemetery)
                .ThenInclude(c => c.Section)
                .ThenInclude(s => s.Area)
                .ThenInclude(a => a.Reien)
                .Where(r => r.CemeteryInfo.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(r => !startDate.HasValue || r.CreateDate.Value.AddSeconds(-1) > startDate)
                .Where(r => !endDate.HasValue || r.CreateDate <= endDate)
                .ToList();

            foreach (var reserveInfo in reserveInfos)
            {
                reserveInfo.Notification = (int)Config.NotificationType.通知済;
            }

            _context.SaveChanges();

            var response = reserveInfos.Select(r => new
            {
                予約日時 = r.CreateDate.HasValue ? r.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                霊園番号 = r.CemeteryInfo.Cemetery.Section.Area.Reien.ReienCode.ToString(),
                区画番号 = r.CemeteryInfo.Cemetery.Section.SectionCode.ToString() + "-" + r.CemeteryInfo.Cemetery.CemeteryCode.ToString(),
                使用料 = r.UsageFee ?? "",
                管理料 = r.ManagementFee ?? "",
                仕置巻石料 = r.StoneFee ?? "",
                墓石セット価格 = r.SetPrice ?? "",
                名前姓 = r.LastName ?? "",
                名前名 = r.FirstName ?? "",
                名前姓ヨミ = r.LastNameYomi ?? "",
                名前名ヨミ = r.FirstNameYomi ?? "",
                郵便番号 = r.ZipCode ?? "",
                住所 = r.Adress ?? "",
                電話番号 = r.TelephoneNumber ?? "",
                メール = r.EMail ?? "",
                質問 = r.Question ?? "",
                希望日時1 = r.PreferredDate1.HasValue ? r.PreferredDate1.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                希望日時2 = r.PreferredDate2.HasValue ? r.PreferredDate2.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                希望日時3 = r.PreferredDate3.HasValue ? r.PreferredDate3.Value.ToString("yyyy/MM/dd HH:mm:ss") : "",
                連携内容 = r.PreferredDate1.HasValue ? "見学予約" : "仮予約",
                販売協力会社index = r.VenderIndex == 0 ? null : r.VenderIndex.ToString()
            }).ToList();
            return Ok(response);
        }
    }
    public class GetGraveInfoRequest
    {
        public string? start_date { get; set; }
        public string? end_date { get; set; }
    }

    public class GetGraveInfoResponse
    {
        public string? 予約日時 { get; set; }
        public string 霊園番号 { get; set; } = string.Empty;
        public string 区画番号 { get; set; } = string.Empty;
        public string 名前姓 { get; set; } = string.Empty;
        public string 名前名 { get; set; } = string.Empty;
        public string 名前姓ヨミ { get; set; } = string.Empty;
        public string 名前名ヨミ { get; set; } = string.Empty;
        public string 郵便番号 { get; set; } = string.Empty;
        public string 住所 { get; set; } = string.Empty;
        public string 電話番号 { get; set; } = string.Empty;
        public string メール { get; set; } = string.Empty;
        public string? 質問 { get; set; }
        public string? 希望日時1 { get; set; }
        public string? 希望日時2 { get; set; }
        public string? 希望日時3 { get; set; }
        public string? 連携内容 { get; set; }
        public string 販売協力会社index { get; set; } = string.Empty;
    }
}
