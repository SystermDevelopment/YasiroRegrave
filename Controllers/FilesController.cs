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
        private string regraveFilePath = "";    //�揊�摜�̃t�@�C���p�X


        public FilesController(ApplicationDbContext context)
        {
            _context = context;
            regraveFilePath = Path.Combine(Config.DataFilesRegravePath, ""); //�t�@�C������ID�Ŋi�[
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
                return Results.Text("�s���ȃA�N�Z�X�ł��B", "text/plain", System.Text.Encoding.UTF8);
            }

            kukaku = Request.Query["k"];
            string imgEx = "";
            string imgPath = "";
            // �g���q�I��i���׌y���̂���DB�A�N�Z�X���Ȃ��j
            foreach (string ext in Config.MIME_IMAGE.Keys)
            {
                imgEx = ext;
                imgPath = $"{regraveFilePath}\\{reien}\\{area}\\{kukaku}-{sel}{imgEx}";
                if (System.IO.File.Exists(imgPath))
                {
                    //�t�@�C���m��
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