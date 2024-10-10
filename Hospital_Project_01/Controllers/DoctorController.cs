using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using PL.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace PL.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;


        public DoctorController(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository, IPatientRepository patientRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }

        public IActionResult Index(int doctorId)
        {
            IEnumerable<Appointment>? schedule = _appointmentRepository.GetDailyAppointmentsForDoctor(doctorId);

            if(schedule == null) return NotFound();

            return View(schedule);
        }


    }
}
