using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;

namespace PL.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AppointmentDetails(int appointmentId)
        {
            Appointment? appointment = _appointmentRepository.Get(appointmentId);
            Patient? patient = _patientRepository.GetPatientWithMedicalRecords(appointment?.PatientId ?? 0);

            if (appointment == null || patient == null) return NotFound();

            AppointmentDetailsVM appointmentDetails = new AppointmentDetailsVM
            {
                Id = appointmentId,
                Date = appointment.Date,
                Finished = appointment.Finished,
                History = patient.History,
                PatientId = patient.Id,
                MedicalRecords = patient.MedicalRecords,
                PatientAge = patient.Age,
                PatientName = patient.Name
            };

            return View(appointmentDetails);
        }
    }
}
