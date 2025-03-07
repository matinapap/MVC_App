using Microsoft.AspNetCore.Mvc;
using MVC_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MVC_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly MVCApp _context;

        public HomeController(MVCApp context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userType = HttpContext.Session.GetString("UserType");

            if (!string.IsNullOrEmpty(userType))
            {
                if (userType == "Clients")
                {
                    ViewData["UserStatus"] = $"Είστε συνδεδεμένος ως Πελάτης.";
                }
                if (userType == "Admins")
                {
                    ViewData["UserStatus"] = $"Είστε συνδεδεμένος ως Διαχειριστής.";
                }
                if (userType == "Sellers")
                {
                    ViewData["UserStatus"] = $"Είστε συνδεδεμένος ως Πωλητής.";
                }
            }
            else
            {
                ViewData["UserStatus"] = "Δεν έχετε συνδεθεί. Παρακαλώ συνδεθείτε.";
            }

            return View();
        }

        public IActionResult LogIn()
        {
            return View(); 
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, int userId)
        {
            if (string.IsNullOrEmpty(username))
            {
                TempData["Message"] = "Το όνομα χρήστη είναι υποχρεωτικό.";
                return RedirectToAction("Index");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.UserId == userId);

            if (user != null)
            {
                if (_context.Clients.Any(c => c.UserId == user.UserId))
                {
                    var client = await _context.Clients.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == user.UserId);
                    HttpContext.Session.SetString("UserType", "Clients");
                    HttpContext.Session.SetString("UserId", user.UserId.ToString()); 
                    return RedirectToAction("Index", "Home");
                }
                else if (_context.Sellers.Any(s => s.UserId == user.UserId))
                {
                    var seller = await _context.Sellers.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == user.UserId);
                    HttpContext.Session.SetString("UserType", "Sellers");
                    HttpContext.Session.SetString("UserId", user.UserId.ToString()); 
                    return RedirectToAction("Index", "Home");
                }
                else if (_context.Admins.Any(a => a.UserId == user.UserId))
                {
                    var admin = await _context.Admins.Include(a => a.User).FirstOrDefaultAsync(a => a.UserId == user.UserId);
                    HttpContext.Session.SetString("UserType", "Admins");
                    HttpContext.Session.SetString("UserId", user.UserId.ToString()); 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Ο χρήστης δεν ανήκει σε καμία κατηγορία.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "Λάθος όνομα χρήστη ή κωδικός.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");  
        }
    }
}
