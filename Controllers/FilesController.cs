using Microsoft.AspNetCore.Mvc;
using System.Data;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace CrayonBookSystem
{
    public enum SelectFile
    {
        ファイル,
        画像
    }

    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string caseStudyFilePath = "";    //実践事例のファイルパス
        private string trainingFilePath = "";    //研修資料のファイルパス


        public FilesController(ApplicationDbContext context)
        {
            _context = context;
            caseStudyFilePath = Path.Combine(Config.DataFilesCaseStudyPath, "csid-{0}{1}"); //ファイル名はIDで格納
            trainingFilePath = Path.Combine(Config.DataFilesTraningPath, "tid-{0}", "tcid-{1}{2}"); //ファイル名はIDで格納
        }

        [HttpGet]
        public IResult? CaseStudy()
        {
            SelectFile? select = null;
            int id = -1;

            if (Request.Query["sel"].ToString() != "")
            {
                switch (Request.Query["sel"].ToString())
                {
                    case "0":
                        select = SelectFile.ファイル;
                        break;
                    case "1":
                        select = SelectFile.画像;
                        break;
                }
            }

            if (select == null || !(Request.Query["csid"].ToString() != "" && int.TryParse(Request.Query["csid"].ToString(), out id)))
            {
                return Results.Text("不正なアクセスです。", "text/plain", System.Text.Encoding.UTF8);
            }

            switch (select)
            {
                case SelectFile.画像:
                    string imgEx = "";
                    string imgPath = "";
                    // 拡張子選定（負荷軽減のためDBアクセスしない）
                    foreach (string ext in Config.MIME_IMAGE.Keys)
                    {
                        imgEx = ext;
                        imgPath = string.Format(caseStudyFilePath, id, imgEx);
                        if (System.IO.File.Exists(imgPath))
                        {
                            //ファイル確定
                            break;
                        }
                    }
                    return ResultImage(imgPath, imgEx);

                default:
                    return Results.Text("不正なアクセスです。", "text/plain", System.Text.Encoding.UTF8);

            }
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
        private IResult ResultDocument(string filePath, string fileEx, string downFileName)
        {
            if (System.IO.File.Exists(filePath))
            {
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                return Results.File(memoryStream.ToArray(), Config.MIME_DOCUMENT[fileEx]); //, downFileName);
            }
            else
            {
                return Results.Text("ファイルが存在しません。", "text/plain", System.Text.Encoding.UTF8);
            }

        }
    }
}