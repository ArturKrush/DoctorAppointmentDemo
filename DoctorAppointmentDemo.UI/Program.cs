using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Service.Interfaces;
using MyDoctorAppointment.Service.Services;

namespace MyDoctorAppointment
{
    public class DoctorAppointment
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public DoctorAppointment()
        {
            _doctorService = new DoctorService();
            _patientService = new PatientService();
        }

        public void Menu()
        {
            //while (true)
            //{
            //    // add Enum for menu items and describe menu
            //}

            /*Console.WriteLine("Current doctors list: ");
            var docs = _doctorService.GetAll();

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.Name);
            }

            Console.WriteLine("Adding doctor: ");

            var newDoctor = new Doctor
            {
                Name = "Illya",
                Surname = "Ivanov",
                Experience = 8,
                DoctorType = Domain.Enums.DoctorTypes.Paramedic
            };

            _doctorService.Create(newDoctor);

            Console.WriteLine("Current doctors list: ");
            docs = _doctorService.GetAll();

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.Name);
            }*/
            
            var patients = _patientService.GetAll();
            Console.WriteLine("Adding patient: ");

            /*var newPatient = new Patient
            {
                Name = "Mykola",
                Surname = "Romanov",
                AdditionalInfo = "Often busy on mondays",
                IllnessType = Domain.Enums.IllnessTypes.Infection
            };

            _patientService.Create(newPatient);*/

            Console.WriteLine("Current patients list: ");
            patients = _patientService.GetAll();

            foreach (var patient in patients)
            {
                Console.WriteLine(patient.Name);
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var doctorAppointment = new DoctorAppointment();
            doctorAppointment.Menu();
        }
    }
}