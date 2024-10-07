using DAL.Data;
using Hospital_Project_01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hospital_Project_01.Controllers
{
    public class HomeController : Controller
    {
        private readonly HospitalDbContext hospitalDbContext;


        public HomeController(HospitalDbContext hospitalDbContext)
        {
            this.hospitalDbContext = hospitalDbContext;
        }

        public IActionResult Index()
        {
            var list = hospitalDbContext.Admins.Include(a => a.AppUser); 
            return Json(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
