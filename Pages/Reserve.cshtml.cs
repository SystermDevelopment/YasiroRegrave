using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Pages
{
    public class Index1Model : PageModel
    {

        [BindProperty]
        [Required(ErrorMessage = "���͕K�{���ڂł��B")]
        [StringLength(20, ErrorMessage = "����20�����ȓ��œ��͂��Ă��������B")]
        public string LastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "���͕K�{���ڂł��B")]
        [StringLength(20, ErrorMessage = "����20�����ȓ��œ��͂��Ă��������B")]
        public string FirstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "��(�ӂ肪��)�͕K�{���ڂł��B")]
        [StringLength(20, ErrorMessage = "��(�ӂ肪��)��20�����ȓ��œ��͂��Ă��������B")]
        public string LastNameKana { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "��(�ӂ肪��)�͕K�{���ڂł��B")]
        [StringLength(20, ErrorMessage = "��(�ӂ肪��)��20�����ȓ��œ��͂��Ă��������B")]
        public string FirstNameKana { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "�X�֔ԍ��͕K�{���ڂł��B")]
        public string PostalCode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "�s���{���͕K�{���ڂł��B")]
        public string Prefecture { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "�s��S�͕K�{���ڂł��B")]
        public string City { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "�����E�Ԓn�͕K�{���ڂł��B")]
        public string Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "�d�b�ԍ��͕K�{���ڂł��B")]
        public string Phone { get; set; }

    }
}
