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
    public class AboutsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public AboutsController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        // GET: Abouts
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            return _context.Abouts != null ? 
                          View(await _context.Abouts.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Abouts'  is null.");
        }

        // GET: Abouts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Abouts/Create
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

        // POST: Abouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Abouttitle,Aboutcontent,ImageFile")] About about)
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

                if (about.ImageFile != null)
                {
                    // 1- get rootpath
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2- filename
                    //Guid.NewGuid() ==> generate unique string
                    string fileName = Guid.NewGuid().ToString() + "_" + about.ImageFile.FileName;

                    //3- path 
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    //4- upload image to folder images  
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await about.ImageFile.CopyToAsync(fileStream);
                    }

                    about.Aboutimage = fileName;
                }

                _context.Add(about);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Abouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        // POST: Abouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Abouttitle,Aboutcontent,ImageFile")] About about)
        {
            if (id != about.Id)
            {
                return NotFound();
            }

            var aboutt = await _context.Abouts.AsNoTracking().Where(a=>a.Id == id).SingleOrDefaultAsync();
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

                    if (about.ImageFile != null)
                    {
                        // 1- get rootpath
                        string wwwRootPath = _webHostEnviroment.WebRootPath;

                        //2- filename
                        //Guid.NewGuid() ==> generate unique string
                        string fileName = Guid.NewGuid().ToString() + "_" + about.ImageFile.FileName;

                        //3- path 
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        //4- upload image to folder images  
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await about.ImageFile.CopyToAsync(fileStream);
                        }

                        about.Aboutimage = fileName;
                    }
                    else
                    {
                        // Keep the existing image if no new image is uploaded
                        about.Aboutimage = aboutt.Aboutimage;
                    }

                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id))
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
            return View(about);
        }

        // GET: Abouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;


            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Abouts == null)
            {
                return Problem("Entity set 'ModelContext.Abouts'  is null.");
            }
            var about = await _context.Abouts.FindAsync(id);
            if (about != null)
            {
                var uid = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;


                _context.Abouts.Remove(about);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(decimal id)
        {
          return (_context.Abouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
