using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WFA_EJ.Data
{
    public class DataBase
    {
        public DataBaseEntity DataBaseEntity => _DataBaseEntity ??= new DataBaseEntity();

        private DataBaseEntity _DataBaseEntity;
        private string _SaveNameFile = Program.cfg["SaveNameFile"];
        private string _SaveTypeFile = Program.cfg["SaveTypeFile"];

        public void SetXML()
        {
            _SaveTypeFile = "XML";
            Program.cfg["SaveTypeFile"] = "XML";
        }

        public void SetJson()
        {
            _SaveTypeFile = "Json";
            Program.cfg["SaveTypeFile"] = "Json";
        }

        public Task Save()
        {
            switch (_SaveTypeFile)
            {
                case "XML":
                {
                    using var fs = new FileStream($"{_SaveNameFile}.xml", FileMode.Create);
                    var serializer = new XmlSerializer(typeof(DataBaseEntity));
                    serializer.Serialize(fs, _DataBaseEntity);
                    break;
                }
                case "Json":
                {
                    using var fs = new StreamWriter($"{_SaveNameFile}.json", false);
                    var Json = JsonSerializer.Serialize(_DataBaseEntity);
                    fs.Write(Json);
                    break;
                }
                default: throw new ApplicationException("Ошибка в файле конфигурации такого формата нету XML или Json");
            }

            return Task.CompletedTask;
        }

        public void Load()
        {
            switch (_SaveTypeFile)
            {
                case "XML" when !File.Exists($"{_SaveNameFile}.xml"): return;
                case "XML" when new FileInfo($"{_SaveNameFile}.xml").Length < 10:
                    File.Delete($"{_SaveNameFile}.xml");
                    return;
                case "XML":
                {
                    using var fs = new FileStream($"{_SaveNameFile}.xml", FileMode.Open);
                    var serializer = new XmlSerializer(typeof(DataBaseEntity));
                    _DataBaseEntity = (DataBaseEntity) serializer.Deserialize(fs);
                    break;
                }
                case "Json" when !File.Exists($"{_SaveNameFile}.json"): return;
                case "Json" when new FileInfo($"{_SaveNameFile}.json").Length < 10:
                    File.Delete($"{_SaveNameFile}.json");
                    return;
                case "Json":
                {
                    using var fs = new FileStream($"{_SaveNameFile}.json", FileMode.Open);
                    _DataBaseEntity = JsonSerializer.DeserializeAsync<DataBaseEntity>(fs).GetAwaiter().GetResult();
                    break;
                }
                default: throw new ApplicationException("Ошибка в файле конфигурации такого формата нету XML или Json");
            }
        }
    }
}