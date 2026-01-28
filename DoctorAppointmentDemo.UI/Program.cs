using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using MyDoctorAppointment.Service.Interfaces;
using MyDoctorAppointment.Service.Services;

namespace MyDoctorAppointment
{
    public static class Program
    {
        private static DoctorAppointment? doctorAppointment;
        public static void Main()
        {
            ChooseStorageFormat();
            doctorAppointment.Menu();

            //doctorAppointment.TestCreatingDoctor();
            //doctorAppointment.TestDeletingDoctor(9);
            //doctorAppointment.TestUpdatingDoctor(5);

            //doctorAppointment.TestCreatingPatient();
            //doctorAppointment.TestDeletingPatient(5);
            //doctorAppointment.TestUpdatingPatient(5);

            //doctorAppointment.TestCreatingAppointment(4, 5);
            //doctorAppointment.TestDeletingAppointment(1);
            //doctorAppointment.TestUpdatingAppointment(2);
        }

        public static void ChooseStorageFormat()
        {
            bool result;
            int storageNumber;
            //Getting storage type: XML or JSON
            Console.WriteLine("Where do you want to save data? For XML enter - 0, for JSON - 1");
            do
            {
                result = int.TryParse(Console.ReadLine(), out storageNumber) &&
                    Enum.IsDefined(typeof(StorageTypes), storageNumber);

                if (!result)
                    Console.WriteLine("Code must be integer 0 or 1. " +
                        "Try again.");
            }
            while (!result);
            StorageTypes storageType = (StorageTypes)storageNumber;

            //Passing data storage service as a parameter based on user choice
            switch (storageType)
            {
                case StorageTypes.XML:
                    Console.WriteLine("You chose XML as data storage format.");
                    doctorAppointment = new DoctorAppointment(new XmlSerializerService());
                    break;
                case StorageTypes.JSON:
                    Console.WriteLine("You chose JSON as data storage format.");
                    doctorAppointment = new DoctorAppointment(new JsonSerializerService());
                    break;
            }
            Console.WriteLine("AppSettings are stored in XML format.");
        }
    }
}