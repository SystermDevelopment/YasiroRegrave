using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;
using static YasiroRegrave.Pages.common.Config;
using static YasiroRegrave.Pages.common.Utils;
using System.IO;

namespace YasiroRegrave.Pages
{
    public class ReserveConfirmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReserveConfirmModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int? LoginId { get; private set; }
        public LoginUserData? LoggedInUser { get; private set; }

        [BindProperty]
        public string CemeteryName { get; set; } = "";
        public string SectionNumber { get; set; } = "";
        [BindProperty]
        public int CemeteryIndex { get; set; } = 0;
        [BindProperty]
        public int ReserveMode { get; set; } = (int)Config.ReserveType.見学予約;
        [BindProperty]
        public string ReserveName { get; set; } = "";

        [BindProperty]
        public string LastName { get; set; } = "";
        [BindProperty]
        public string FirstName { get; set; } = "";
        [BindProperty]
        public string LastNameKana { get; set; } = "";
        [BindProperty]
        public string FirstNameKana { get; set; } = "";
        [BindProperty]
        public string PostalCode { get; set; } = "";
        [BindProperty]
        public string Prefecture { get; set; } = "";
        [BindProperty]
        public string City { get; set; } = "";
        [BindProperty]
        public string Address { get; set; } = "";
        [BindProperty]
        public string Building { get; set; } = "";
        [BindProperty]
        public string Phone { get; set; } = "";
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string Inquiry { get; set; } = "";
        [BindProperty]
        public string Date1 { get; set; } = "";
        [BindProperty]
        public string Time1 { get; set; } = "";
        [BindProperty]
        public string Date2 { get; set; } = "";
        [BindProperty]
        public string Time2 { get; set; } = "";
        [BindProperty]
        public string Date3 { get; set; } = "";
        [BindProperty]
        public string Time3 { get; set; } = "";
        [BindProperty]
        public string IsContactByPhone { get; set; } = "";
        [BindProperty]
        public string IsContactByEmail { get; set; } = "";
        [BindProperty]
        public string Subscription { get; set; } = "";


        /// <summary>
        /// OnGet処理
        /// </summary>
        /// <param</param>
        /// <returns></returns>
        public void OnGet()
        {
            if (!int.TryParse(TempData["CemeteryIndex"]?.ToString(), out int index)) { index = -1; }
            if (!int.TryParse(TempData["ReserveMode"]?.ToString(), out int mode)) { mode = -1; }
            CemeteryIndex = index;
            ReserveMode = mode;
            ReserveName = mode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();

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
            LoginId = HttpContext.Session.GetInt32("LoginId");

            ReserveName = ReserveMode == (int)Config.ReserveType.見学予約 ? Config.ReserveType.見学予約.ToString() : Config.ReserveType.仮予約.ToString();

            var cemeteryInfo = _context.CemeteryInfos
                .FirstOrDefault(ci => ci.CemeteryIndex == CemeteryIndex && ci.DeleteFlag == (int)Config.DeleteType.未削除);
            User? user = null;
            if (LoginId != null)
            {
                user = _context.Users
                    .Include(u => u.Vender)
                    .FirstOrDefault(u => u.UserIndex == LoginId && u.DeleteFlag == (int)Config.DeleteType.未削除);
            }
            DateTime ReceptionHours = DateTime.Now;
            var reserveInfo = new ReserveInfo
            {
                CemeteryInfoIndex = CemeteryIndex,
                PreferredDate1 = DateTime.TryParse(Date1, out var date1) ? date1 : (DateTime?)null,
                PreferredDate2 = DateTime.TryParse(Date2, out var date2) ? date2 : (DateTime?)null,
                PreferredDate3 = DateTime.TryParse(Date3, out var date3) ? date3 : (DateTime?)null,
                LastName = LastName,
                FirstName = FirstName,
                LastNameYomi = LastNameKana,
                FirstNameYomi = FirstNameKana,
                ZipCode = PostalCode,
                Adress = Address,
                TelephoneNumber = Phone,
                EMail = Email,
                Question = Inquiry,
                AreaValue = cemeteryInfo.AreaValue,
                UsageFee = cemeteryInfo.UsageFee,
                ManagementFee = cemeteryInfo.ManagementFee,
                StoneFee = cemeteryInfo.StoneFee,
                SetPrice = cemeteryInfo.SetPrice,
                VenderIndex = user?.VenderIndex,
                Notification = (int)Config.NotificationType.未通知,
                CreateDate = ReceptionHours,
                CreateUser = LoginId,
                CemeteryInfo = cemeteryInfo
            };

            try
            {
                _context.ReserveInfos.Add(reserveInfo);
                _context.SaveChanges();
                cemeteryInfo.SectionStatus = (int)SectionStatusType.WEB予約;
                cemeteryInfo.UpdateDate = DateTime.Now;
                cemeteryInfo.UpdateUser = LoginId;
                _context.CemeteryInfos.Update(cemeteryInfo);
                _context.SaveChanges();

                var fromAddress = new MailAddress(Config.SendMailAddress, Config.SendMailName);

                string templatePath;
                string templateReienPath;

                // 一般ユーザー
                if (LoginId == null)
                {
                    templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Data", "EMAIL_一般.txt");
                    templateReienPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Data", "EMAIL_一般_霊園.txt");
                }
                // 販売会社
                else
                {
                    templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Data", "EMAIL_販社.txt");
                    templateReienPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Data", "EMAIL_販社_霊園.txt");
                }

                var reienData = _context.Cemeteries
                    .Include(s => s.Section)
                        .ThenInclude(s => s.Area)
                        .ThenInclude(a => a.Reien)
                    .Where(ci => ci.CemeteryIndex == CemeteryIndex && ci.DeleteFlag == (int)Config.DeleteType.未削除)
                    .Where(ci => ci.Section.DeleteFlag == (int)Config.DeleteType.未削除)
                    .Where(ci => ci.Section.Area.DeleteFlag == (int)Config.DeleteType.未削除)
                    .Where(ci => ci.Section.Area.Reien.DeleteFlag == (int)Config.DeleteType.未削除)
                    .Select(ci => new
                    {
                        ReienName = ci.Section.Area.Reien.ReienName,
                        ReienMail = ci.Section.Area.Reien.MailAddress,
                        ReienCode = ci.Section.Area.Reien.ReienCode,
                    })
                    .FirstOrDefault();

                string IsContact = "";
                if (IsContactByPhone == "1" && IsContactByEmail == "1")
                {
                    IsContact = "電話またはメール";
                }
                else if (IsContactByPhone == "1")
                {
                    IsContact = "電話";
                }
                else if (IsContactByEmail == "1")
                {
                    IsContact = "メール";
                }

                var emailContent = EmailHelper.ParseEmailTemplate(templatePath, new
                {
                    CemeteryName,    
                    VenderName = user?.Vender.Name ?? "",
                    UserName = user?.Name ?? "",
                    ReienName = reienData?.ReienName ?? "",
                    ReserveName,
                    LastName,
                    FirstName,
                    SectionNumber,
                    LastNameKana,
                    FirstNameKana,
                    PostalCode,
                    Prefecture,
                    City,
                    Address,
                    Building,
                    Phone,
                    Email,
                    Date1,
                    Time1,
                    Date2,
                    Time2,
                    Date3,
                    Time3,
                    Inquiry,
                    IsContact,
                    Subscription = Subscription == "1" ? "受信する" : "受信しない"
                });

                var emailReienContent = EmailHelper.ParseEmailTemplate(templateReienPath, new
                {
                    CemeteryName,
                    VenderName = user?.Vender.Name ?? "",
                    UserName = user?.Name ?? "",
                    ReienName = reienData?.ReienName ?? "",
                    ReceptionDate = ReceptionHours.ToString("yyyy/MM/dd"),
                    ReceptionTime = ReceptionHours.ToString("HH:mm"),
                    ReserveName,
                    LastName,
                    FirstName,
                    SectionNumber,
                    LastNameKana,
                    FirstNameKana,
                    PostalCode,
                    Prefecture,
                    City,
                    Address,
                    Building,
                    Phone,
                    Email,
                    Date1,
                    Time1,
                    Date2,
                    Time2,
                    Date3,
                    Time3,
                    Inquiry,
                    IsContact,
                    Subscription = Subscription == "1" ? "受信する" : "受信しない"
                });

                string signaturePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Data", $"SIGNATURE_{reienData?.ReienCode}.txt");
                string signatureText = System.IO.File.Exists(signaturePath) ? System.IO.File.ReadAllText(signaturePath, System.Text.Encoding.UTF8) : "";


                // 署名をメール本文に追加
                emailContent.Body += signatureText;


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

                if (LoginId == null || (user != null && user.VenderIndex == 0))
                {
                    //一般ユーザー
                    var toAddress = new MailAddress($"{Email}", $"{LastName} {FirstName} 様");
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = emailContent.Subject,
                        Body = emailContent.Body,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8
                    })
                    {
                        smtp.Send(message);
                    }
                }
                else if (user != null)
                {
                    //担当者
                    var toAddress = new MailAddress($"{user.MailAddress}", $"{user.Name} 様");
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = emailContent.Subject,
                        Body = emailContent.Body,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8
                    })
                    {
                        smtp.Send(message);
                    }
                }
                if (reienData != null && reienData.ReienMail != null)
                {
                    string[] emailAddresses = reienData.ReienMail.Split(',');
                    var toAddresses = new MailAddressCollection();

                    foreach (string emailAddress in emailAddresses)
                    {
                        toAddresses.Add(new MailAddress(emailAddress.Trim()));
                    }

                    using (var message = new MailMessage()
                    {
                        From = fromAddress,
                        Subject = emailReienContent.Subject,
                        Body = emailReienContent.Body,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8
                    })
                    {
                        foreach (var address in toAddresses)
                        {
                            message.To.Add(address);
                        }
                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your reservation.");
            }
            return RedirectToPage("ReserveComplate");
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
            }
            // 区画番号
            var cemeteryData = _context.Cemeteries
                .Where(ci => ci.CemeteryIndex == CemeteryIndex && ci.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.DeleteFlag == (int)Config.DeleteType.未削除)
                .Where(ci => ci.Section.Area.Reien.DeleteFlag == (int)Config.DeleteType.未削除)
                .Select(ci => new
                {
                    SectionNumber = Utils.SectionCode2Name(ci.Section.SectionCode) + " " + Utils.CemeteryCode2Name(ci.CemeteryCode),
                })
                .FirstOrDefault();

            if (cemeteryData != null)
            {
                SectionNumber = cemeteryData.SectionNumber ?? "";
            }

            // 入力項目
            CemeteryName = TempData["CemeteryName"]?.ToString() ?? "";
            LastName = TempData["LastName"]?.ToString() ?? "";
            FirstName = TempData["FirstName"]?.ToString() ?? "";
            LastNameKana = TempData["LastNameKana"]?.ToString() ?? "";
            FirstNameKana = TempData["FirstNameKana"]?.ToString() ?? "";
            PostalCode = TempData["PostalCode"]?.ToString() ?? "";
            Prefecture = TempData["Prefecture"]?.ToString() ?? "";
            City = TempData["City"]?.ToString() ?? "";
            Address = TempData["Address"]?.ToString() ?? "";
            Building = TempData["Building"]?.ToString() ?? "";
            Phone = TempData["Phone"]?.ToString() ?? "";
            Email = TempData["Email"]?.ToString() ?? "";
            Date1 = (TempData["Date1"]?.ToString() ?? "").Replace(" 0:00:00", "");
            Time1 = TempData["Time1"]?.ToString() ?? "";
            Date2 = (TempData["Date2"]?.ToString() ?? "").Replace(" 0:00:00", "");
            Time2 = TempData["Time2"]?.ToString() ?? "";
            Date3 = (TempData["Date3"]?.ToString() ?? "").Replace(" 0:00:00", "");
            Time3 = TempData["Time3"]?.ToString() ?? "";
            Inquiry = TempData["Inquiry"]?.ToString() ?? "";
            IsContactByPhone = TempData["IsContactByPhone"]?.ToString() ?? "";
            IsContactByEmail = TempData["IsContactByEmail"]?.ToString() ?? "";
            Subscription = TempData["Subscription"]?.ToString() ?? "";


            //// テストータ
            //SectionNumber = "親鸞E区 07列-03";
            //LastName = "生駒";
            //FirstName = "太郎";
            //LastNameKana = "いこま";
            //FirstNameKana = "たろう";
            //PostalCode = "5750014";
            //Prefecture = "大阪府";
            //City = "四條畷市";
            //Address = "上田原";
            //Building = "1366番地";
            //Phone = "0120753948";
            //Email = "yoyaku@yashiro.co.jp";
            //Date1 = "2024/07/01";
            //Time1 = "11:00";
            //Date2 = "2024/07/02";
            //Time2 = "12:00";
            //Date3 = "2024/07/03";
            //Time3 = "13:00";
            //Inquiry = "ああああああああああ";
            //IsContactByPhone = "1";
            //IsContactByEmail = "1";
            //Subscription = "1";

            return;
        }
    }
}
