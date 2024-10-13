using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMVC.Models;

namespace HotelMVC.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public RoomsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;
            


            var modelContext = _context.Rooms.Include(r => r.Hotel);
            return View(await modelContext.ToListAsync());            
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(m => m.Roomid == id);
            if (room == null)
            {
                return NotFound();
            }
            
            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelname");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Roomid,Roomprice,Roomcapacity,Roomdescription,ImageFile,Availabilitystatus,Hotelid")] Room room)
        {
            if (ModelState.IsValid)
            {
                var uid = HttpContext.Session.GetInt32("UserId");

                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;


                if (room.ImageFile != null)
                {
                    // 1- get rootpath
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2- filename
                    //Guid.NewGuid() ==> generate unique string
                    string fileName = Guid.NewGuid().ToString() + "_" + room.ImageFile.FileName;

                    //3- path 
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    //4- upload image to folder images  
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await room.ImageFile.CopyToAsync(fileStream);
                    }

                    room.Roomimage = fileName;
                }

                room.Availabilitystatus = "available";

                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", room.Hotelid);
            //return View(room);
            return RedirectToAction("Index","Hotels");
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelname", room.Hotelid);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Roomid,Roomprice,Roomcapacity,Roomdescription,ImageFile,Availabilitystatus,Hotelid")] Room room)
        {
            if (id != room.Roomid)
            {
                return NotFound();
            }


            var roomm = await _context.Rooms.AsNoTracking().Where(a => a.Roomid == id).SingleOrDefaultAsync();
            //remove the validation of these two inputs(if they are empty)
            //عشان يفوت عال if 
            ModelState.Remove("ImageFile");

            if (ModelState.IsValid)
            {
                try
                {
                    var uid = HttpContext.Session.GetInt32("UserId");

                    ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                    //object of the user (all user's data)
                    var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                    ViewBag.Fname = user.Userfname;
                    ViewBag.Lname = user.Userlname;
                    ViewBag.UserImage = user.Imagepath;


                    if (room.ImageFile != null)
                    {
                        // 1- get rootpath
                        string wwwRootPath = _webHostEnviroment.WebRootPath;

                        //2- filename
                        //Guid.NewGuid() ==> generate unique string
                        string fileName = Guid.NewGuid().ToString() + "_" + room.ImageFile.FileName;

                        //3- path 
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        //4- upload image to folder images  
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await room.ImageFile.CopyToAsync(fileStream);
                        }

                        room.Roomimage = fileName;
                    }
                    else
                    {
                        room.Roomimage = roomm.Roomimage;
                    }


                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Roomid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Hotelid"] = new SelectList(_context.Hotels, "Hotelid", "Hotelid", room.Hotelid);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(m => m.Roomid == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ModelContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                var uid = HttpContext.Session.GetInt32("UserId");

                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;


                _context.Rooms.Remove(room);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(decimal id)
        {
          return (_context.Rooms?.Any(e => e.Roomid == id)).GetValueOrDefault();
        }
    }
}
