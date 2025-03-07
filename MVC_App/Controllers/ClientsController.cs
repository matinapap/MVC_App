using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_App.Models;

namespace MVC_App.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MVCApp _context;

        public ClientsController(MVCApp context)
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

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("LogIn", "Home"); 
            }
            else
            {
                var userId = HttpContext.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userId))
                {
                    ViewData["AlertMessage"] = "You need to log in!";
                    return RedirectToAction("Index", "Home");
                }

                var client = await _context.Clients
                    .Include(c => c.User)
                    .Where(c => c.UserId.ToString() == userId) 
                    .FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound();
                }

                return View(client); 
            }
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }
                var client = await _context.Clients
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            }
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            if (!IsSeller()) 
            {
                TempData["AlertMessage"] = "You need to log in as Seller!";
                return RedirectToAction("LogIn", "Home"); 
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
                return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Afm,PhoneNumber,User")] Client client)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    FirstName = client.User.FirstName,
                    LastName = client.User.LastName,
                    Username = client.User.Username,
                };

                client.UserId = newUser.UserId;

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Ο χρήστης καταχωρήθηκε με επιτυχία με ID: {client.UserId}";
            }
            else
            {
                TempData["SuccessMessage"] = $"Ο χρήστης δεν καταχωρήθηκε!";
            }

            return View(client);
        }


        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Clients.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", client.UserId);
                return View(client);
            }
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Afm,PhoneNumber,UserId")] Client client)
        {
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                if (id != client.ClientId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(client);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClientExists(client.ClientId))
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
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", client.UserId);
                return View(client);
            }
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Clients
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(m => m.ClientId == id);
                if (client == null)
                {
                    return NotFound();
                }

                return View(client);
            }
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsClient()) 
            {
                TempData["AlertMessage"] = "You need to log in as Client!";
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                var client = await _context.Clients.FindAsync(id);
                if (client != null)
                {
                    _context.Clients.Remove(client);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}
