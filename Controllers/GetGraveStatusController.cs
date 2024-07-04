using Microsoft.AspNetCore.Mvc;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetGraveStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GetGraveStatusController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult GetGraveIStatus([FromBody] GetGraveStatusRequest request)
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
            var response = _context.CemeteryInfos
                .Where(r => r.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(r => !startDate.HasValue || r.UpdateDate.Value.AddSeconds(-1) > startDate)
                .Where(r => !endDate.HasValue || r.UpdateDate <= endDate)
                .Select(r => new GetGraveStatusResponse
                {
                    霊園番号 = r.Cemetery.Section.Area.Reien.ReienCode.ToString(),
                    区画番号 = r.Cemetery.Section.SectionCode.ToString() + "-" + r.Cemetery.CemeteryCode.ToString(),
                    面積 = r.AreaValue,
                    区画区分 = r.SectionType,
                    使用料 = r.UsageFee,
                    管理料 = r.ManagementFee,
                    仕置巻石料 = r.StoneFee,
                    石碑代金 = r.SetPrice,
                    区画状態 = r.SectionStatus == 0 ? "空" : r.SectionStatus == 1 ? "予約中" : r.SectionStatus == 2 ? "拠点予約" : r.SectionStatus == 3 ? "成約" : "不明",
                    販売ステータス = r.ReleaseStatus == 0 ? "準備中" : r.ReleaseStatus == 1 ? "販売中" : "不明",
                    画像情報登録状態 = (string.IsNullOrEmpty(r.Image1Fname)|| string.IsNullOrEmpty(r.Image2Fname) ) ? "未" : "済",
                    最終更新日時 = r.UpdateDate.HasValue ? r.UpdateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : ""
                }).ToList();
            return Ok(response);
        }
    }
    public class GetGraveStatusRequest
    {
        public string? start_date { get; set; }
        public string? end_date { get; set; }
    }

    public class GetGraveStatusResponse
    {
        public string 霊園番号 { get; set; }
        public string 区画番号 { get; set; }
        public string 面積 { get; set; }
        public string 区画区分 { get; set; }
        public string 使用料 { get; set; }
        public string 管理料 { get; set; }
        public string 仕置巻石料 { get; set; }
        public string 石碑代金 { get; set; }
        public string 区画状態 { get; set; }
        public string 販売ステータス { get; set; }
        public string 画像情報登録状態 { get; set; }
        public string 最終更新日時 { get; set; }
    }
}
