using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using System.Xml.Linq;

namespace MyDoctorAppointment.Data.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public override string Path { get; set; }

        public override int LastId { get; set; }

        public DoctorRepository(ISerializationService serializationService) : base(serializationService)
        {
            AppSettings settings = ReadFromAppSettings();

            Path = settings.Database.Doctors.Path;
            LastId = settings.Database.Doctors.LastId;
        }

        public override void ShowInfo(Doctor doctor)
        {
            Console.WriteLine(doctor.Name + " " + doctor.Surname);
            //Console.WriteLine("Number is system: {0}", doctor.Id);
            Console.WriteLine("Doctor's specialization: {0}", doctor.DoctorType.ToString());
            Console.WriteLine("Doctor's experience: {0} years", doctor.Experience);
            Console.WriteLine("Phone number: {0}", doctor.Phone);
            Console.WriteLine("Email: {0}", doctor.Email);
            Console.WriteLine("Doctor's salary: {0}", doctor.Salary);
            Console.WriteLine("Profile was created at {0}", doctor.CreatedAt);
            Console.WriteLine("Profile was updated at {0}", doctor.UpdatedAt);
        }

        protected override void SaveLastId()
        {
            XDocument xDoc = XDocument.Load(Constants.AppSettingsPath);
            XElement? xCommon = xDoc.Element("Database").Element("Doctors");
            XElement? lastId;

            switch (SerializationService.Storage)
            {
                case Domain.Enums.StorageTypes.JSON:
                    lastId = xCommon.Element("JSONSource").Element("LastId");
                    if(lastId != null)
                        lastId.Value = LastId.ToString();
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId in Doctors/JSONSource");
                    break;
                case Domain.Enums.StorageTypes.XML:
                    lastId = xCommon.Element("XMLSource").Element("LastId");
                    if (lastId != null)
                        lastId.Value = LastId.ToString();
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId in Doctors/XMLSource");
                    break;
            }

            xDoc.Save(Constants.AppSettingsPath);
        }
    }
}
