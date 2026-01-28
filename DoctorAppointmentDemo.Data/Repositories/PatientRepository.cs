using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using System;
using System.Numerics;
using System.Xml.Linq;

namespace MyDoctorAppointment.Data.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public override string Path { get; set; }

        public override int LastId { get; set; }

        public PatientRepository(ISerializationService serializationService) : base(serializationService)
        {
            AppSettings result = ReadFromAppSettings();

            Path = result.Database.Patients.Path;
            LastId = result.Database.Patients.LastId;
        }

        public override void ShowInfo(Patient patient)
        {
            Console.WriteLine(patient.Name + " " + patient.Surname);
            //Console.WriteLine("Number is system: {0}", patient.Id);
            Console.WriteLine("Patients address: {0}", patient.Address);
            Console.WriteLine("Phone number: {0}", patient.Phone);
            Console.WriteLine("Email: {0}", patient.Email);
            Console.WriteLine("Parents illness is: {0}", patient.IllnessType.ToString());
            Console.WriteLine("Profile was created at {0}", patient.CreatedAt);
            Console.WriteLine("Profile was updated at {0}", patient.UpdatedAt);
        }

        protected override void SaveLastId()
        {
            XDocument xDoc = XDocument.Load(Constants.AppSettingsPath);
            XElement? xCommon = xDoc.Element("Database").Element("Patients");
            XElement? lastId;

            switch (SerializationService.Storage)
            {
                case Domain.Enums.StorageTypes.JSON:
                    lastId = xCommon.Element("JSONSource").Element("LastId");
                    if (lastId != null)
                        lastId.Value = LastId.ToString();
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId in Patients/JSONSource");
                    break;
                case Domain.Enums.StorageTypes.XML:
                    lastId = xCommon.Element("XMLSource").Element("LastId");
                    if (lastId != null)
                        lastId.Value = LastId.ToString();
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId in Patients/XMLSource");
                    break;
            }

            xDoc.Save(Constants.AppSettingsPath);
        }
    }
}
