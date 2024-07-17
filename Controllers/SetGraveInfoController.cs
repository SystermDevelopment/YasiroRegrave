using Microsoft.AspNetCore.Mvc;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using System.Linq;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.ReienListModel;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
using System.Net;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetGraveInfoController : Controller
    {
        private List<Cemetery> cemeteries;
        private string JsonFilePath = "";    //jsonのファイルパス
        private readonly ApplicationDbContext _context;
        public SetGraveInfoController(ApplicationDbContext context)
        {
            _context = context;
            JsonFilePath = Path.Combine(Config.JsonDataFilesPath, ""); //ファイル名はIDで格納
        }

        [HttpPost]
        public IActionResult SetGraveInfo([FromBody] List<SetGraveInfo> infos)
        {
            foreach (var info in infos)
            {
                YasiroRegrave.Model.Reien? existingReien;
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

                // 区画区分の確認
                if (info.区画区分 != "新規" && info.区画区分 != "再販")
                {
                    return BadRequest($"Invalid value for 区画区分: {info.区画区分}. Expected value");
                }

                // 価格の確認（使用料、管理料、仕置巻石料、墓石セット価格）
                if (!decimal.TryParse(info.使用料, out _))
                {
                    return BadRequest($"Invalid value for 使用料: {info.使用料}. Expected numeric value.");
                }
                if (!decimal.TryParse(info.管理料, out _))
                {
                    return BadRequest($"Invalid value for 管理料: {info.管理料}. Expected numeric value.");
                }
                if (!decimal.TryParse(info.仕置巻石料, out _))
                {
                    return BadRequest($"Invalid value for 仕置巻石料: {info.仕置巻石料}. Expected numeric value.");
                }
                if (!decimal.TryParse(info.墓石セット価格, out _))
                {
                    return BadRequest($"Invalid value for 墓石セット価格: {info.墓石セット価格}. Expected numeric value.");
                }

                // エリア(工区)
                existingArea = _context.Areas.FirstOrDefault(r => r.DeleteFlag == (int)Config.DeleteType.未削除 && r.ReienIndex == existingReien.ReienIndex && r.AreaCode == info.工区番号);
                if (existingArea == null)
                {
                    return BadRequest($"Invalid value for 工区番号: {info.工区番号}. Expected value");
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
                        ChangeStatusDate = DateTime.Now,
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

                    if (existingCemeteryInfo.ReleaseStatus != releaseStatus || existingCemeteryInfo.SectionType == null || existingCemeteryInfo.SectionType != info.区画区分)
                    {
                        existingCemeteryInfo.ChangeStatusDate = DateTime.Now;
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

                var JsonPath = $"{JsonFilePath}\\SEC_{reienCode}_{info.工区番号}_{section}.json";
                bool exists = System.IO.File.Exists(JsonPath);
                if(exists)
                {
                    string json = System.IO.File.ReadAllText(JsonPath);
                    cemeteries = JsonConvert.DeserializeObject<List<Cemetery>>(json);
                    exists = cemeteries.Any(c => c.CemeteryCode == cemetery);
                }
                if (!exists)
                {
                    var smtp = new SmtpClient
                    {
                        Host = Config.SMTPHost,
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(Config.SMTPId, Config.SMTPPass),
                        Timeout = 20000
                    };
                    var fromAddress = new MailAddress(Config.SendMailAddress, Config.SendMailName);
                    var toAddress = new MailAddress($"{Config.AlartNotificationMailAddress}", $"");
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = "<再販webシステム　アラートメール>",
                        Body = $"区画番号「{info.区画番号}」のjsonファイルが存在しません。",
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8
                    })
                    {
                        smtp.Send(message);
                    }
                    var toAddress2 = new MailAddress($"{Config.AlartNotificationMailAddressTechno}", $"");
                    using (var message = new MailMessage(fromAddress, toAddress2)
                    {
                        Subject = "<再販webシステム　アラートメール>",
                        Body = $"区画番号「{info.区画番号}」のjsonファイルが存在しません。",
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8
                    })
                    {
                        smtp.Send(message);
                    }
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
        private class Coordinate
        {
            public int x { get; set; }
            public int y { get; set; }
        }
        private class CemeteryJson
        {
            public string? CemeteryCode { get; set; }
            public List<List<Coordinate>>? Coordinates { get; set; }
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
