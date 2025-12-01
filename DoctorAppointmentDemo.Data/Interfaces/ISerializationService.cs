using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDoctorAppointment.Service.Interfaces
{
    public interface ISerializationService
    {
        void Serialize<T>(string path, T data);

        T Deserialize<T>(string path);
    }
}
