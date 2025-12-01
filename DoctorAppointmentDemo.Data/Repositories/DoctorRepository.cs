using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;

namespace MyDoctorAppointment.Data.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ISerializationService serializationService;
        public override string Path { get; set; }

        public override int LastId { get; set; }

        public DoctorRepository()
        {
            AppSettings result = ReadFromAppSettings();

            Path = result.Database.Doctors.Path;
            LastId = result.Database.Doctors.LastId;
        }

        public override void ShowInfo(Doctor doctor)
        {
            Console.WriteLine(doctor.Name + " " + doctor.Surname);
            Console.WriteLine("Number is system: {0}", doctor.Id);
            Console.WriteLine("Doctor's specialization: {0}", doctor.DoctorType.ToString());
            Console.WriteLine("Phone number: {0}", doctor.Phone);
            Console.WriteLine("Email: {0}", doctor.Email);
            Console.WriteLine("Doctor's salary: {0}", doctor.Salary);
            Console.WriteLine("Profile was created at {0}", doctor.CreatedAt);
            Console.WriteLine("Profile was updated at {0}", doctor.UpdatedAt);
        }

        protected override void SaveLastId()
        {
            AppSettings result = ReadFromAppSettings();
            result.Database.Doctors.LastId = LastId;

            File.WriteAllText(Constants.AppSettingsPath, result.ToString());
        }
    }
}
