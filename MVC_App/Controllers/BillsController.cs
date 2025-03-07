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
    public class BillsController : Controller
    {
        private readonly MVCApp _context;

        public BillsController(MVCApp context)
        {
            _context = context;
        }

        private bool IsClient()
        {
            var userType = HttpContext.Session.GetString("UserType");
            return userType == "Clients"; 
        }

        private bool IsSeller()
        {
            var userType = HttpContext.Session.GetString("UserType");
            return userType == "Sellers"; 
        }

        public async Task<IActionResult> Index()
        {
            
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("LogIn", "Home"); 
            }

            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                TempData["AlertMessage"] = "You need to log in!";
                return RedirectToAction("Index", "Home");
            }

            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId.ToString() == userId);

            if (client == null)
            {
                return NotFound();
            }

            var phoneNumber = client.PhoneNumber;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                TempData["AlertMessage"] = "No phone number found for this client!";
                return RedirectToAction("Index", "Home");
            }

            var bills = await _context.Bills
                .Where(b => b.PhoneNumber == phoneNumber)
                .Include(b => b.PhoneNumberNavigation)  
                .ToListAsync();

            return View(bills); 
        }


        [HttpGet]
        public IActionResult ShowClientDept()
        {
            if (!IsSeller()) 
            {
                TempData["AlertMessage"] = "You need to log in as Seller!";
                return RedirectToAction("LogIn", "Home"); 
            }
            return View();
        }


        // GET: Bills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .Include(b => b.PhoneNumberNavigation)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        [HttpPost]
        public IActionResult Dept(int? userId)
        {

            if (!userId.HasValue)
            {
                ViewData["ErrorMessage"] = "Παρακαλώ εισάγετε έναν έγκυρο κωδικό χρήστη.";
                return View("ShowClientDept");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId.Value);

            if (user != null)
            {
                var bill = (from b in _context.Bills
                            join c in _context.Clients on b.PhoneNumber equals c.PhoneNumber
                            where c.UserId == user.UserId
                            select b).FirstOrDefault();

                if (bill != null)
                {
                    return RedirectToAction("Details", new { id = bill.BillId });
                }

                ViewData["ErrorMessage"] = $"Δεν βρέθηκε λογαριασμός για τον χρήστη με κωδικό: {userId}";
                return View("ShowClientDept");
            }

            ViewData["ErrorMessage"] = $"Δεν βρέθηκε χρήστης με κωδικό: {userId}";
            return View("ShowClientDept");
        }



        // POST: Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillId,PhoneNumber,Costs")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhoneNumber"] = new SelectList(_context.Phones, "PhoneNumber", "PhoneNumber", bill.PhoneNumber);
            return View(bill);
        }

        // GET: Bills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            ViewData["PhoneNumber"] = new SelectList(_context.Phones, "PhoneNumber", "PhoneNumber", bill.PhoneNumber);
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillId,PhoneNumber,Costs")] Bill bill)
        {
            if (id != bill.BillId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.BillId))
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
            ViewData["PhoneNumber"] = new SelectList(_context.Phones, "PhoneNumber", "PhoneNumber", bill.PhoneNumber);
            return View(bill);
        }

        // GET: Bills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills
                .Include(b => b.PhoneNumberNavigation)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill != null)
            {
                _context.Bills.Remove(bill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }
    }
}
