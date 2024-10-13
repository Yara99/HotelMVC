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
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            if (uid.HasValue)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;
                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.RoleId = roleid.Roleid;
            }

            var modelContext = _context.Testimonials.Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            var uid = (decimal)HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;

            /* var users = _context.Userlogins
             .Include(u => u.User)
             .Where(x => x.Roleid == 2)
             .Select(x => new { x.User.Userid, x.User.Userfname })
             .ToList();

             ViewData["Userid"] = new SelectList(users, "Userid", "Userfname");*/
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Testimonialid,Testimonialcontent,Testimonialdate,Testimonialstatus,Userid")] Testimonial testimonial)
        {
            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;

            if (string.IsNullOrWhiteSpace(testimonial.Testimonialcontent))
            {
                ModelState.AddModelError(String.Empty, "Enteryour feedback.");
                return View(testimonial);
            }

            if (ModelState.IsValid)
            {
                testimonial.Testimonialstatus = "pending";
                testimonial.Userid = uid;
                testimonial.Testimonialdate = DateTime.Now.Date;


                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            /*ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Userid);*/
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }

            //role id for users, 
            var users = _context.Userlogins
           .Include(u => u.User)
           .Where(x => x.Roleid == 2)
           .Select(x => new { x.User.Userid, x.User.Userfname })
           .ToList();

            ViewData["Userid"] = new SelectList(users, "Userid", "Userfname", testimonial.Userid);
            //ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userfname", testimonial.Userid);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Testimonialcontent,Testimonialdate,Testimonialstatus,Userid")] Testimonial testimonial)
        {
            if (id != testimonial.Testimonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Testimonialid))
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
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Userid);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = (decimal)HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;


            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var uid = (decimal)HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;


            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
            return (_context.Testimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<IActionResult> ChangeTestimonialStatus(decimal id, string status)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            testimonial.Testimonialstatus = status;
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Testimonials");
        }



    }
}
