using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Hospital_Project_01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hospital_Project_01.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public HomeController(IDoctorRepository genericRepository)
        {
            _doctorRepository = genericRepository;
        }

        public IActionResult Index()
        {
            var doc = _doctorRepository.Get(1);
            return Json(doc);
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
