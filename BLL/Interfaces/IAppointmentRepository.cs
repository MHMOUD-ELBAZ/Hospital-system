using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        IEnumerable<Appointment>? GetAppointments(int doctorId, int patientId);
        IEnumerable<Appointment>? GetAppointmentsForDoctor(int doctorId);
        IEnumerable<Appointment>? GetAppointmentsForPatient(int patientId);
    }
}
