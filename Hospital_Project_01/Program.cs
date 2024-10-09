using BLL.Interfaces;
using BLL.Repositories;
using DAL.Data;
using System.ComponentModel.DataAnnotations;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region register services
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<HospitalDbContext>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>(); 
            builder.Services.AddScoped<IPatientRepository, PatientRepository>(); 
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>(); 
            builder.Services.AddScoped<INurseRepository, NurseRepository>(); 
            builder.Services.AddScoped<INursePatientRepository, NursePatientRepository>(); 
            builder.Services.AddScoped<IBillRepository, BillRepository>(); 
            builder.Services.AddScoped<IDrugRepository, DrugRepository>(); 
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); 
            builder.Services.AddScoped<IAdminRepository, AdminRepository>(); 
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>(); 

            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
