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
        /// 墓所コードを 墓所名に変換
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
    }

}
