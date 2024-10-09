using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly INursePatientRepository _nursePatientRepository;
        private readonly IAppointmentRepository appointmentRepository;

        public HomeController(IDoctorRepository genericRepository, INursePatientRepository nursePatientRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = genericRepository;
            _nursePatientRepository = nursePatientRepository;
            this.appointmentRepository = appointmentRepository;
        }

        public IActionResult Index()
        {
            var doc = appointmentRepository.Get(1);
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
