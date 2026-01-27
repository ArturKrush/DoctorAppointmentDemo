using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDoctorAppointment.Domain.Enums;

namespace MyDoctorAppointment.Data.Interfaces
{
    public interface ISerializationService
    {
        StorageTypes Storage { get; }
        void Serialize<T>(string path, T data);

        T Deserialize<T>(string path);
    }
}
