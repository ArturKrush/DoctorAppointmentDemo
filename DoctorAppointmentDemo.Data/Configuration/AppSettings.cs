using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDoctorAppointment.Data.Configuration
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }

        public AppSettings()
        {
            Database = new DatabaseSettings();
        }
    }

    public class DatabaseSettings
    {
        public DoctorsSettings Doctors { get; set; }
        public PatientsSettings Patients { get; set; }
        public AppointmentsSettings Appointments { get; set; }

        public DatabaseSettings()
        {
            Doctors = new DoctorsSettings();
            Patients = new PatientsSettings();
            Appointments = new AppointmentsSettings();
        }
    }

    public class DoctorsSettings
    {
        public int LastId { get; set; }
        public string Path { get; set; }
    }

    public class PatientsSettings
    {
        public int LastId { get; set; }
        public string Path { get; set; }
    }

    public class AppointmentsSettings
    {
        public int LastId { get; set; }
        public string Path { get; set; }
    }
}
