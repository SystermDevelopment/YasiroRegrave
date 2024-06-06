using Microsoft.AspNetCore.Mvc;
using System.Data;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace CrayonBookSystem
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string regraveFilePath = "";    //墓所画像のファイルパス


        public FilesController(ApplicationDbContext context)
        {
            _context = context;
            regraveFilePath = Path.Combine(Config.DataFilesRegravePath, ""); //ファイル名はIDで格納
        }

        [HttpGet]
        public IResult? GraveImg()
        {
            int reien = -1;
            int area = -1;
            int sel = -1;
            string? kukaku = string.Empty;

            if (!(Request.Query["r"].ToString() != "" && int.TryParse(Request.Query["r"].ToString(), out reien)) ||
                !(Request.Query["a"].ToString() != "" && int.TryParse(Request.Query["a"].ToString(), out area)) ||
                !(Request.Query["sel"].ToString() != "" && int.TryParse(Request.Query["sel"].ToString(), out sel)) ||
                !Request.Query.ContainsKey("k")
                )
            {
                return Results.Text("不正なアクセスです。", "text/plain", System.Text.Encoding.UTF8);
            }

            kukaku = Request.Query["k"];
            string imgEx = "";
            string imgPath = "";
            // 拡張子選定（負荷軽減のためDBアクセスしない）
            foreach (string ext in Config.MIME_IMAGE.Keys)
            {
                imgEx = ext;
                imgPath = $"{regraveFilePath}\\{reien}\\{area}\\{kukaku}-{sel}{imgEx}";
                if (System.IO.File.Exists(imgPath))
                {
                    //ファイル確定
                    break;
                }
            }
            return ResultImage(imgPath, imgEx);
        }
        private IResult ResultImage(string imgPath, string imgEx)
        {
            if (System.IO.File.Exists(imgPath))
            {
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                return Results.File(memoryStream.ToArray(), Config.MIME_IMAGE[imgEx]);
            }
            else
            {
                return Results.File(@".\img\noimage.png", Config.MIME_IMAGE[".png"]);
            }

        }
    }
}