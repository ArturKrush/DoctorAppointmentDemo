using MyDoctorAppointment.Domain.Enums;

namespace MyDoctorAppointment.Domain.Entities
{
    public class Doctor : UserBase
    {
        public DoctorTypes DoctorType { get; set; }

        public byte Experience { get; set; }

        public decimal Salary { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Id.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is Doctor doc)
                return doc.Id == Id;
            return false;
        }
    }
}
