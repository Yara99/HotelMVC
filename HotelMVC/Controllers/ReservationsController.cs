using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMVC.Models;

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

using System.Net;
using System.Net.Mail;


namespace HotelMVC.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ModelContext _context;

        public ReservationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Reservations.Include(r => r.Room).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {

            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create(decimal? roomId, decimal? userId)
        {

            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;


            /*var users = _context.Userlogins
           .Include(u => u.User)
           .Where(x => x.Roleid == 2)
           .Select(x => new { x.User.Userid, x.User.Userfname })
           .ToList();

            ViewData["Userid"] = new SelectList(users, "Userid", "Userfname");

            ViewData["Roomid"] = new SelectList(_context.Rooms.Where(r=>r.Availabilitystatus == "available"), "Roomid", "Roomid");*/
            //ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");

            userId = uid;
            var reservation = new Reservation
            {
                Roomid = roomId, //sets the Roomid
                Userid = userId, //sets the Userid
            };

            return View(reservation);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reservationid,Checkindate,Checkoutdate,Totalprice,Userid,Roomid")] Reservation reservation, decimal roomId)
        {

            if (ModelState.IsValid)
            {
                var room = await _context.Rooms.FindAsync(reservation.Roomid);
                //var room = await _context.Rooms.FindAsync(roomId);
                if (room == null)
                {
                    return NotFound();
                }

                if (room.Availabilitystatus == "available")
                {
                    var totalDays = (reservation.Checkoutdate.Value - reservation.Checkindate.Value).Days + 1;
                    var totalPrice = totalDays * room.Roomprice;
                    reservation.Totalprice = totalPrice;
                    room.Availabilitystatus = "booked";

                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                    //return RedirectToAction("Index", "Home");
                    //return RedirectToAction("BookNow", reservation);
                }
            }
            /*ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservation.Roomid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", reservation.Userid);*/
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var users = _context.Userlogins
           .Include(u => u.User)
           .Where(x => x.Roleid == 2)
           .Select(x => new { x.User.Userid, x.User.Userfname })
           .ToList();

            ViewData["Userid"] = new SelectList(users, "Userid", "Userfname");


            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservation.Roomid);
            //ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", reservation.Userid);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Reservationid,Checkindate,Checkoutdate,Totalprice,Userid,Roomid")] Reservation reservation)
        {
            if (id != reservation.Reservationid)
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
                    var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                    ViewBag.Fname = user.Userfname;
                    ViewBag.Lname = user.Userlname;
                    ViewBag.UserImage = user.Imagepath;
                    var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
                    ViewBag.RoleId = roleid.Roleid;


                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Reservationid))
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
            ViewData["Roomid"] = new SelectList(_context.Rooms, "Roomid", "Roomid", reservation.Roomid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", reservation.Userid);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;


            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Reservationid == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ModelContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {

                var room = await _context.Rooms.FindAsync(reservation.Roomid);
                room.Availabilitystatus = "available";


                var uid = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

                //object of the user (all user's data)
                var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.Fname = user.Userfname;
                ViewBag.Lname = user.Userlname;
                ViewBag.UserImage = user.Imagepath;
                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.RoleId = roleid.Roleid;



                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(decimal id)
        {
            return (_context.Reservations?.Any(e => e.Reservationid == id)).GetValueOrDefault();
        }







        public IActionResult BookNow(decimal? roomId)
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

                var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
                ViewBag.RoleId = roleid.Roleid;               
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }


            //userId = uid;
            var reservation = new Reservation
            {
                Roomid = roomId, //sets the Roomid
                Userid = uid, //sets the Userid
            };


            var room = _context.Rooms.SingleOrDefault(r => r.Roomid == roomId);
            var hotel = _context.Hotels.FirstOrDefault(h => h.Hotelid == room.Hotelid);
            ViewBag.hotelId = hotel.Hotelid;

            return View(reservation);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookNow([Bind("Reservationid,Checkindate,Checkoutdate,Totalprice,Userid,Roomid")] Reservation reservation)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var uid = HttpContext.Session.GetInt32("UserId");
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;

            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;

            if (ModelState.IsValid)
            {
                var room = await _context.Rooms.FindAsync(reservation.Roomid);
                if (room == null)
                {
                    return NotFound();
                }

                var today = DateTime.Now.Date;
                //عشان نتاكد انه تاريخ الدخول اكبر من تاريخ اليوم
                if (reservation.Checkindate.Value < today || reservation.Checkoutdate.Value <= reservation.Checkindate.Value)
                {
                    ModelState.AddModelError(String.Empty, "Make sure the check-in date is not before today and that the check-out date is after the check-in date.");
                    return View("BookNow", reservation);
                }

                if (room.Availabilitystatus == "available")
                {
                    var totalDays = (reservation.Checkoutdate.Value - reservation.Checkindate.Value).Days + 1;
                    var totalPrice = totalDays * room.Roomprice;
                    reservation.Totalprice = totalPrice;
                    
                    ViewBag.RoomDesc = room.Roomdescription;
                    ViewBag.RoomPrice = room.Roomprice;
                    ViewBag.TotalDays = totalDays;

                    return View("PaymentDetails", reservation);

                }
                else
                {
                    ModelState.AddModelError(String.Empty, "The room is not available");
                    return View("BookNow", reservation);
                }
            }
               
            return View(reservation);
        }

        public IActionResult PaymentDetails(Reservation reservation)
        {
            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;



            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;

            return View("Payment", reservation);
        }


        public IActionResult Payment([Bind("Id,Cardnumber,Cvv,Expirydate,Balance")] Bankaccount bank, 
            Reservation reservation)
        {
            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;


            var uid = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            //object of the user (all user's data)
            var user = _context.Users.Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.Fname = user.Userfname;
            ViewBag.Lname = user.Userlname;
            ViewBag.UserImage = user.Imagepath;
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;

            var card = _context.Bankaccounts.Where(c => c.Cardnumber == bank.Cardnumber
            && c.Cvv == bank.Cvv && c.Expirydate == bank.Expirydate).FirstOrDefault();
            if (card == null)
            {
                return View("Payment", reservation);
            }

            //return View(reservation);
            return RedirectToAction("PayCheck", reservation);
        }

        public class SenderEmailInfo
        {
            public string SenderEmail { get; set; }
            public string SenderPassword { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayCheck([Bind("Id,Cardnumber,Cvv,Expirydate,Balance")] Bankaccount bank, 
            Reservation reservation)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            var uuid = HttpContext.Session.GetInt32("UserId");
            var roleid = _context.Userlogins.Include(u => u.Role).Where(x => x.Userid == uuid).SingleOrDefault();
            ViewBag.RoleId = roleid.Roleid;

            var lastRecordHome = _context.Homes.OrderByDescending(h => h.Id).FirstOrDefault();
            ViewBag.HomeLogo = lastRecordHome.Logo;

            var lastRecordContact = _context.Contactus.OrderByDescending(c => c.Id).FirstOrDefault();
            ViewBag.ContactEmail = lastRecordContact.Contactemail;
            ViewBag.ContactPhone = lastRecordContact.Contactphone;
            ViewBag.ContactAddress = lastRecordContact.Contactaddress;


            //check  the balance 
            var card = _context.Bankaccounts.Where(c => c.Cardnumber == bank.Cardnumber
            && c.Cvv == bank.Cvv && c.Expirydate == bank.Expirydate).FirstOrDefault();

            if (card == null || bank.Balance < reservation.Totalprice)
            {
                ModelState.AddModelError(String.Empty, "Invalid card details or insufficient balance");
                return View("Payment", reservation);
            }

            card.Balance -= reservation.Totalprice;

            var room = await _context.Rooms.FindAsync(reservation.Roomid);
            if (room != null)
            {
                room.Availabilitystatus = "booked";
                //_context.Update(room);
            }
            //_context.Update(card);
            _context.Add(reservation);
            await _context.SaveChangesAsync();

            byte[] invoice = GenerateInvoice(reservation);

            var uid = reservation.Userid;
            var user = await _context.Users.FindAsync(uid);

            //the admin
            var sender = await _context.Userlogins.Include(u => u.User).Include(r => r.Role).Where(x => x.Roleid == 1).FirstOrDefaultAsync();

            SenderEmailInfo senderEmailInfo = new SenderEmailInfo
            {
                SenderEmail = sender.User.Useremail,
                SenderPassword = "anotheraccount-outlook"
            };

            SendInvoiceEmail(senderEmailInfo, user.Useremail, invoice);

            //System.IO.File.WriteAllBytes(@"C:\Users\juman\Downloads\invoice.pdf", invoice);


            return RedirectToAction("Index", "Home");
        }

        public byte[] GenerateInvoice(Reservation reservation)
        {
            var uid = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Where(u=>u.Userid == uid).SingleOrDefault();
            using (MemoryStream doc = new MemoryStream())
            {
                //Creates a PdfWriter instance that writes PDF content to the MemoryStream (doc)
                var writer = new PdfWriter(doc);
                //The PdfDocument class represents the PDF document itself
                var pdf = new PdfDocument(writer);
                //The Document class provides an API to add elements like paragraphs to the PDF document
                var document = new Document(pdf);

                document.Add(new Paragraph("Invoice")
                .SetFontSize(24)
                .SetBold()
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph($"Name: {user.Userfname + " " + user.Userlname}"));
                document.Add(new Paragraph($"Check-in Date: {reservation.Checkindate.Value.ToShortDateString()}"));
                document.Add(new Paragraph($"Check-out Date: {reservation.Checkoutdate.Value.ToShortDateString()}"));
                document.Add(new Paragraph($"Total Price: ${reservation.Totalprice}"));
                document.Add(new Paragraph($"Room Description: {reservation.Room.Roomdescription}"));
                document.Add(new Paragraph($"Room Price per day: ${reservation.Room.Roomprice}"));
                var totalDays = (reservation.Checkoutdate.Value - reservation.Checkindate.Value).Days + 1;
                document.Add(new Paragraph($"Total Days: {totalDays}"));


                document.Close();
                return doc.ToArray();
            }

        }


        public void SendInvoiceEmail(SenderEmailInfo senderEmailInfo, string toUserEmail, byte[] pdfInvoice)
        {
            string subject = "Your Booking Invoice";
            string body = "<!DOCTYPE html> <html> <body style=\"text-align:center;\">"
                + "<h1>Thanks for choosing us for your stay! </h1>"
                + "<h2>Please find your invoice attached for the booking made through our website.</h2>"
                + "</body></html>";
            string attachmentFileName = "invoice.pdf";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmailInfo.SenderEmail); // Sender email address
                mail.To.Add(toUserEmail); // Recipient email address
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                // Create an attachment from the byte array
                using (MemoryStream ms = new MemoryStream(pdfInvoice))
                {
                    Attachment attachment = new Attachment(ms, attachmentFileName, "application/pdf");
                    mail.Attachments.Add(attachment);

                    // Send the email
                    using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587)) // SMTP server address and port
                    {
                        //Use the email address as the username in NetworkCredential
                        //authenticate the sender with the SMTP server.
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(senderEmailInfo.SenderEmail, senderEmailInfo.SenderPassword); // SMTP credentials
                        smtp.EnableSsl = true; // Enable SSL if required
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        
                        smtp.Send(mail);
                        
                    }
                }
            }
        }








    }
}
