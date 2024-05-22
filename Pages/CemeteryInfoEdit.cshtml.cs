using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using YasiroRegrave.Data;
using YasiroRegrave.Model;
using YasiroRegrave.Pages.common;

namespace YasiroRegrave.Pages
{
    public class CemeteryInfoEditModel : PageModel
    {

        //[BindProperty]

        //public float? AreaValue { get; set; } 

        //[BindProperty]
        ////[Required(ErrorMessage = Message.M_E0001)]
        ////[StringLength(100, ErrorMessage = Message.M_E0002)]

        //public int? ReleaseStatus { get; set; } 

        //[BindProperty]
        //public int? SectionStatus { get; set; }
        //[BindProperty]
        //public int? SectionType { get; set; }
        //[BindProperty]
        //public int? UsageFee { get; set; }
        //[BindProperty]
        //public int? ManagementFee { get; set; }
        //[BindProperty]
        //public int? MonumentCost { get; set; }
        //[BindProperty]
        //public int? SetPrice { get; set; }
        //[BindProperty]
        //public int? TotalPrice { get; set; }
        [BindProperty]
        public string Image1Fname { get; set; }
        [BindProperty]
        public string Image2Fname { get; set; }

        //[BindProperty]
        public int? CemeteryInfoIndex { get; set; }

        public int? CemeteryIndex { get; set; }

        private readonly ApplicationDbContext _context;
        public CemeteryInfoEditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PageCemeteryInfo> Users { get; set; } = new List<PageCemeteryInfo>();
        public void OnGet(int? index)
        {
            CemeteryInfoIndex = index;
            if (index.HasValue)
            {
                var cemeteryinfo = _context.CemeteryInfos
                    .Where(c => c.DeleteFlag == 0 && c.CemeteryInfoIndex == index.Value)
                    .FirstOrDefault();
                if (cemeteryinfo != null)
                {
                    //AreaValue = cemeteryinfo.AreaValue;
                    //ReleaseStatus = cemeteryinfo.ReleaseStatus;
                    //SectionStatus = cemeteryinfo.SectionStatus;
                    //SectionType = cemeteryinfo.SectionType;
                    //UsageFee = cemeteryinfo.UsageFee;
                    //ManagementFee = cemeteryinfo.ManagementFee;
                    //MonumentCost = cemeteryinfo.MonumentCost;
                    //SetPrice = cemeteryinfo.SetPrice;
                    //TotalPrice = cemeteryinfo.TotalPrice;
                    Image1Fname = cemeteryinfo.Image1Fname;
                    Image2Fname = cemeteryinfo.Image2Fname;

                }
            }
     
        }
        public IActionResult OnPost(int? index)
        {
            try
            {
                if (index == null)
                {
                    // ššššTDB.VenderID‰¼‘Î‰žšššš
                    //var forignVender = _context.Venders.FirstOrDefault();
                    //if (forignVender == null)
                    //{
                    //    throw new InvalidOperationException();
                    //}

                    //INSERT
                    var newCemeteryInfo = new Models.CemeteryInfo
                    {
                    //AreaValue = AreaValue,
                    //ReleaseStatus = ReleaseStatus,
                    //SectionStatus = SectionStatus,
                    //SectionType = SectionType,
                    //UsageFee = UsageFee,
                    //ManagementFee = ManagementFee,
                    //MonumentCost = MonumentCost,
                    //SetPrice = SetPrice,
                    //TotalPrice = TotalPrice,
                    Image1Fname = Image1Fname,
                    Image2Fname = Image2Fname,
                    CreateDate = DateTime.UtcNow,
                        //CreateUser = LoginId,
                    DeleteFlag = 0,
                        //Vendor = forignVender,

                        

                    };
                    _context.CemeteryInfos.Add(newCemeteryInfo);
                    _context.SaveChanges();
                }
                else
                {
                    var existingCemeteryinfo = _context.CemeteryInfos.Where(c => c.DeleteFlag == 0 && c.CemeteryInfoIndex == index.Value).FirstOrDefault();
                    if (existingCemeteryinfo != null)
                    {
                        // UPDATE
                    //existingCemeteryinfo.AreaValue = AreaValue;
                    //existingCemeteryinfo.ReleaseStatus = ReleaseStatus;
                    //existingCemeteryinfo.SectionStatus = SectionStatus;
                    //existingCemeteryinfo.SectionType = SectionType;
                    //existingCemeteryinfo.UsageFee = UsageFee;
                    //existingCemeteryinfo.ManagementFee = ManagementFee;
                    //existingCemeteryinfo.MonumentCost = MonumentCost;
                    //existingCemeteryinfo.SetPrice = SetPrice;
                    //existingCemeteryinfo.TotalPrice = TotalPrice;
                    existingCemeteryinfo.Image1Fname = Image1Fname;
                    existingCemeteryinfo.Image2Fname = Image2Fname;


                    existingCemeteryinfo.UpdateDate = DateTime.UtcNow;
                        //existingVender.UpdateUser = LoginId,

                        _context.SaveChanges();
                    }
                }
            }
            catch
            {
                return Page();
            }
            return RedirectToPage("/CemeteryInfoList");
        }
        public class PageCemeteryInfo
        {
            //public float? AreaValue { get; set; }
            //public int? ReleaseStatus { get; set; }
            //public int? SectionStatus { get; set; }
            //public int? SectionType { get; set; }
            //public int? UsageFee { get; set; }
            //public int? ManagementFee { get; set; }
            //public int? MonumentCost { get; set; }
            //public int? SetPrice { get; set; }
            //public int? TotalPrice { get; set; }
            public string Image1Fname { get; set; }
            public string Image2Fname { get; set; }

        }
    }
}
