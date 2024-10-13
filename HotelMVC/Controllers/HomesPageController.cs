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
    public class HomesPageController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;


        public HomesPageController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: HomesPage
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            return _context.Homes != null ? 
                          View(await _context.Homes.OrderByDescending(h=>h.Id).ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homes'  is null.");
        }

        // GET: HomesPage/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            return View(home);
        }

        // GET: HomesPage/Create
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

        // POST: HomesPage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Logo,Hometitle,Homecontent,ImageFile")] Home home)
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

                if (home.ImageFile != null)
                {
                    // 1- get rootpath
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2- filename
                    //Guid.NewGuid() ==> generate unique string
                    string fileName = Guid.NewGuid().ToString() + "_" + home.ImageFile.FileName;

                    //3- path 
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    //4- upload image to folder images  
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await home.ImageFile.CopyToAsync(fileStream);
                    }

                    home.Homeimage = fileName;
                }

                _context.Add(home);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(home);
        }

        // GET: HomesPage/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }
            return View(home);
        }

        // POST: HomesPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Logo,Hometitle,Homecontent,ImageFile")] Home home)
        {
            if (id != home.Id)
            {
                return NotFound();
            }

            var homee = await _context.Homes.AsNoTracking().Where(a => a.Id == id).SingleOrDefaultAsync();
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

                    if (home.ImageFile != null)
                    {
                        // 1- get rootpath
                        string wwwRootPath = _webHostEnviroment.WebRootPath;

                        //2- filename
                        //Guid.NewGuid() ==> generate unique string
                        string fileName = Guid.NewGuid().ToString() + "_" + home.ImageFile.FileName;

                        //3- path 
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        //4- upload image to folder images  
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile.CopyToAsync(fileStream);
                        }

                        home.Homeimage = fileName;
                    }
                    else
                    {
                        // Keep the existing image if no new image is uploaded
                        home.Homeimage = homee.Homeimage;
                    }


                    _context.Update(home);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeExists(home.Id))
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
            return View(home);
        }

        // GET: HomesPage/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Homes == null)
            {
                return NotFound();
            }

            var home = await _context.Homes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            return View(home);
        }

        // POST: HomesPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {

            if (_context.Homes == null)
            {
                return Problem("Entity set 'ModelContext.Homes'  is null.");
            }
            var home = await _context.Homes.FindAsync(id);
            if (home != null)
            {
                var uid = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;


                _context.Homes.Remove(home);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeExists(decimal id)
        {
          return (_context.Homes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
