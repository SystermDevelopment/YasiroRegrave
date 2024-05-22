//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using YasiroRegrave.Data;

//namespace YasiroRegrave.Pages
//{
//    public class ReserveInfoListModel : PageModel
//    {
//        private readonly ApplicationDbContext _context;
//        public ReserveInfoListModel(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public List<Reserve_Info> Reserve_Infos{ get; set; } = new List<Reserve_Info>();
//        public void OnGet()
//        {
//            GetPage();
//        }
//        private void GetPage()
//        {
//            var reserve_info_List = _context.Reserve_Infos
//                //.Where(r => r.DeleteFlag == 0)
//                .Select(r => new Reserve_Info
//                {
//                    Reserve_Index = r.Reserve_Index,
//                    Cemetery_Info_Index = r.Cemetery_Info_Index,
//                    Last_Name = r.Last_Name,
//                    First_Name = r.First_Name,
//                    Last_Name_Yomi = r.Last_Name_yomi,
//                    First_Name_Yomi = r.First_Name_yomi,
//                    Zip_Code = r.Zip_Code,
//                    Adress = r.Adress,
//                    Telephone_Number = r.Telephone_Number,
//                    E_Mail = r.E_Mail,
//                    Question = r.Question,
//                    VenderIndex = r.VenderIndex,
//                })
//                .ToList();
//            Reserve_Infos = Reserve_Infos;
//        }
//        public IActionResult OnPost(int index)
//        {
//            var userDelete = _context.Users.FirstOrDefault(u => u.Index == index);
//            if (userDelete != null)
//            {
//                //DELITE
//                userDelete.DeleteFlag = 1;
//                _context.SaveChanges();
//            }
//            return RedirectToPage("/reserve_Info_List");
//        }

//        public class Reserve_Info
//        {
//            public int Reserve_Index { get; set; }
//            public int Cemetery_Info_Index { get; set; }
//            public string Last_Name { get; set; }
//            public string First_Name { get; set; }
//            public string Last_Name_Yomi { get; set; }
//            public string First_Name_Yomi { get; set; }
//            public string Zip_Code { get; set; }
//            public string Adress { get; set; }
//            public string Telephone_Number { get; set; }
//            public string E_Mail { get; set; }
//            public string Question { get; set; }
//            public int VenderIndex { get; set; }
//        }
//    }
//}
