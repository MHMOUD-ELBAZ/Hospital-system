using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;


namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BillController : Controller
    {
        private readonly IBillRepository _billRepository;
        private readonly IPatientRepository _patientRepository;

        public BillController(IBillRepository billRepository, IPatientRepository patientRepository)
        {
            _billRepository = billRepository;
            _patientRepository = patientRepository;
        }

        [Authorize(Roles = "Admin, Patient")]
        public IActionResult Index(int? patientId, int? billId)
        {           
            
            if(patientId.HasValue) 
                return View(_billRepository.GetBillsForPatient(patientId.Value));

            if (billId.HasValue)
            {
                var bill = _billRepository.Get(billId.Value);
                return View(bill is null ? null : new List<Bill> { bill });
            }


            if (User.IsInRole("Patient"))
                return View();

            return View(_billRepository.GetAll());
        }


        [HttpPost]
		public IActionResult MarkAsPaid(int id)
		{
			var bill = _billRepository.Get(id);
			if (bill == null) return NotFound();

			bill.Paid = true;
			_billRepository.Update(bill);
			_billRepository.Save();

			return RedirectToAction(nameof(Index), new {billId = id});
		}


		#region Add . tested
		[HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(BillAddVM bill)
        {
            if (!ModelState.IsValid)
                return View(bill);

            Bill entity = new Bill { Amount = bill.Amount, Date = bill.Date, PatientId = bill.PatientId, Paid = bill.Paid}; 
            Patient? patient = _patientRepository.Get(bill.PatientId);
            
            if (patient == null)
            {
                ModelState.AddModelError("PatientId", "No such patient with this ID.");
                return View(bill);
            }
            
            try
            {
                _billRepository.Add(entity);
                _billRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        #endregion

        #region Edit .. tested
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bill = _billRepository.Get(id);
            if (bill == null) return NotFound();

            BillEditVM vM = new BillEditVM() {
                Id = id,
                Amount = bill.Amount,
                Date = bill.Date,
                PatientId = bill.PatientId,
                Paid = bill.Paid
            };

            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BillEditVM bill)
        {
            if (!ModelState.IsValid)
                return View(bill);

            Bill entity = new Bill { Amount = bill.Amount, Date = bill.Date, PatientId = bill.PatientId, Paid = bill.Paid, Id = bill.Id };

            try
            {
                _billRepository.Update(entity);
                _billRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete . tested
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var bill = _billRepository.GetBillWithPatient(id);
            if (bill == null) return NotFound();

            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Bill bill)
        {
            if (!ModelState.IsValid) return View(bill);

            try
            {
                _billRepository.Delete(bill);
                _billRepository.Save();
                return RedirectToAction("Index" , new { patientId = bill.PatientId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Details . tested
        [Authorize(Roles = "Admin, Patient")]
        public IActionResult Details(int id)
        {
            var bill = _billRepository.GetBillWithPatient(id);
            if (bill == null) return NotFound();

            return View(bill);
        }
        #endregion
    }
}


