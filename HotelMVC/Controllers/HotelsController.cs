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
    public class HotelsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public HotelsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment; 
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");


            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            return _context.Hotels != null ? 
                          View(await _context.Hotels.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Hotelid,Hotelname,Hoteladdress,Hotelphone,Hotelemail,Hoteldescription,ImageFile")] Hotel hotel)
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


                if (hotel.ImageFile != null)
                {
                    // 1- get rootpath
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2- filename
                    //Guid.NewGuid() ==> generate unique string
                    string fileName = Guid.NewGuid().ToString() + "_" + hotel.ImageFile.FileName;

                    //3- path 
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    //4- upload image to folder images  
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await hotel.ImageFile.CopyToAsync(fileStream);
                    }

                    hotel.Hotelimage = fileName;
                }


                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Hotelid,Hotelname,Hoteladdress,Hotelphone,Hotelemail,Hoteldescription,ImageFile")] Hotel hotel)
        {
            if (id != hotel.Hotelid)
            {
                return NotFound();
            }


            var hotell = await _context.Hotels.AsNoTracking().Where(a => a.Hotelid == id).SingleOrDefaultAsync();
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


                    if (hotel.ImageFile != null)
                    {
                        // 1- get rootpath
                        string wwwRootPath = _webHostEnviroment.WebRootPath;

                        //2- filename
                        //Guid.NewGuid() ==> generate unique string
                        string fileName = Guid.NewGuid().ToString() + "_" + hotel.ImageFile.FileName;

                        //3- path 
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        //4- upload image to folder images  
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await hotel.ImageFile.CopyToAsync(fileStream);
                        }

                        hotel.Hotelimage = fileName;
                    }
                    else
                    {
                        // Keep the existing image if no new image is uploaded
                        hotel.Hotelimage = hotell.Hotelimage;
                    }

                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Hotelid))
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
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Hotelid == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'ModelContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                var uid = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;


                _context.Hotels.Remove(hotel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(decimal id)
        {
          return (_context.Hotels?.Any(e => e.Hotelid == id)).GetValueOrDefault();
        }
    }
}
