using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Data;

namespace YasiroRegrave.Pages.common
{
    public class Utils
    {
        /// <summary>
        /// 区画コードを区画名に変換
        /// </summary>
        /// <param name="src"></param>
        /// <returns>string</returns>
        public static string SectionCode2Name(string src)
        {
            string dst = src;

            // 置換
            dst = dst.Replace("日", "日蓮");
            dst = dst.Replace("親", "親鸞");
            dst = dst.Replace("釈", "釈尊");
            dst = dst.Replace("ｼﾝ", "新");
            dst = dst.Replace("ﾄｸ", "特");

            // 接尾語（付与可）
            //if (dst != "日蓮" && dst != "緑風")
            {
                dst = dst + "区";
            }

            return dst;
        }

        /// <summary>
        /// 墓所コードを墓所名に変換
        /// </summary>
        /// <param name="src"></param>
        /// <returns>string</returns>
        public static string CemeteryCode2Name(string src)
        {
            string dst = src;

            // 置換
            if (!dst.Contains("㎡"))
            {
                dst = dst.Replace("-", "列-");
            }

            return dst;
        }

        /// <summary>
        /// 墓所コードを墓所表示名に変換
        /// </summary>
        /// <param name="src"></param>
        /// <returns>string</returns>
        public static string CemeteryCode2Disp(string src)
        {
            string dst = src;

            // 番号を取得
            string[] parts = dst.Split('-');
            if (parts.Length > 1)
            {
                dst = parts[1];
            }
            // 置換
            if (!dst.Contains("㎡"))
            {
                dst = dst.Replace("㎡", "");
            }

            // ０埋め除去
            dst = dst.TrimStart('0');

            return dst;
        }

        /// <summary>
        /// stringをintに変換
        /// </summary>
        /// <param name="src"></param>
        /// <returns>int</returns>
        public static int StringToInt(string? src)
        {
            if (int.TryParse(src, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// メールアドレス形式チェック
        /// </summary>
        /// <param name="src"></param>
        /// <returns>int</returns>
        public static bool IsValidMailAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }
            try
            {
                var mailAddress = new MailAddress(address);
                //System.Net.Mail.MailAddress a =
                //    new System.Net.Mail.MailAddress(address);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 画像を縮小
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns>IFormFile</returns>
        public static IFormFile ResizeImage(IFormFile imageFile) 
        {
            // 画像サイズ閾値：500px
            var MAX_SIZE = 500;

            var fileName = imageFile.FileName;

            using (var stream = imageFile.OpenReadStream())
            {
                using (var image = System.Drawing.Image.FromStream(stream))
                {
                    // 画像サイズが閾値を超えない場合、元の画像をそのまま返す
                    if (image.Width <= MAX_SIZE && image.Height <= MAX_SIZE)
                    {
                        return imageFile;
                    }

                    // 元のアスペクト比を保持する
                    var ratioX = (double)MAX_SIZE / image.Width;
                    var ratioY = (double)MAX_SIZE / image.Height;
                    var ratio = Math.Min(ratioX, ratioY);

                    var newWidth = (int)(image.Width * ratio);
                    var newHeight = (int)(image.Height * ratio);

                    var resizedImage = new Bitmap(newWidth, newHeight);

                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                    }

                    var resizedStream = new MemoryStream();
                    // 画像のフォーマットに応じて保存
                    resizedImage.Save(resizedStream, image.RawFormat);
                    resizedStream.Seek(0, SeekOrigin.Begin);

                    return new FormFile(resizedStream, 0, resizedStream.Length, imageFile.Name, fileName);
                }
            }
        }
        public static class EmailHelper
        {
            public static (string Subject, string Body) ParseEmailTemplate(string templatePath, object model)
            {
                string template = File.ReadAllText(templatePath, System.Text.Encoding.UTF8);

                // 件名を取得
                var lines = template.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                string subjectLine = lines.FirstOrDefault(line => line.StartsWith("件名:"));
                string subject = subjectLine != null ? subjectLine.Replace("件名:", "").Trim() : "No Subject";

                // 本文を取得
                string body = string.Join("\n", lines.SkipWhile(line => !line.StartsWith("件名:")).Skip(1));

                foreach (var prop in model.GetType().GetProperties())
                {
                    body = body.Replace($"{{{prop.Name}}}", prop.GetValue(model, null)?.ToString());
                    subject = subject.Replace($"{{{prop.Name}}}", prop.GetValue(model, null)?.ToString());
                }
                var reserveModeProp = model.GetType().GetProperty("ReserveName");
                string reserveMode = reserveModeProp.GetValue(model, null)?.ToString();

                // 仮予約の場合は希望日時部分を削除
                if (reserveMode == "仮予約")
                {
                    body = RemoveDateSection(body);
                }
                string startTag = "<!-- Start 希望日時 -->";
                string endTag = "<!-- End 希望日時 -->";
                body = body.Replace(startTag, "").Replace(endTag, "");

                return (subject, body);
            }
        }
        private static string RemoveDateSection(string body)
        {
            string startTag = "<!-- Start 希望日時 -->";
            string endTag = "<!-- End 希望日時 -->";

            int startIndex = body.IndexOf(startTag);
            int endIndex = body.IndexOf(endTag) + endTag.Length;

            if (startIndex != -1 && endIndex != -1)
            {
                // 開始タグの前の改行も削除
                int lineStartIndex = body.LastIndexOf('\n', startIndex) + 1;
                // 終了タグの後の改行も削除
                int lineEndIndex = body.IndexOf('\n', endIndex);

                if (lineStartIndex > 0 && lineEndIndex > 0)
                {
                    body = body.Remove(lineStartIndex, lineEndIndex - lineStartIndex + 1);
                }
                else
                {
                    body = body.Remove(startIndex, endIndex - startIndex);
                }
            }

            return body;
        }
        /// <summary>
        /// ログインユーザー情報を取得
        /// </summary>
        public static LoginUserData? GetLoggedInUser(ApplicationDbContext context, int? loginId)
        {
            if (!loginId.HasValue)
            {
                return null;
            }

            return context.Users
                .Include(u => u.Vender)
                .Where(u => u.UserIndex == loginId.Value && u.DeleteFlag == (int)Config.DeleteType.未削除)
                .Select(u => new LoginUserData
                {
                    Index = u.UserIndex,
                    Id = u.Id,
                    Authority = u.Authority,
                    Name = u.Name,
                    VenderIndex = u.VenderIndex,
                    VenderName = u.Vender.Name,
                    Password = u.Password,
                })
                .FirstOrDefault();
        }
    }

    public class LoginUserData
    {
        public int Index { get; set; }
        public string Id { get; set; } = string.Empty;
        public int Authority { get; set; }
        public string Name { get; set; } = string.Empty;
        public int VenderIndex { get; set; }
        public string VenderName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
