using Microsoft.AspNetCore.Mvc;
using YasiroRegrave.Data;

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
                .Where(r => r.CemeteryInfo.DeleteFlag == 0)
                .Where(r => !startDate.HasValue || r.CreateDate >= startDate)
                .Where(r => !endDate.HasValue || r.CreateDate <= endDate)
                .ToList();

            foreach (var reserveInfo in reserveInfos)
            {
                reserveInfo.Notification = 1;
            }

            _context.SaveChanges();

            var response = reserveInfos.Select(r => new
            {
                霊園番号 = r.CemeteryInfo.CemeteryIndex.ToString(),
                区画番号 = r.CemeteryInfo.CemeteryInfoIndex.ToString(),
                使用料 = r.CemeteryInfo.UsageFee ?? "N/A",
                管理料 = r.CemeteryInfo.ManagementFee ?? "N/A",
                仕置巻石料 = r.CemeteryInfo.StoneFee ?? "N/A",
                墓石セット価格 = r.CemeteryInfo.SetPrice ?? "N/A",
                名前姓 = r.LastName ?? "N/A",
                名前名 = r.FirstName ?? "N/A",
                名前姓ヨミ = r.LastNameYomi ?? "N/A",
                名前名ヨミ = r.FirstNameYomi ?? "N/A",
                郵便番号 = r.ZipCode ?? "N/A",
                住所 = r.Adress ?? "N/A",
                電話番号 = r.TelephoneNumber ?? "N/A",
                メール = r.EMail ?? "N/A",
                質問 = r.Question ?? "N/A",
                希望日時1 = r.PreferredDate1.HasValue ? r.PreferredDate1.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A",
                希望日時2 = r.PreferredDate2.HasValue ? r.PreferredDate2.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A",
                希望日時3 = r.PreferredDate3.HasValue ? r.PreferredDate3.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A",
                連携内容 = r.Notification == 0 ? "仮予約" : "見学予約",
                販売協力会社index = r.VenderIndex.ToString()
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
