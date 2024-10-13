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
    public class ContactusController : Controller
    {
        private readonly ModelContext _context;

        public ContactusController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactus
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = userr.Userfname;
            ViewBag.Lname = userr.Userlname;
            ViewBag.UserImage = userr.Imagepath;


            return _context.Contactus != null ? 
                          View(await _context.Contactus.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Contactus'  is null.");
        }

        // GET: Contactus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = userr.Userfname;
            ViewBag.Lname = userr.Userlname;


            ViewBag.UserImage = userr.Imagepath;
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactus = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactus == null)
            {
                return NotFound();
            }

            return View(contactus);
        }

        // GET: Contactus/Create
        public IActionResult Create()
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = userr.Userfname;
            ViewBag.Lname = userr.Userlname;
            ViewBag.UserImage = userr.Imagepath;


            return View();
        }

        // POST: Contactus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Contactaddress,Contactemail,Contactphone")] Contactus contactus)
        {
            if (ModelState.IsValid)
            {
                var uid = HttpContext.Session.GetInt32("UserId");

                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = userr.Userfname;
                ViewBag.Lname = userr.Userlname;
                ViewBag.UserImage = userr.Imagepath;


                _context.Add(contactus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactus);
        }

        // GET: Contactus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = userr.Userfname;
            ViewBag.Lname = userr.Userlname;
            ViewBag.UserImage = userr.Imagepath;


            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactus = await _context.Contactus.FindAsync(id);
            if (contactus == null)
            {
                return NotFound();
            }
            return View(contactus);
        }

        // POST: Contactus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Contactaddress,Contactemail,Contactphone")] Contactus contactus)
        {
            if (id != contactus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var uid = HttpContext.Session.GetInt32("UserId");

                    ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                    //object of the user (all user's data)
                    var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                    ViewBag.Fname = userr.Userfname;
                    ViewBag.Lname = userr.Userlname;


                    ViewBag.UserImage = userr.Imagepath;
                    _context.Update(contactus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactusExists(contactus.Id))
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
            return View(contactus);
        }

        // GET: Contactus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = userr.Userfname;
            ViewBag.Lname = userr.Userlname;


            ViewBag.UserImage = userr.Imagepath;
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactus = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactus == null)
            {
                return NotFound();
            }

            return View(contactus);
        }

        // POST: Contactus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactus == null)
            {
                return Problem("Entity set 'ModelContext.Contactus'  is null.");
            }
            var contactus = await _context.Contactus.FindAsync(id);
            if (contactus != null)
            {
                var uid = HttpContext.Session.GetInt32("UserId");

                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var userr = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = userr.Userfname;
                ViewBag.Lname = userr.Userlname;
                ViewBag.UserImage = userr.Imagepath;


                _context.Contactus.Remove(contactus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactusExists(decimal id)
        {
          return (_context.Contactus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
