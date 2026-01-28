using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            //Console.WriteLine("Number is system: {0}", appointment.Id);

            //Console.WriteLine($"Doctor: {appointment.Doctor.Name} {appointment.Doctor.Surname}");
            //Console.WriteLine("Doctor's specialization: {0}", appointment.Doctor.DoctorType.ToString());
            //Console.WriteLine("Doctor's phone number: {0}", appointment.Doctor.Phone);

            Console.WriteLine($"Patient: {appointment.Patient.Name} {appointment.Patient.Surname}");
            Console.WriteLine("Patient's illness: {0}", appointment.Patient.IllnessType.ToString());
            Console.WriteLine("Patient's phone number: {0}", appointment.Patient.Phone);

            Console.WriteLine($"From: {appointment.DateTimeFrom}");
            Console.WriteLine($"Untill: {appointment.DateTimeTo}");
        }

        protected override void SaveLastId()
        {
            XDocument xDoc = XDocument.Load(Constants.AppSettingsPath);
            XElement? xCommon = xDoc.Element("Database").Element("Appointments");
            XElement? lastId;

            switch (SerializationService.Storage)
            {
                case Domain.Enums.StorageTypes.JSON:
                    lastId = xCommon.Element("JSONSource").Element("LastId");
                    if (lastId != null)
                        lastId.Value = LastId.ToString();
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId in Appointments/JSONSource");
                    break;
                case Domain.Enums.StorageTypes.XML:
                    lastId = xCommon.Element("XMLSource").Element("LastId");
                    if (lastId != null)
                        lastId.Value = LastId.ToString();
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId in Appointments/XMLSource");
                    break;
            }

            xDoc.Save(Constants.AppSettingsPath);
        }
    }
}
