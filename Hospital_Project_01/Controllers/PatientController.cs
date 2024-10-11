using BLL.Interfaces;
using BLL.Repositories;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;
using System.Diagnostics;

public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;
    private readonly UserManager<AppUser> _userManager;

    public PatientController(IPatientRepository patientRepository, UserManager<AppUser> userManager)
    {
        _patientRepository = patientRepository;
        _userManager = userManager;
    }


    public IActionResult Index()
    {
        var patients = _patientRepository.GetAll();
        return View(patients);
    }

    #region Details .. tested
    public IActionResult Details(int id)
    {
        var patient = _patientRepository.GetPatientWithAppointments(id);
        if (patient == null) return NotFound();

        return View(patient);
    }

    #endregion


    #region Add .. tested
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(PatientCreateVM model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _userManager.FindByIdAsync(model.AspNetUsersId) is null)
        {
            ModelState.AddModelError("AspNetUsersId", "Please register the patient with a system account before proceeding or make sure the id is right.");
            return View(model);
        }
        var newPatient = new Patient
        {
            Name = model.Name,
            Age = model.Age,
            Address = model.Address,
            History = model.History,
            AspNetUsersId = model.AspNetUsersId
        };

        try
        {
            _patientRepository.Add(newPatient);
            _patientRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    #endregion

    #region Edit .. tested

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var patient = _patientRepository.Get(id);
        if (patient == null) return NotFound();
        
        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Patient model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            _patientRepository.Update(model);
            _patientRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { 
            ModelState.AddModelError("", ex.Message);
            return View(model);  
        }

    }

    #endregion

    #region Delete
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var patient = _patientRepository.Get(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var patient = _patientRepository.Get(id);
        if (patient == null) return NotFound();


        try
        {
            //The delete action is CASCADE 
            await _userManager.DeleteAsync(await _userManager.FindByIdAsync(patient.AspNetUsersId));
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { return BadRequest(ex.Message); }

    }
    #endregion

}
