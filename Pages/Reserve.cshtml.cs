using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Pages
{
    public class Index1Model : PageModel
    {

        [BindProperty]
        [Required(ErrorMessage = "姓は必須項目です。")]
        [StringLength(20, ErrorMessage = "姓は20文字以内で入力してください。")]
        public string LastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "名は必須項目です。")]
        [StringLength(20, ErrorMessage = "名は20文字以内で入力してください。")]
        public string FirstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "姓(ふりがな)は必須項目です。")]
        [StringLength(20, ErrorMessage = "姓(ふりがな)は20文字以内で入力してください。")]
        public string LastNameKana { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "名(ふりがな)は必須項目です。")]
        [StringLength(20, ErrorMessage = "名(ふりがな)は20文字以内で入力してください。")]
        public string FirstNameKana { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "郵便番号は必須項目です。")]
        public string PostalCode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "都道府県は必須項目です。")]
        public string Prefecture { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "市区郡は必須項目です。")]
        public string City { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "町名・番地は必須項目です。")]
        public string Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "電話番号は必須項目です。")]
        public string Phone { get; set; }

    }
}
