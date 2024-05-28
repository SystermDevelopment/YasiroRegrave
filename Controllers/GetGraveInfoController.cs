using Microsoft.AspNetCore.Mvc;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetGraveInfoController : Controller
    {
        [HttpPost]
        public IActionResult SetGraveInfo([FromBody] GetGraveInfoRequest request)
        {
            // ここでリクエストを処理し、レスポンスを作成します。
            var response = new List<GetGraveInfoResponse>
            {
                new GetGraveInfoResponse
                {
                    予約日時 = "2024/01/01 00:00:00",
                    霊園番号 = "11",
                    区画番号 = "21-08-01",
                    名前姓 = "山田",
                    名前名 = "太郎",
                    名前姓ヨミ = "やまだ",
                    名前名ヨミ = "たろう",
                    郵便番号 = "123-4567",
                    住所 = "東京都渋谷区",
                    電話番号 = "012-345-6789",
                    メール = "yamada.taro@example.com",
                    質問 = "墓地の状態は？",
                    希望日時1 = "2024/01/01 00:00:00",
                    希望日時2 = "",
                    希望日時3 = "",
                    連携内容 = "仮予約",
                    販売協力会社index = "1"
                },
                new GetGraveInfoResponse
                {
                    予約日時 = "2024/01/01 00:00:00",
                    霊園番号 = "11",
                    区画番号 = "21-08-02",
                    名前姓 = "佐藤",
                    名前名 = "花子",
                    名前姓ヨミ = "さとう",
                    名前名ヨミ = "はなこ",
                    郵便番号 = "987-6543",
                    住所 = "大阪府大阪市",
                    電話番号 = "090-1234-5678",
                    メール = "sato.hanako@example.com",
                    質問 = "墓石の種類は？",
                    希望日時1 = "2024-01-01T00:00:00Z",
                    希望日時2 = "",
                    希望日時3 = "",
                    連携内容 = "見学予約",
                    販売協力会社index = "1"
                }
            };
            return Ok(response);
        }
    }
    public class GetGraveInfoRequest
    {
        public string Data { get; set; }
    }

    public class GetGraveInfoResponse
    {
        public string 予約日時 { get; set; }
        public string 霊園番号 { get; set; }
        public string 区画番号 { get; set; }
        public string 名前姓 { get; set; }
        public string 名前名 { get; set; }
        public string 名前姓ヨミ { get; set; }
        public string 名前名ヨミ { get; set; }
        public string 郵便番号 { get; set; }
        public string 住所 { get; set; }
        public string 電話番号 { get; set; }
        public string メール { get; set; }
        public string 質問 { get; set; }
        public string 希望日時1 { get; set; }
        public string 希望日時2 { get; set; }
        public string 希望日時3 { get; set; }
        public string 連携内容 { get; set; }
        public string 販売協力会社index { get; set; }
    }
}
