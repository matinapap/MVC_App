using Microsoft.EntityFrameworkCore;
using MVC_App.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Καταχωρούμε το IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Προσθήκη υπηρεσιών για session
builder.Services.AddDistributedMemoryCache(); // Χρησιμοποιούμε την μνήμη για την αποθήκευση της συνεδρίας
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Χρόνος διάρκειας του session
    options.Cookie.HttpOnly = true; // Για αυξημένη ασφάλεια
    options.Cookie.IsEssential = true; // Σημαντικό για συμμόρφωση με τον GDPR
});

// Προσθήκη υπηρεσιών για controllers και views
builder.Services.AddControllersWithViews();

// Ρύθμιση για το Entity Framework και τη σύνδεση με την βάση δεδομένων
builder.Services.AddDbContext<MVCApp>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Διαμόρφωση του HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Αν δεν είναι σε περιβάλλον ανάπτυξης, ενεργοποιείται το HSTS για μεγαλύτερη ασφάλεια
}

// Χρήση session middleware για την διαχείριση των συνεδριών
app.UseSession();

app.UseHttpsRedirection(); // Ανακατεύθυνση σε HTTPS
app.UseStaticFiles(); // Εξυπηρέτηση static αρχείων (CSS, JS, εικόνες κτλ.)

app.UseRouting(); // Καθορισμός των routes για την εφαρμογή

app.UseAuthorization(); // Ενεργοποίηση εξουσιοδότησης για την εφαρμογή

// Χάρτης για τα controllers και τα actions
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run(); // Εκκίνηση της εφαρμογής
