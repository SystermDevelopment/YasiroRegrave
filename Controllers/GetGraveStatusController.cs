using Microsoft.AspNetCore.Mvc;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetGraveStatusController : Controller
    {
        [HttpPost]
        public IActionResult GetGraveIStatus([FromBody] GetGraveStatusRequest request)
        {
            // ここでリクエストを処理し、レスポンスを作成します。
            var response = new List<GetGraveStatusResponse>
            {
                new GetGraveStatusResponse
                {
                    霊園番号 = "11",
                    区画番号 = "21-08-01",
                    面積 = "1.00聖地",
                    区画区分 = "再販",
                    使用料 = "600000",
                    管理料 = "250000",
                    仕置巻石料 = "0",
                    石碑代金 = "1000000",
                    区画状態 = "空",
                    販売ステータス = "準備中",
                    画像1登録 = "済",
                    画像2登録 = "済",
                    最終更新日時 = "2024/01/01 00:00:00"
                },
                new GetGraveStatusResponse
                {
                    霊園番号 = "11",
                    区画番号 = "21-08-02",
                    面積 = "1.00聖地",
                    区画区分 = "再販",
                    使用料 = "600000",
                    管理料 = "250000",
                    仕置巻石料 = "0",
                    石碑代金 = "1000000",
                    区画状態 = "予約中",
                    販売ステータス = "販売中",
                    画像1登録 = "未",
                    画像2登録 = "済",
                    最終更新日時 = "2024/01/01 00:00:00"
                }
            };
            return Ok(response);
        }
    }
    public class GetGraveStatusRequest
    {
        public string Data { get; set; }
    }

    public class GetGraveStatusResponse
    {
        public string 霊園番号 { get; set; }
        public string 区画番号 { get; set; }
        public string 面積 { get; set; }
        public string 区画区分 { get; set; }
        public string 使用料 { get; set; }
        public string 管理料 { get; set; }
        public string 仕置巻石料 { get; set; }
        public string 石碑代金 { get; set; }
        public string 区画状態 { get; set; }
        public string 販売ステータス { get; set; }
        public string 画像1登録 { get; set; }
        public string 画像2登録 { get; set; }
        public string 最終更新日時 { get; set; }
    }
}
