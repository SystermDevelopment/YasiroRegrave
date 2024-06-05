using Microsoft.AspNetCore.Mvc;
using System.Data;
using YasiroRegrave.Data;
using YasiroRegrave.Pages.common;

namespace CrayonBookSystem
{
    public enum SelectFile
    {
        �t�@�C��,
        �摜
    }

    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string caseStudyFilePath = "";    //���H����̃t�@�C���p�X
        private string trainingFilePath = "";    //���C�����̃t�@�C���p�X


        public FilesController(ApplicationDbContext context)
        {
            _context = context;
            caseStudyFilePath = Path.Combine(Config.DataFilesCaseStudyPath, "csid-{0}{1}"); //�t�@�C������ID�Ŋi�[
            trainingFilePath = Path.Combine(Config.DataFilesTraningPath, "tid-{0}", "tcid-{1}{2}"); //�t�@�C������ID�Ŋi�[
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
                        select = SelectFile.�t�@�C��;
                        break;
                    case "1":
                        select = SelectFile.�摜;
                        break;
                }
            }

            if (select == null || !(Request.Query["csid"].ToString() != "" && int.TryParse(Request.Query["csid"].ToString(), out id)))
            {
                return Results.Text("�s���ȃA�N�Z�X�ł��B", "text/plain", System.Text.Encoding.UTF8);
            }

            switch (select)
            {
                case SelectFile.�摜:
                    string imgEx = "";
                    string imgPath = "";
                    // �g���q�I��i���׌y���̂���DB�A�N�Z�X���Ȃ��j
                    foreach (string ext in Config.MIME_IMAGE.Keys)
                    {
                        imgEx = ext;
                        imgPath = string.Format(caseStudyFilePath, id, imgEx);
                        if (System.IO.File.Exists(imgPath))
                        {
                            //�t�@�C���m��
                            break;
                        }
                    }
                    return ResultImage(imgPath, imgEx);

                default:
                    return Results.Text("�s���ȃA�N�Z�X�ł��B", "text/plain", System.Text.Encoding.UTF8);

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
                return Results.Text("�t�@�C�������݂��܂���B", "text/plain", System.Text.Encoding.UTF8);
            }

        }
    }
}