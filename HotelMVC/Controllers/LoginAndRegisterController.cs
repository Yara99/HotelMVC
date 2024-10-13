using HotelMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelMVC.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        public LoginAndRegisterController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Userfname,Userlname,Userphone,Useremail,ImageFile")] User user, string username, string password)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists in the Userlogins table
                var existingUsername= await _context.Userlogins.FirstOrDefaultAsync(u => u.Username == username);
                if (existingUsername != null)
                {
                    ModelState.AddModelError(String.Empty, "The username already exists. Please choose another one.");
                    return View(user);
                }

                if (user.ImageFile != null)
                {
                    // 1- get rootpath
                    string wwwRootPath = _webHostEnviroment.WebRootPath;

                    //2- filename
                    //Guid.NewGuid() ==> generate unique string
                    //dhchcvhcbdjcnbhcbhc_Aseel.png
                    //wiueyrueiryeuirueiori_Aseel.png
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;

                    //3- path == ~/Images/dhchcvhcbdjcnbhcbhc_Aseel.png
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    //4- upload image to folder images  
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }

                    user.Imagepath = fileName;
                }

                _context.Add(user);// insert custid + fname + lname + imagepath into table users 
                await _context.SaveChangesAsync();

                // insert username + password + roleid +custid into table userlogin 
                Userlogin userr = new Userlogin();
                userr.Userid = user.Userid;
                userr.Username = username;
                userr.Userpassword = password;
                userr.Roleid = 2;

                _context.Add(userr);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "LoginAndRegister");
            }
            return View(user);
        }


        //Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username, Userpassword")] Userlogin userlogin)
        {

            var auth = _context.Userlogins.Where(x => x.Username == userlogin.Username && x.Userpassword == userlogin.Userpassword).SingleOrDefault();

            // if the username entered not registered
            //var existingUsername = await _context.Userlogins.FirstOrDefaultAsync(u => u.Username == userlogin.Username || u.Userpassword == userlogin.Userpassword);
            if (auth == null)
            {
                ModelState.AddModelError(String.Empty, "Wrong username or password.");
                return View(userlogin);
            }


            if (auth != null)
            {
                if (auth.Roleid == 1)
                {
                    HttpContext.Session.SetInt32("UserId", (int)auth.Userid);
                    HttpContext.Session.SetString("UserName", auth.Username);

                    return RedirectToAction("Index", "Admin");
                }
                else if (auth.Roleid == 2)
                {

                    HttpContext.Session.SetInt32("UserId", (int)auth.Userid);
                    HttpContext.Session.SetString("UserName", auth.Username);

                    return RedirectToAction("Index", "Home");
                }
            }


            return View(userlogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }




    }
}
