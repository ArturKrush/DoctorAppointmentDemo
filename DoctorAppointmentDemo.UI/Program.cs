using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Service.Interfaces;
using MyDoctorAppointment.Service.Services;

namespace MyDoctorAppointment
{
    public class DoctorAppointment
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public DoctorAppointment()
        {
            _doctorService = new DoctorService();
            _patientService = new PatientService();
            _appointmentService = new AppointmentService();
        }

        public void Menu()
        {
            bool result;
            int userNumber;
            int doctorNumber;
            List<Doctor> doctors = (List<Doctor>)_doctorService.GetAll();
            int docCounter = doctors.Count;
            Doctor goalDoc;
            List<Appointment> appointments = (List<Appointment>)_appointmentService.GetAll();

            while (true)
            {
                Console.WriteLine("Who are you? If you are Patient enter - 0, if you are Doctor - 1");
                do
                {
                    result = int.TryParse(Console.ReadLine(), out userNumber) &&
                        Enum.IsDefined(typeof(UserType), userNumber);

                    if (!result)
                        Console.WriteLine("Code must be integer from 1 or 2. " +
                            "Try again.");
                }
                while (!result);

                UserType userType = (UserType)userNumber;

                switch (userType)
                {
                    case UserType.Patient:
                        Console.WriteLine("Doctors in our clinic: ");
                        foreach (var doc in doctors)
                        {
                            Console.WriteLine(doc.Name + " " + doc.Surname + "\t" +
                                "Specialty: " + doc.DoctorType.ToString() + "\t"
                                + "Years of experience: " + doc.Experience + "\t"
                                + "Telephone number: " + doc.Phone);
                        }

                        Console.WriteLine("-------");
                        Console.WriteLine("To which doctor you want to make an appointment?");


                        for (int i = 0; i < docCounter; i++)
                        {
                            Console.WriteLine("Enter {0} for doctor {1} {2}", i, doctors[i].Name, doctors[i].Surname);
                        }

                        do
                        {
                            result = int.TryParse(Console.ReadLine(), out doctorNumber) &&
                                doctorNumber >= 0 && doctorNumber <= docCounter - 1;

                            if (!result)
                                Console.WriteLine("Code must be integer from 0 to {0}. " +
                                    "Try again.", docCounter - 1);
                        }
                        while (!result);

                        Console.WriteLine("-------");
                        goalDoc = doctors[doctorNumber];

                        Console.WriteLine("Doctor's appointments, when doctor is busy:");
                        foreach (var appointment in appointments)
                        {
                            if (goalDoc.Equals(appointment.Doctor))
                            {
                                Console.WriteLine(appointment.Description + "\t" +
                                    "Time from: " + appointment.DateTimeFrom + "\t" +
                                    "Time to: " + appointment.DateTimeTo);
                            }
                        }

                        Console.WriteLine("Please, come when the doctor is free.");
                        Console.WriteLine("-------");
                        break;
                    case UserType.Doctor:
                        Console.WriteLine("Who are you, doctor?");
                        for (int i = 0; i < docCounter; i++)
                        {
                            Console.WriteLine("Enter {0} if you are {1} {2}", i, doctors[i].Name, doctors[i].Surname);
                        }

                        do
                        {
                            result = int.TryParse(Console.ReadLine(), out doctorNumber) &&
                                doctorNumber >= 0 && doctorNumber <= docCounter - 1;

                            if (!result)
                                Console.WriteLine("Code must be integer from 0 to {0}. " +
                                    "Try again.", docCounter - 1);
                        }
                        while (!result);

                        Console.WriteLine("-------");
                        goalDoc = doctors[doctorNumber];
                        Console.WriteLine("These are your appointments:");
                        foreach (var appointment in appointments)
                        {
                            if (goalDoc.Equals(appointment.Doctor))
                            {
                                Console.WriteLine(appointment.Description + "\t" +
                                    "Time from: " + appointment.DateTimeFrom + "\t" +
                                    "Time to: " + appointment.DateTimeTo);
                                Console.WriteLine("Patient: " + appointment.Patient.Name + " " +
                                    appointment.Patient.Surname + "\t" + "Patient's illness: "
                                    + appointment.Patient.IllnessType.ToString() + "\t" +
                                    "Patient's phone number: " + appointment.Patient.Phone);
                            }
                        }
                        Console.WriteLine("-------");
                        break;
                }
            }
        }
        public void TestCreatingDoctor()
        {
            Console.WriteLine("Current doctors list: ");
            var docs = _doctorService.GetAll();

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.Name);
            }

            Console.WriteLine("Adding doctor: ");

            var newDoctor = new Doctor
            {
                Name = "Kyrylo",
                Surname = "Kullikov",
                Experience = 12,
                DoctorType = Domain.Enums.DoctorTypes.Dermatologist
            };

            _doctorService.Create(newDoctor);

            Console.WriteLine("Doctors list after changes: ");
            docs = _doctorService.GetAll();

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.Name);
            }
            Console.WriteLine("------");
        }

        public void TestCreatingPatient()
        {
            Console.WriteLine("Current patients list: ");
            var patients = _patientService.GetAll();

            foreach (var patient in patients)
            {
                Console.WriteLine(patient.Name);
            }

            Console.WriteLine("Adding patient: ");

            var newPatient = new Patient
            {
                Name = "Vadim",
                Surname = "Alexandrov",
                AdditionalInfo = "Comes to the clinic for the first visit",
                IllnessType = Domain.Enums.IllnessTypes.Ambulance,
                Address = "Odesa, Ataman Golovatyi st., 15"
            };

            _patientService.Create(newPatient);

            Console.WriteLine("Patients list after changes: ");
            patients = _patientService.GetAll();

            foreach (var patient in patients)
            {
                Console.WriteLine(patient.Name);
            }
            Console.WriteLine("------");
        }

        public void TestCreatingAppointment()
        {
            Console.WriteLine("Current appointments list: ");
            var appointments = _appointmentService.GetAll();

            foreach (var appointment in appointments)
            {
                Console.WriteLine(appointment.Description);
            }

            List<Doctor> docs = _doctorService.GetAll().ToList();
            List<Patient> patients = _patientService.GetAll().ToList();

            Console.WriteLine("Adding an appointment: ");
            var newAppointment = new Appointment
            {
                Description = "Ambulance, urgent appointment",
                Doctor = docs.FirstOrDefault(d => d.Id == 3),
                Patient = patients.FirstOrDefault(p => p.Id == 2),
            };

            _appointmentService.Create(newAppointment);
            appointments = _appointmentService.GetAll();

            foreach (var appointment in appointments)
            {
                Console.WriteLine(appointment.Description);
            }
            Console.WriteLine("------");
        }

        public enum UserType
        {
            Patient,
            Doctor
        }
    }
    public static class Program
    {
        public static void Main()
        {
            var doctorAppointment = new DoctorAppointment();
            doctorAppointment.Menu();
            //doctorAppointment.TestCreatingDoctor();
            //doctorAppointment.TestCreatingPatient();
            //doctorAppointment.TestCreatingAppointment();
        }
    }
}