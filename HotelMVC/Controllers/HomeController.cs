using HotelMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HotelMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
             /*   var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;*/

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault(); ;
                ViewBag.RoleId = roleid.Roleid;
            }


            var lastRecordHome = _context.Homes.OrderByDescending(h=>h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;
            ViewBag.HomeTitle = lastRecordHome.Hometitle;
            ViewBag.HomeContent = lastRecordHome.Homecontent;
            ViewBag.HomeImage = lastRecordHome.Homeimage;


            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;


            var hotel = _context.Hotels.ToList();
            var testimonial = _context.Testimonials.Where(t=>t.Testimonialstatus == "approved").Include(u=>u.User).ToList();
            var model = Tuple.Create<IEnumerable<Hotel>, IEnumerable<Testimonial>>(hotel, testimonial);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault(); ;
                ViewBag.RoleId = roleid.Roleid;
            }


            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            var lastRecordAbout = _context.Abouts.OrderByDescending(c => c.Id).FirstOrDefault();
            return View(lastRecordAbout);
        }
        public IActionResult ContactUs()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault(); ;
                ViewBag.RoleId = roleid.Roleid;
            }


            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            
            return View(lastRecordContact);
        }
        public IActionResult HotelsPage()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault(); ;
                ViewBag.RoleId = roleid.Roleid;
            }


            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            var hotel = _context.Hotels.ToList();
            return View(hotel);
        }
        
        public IActionResult UserProfile()
        {
            
            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = userr.Userfname;
                ViewBag.Lname = userr.Userlname;
                ViewBag.UserImage = userr.Imagepath;

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault(); ;
                ViewBag.RoleId = roleid.Roleid;
            }

            var user = _context.Userlogins.Include(u=>u.User).FirstOrDefault(m => m.Userid == uid);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public IActionResult HotelsSearch(string? searchQuery)
        {

            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;


            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault(); ;
                ViewBag.RoleId = roleid.Roleid;
            }

            var hotel = string.IsNullOrEmpty(searchQuery) 
                ? _context.Hotels.ToList() 
                : _context.Hotels.Where(h => h.Hotelname.ToLower().Contains(searchQuery.ToLower())).ToList();

            //var hotel = _context.Hotels.Where(h=>h.Hotelname.ToLower().Contains(searchQuery.ToLower())).ToList();

            return View("HotelsPage", hotel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}