using System.Collections.Generic;

namespace YasiroRegrave.Pages.common
{
    public class Config
    {
        // 削除フラグ
        public enum DeleteType
        {
            未削除 = 0,
            削除 = 1
        }
        // 公開状態
        public enum ReleaseStatusType
        {
            準備中 = 0,
            販売中 = 1,
        }
        // 区画状態
        public enum SectionStatusType
        {
            空 = 0,
            WEB予約 = 1,
            拠点予約 = 2,
            成約 = 3, 
        }
        // 有無
        public enum ExistType
        {
            未 = 0,
            済 = 1,
        }
        // 権限
        public enum AuthorityType
        {
            管理者 = 0,
            担当者 = 1,
        }
        // 予約種別
        public enum ReserveType
        {
            見学予約 = 0,
            仮予約 = 1,
        }
        // 通知フラグ
        public enum NotificationType
        {
            未通知 = 0,
            通知済 = 1,
        }
        public static readonly Dictionary<string, string> MIME_IMAGE = new Dictionary<string, string>
        {
            {".jpg","image/jpeg"},
            {".png","image/png"},
            {".tif","image/tiff"},
            {".svg","image/svg+xml"},
            {".gif","image/gif"},
            {".jpeg","image/jpeg"},
            {".jpe","image/jpeg"},
            {".jpz","image/jpeg"},
            {".pnz","image/png"},
            {".tiff","image/tiff"},
        };
        public static readonly Dictionary<string, string> MIME_DOCUMENT = new Dictionary<string, string>
        {
            {".pdf","application/pdf"},    //Adobe Portable Document Format (PDF)
            {".pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},    //Microsoft PowerPoint (OpenXML)
            {".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},    //Microsoft Excel (OpenXML)
            {".docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},    //Microsoft Word (OpenXML)
            {".ppt","application/vnd.ms-powerpoint"},    //Microsoft PowerPoint
            {".doc","application/msword"},    //Microsoft Word
            {".xls","application/vnd.ms-excel"},    //Microsoft Excel
            {".csv","text/csv"},    //カンマ区切り値 (CSV)
            {".zip","application/zip"},    //ZIP アーカイブ
        };
        /// <summary>
        /// ファイルデータ格納先のルートパス
        /// </summary>
        public static string DataFilesRootPath
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                string? temp = configuration.GetValue<string>("DataFileSettings:RootPath");
                if (temp == null)
                {
                    throw new Exception("データフォルダのパスが設定されていません。");
                }
                return temp;
            }
        }
        /// <summary>
        /// ファイルデータ格納先
        /// </summary>
        public static string DataFilesRegravePath
        {
            get
            {
                string rootPath = DataFilesRootPath;
                return Path.Combine(rootPath, "images");
            }
        }
        /// <summary>
        /// jsonデータ格納先
        /// </summary>
        public static string JsonDataFilesPath
        {
            get
            {
                string rootPath = wwwRootPath;
                return Path.Combine(rootPath, "data");
            }
        }
        /// <summary>
        /// wwwrootのパス
        /// </summary>
        public static string wwwRootPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }
        public static string SendMailAddress { get { return "r-haka@yasiro.jp"; } }
        public static string SendMailName { get { return "霊園・墓石のヤシロ"; } }
        public static string SMTPHost { get { return "elephant-apricot-817fce358f26de89.znlc.jp"; } }
        public static string SMTPId { get { return "r-haka@yasiro.jp"; } }
        public static string SMTPPass { get { return "$sohd846"; } }

        public static string AlartNotificationMailAddress { get { return "web_yoyaku_alert@yasiro.co.jp"; } }
        public static string AlartNotificationMailAddressTechno { get { return "alert@technosphere.co.jp"; } }
    }
}
