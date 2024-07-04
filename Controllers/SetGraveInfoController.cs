using Microsoft.AspNetCore.Mvc;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using System.Linq;
using YasiroRegrave.Pages.common;

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
        public IActionResult SetGraveInfo([FromBody] List<SetGraveInfo> infos)
        {
            foreach (var info in infos)
            {
                Reien? existingReien;
                Area? existingArea;
                Section? existingSection;
                Cemetery? existingCemetery;

                // 霊園
                var reienCode = info.霊園番号;
                existingReien = _context.Reiens.FirstOrDefault(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.ReienCode == reienCode);
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
                existingArea = _context.Areas.FirstOrDefault(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.ReienIndex == existingReien.ReienIndex && r.AreaCode == info.工区番号);
                if (existingArea == null)
                {
                    // INSERT
                    var newArea = new Area
                    {
                        ReienIndex = existingReien.ReienIndex,
                        AreaCode = info.工区番号,
                        AreaName = info.工区名,
                        CreateDate = DateTime.Now,
                        CreateUser = null,
                        UpdateDate = DateTime.Now,
                        UpdateUser = null,
                        DeleteFlag = (int)Config.DeleteType.未削除,
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
                    existingArea.UpdateDate = DateTime.Now;
                    existingArea.UpdateUser = null;
                    existingArea.DeleteFlag = (int)Config.DeleteType.未削除;
                    _context.SaveChanges();
                }

                // 区画
                existingSection = _context.Sections.FirstOrDefault(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.AreaIndex == existingArea.AreaIndex && r.SectionCode == section);
                if (existingSection == null)
                {
                    // INSERT
                    var newSection = new Section
                    {
                        AreaIndex = existingArea.AreaIndex,
                        SectionCode = section,
                        SectionName = section, // 未使用
                        CreateDate = DateTime.Now,
                        CreateUser = null,
                        UpdateDate = DateTime.Now,
                        UpdateUser = null,
                        DeleteFlag = (int)Config.DeleteType.未削除,
                        Area = existingArea,
                    };
                    _context.Sections.Add(newSection);
                    _context.SaveChanges();
                    existingSection = newSection;
                }
                else
                {
                    // UPDATE
                    existingSection.SectionName = section; // 未使用
                    existingSection.UpdateDate = DateTime.Now;
                    existingSection.UpdateUser = null;
                    existingSection.DeleteFlag = (int)Config.DeleteType.未削除;
                    _context.SaveChanges();
                }

                // 墓所
                existingCemetery = _context.Cemeteries.FirstOrDefault(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.SectionIndex == existingSection.SectionIndex && r.CemeteryCode == cemetery);
                if (existingCemetery == null)
                {
                    // INSERT
                    var newCemetery = new Cemetery
                    {
                        SectionIndex = existingSection.SectionIndex,
                        CemeteryCode = cemetery,
                        CemeteryName = cemetery, // 未使用
                        CreateDate = DateTime.Now,
                        CreateUser = null,
                        UpdateDate = DateTime.Now,
                        UpdateUser = null,
                        DeleteFlag = (int)Config.DeleteType.未削除,
                        Section = existingSection,
                    };
                    _context.Cemeteries.Add(newCemetery);
                    _context.SaveChanges();
                    existingCemetery = newCemetery;
                }
                else
                {
                    // UPDATE
                    existingCemetery.CemeteryName = cemetery; // 未使用
                    existingCemetery.UpdateDate = DateTime.Now;
                    existingCemetery.UpdateUser = null;
                    existingCemetery.DeleteFlag = (int)Config.DeleteType.未削除;
                    _context.SaveChanges();
                }

                // 墓所情報
                var existingCemeteryInfo = _context.CemeteryInfos.FirstOrDefault(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.CemeteryIndex == existingCemetery.CemeteryIndex);
                if (existingCemeteryInfo == null)
                {
                    // INSERT
                    var newCemeteryInfo = new CemeteryInfo
                    {
                        CemeteryIndex = existingCemetery.CemeteryIndex,
                        ReleaseStatus = (int)Config.ReleaseStatusType.準備中,
                        SectionStatus = (int)Config.SectionStatusType.空,
                        SectionType = info.区画区分,
                        AreaValue = info.面積,
                        UsageFee = info.使用料,
                        ManagementFee = info.管理料,
                        StoneFee = info.仕置巻石料,
                        SetPrice = info.墓石セット価格,
                        CreateDate = DateTime.Now,
                        CreateUser = null,
                        UpdateDate = DateTime.Now,
                        UpdateUser = null,
                        DeleteFlag = (int)Config.DeleteType.未削除,
                        Cemetery = existingCemetery
                    };
                    _context.CemeteryInfos.Add(newCemeteryInfo);
                    _context.SaveChanges();
                    existingCemeteryInfo = newCemeteryInfo;
                }
                else
                {
                    // 画像登録済
                    var releaseStatus = (int)Config.ReleaseStatusType.準備中;
                    if (!string.IsNullOrEmpty(existingCemeteryInfo.Image1Fname) && !string.IsNullOrEmpty(existingCemeteryInfo.Image2Fname))
                    {
                        // 価格設定済
                        decimal usageFee = 0;
                        decimal managFee = 0;
                        decimal stoneFee = 0;
                        decimal setPrice = 0;
                        decimal.TryParse(info.使用料, out usageFee);
                        decimal.TryParse(info.管理料, out managFee);
                        decimal.TryParse(info.仕置巻石料, out stoneFee);
                        decimal.TryParse(info.墓石セット価格, out setPrice);
                        decimal totalPrice = usageFee + managFee + stoneFee + setPrice;
                        if (totalPrice > 0)
                        {
                            releaseStatus = (int)Config.ReleaseStatusType.販売中;
                        }
                    }

                    // UPDATE
                    existingCemeteryInfo.ReleaseStatus = releaseStatus;
                    existingCemeteryInfo.SectionType = info.区画区分;
                    existingCemeteryInfo.AreaValue = info.面積;
                    existingCemeteryInfo.UsageFee = info.使用料;
                    existingCemeteryInfo.ManagementFee = info.管理料;
                    existingCemeteryInfo.StoneFee = info.仕置巻石料;
                    existingCemeteryInfo.SetPrice = info.墓石セット価格;
                    existingCemeteryInfo.UpdateDate = DateTime.Now;
                    existingCemeteryInfo.UpdateUser = null;
                    existingCemeteryInfo.DeleteFlag = (int)Config.DeleteType.未削除;
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
