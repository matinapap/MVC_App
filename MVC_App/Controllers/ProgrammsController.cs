using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_App.Models;

namespace MVC_App.Controllers
{
    public class ProgrammsController : Controller
    {
        private readonly MVCApp _context;

        public ProgrammsController(MVCApp context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var userType = HttpContext.Session.GetString("UserType");
            return userType == "Admins"; 
        }

        private bool IsSeller()
        {
            var userType = HttpContext.Session.GetString("UserType");
            return userType == "Sellers"; 
        }

        // GET: Programms
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) 
            {
                TempData["AlertMessage"] = "You need to log in as Admin!";
                return RedirectToAction("LogIn", "Home"); 
            }
            return View(await _context.Programms.ToListAsync());
        }

        [HttpGet]
        public IActionResult EditClientsProgram()
        {
            if (!IsSeller()) 
            {
                TempData["AlertMessage"] = "You need to log in as Seller!";
                return RedirectToAction("LogIn", "Home"); 
            }
            return View();
        }


        [HttpPost]
        public IActionResult Pergram(int? userId)
        {
            if (!userId.HasValue)
            {
                ViewData["ErrorMessage"] = "Παρακαλώ εισάγετε έναν έγκυρο κωδικό χρήστη.";
                return View("EditClientsProgram");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId.Value);

            if (user == null)
            {
                ViewData["ErrorMessage"] = $"Δεν βρέθηκε χρήστης με κωδικό: {userId}";
                return View("EditClientsProgram");
            }

            var program = (from client in _context.Clients
                           join phone in _context.Phones on client.PhoneNumber equals phone.PhoneNumber
                           join programm in _context.Programms on phone.ProgrammName equals programm.ProgrammName
                           where client.UserId == userId.Value
                           select programm).FirstOrDefault();

            if (program != null)
            {
                return RedirectToAction("Edit", new { id = program.ProgrammName });
            }

            ViewData["ErrorMessage"] = $"Δεν βρέθηκε πρόγραμμα για τον χρήστη με κωδικό: {userId}";
            return View("EditClientsProgram");
        }

        // GET: Programms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programm = await _context.Programms
                .FirstOrDefaultAsync(m => m.ProgrammName == id);
            if (programm == null)
            {
                return NotFound();
            }

            return View(programm);
        }

        // GET: Programms/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) 
            {
                TempData["AlertMessage"] = "You need to log in as Admin!";
                return RedirectToAction("LogIn", "Home"); 
            }
            return View();
        }

        // POST: Programms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgrammName,Benfits,Charge")] Programm programm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programm);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programm = await _context.Programms
                .FirstOrDefaultAsync(m => m.ProgrammName == id);
            if (programm == null)
            {
                return NotFound();
            }

            return View(programm);
        }


        // POST: Programms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProgrammName,Benfits,Charge")] Programm programm)
        {
            if (id != programm.ProgrammName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammExists(programm.ProgrammName))
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
            return View(programm);
        }

        // GET: Programms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programm = await _context.Programms
                .FirstOrDefaultAsync(m => m.ProgrammName == id);
            if (programm == null)
            {
                return NotFound();
            }

            return View(programm);
        }

        // POST: Programms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var programm = await _context.Programms.FindAsync(id);
            if (programm != null)
            {
                _context.Programms.Remove(programm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammExists(string id)
        {
            return _context.Programms.Any(e => e.ProgrammName == id);
        }
    }
}
