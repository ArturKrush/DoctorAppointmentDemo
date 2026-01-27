using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Data.Repositories;
using MyDoctorAppointment.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyDoctorAppointment.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public override string Path { get; set; }

        public override int LastId { get; set; }

        public AppointmentRepository(ISerializationService serializationService) : base(serializationService)
        {
            AppSettings result = ReadFromAppSettings();

            Path = result.Database.Appointments.Path;
            LastId = result.Database.Appointments.LastId;
        }

        public override void ShowInfo(Appointment appointment)
        {
            Console.WriteLine(appointment.Description);
            Console.WriteLine("Appointment was created at {0}", appointment.CreatedAt);
            Console.WriteLine("Appointment was updated at {0}", appointment.UpdatedAt);
            Console.WriteLine("Number is system: {0}", appointment.Id);

            Console.WriteLine("Doctor: {0}", appointment.Doctor.Name + appointment.Doctor.Surname);
            Console.WriteLine("Doctor's specialization: {0}", appointment.Doctor.DoctorType.ToString());
            Console.WriteLine("Doctor's phone number: {0}", appointment.Doctor.Phone);

            Console.WriteLine("Patient: {0}", appointment.Patient.Name + appointment.Patient.Surname);
            Console.WriteLine("Patient's illnes: {0}", appointment.Patient.IllnessType.ToString());
            Console.WriteLine("Patient's phone number: {0}", appointment.Patient.Phone);

            Console.WriteLine("From: ", appointment.DateTimeFrom);
            Console.WriteLine("Untill: ", appointment.DateTimeTo);
        }

        protected override void SaveLastId()
        {
            AppSettings result = ReadFromAppSettings();
            result.Database.Appointments.LastId = LastId;

            File.WriteAllText(Constants.AppSettingsPath, result.ToString());
        }
    }
}
