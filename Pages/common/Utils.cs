using System.Drawing.Imaging;
using System.Drawing;

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
    }
}
