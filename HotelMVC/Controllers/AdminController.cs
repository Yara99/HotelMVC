using HotelMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        public AdminController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == id).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;

            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == id).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;


            ViewBag.UserloginsId = roleid.Id;



            ViewBag.registeredUsers = _context.Userlogins.Where(r=>r.Roleid==2).Count();

            ViewBag.allAvailableRooms = _context.Rooms.Where(x => x.Availabilitystatus == "available").Count();
            ViewBag.allBookedRooms = _context.Rooms.Where(x => x.Availabilitystatus == "booked").Count();

            

            string HotelName;
            int AvailableRoomCount;
            int BookedRoomCount;
            var hotelAvailabilityList = _context.Rooms
            .GroupBy(r => r.Hotel) // Group by hotel
            .Select(g => new 
            {
                HotelName = g.Key.Hotelname,
                AvailableRoomCount = g.Count(r => r.Availabilitystatus == "available"), // Count of available rooms
                BookedRoomCount = g.Count(r => r.Availabilitystatus == "booked") // Count of booked rooms
            })
            .ToList();

            ViewBag.HotelAvailability = hotelAvailabilityList;



            var hotel = _context.Hotels.ToList();
            var users = _context.Userlogins.Include(u=>u.User).Include(u => u.Role).Where(x=>x.Roleid == 2).ToList();
            var model = Tuple.Create<IEnumerable<Hotel>, IEnumerable<Userlogin>>(hotel,users);
            return View(model);
        }



        public IActionResult GetRoomsByHotelId(int id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.RoleId = roleid.Roleid;
            }


            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;


            var rooms = _context.Rooms.Include(h=>h.Hotel).Where(x => x.Hotelid == id).ToList();
            var hotel = _context.Hotels.Where(h=>h.Hotelid == id).SingleOrDefault();
            var model = Tuple.Create<IEnumerable<Room>, Hotel>(rooms, hotel);

            return View(model);
        }

        public IActionResult GetRoomsByHotelIdAdmin(int id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            ViewBag.HotelId = id;


            var rooms = _context.Rooms.Include(r => r.Hotel).Where(x => x.Hotelid == id).ToList();
            return View(rooms);
        }






        [HttpGet]
        public IActionResult Report()
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            //var data = _context.Reservations.Include(u => u.User).Include(r => r.Room).ThenInclude(h => h.Hotel).Where(r => r.Room.Availabilitystatus == "booked");
            var data = _context.Reservations.Include(u => u.User).Include(r => r.Room).ThenInclude(h=>h.Hotel).ToList();
            ViewBag.Benefits = data.Sum(r => r.Totalprice);


            ViewBag.allAvailableRooms = _context.Rooms.Where(x => x.Availabilitystatus == "available").Count();
            ViewBag.allBookedRooms = _context.Rooms.Where(x => x.Availabilitystatus == "booked").Count();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(DateTime? CheckinDate, DateTime? CheckoutDate, int? Year, int? Month)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;
            
            var data = _context.Reservations.Include(u => u.User).Include(r => r.Room).ThenInclude(h => h.Hotel);


            ViewBag.allAvailableRooms = _context.Rooms.Where(x => x.Availabilitystatus == "available").Count();
            ViewBag.allBookedRooms = _context.Rooms.Where(x => x.Availabilitystatus == "booked").Count();

            if (Year.HasValue && Month.HasValue)
            {
                //Monthly
                // creates a DateTime object representing the first day of the specified month in the given year.
                var startOfMonth = new DateTime(Year.Value, Month.Value, 1);
                //add one month then subtract one day (give  the last day of the original month)
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var result = await data.Where(x => x.Checkindate.Value.Date <= endOfMonth && x.Checkoutdate.Value.Date >= startOfMonth).ToListAsync();
                ViewBag.Benefits = result.Sum(r => r.Totalprice);
                return View(result);
            }
            else if (Year.HasValue)
            {
                //yearly
                var startOfYear = new DateTime(Year.Value, 1, 1);
                var endOfYear = new DateTime(Year.Value, 12, 31);

                var result = await data.Where(x => x.Checkindate.Value.Date <= endOfYear && x.Checkoutdate.Value.Date >= startOfYear).ToListAsync();
                ViewBag.Benefits = result.Sum(r => r.Totalprice);
                return View(result);
            }
            else
            if (CheckinDate == null && CheckoutDate == null)
            {
                ViewBag.Benefits = data.Sum(r => r.Totalprice);
                return View(data);
            }
            else if (CheckinDate != null && CheckoutDate == null)
            {
                var result = await data.Where(x => x.Checkindate.Value.Date >= CheckinDate).ToListAsync();
                ViewBag.Benefits = result.Sum(r => r.Totalprice);
                return View(result);
            }
            else if (CheckinDate == null && CheckoutDate != null)
            {
                var result = await data.Where(x => x.Checkindate.Value.Date <= CheckoutDate).ToListAsync();
                ViewBag.Benefits = result.Sum(r => r.Totalprice);
                return View(result);
            }
            else
            {
                var result = await data.Where(x => x.Checkindate.Value.Date >= CheckinDate &&
                                                   x.Checkindate.Value.Date <= CheckoutDate).ToListAsync();
                ViewBag.Benefits = result.Sum(r => r.Totalprice);
                return View(result);
            }
        }




    }
}
