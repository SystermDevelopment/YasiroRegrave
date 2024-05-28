using Microsoft.AspNetCore.Mvc;

namespace YasiroRegrave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetGraveStatusController : Controller
    {
        [HttpPost]
        public IActionResult SetGraveStatus([FromBody] SetGraveStatusRequest request)
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
    public class SetGraveStatusRequest
    {
        public string Data { get; set; }
    }

    public class SetGraveInfoResponse
    {
        public string Message { get; set; }
        public string ReceivedData { get; set; }
    }
}
