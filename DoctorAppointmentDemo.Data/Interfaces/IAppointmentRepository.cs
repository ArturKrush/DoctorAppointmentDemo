using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;

using System;

namespace MyDoctorAppointment.Data.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        // you can add more specific appointment methods
    }
}