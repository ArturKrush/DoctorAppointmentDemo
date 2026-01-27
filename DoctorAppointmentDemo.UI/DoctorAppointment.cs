using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using MyDoctorAppointment.Service.Interfaces;
using MyDoctorAppointment.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDoctorAppointment.UI
{
    public class DoctorAppointment
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public DoctorAppointment(ISerializationService serializationService)
        {
            _doctorService = new DoctorService(serializationService);
            _patientService = new PatientService(serializationService);
            _appointmentService = new AppointmentService(serializationService);
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
                //Getting user type: Doctor or Patient
                Console.WriteLine("Who are you? If you are Patient enter - 0, if you are Doctor - 1");
                do
                {
                    //Input must be able to be converted to the integer and to be mapped for UserType enum 
                    result = int.TryParse(Console.ReadLine(), out userNumber) &&
                        Enum.IsDefined(typeof(UserType), userNumber);

                    if (!result)
                        Console.WriteLine("Code must be an integer 0 or 1. " +
                            "Try again.");
                }
                while (!result);
                UserType userType = (UserType)userNumber;

                switch (userType)
                {
                    case UserType.Patient:
                        //List of doctors is shown to the patient
                        Console.WriteLine("Doctors in our clinic: ");
                        foreach (var doc in doctors)
                        {
                            //Console.WriteLine(doc.Name + " " + doc.Surname + "\t" +
                            //    "Specialty: " + doc.DoctorType.ToString() + "\t"
                            //    + "Years of experience: " + doc.Experience + "\t"
                            //    + "Telephone number: " + doc.Phone);
                            _doctorService.ShowInfo(doc);
                        }
                        Console.WriteLine("-------");

                        //Identifiying the doctor the user would like to make an appointment with
                        Console.WriteLine("To which doctor you want to make an appointment?");

                        for (int i = 0; i < docCounter; i++)
                        {
                            Console.WriteLine("Enter {0} for doctor {1} {2}", i, doctors[i].Name, doctors[i].Surname);
                        }
                        do
                        {
                            //Identifying if the input is valid and the doctor with input number was listed
                            result = int.TryParse(Console.ReadLine(), out doctorNumber) &&
                                doctorNumber >= 0 && doctorNumber <= docCounter - 1;

                            if (!result)
                                Console.WriteLine("Code must be integer from 0 to {0}. " +
                                    "Try again.", docCounter - 1);
                        }
                        while (!result);
                        goalDoc = doctors[doctorNumber];
                        Console.WriteLine("-------");

                        //Showing to the patient periods when doctor, which was chosen, is busy
                        Console.WriteLine("Doctor's appointments, when doctor is busy:");
                        foreach (var appointment in appointments)
                        {
                            if (goalDoc.Equals(appointment.Doctor))
                            {
                                //Зробити ShowInfo
                                Console.WriteLine(appointment.Description + "\t" +
                                    "Time from: " + appointment.DateTimeFrom + "\t" +
                                    "Time to: " + appointment.DateTimeTo);
                            }
                        }
                        Console.WriteLine("Please, come when the doctor is free.");
                        Console.WriteLine("-------");
                        break;

                    case UserType.Doctor:
                        //Doctor is "logging in"
                        Console.WriteLine("Who are you, doctor?");
                        for (int i = 0; i < docCounter; i++)
                        {
                            Console.WriteLine("Enter {0} if you are {1} {2}", i, doctors[i].Name, doctors[i].Surname);
                        }
                        do
                        {
                            //Identifying if the input is valid and the doctor with input number was listed
                            result = int.TryParse(Console.ReadLine(), out doctorNumber) &&
                                doctorNumber >= 0 && doctorNumber <= docCounter - 1;

                            if (!result)
                                Console.WriteLine("Code must be integer from 0 to {0}. " +
                                    "Try again.", docCounter - 1);
                        }
                        while (!result);
                        goalDoc = doctors[doctorNumber];
                        Console.WriteLine("-------");

                        //Showing to the doctor his appointments
                        Console.WriteLine("These are your appointments:");
                        foreach (var appointment in appointments)
                        {
                            if (goalDoc.Equals(appointment.Doctor))
                            {
                                // Зробити ShowInfo для Appointment та Doctor
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
            foreach (var doc in _doctorService.GetAll())
            {
                _doctorService.ShowInfo(doc);
            }

            Console.WriteLine("Adding doctor: ");

            var newDoctor = new Doctor
            {
                Name = "Eugen",
                Surname = "Afanasyev",
                Experience = 15,
                DoctorType = DoctorTypes.FamilyDoctor,
                Email = "eugen1962@ukr.net",
                Phone = "+380951036483",
                Salary = 3500
            };
            _doctorService.Create(newDoctor);

            Console.WriteLine("Doctors list after inserting: ");

            foreach (var doc in _doctorService.GetAll())
            {
                _doctorService.ShowInfo(doc);
            }
            Console.WriteLine("------");
        }

        public void TestDeletingDoctor(int id)
        {
            //Console.WriteLine("Current doctors list: ");
            //foreach (var doc in _doctorService.GetAll())
            //{
            //    _doctorService.ShowInfo(doc);
            //}

            Console.WriteLine("Deleting doctor: ");
            _doctorService.Delete(id);

            Console.WriteLine("Doctors list after deleting: ");
            foreach (var doc in _doctorService.GetAll())
            {
                _doctorService.ShowInfo(doc);
            }
            Console.WriteLine("------");
        }

        public void TestUpdatingDoctor(int id)
        {
            //Console.WriteLine("Current doctors list: ");
            //foreach (var doc in _doctorService.GetAll())
            //{
            //    _doctorService.ShowInfo(doc);
            //}

            Console.WriteLine("Updating doctor: ");
            var doctor = _doctorService.Get(id);
            if (doctor is null)
            {
                Console.WriteLine("There is no doctor with such ID.");
                return;
            }

            doctor.Salary = 3000;
            ++doctor.Experience;

            _doctorService.Update(id, doctor);

            Console.WriteLine("Doctors list after updating: ");
            foreach (var doc in _doctorService.GetAll())
            {
                _doctorService.ShowInfo(doc);
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
                IllnessType = IllnessTypes.Ambulance,
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
}
