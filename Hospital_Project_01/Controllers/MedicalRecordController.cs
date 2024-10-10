using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PL.ViewModels;

namespace PL.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordController(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public IActionResult Index(int patientId)
        {
            ViewBag.PatientId = patientId;
            return View(_medicalRecordRepository.GetPatientRecords(patientId));
        }

        #region Details
        [HttpGet]
        public IActionResult Details(int recordId) 
        { 
            MedicalRecord? record = _medicalRecordRepository.GetRecordWithDrugs(recordId);
            
            if(record is null) return NotFound();

            return View(record); 
        }
        #endregion

        #region Add record

        [HttpGet]
        public IActionResult Add(int patientId)
        {
            AddRecordVM vm = new AddRecordVM() { PatientId = patientId };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Add(AddRecordVM recordVM)
        {
            if (!ModelState.IsValid) return View(recordVM);

            MedicalRecord record = new MedicalRecord()
            {
                PatientId = recordVM.PatientId,
                Diagnose = recordVM.Diagnose,
                Drugs = recordVM.Drugs
            };

            try
            {
                _medicalRecordRepository.Add(record);
                _medicalRecordRepository.Save();
                return RedirectToAction(nameof(Index), new { patientId = recordVM.PatientId });
            }
            catch (Exception ex) { return BadRequest(ModelState); }
        }

        #endregion

        #region Delete record
        public IActionResult Delete(int recordId) 
        {
            MedicalRecord? record = _medicalRecordRepository.GetRecordWithDrugs(recordId);

            if (record is null) return NotFound();

            return View(record);
        }

        [HttpPost]
        public IActionResult Delete(MedicalRecord record)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _medicalRecordRepository.Delete(record);
                _medicalRecordRepository.Save();
                return RedirectToAction(nameof(Index), new { recordId = record.PatientId }); 
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        #endregion
    }
}
