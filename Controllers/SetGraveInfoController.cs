using Microsoft.AspNetCore.Mvc;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetGraveInfoController : Controller
    {
        [HttpPost]
        public IActionResult SetGraveInfo([FromBody] GraveInfoRequest request)
        {
            // ここでリクエストを処理し、レスポンスを作成します。
            var response = new
            {
                success = true,
                message = "Reyasiro情報の設定に成功しました。"
            };
            return Ok(response);
        }
    }
    public class GraveInfoRequest
    {
        public string Data { get; set; }
    }

    public class GraveInfoResponse
    {
        public string Message { get; set; }
        public string ReceivedData { get; set; }
    }
}
