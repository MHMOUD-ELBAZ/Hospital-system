using BLL.Interfaces;
using BLL.Repositories;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using PL.ViewModels;

namespace PL.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly UserManager<AppUser> _userManager;

        public DoctorController(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository, 
            IDepartmentRepository departmentRepository, UserManager<AppUser> userManager)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _departmentRepository = departmentRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //check if the user is in doctor role, redirect him to the schedule 

            return View(_doctorRepository.GetAll());
        }


        public IActionResult Schedule(int id) 
        {
            IEnumerable<Appointment>? schedule = _appointmentRepository.GetDailyAppointmentsForDoctor(id);

            if (schedule == null) return NotFound();

            return View(schedule);
        }


        #region Add . tested
        [HttpGet]
        public IActionResult Add(AddDoctorVM? doctorVM)
        {
            // Retrieve any necessary data for the view, such as departments
            var departments = _departmentRepository.GetAll();
            ViewBag.Departments = departments;

            return doctorVM is null ? View(doctorVM) : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(AddDoctorVM doctorVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index) , doctorVM); 

            if(await _userManager.FindByIdAsync(doctorVM.AspNetUsersId) is null)
            {
                ModelState.AddModelError("AspNetUsersId", "Please register the doctor with a system account before proceeding.");
                var departments = _departmentRepository.GetAll();
                ViewBag.Departments = departments;
                return View(doctorVM);
            }


            Doctor newDoctor = new Doctor
            {
                Name = doctorVM.Name,
                Age = doctorVM.Age,
                Address = doctorVM.Address,
                Salary = doctorVM.Salary,
                Rank = doctorVM.Rank,
                Shift = doctorVM.Shift,
                AspNetUsersId = doctorVM.AspNetUsersId,  
                DepartmentId = doctorVM.DepartmentId
            };

            try
            {
                _doctorRepository.Add(newDoctor);
                _doctorRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }
        #endregion


        #region Edit . tested

        public IActionResult Edit(int id)
        {
            var doctor = _doctorRepository.Get(id);
            if (doctor == null) return NotFound();


            var doctorVM = new AddDoctorVM
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Age = doctor.Age,
                Address = doctor.Address,
                Salary = doctor.Salary,
                Rank = doctor.Rank,
                Shift = doctor.Shift,
                AspNetUsersId = doctor.AspNetUsersId,
                DepartmentId = doctor.DepartmentId
            };

            var departments = _departmentRepository.GetAll();
            ViewBag.Departments = departments;

            return View(doctorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AddDoctorVM doctorVM)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentRepository.GetAll();
                ViewBag.Departments = departments;
                return View(doctorVM);
            }

            var doctor = _doctorRepository.Get(doctorVM.Id);
            if (doctor == null || doctor.AspNetUsersId != doctorVM.AspNetUsersId) return NotFound();

            doctor.Name = doctorVM.Name;
            doctor.Age = doctorVM.Age;
            doctor.Address = doctorVM.Address;
            doctor.Salary = doctorVM.Salary;
            doctor.Rank = doctorVM.Rank;
            doctor.Shift = doctorVM.Shift;
            doctor.DepartmentId = doctorVM.DepartmentId;

            try
            {
                _doctorRepository.Update(doctor);
                _doctorRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

        }


        #endregion


        #region Delete . tested
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var doctor = _doctorRepository.GetDoctorWithDepartment(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Doctor doctor)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _doctorRepository.Delete(doctor);
                _doctorRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        #endregion


        #region Details . tested

        public IActionResult Details(int id) 
        {
            var doctor = _doctorRepository.GetDoctorWithDepartment(id);
            if (doctor == null) return NotFound();  

            return View (doctor);
        }

        #endregion
    }
}
