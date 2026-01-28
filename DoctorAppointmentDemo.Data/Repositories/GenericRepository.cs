using MyDoctorAppointment.Data.Configuration;
using MyDoctorAppointment.Data.Interfaces;
using MyDoctorAppointment.Domain.Entities;
using MyDoctorAppointment.Domain.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;
//using System.Xml.Linq;

namespace MyDoctorAppointment.Data.Repositories
{
    public abstract class GenericRepository<TSource> : IGenericRepository<TSource> where TSource : Auditable
    {
        public abstract string Path { get; set; }

        public abstract int LastId { get; set; }

        public ISerializationService SerializationService { get; private set; }

        private List<TSource>? sources; //Все текущие объекты этого типа

        public GenericRepository(ISerializationService serializationService)
        {
            SerializationService = serializationService;
        }

        public TSource Create(TSource source)
        {
            source.Id = ++LastId;
            source.CreatedAt = DateTime.Now;

            List<TSource> list;

            //If the storage XML-file is empty, new list is created
            try
            {
                var existingData = GetAll();

                list = existingData.ToList();
            }
            catch (Exception)
            {
                list = new List<TSource>();
            }

            //In list, which is implementing IEnumerable, is added the 1st or the next source
            list.Add(source);
            SerializationService.Serialize(Path, list);

            SaveLastId();

            return source;
        }

        public bool Delete(int id)
        {
            if (GetById(id) is null)
                return false;

            List<TSource> list = sources.Where(x => x.Id != id).ToList();

            SerializationService.Serialize(Path, list);

            return true;
        }

        public IEnumerable<TSource> GetAll()
        {
            //Результат зберігається у змінну для попередження 2x викликів GetAll() в Delete()
            //IEnumerable<> змінено на List<>, бо XmlSerializer не може десеріалізувати інтерфейси
            sources = SerializationService.Deserialize<List<TSource>>(Path);
            return sources;
        }

        public TSource? GetById(int id)
        {
            TSource? source = null;
            try
            {
                source = GetAll().SingleOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("More than 1 object with given Id was found in the file.");
            }
            return source;
        }

        public TSource Update(int id, TSource source)
        {
            source.UpdatedAt = DateTime.Now;
            source.Id = id;

            sources = GetAll().Select(x => x.Id == id ? source : x).ToList();

            SerializationService.Serialize(Path, sources);

            return source;
        }

        public abstract void ShowInfo(TSource source);

        protected abstract void SaveLastId();

        protected AppSettings ReadFromAppSettings()
        {
            AppSettings settings = new AppSettings();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Constants.AppSettingsPath);
            XmlElement? xRoot = xDoc.DocumentElement;
            XmlNode? source;

            switch (SerializationService.Storage)
            {
                case StorageTypes.JSON:
                    source = xRoot.SelectSingleNode("Doctors/JSONSource");
                    if (source != null)
                    {
                        settings.Database.Doctors.LastId = int.Parse(source.SelectSingleNode("LastId").InnerText);
                        settings.Database.Doctors.Path = source.SelectSingleNode("Path").InnerText;
                    }
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId or Path properties in Doctors");

                    source = xRoot.SelectSingleNode("Patients/JSONSource");
                    if (source != null)
                    {
                        settings.Database.Patients.LastId = int.Parse(source.SelectSingleNode("LastId").InnerText);
                        settings.Database.Patients.Path = source.SelectSingleNode("Path").InnerText;
                    }
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId or Path properties in Patients");

                    source = xRoot.SelectSingleNode("Appointments/JSONSource");
                    if (source != null)
                    {
                        settings.Database.Appointments.LastId = int.Parse(source.SelectSingleNode("LastId").InnerText);
                        settings.Database.Appointments.Path = source.SelectSingleNode("Path").InnerText;
                    }
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId or Path properties in Appointments");
                    break;

                case StorageTypes.XML:
                    source = xRoot.SelectSingleNode("Doctors/XMLSource");
                    if (source != null)
                    {
                        settings.Database.Doctors.LastId = int.Parse(source.SelectSingleNode("LastId").InnerText);
                        settings.Database.Doctors.Path = source.SelectSingleNode("Path").InnerText;
                    }
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId or Path properties in Doctors");

                    source = xRoot.SelectSingleNode("Patients/XMLSource");
                    if (source != null)
                    {
                        settings.Database.Patients.LastId = int.Parse(source.SelectSingleNode("LastId").InnerText);
                        settings.Database.Patients.Path = source.SelectSingleNode("Path").InnerText;
                    }
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId or Path properties in Patients");

                    source = xRoot.SelectSingleNode("Appointments/XMLSource");
                    if (source != null)
                    {
                        settings.Database.Appointments.LastId = int.Parse(source.SelectSingleNode("LastId").InnerText);
                        settings.Database.Appointments.Path = source.SelectSingleNode("Path").InnerText;
                    }
                    else
                        throw new InvalidOperationException($"Structure in AppSettings is broken." +
                            $"Cannot find LastId or Path properties in Appointments");
                    break;
            }

            return settings;
        }
    }
}
