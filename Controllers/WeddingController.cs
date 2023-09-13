using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Wedding_Planner.Controllers;

[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;

    private MyContext _context;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }


    [HttpGet("weddings")]
    public IActionResult ViewAllWeddings()
    {
        List<Wedding> AllWeddings = _context.Weddings.Include(w => w.Planner).Include(w => w.UsersGuest).ToList();
        return View(AllWeddings);
    }





    [HttpGet("weddings/new")]
    public ViewResult NewWedding()
    {
        return View();
    }

    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!ModelState.IsValid)
        {
            return View("NewWedding");
        }

        newWedding.UserId = (int)HttpContext.Session.GetInt32("UUID");
        _context.Add(newWedding);
        _context.SaveChanges();
        return ViewWedding(newWedding.WeddingId);
    }



    [HttpPost("weddings/{id}")]
    public IActionResult RegisterUser(User newUser)
    {
        if (ModelState.IsValid)
        {

            PasswordHasher<User> Hasher = new();

            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

            _context.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UUID", newUser.UserId);
            return RedirectToAction("Success");
        }
        else
        {
            return View("Index");
        }
    }


    [HttpPost("weddings/{id}/delete")]
    public RedirectToActionResult DeleteWedding(int id)
    {
        Wedding? toDelete = _context.Weddings.SingleOrDefault(w => w.WeddingId == id);
        if (toDelete != null)
        {
            _context.Remove(toDelete);
            _context.SaveChanges();
        }

        return RedirectToAction("ViewAllWeddings");
    }

    [HttpPost("weddings/{id}/rsvp")]
    public RedirectToActionResult ToggleRSVP(int id)
    {
        int UUID = (int)HttpContext.Session.GetInt32("UUID");
        UserWeddingRSVP existingRSVP = _context.UserWeddingRSVPs.FirstOrDefault(r => r.WeddingId == id && r.UserId == UUID);
        if (existingRSVP == null)
        {
            UserWeddingRSVP newRSVP = new()
            {
                UserId = UUID,
                WeddingId = id
            };
            _context.Add(newRSVP);
        }
        else
        {
            _context.Remove(existingRSVP);
        }
        _context.SaveChanges();
        return RedirectToAction("ViewAllWeddings");

    }

    [HttpGet("weddings/{id}/view")]
    public IActionResult ViewWedding(int id)
    {
        Wedding? oneWedding = _context.Weddings.Include(w => w.Planner).Include(w => w.UsersGuest).ThenInclude(ug => ug.Guest).FirstOrDefault(w => w.WeddingId == id);

        if (oneWedding == null)
        {
            return RedirectToAction("ViewAllWeddings");
        }
        return View("ViewWedding", oneWedding);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
