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

            // 接尾語
            if (dst != "日蓮" && dst != "緑風")
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

        ///// <summary>
        ///// 画像を圧縮
        ///// </summary>
        ///// <param name="imageFile"></param>
        ///// <returns>IFormFile</returns>
        //public static IFormFile CompressImage(IFormFile imageFile)
        //{
        //    // 画像サイズ閾値：700KB
        //    var MAX_SIZE_BYTES = 700 * 1024;

        //    // 画像の品質を0から100の間で制限（元画像／閾値の30％）
        //    var quality = Math.Min(100, Math.Max(0, MAX_SIZE_BYTES * 30 / imageFile.Length));

        //    var fileName = imageFile.FileName;

        //    // JPEG形式またはPNG形式の画像を処理
        //    if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
        //    {
        //        return imageFile;
        //    }

        //    // 画像サイズが閾値を超える場合、画像を圧縮
        //    if (imageFile.Length >= MAX_SIZE_BYTES)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            // 画像をメモリストリームにコピー
        //            imageFile.CopyTo(memoryStream);
        //            memoryStream.Position = 0; // ストリームの位置をリセット

        //            // メモリストリームで画像の圧縮を行う
        //            using (var image = Image.FromStream(memoryStream))
        //            {
        //                // 圧縮した画像を保存するメモリストリーム
        //                var compressedStream = new MemoryStream();

        //                // 画像のエンコーダー情報を取得
        //                var format = imageFile.ContentType == "image/jpeg" ? ImageFormat.Jpeg : ImageFormat.Png;
        //                var codecInfo = GetEncoderInfo(format);
        //                if (codecInfo == null)
        //                {
        //                    return imageFile;
        //                }

        //                var encoder = System.Drawing.Imaging.Encoder.Quality;
        //                var encoderParameters = new EncoderParameters(1);
        //                encoderParameters.Param[0] = new EncoderParameter(encoder, quality);

        //                // 画像を圧縮して保存
        //                image.Save(compressedStream, codecInfo, encoderParameters);

        //                // 圧縮された画像のファイル名を生成
        //                var compressedFileName = fileName;

        //                // 圧縮された画像を返す
        //                return new FormFile(compressedStream, 0, compressedStream.Length, "compressedImage", compressedFileName);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // 画像変更なし
        //        return imageFile;
        //    }
        //}

        //// 画像のエンコーダー情報を取得
        //private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        //{
        //    var codecs = ImageCodecInfo.GetImageDecoders();
        //    foreach (var codec in codecs)
        //    {
        //        if (codec.FormatID == format.Guid)
        //        {
        //            return codec;
        //        }
        //    }
        //    return null;
        //}
    }
}
