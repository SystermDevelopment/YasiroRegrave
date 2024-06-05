using Microsoft.AspNetCore.Mvc;
using YasiroRegrave.Data;
using YasiroRegrave.Model;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetGraveInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SetGraveInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SetGraveInfo([FromBody] GraveInfoRequest request)
        {
            Reien? existingReien;
            Area? existingArea;
            Section? existingSection;
            Cemetery? existingCemetery;
            foreach (var info in request.Data)
            {
                // 霊園
                var reienCode = info.霊園番号;
                existingReien = _context.Reiens.Where(r => r.DeleteFlag == 0 && r.ReienCode == reienCode).FirstOrDefault();
                if (existingReien == null)
                {
                    // 当初は霊園番号がないことはないのでエラーとする
                    return BadRequest($"Invalid value for 霊園番号: {info.霊園番号}. Expected value");
                }

                var parts = info.区画番号.Split('-');
                if (parts.Length != 3)
                {
                    return BadRequest($"Invalid format for 区画番号: {info.区画番号}. Expected format: xx-xx-xx");
                }
                string section = parts[0];
                string cemetery = parts[1] + "-" + parts[2];

                // エリア(工区)
                existingArea = _context.Areas.Where(r => r.DeleteFlag == 0 && r.AreaCode == info.工区番号).FirstOrDefault();
                if (existingArea == null)
                {
                    // INSERT
                    var newArea = new Area
                    {
                        ReienIndex = existingReien.Index,
                        AreaCode = info.工区番号,
                        AreaName = info.工区名,
                        CreateDate = DateTime.UtcNow,
                        CreateUser = null,
                        UpdateDate = DateTime.UtcNow,
                        UpdateUser = null,
                        DeleteFlag = 0,
                        Reien = existingReien
                    };
                    _context.Areas.Add(newArea);
                    _context.SaveChanges();
                existingArea = newArea;
                }
                else
                {
                    // UPDATE
                    existingArea.AreaName = info.工区名;
                    existingArea.UpdateDate = DateTime.UtcNow;
                    existingArea.UpdateUser = null;
                    existingArea.DeleteFlag = 0;
                    _context.SaveChanges();
                }
                // 区画
                existingSection = _context.Sections.Where(r => r.DeleteFlag == 0 && r.SectionCode == section).FirstOrDefault();
                if (existingSection == null)
                {
                    // INSERT
                    var newSection = new Section
                    {
                        AreaIndex = existingArea.AreaIndex,
                        SectionCode = section,
                        SectionName = section, //TODO:本来のNameをもらったら修正すること
                        CreateDate = DateTime.UtcNow,
                        CreateUser = null,
                        UpdateDate = DateTime.UtcNow,
                        UpdateUser = null,
                        DeleteFlag = 0,
                        Area = existingArea,
                    };
                    _context.Sections.Add(newSection);
                    _context.SaveChanges();
                    existingSection = newSection;
                }
                else
                {
                    // UPDATE
                    existingSection.SectionName = section; //TODO:本来のNameをもらったら修正すること
                    existingSection.UpdateDate = DateTime.UtcNow;
                    existingSection.UpdateUser = null;
                    existingSection.DeleteFlag = 0;
                    _context.SaveChanges();
                }
                // 墓所
                existingCemetery = _context.Cemeteries.Where(r => r.DeleteFlag == 0 && r.CemeteryCode == cemetery).FirstOrDefault();
                if (existingCemetery == null)
                {
                    // INSERT
                    var newCemetery = new Cemetery
                    {
                        SectionIndex = existingSection.SectionIndex,
                        CemeteryCode = cemetery,
                        CemeteryName = cemetery, //TODO:本来のNameをもらったら修正すること
                        CreateDate = DateTime.UtcNow,
                        CreateUser = null,
                        UpdateDate = DateTime.UtcNow,
                        UpdateUser = null,
                        DeleteFlag = 0,
                        Section = existingSection,
                    };
                    _context.Cemeteries.Add(newCemetery);
                    _context.SaveChanges();
                    existingCemetery = newCemetery;
                }
                else
                {
                    // UPDATE
                    existingCemetery.CemeteryName = cemetery; //TODO:本来のNameをもらったら修正すること
                    existingCemetery.UpdateDate = DateTime.UtcNow;
                    existingCemetery.UpdateUser = null;
                    existingCemetery.DeleteFlag = 0;
                    _context.SaveChanges();
                }
                // 墓所情報
                var existingCemeteryInfo = _context.CemeteryInfos.Where(r => r.DeleteFlag == 0 && r.CemeteryIndex == existingCemetery.CemeteryIndex).FirstOrDefault();
                if (existingCemeteryInfo == null)
                {
                    // INSERT
                    var newCemeteryInfo = new CemeteryInfo
                    {
                        CemeteryIndex = existingCemetery.CemeteryIndex,
                        AreaValue = info.面積,
                        ReleaseStatus = 0,
                        SectionStatus = 0,
                        CreateDate = DateTime.UtcNow,
                        CreateUser = null,
                        UpdateDate = DateTime.UtcNow,
                        UpdateUser = null,
                        DeleteFlag = 0,
                        Cemetery = existingCemetery
                    };
                    _context.CemeteryInfos.Add(newCemeteryInfo);
                    _context.SaveChanges();
                    existingCemeteryInfo = newCemeteryInfo;
                }
                else
                {
                    // UPDATE
                    existingCemeteryInfo.AreaValue = info.面積;
                    existingCemeteryInfo.SectionType = info.区画区分;
                    existingCemeteryInfo.UsageFee = info.使用料;
                    existingCemeteryInfo.ManagementFee = info.管理料;
                    existingCemeteryInfo.StoneFee = info.仕置巻石料;
                    existingCemeteryInfo.SetPrice = info.墓石セット価格;
                    existingCemeteryInfo.UpdateDate = DateTime.UtcNow;
                    existingCemeteryInfo.UpdateUser = null;
                    existingCemeteryInfo.DeleteFlag = 0;
                    _context.SaveChanges();
                }
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
    public class GraveInfoRequest
    {
        public List<SetGraveInfo> Data { get; set; } = new List<SetGraveInfo>();
    }

    public class SetGraveInfo
    {
        public string 霊園番号 { get; set; } = string.Empty;
        public string 工区番号 { get; set; } = string.Empty;
        public string 工区名 { get; set; } = string.Empty;
        public string 区画番号 { get; set; } = string.Empty;
        public string? 面積 { get; set; }
        public string? 区画区分 { get; set; }
        public string? 使用料 { get; set; }
        public string? 管理料 { get; set; }
        public string? 仕置巻石料 { get; set; }
        public string? 墓石セット価格 { get; set; }
    }
}
