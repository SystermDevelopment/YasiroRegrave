using Microsoft.AspNetCore.Mvc;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using System.Linq;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetGraveStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SetGraveStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SetGraveStatus([FromBody] List<SetGGraveStatus> infos)
        {
            foreach (var info in infos)
            {
                var parts = info.区画番号.Split('-');
                if (parts.Length != 3)
                {
                    return BadRequest($"Invalid format for 区画番号: {info.区画番号}. Expected format: xx-xx-xx");
                }
                string section = parts[0];
                string cemetery = parts[1] + "-" + parts[2];

                // 霊園
                var existingReien = _context.Reiens.FirstOrDefault(r => r.DeleteFlag == 0 && r.ReienCode == info.霊園番号);
                if (existingReien == null)
                {
                    return BadRequest($"Invalid value for 霊園番号: {info.霊園番号}. Expected value");
                }

                // エリア(工区)
                var existingArea = _context.Areas.FirstOrDefault(r => r.DeleteFlag == 0 && r.ReienIndex == existingReien.Index && r.AreaCode == info.工区番号);
                if (existingArea == null)
                {
                    return BadRequest($"Invalid value for 工区番号: {info.工区番号}. Expected value");
                }

                // 区画
                var existingSection = _context.Sections.FirstOrDefault(r => r.DeleteFlag == 0 && r.AreaIndex == existingArea.AreaIndex && r.SectionCode == section);
                if (existingSection == null)
                {
                    return BadRequest($"Invalid value for 区画番号: {section}. Expected value");
                }

                // 墓所
                var existingCemetery = _context.Cemeteries.FirstOrDefault(r => r.DeleteFlag == 0 && r.SectionIndex == existingSection.SectionIndex && r.CemeteryCode == cemetery);
                if (existingCemetery == null)
                {
                    return BadRequest($"Invalid value for 墓所番号: {cemetery}. Expected value");
                }

                // 墓所情報
                var existingCemeteryInfo = _context.CemeteryInfos.FirstOrDefault(r => r.DeleteFlag == 0 && r.CemeteryIndex == existingCemetery.CemeteryIndex);
                if (existingCemeteryInfo == null)
                {
                    return BadRequest($"Invalid value for 墓所情報: {existingCemetery.CemeteryIndex}. Expected value");
                }

                // 予約情報
                var existingReserveInfo = _context.ReserveInfos.FirstOrDefault(r => r.CemeteryInfoIndex == existingCemetery.CemeteryIndex);
                if (existingReserveInfo != null)
                {
                    // 未通知の予約情報があった場合の状態変更抑止
                    if (existingReserveInfo.Notification == 0)
                    {
                        return BadRequest($"There is an unnotified reservation. {existingReserveInfo.LastName}様　{(existingReserveInfo.CreateDate.HasValue ? existingReserveInfo.CreateDate.Value.ToString("yyyy年MM月dd日 HH:mm:ss") : "日付が不明")}");
                    }
                }

                switch (info.区画状態)
                {
                    case "空":
                        existingCemeteryInfo.SectionStatus = 0;
                        break;
                    case "拠点予約":
                        existingCemeteryInfo.SectionStatus = 2;
                        break;
                    case "成約":
                        existingCemeteryInfo.SectionStatus = 3;
                        break;
                    default:
                        return BadRequest($"Invalid value for 区画状態: {info.区画状態}. Expected value");
                }
                existingCemeteryInfo.UpdateDate = DateTime.UtcNow;
                existingCemeteryInfo.UpdateUser = null;
                existingCemeteryInfo.DeleteFlag = 0;
                _context.SaveChanges();
            }

            // ここでリクエストを処理し、レスポンスを作成します。
            var response = new
            {
                success = true,
                message = "Reyasiro情報の設定に成功しました。"
            };
            return Ok(response);
        }
    }

    public class SetGGraveStatus
    {
        public string 霊園番号 { get; set; } = string.Empty;
        public string 工区番号 { get; set; } = string.Empty;
        public string 区画番号 { get; set; } = string.Empty;
        public string 区画状態 { get; set; } = string.Empty;
    }
}
