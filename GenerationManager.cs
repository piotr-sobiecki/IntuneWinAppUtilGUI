using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntuneWinAppUtilGUI
{
    internal class GenerationManager
    {
        private List<Generation> generationList = new List<Generation>();
        private string xmlFilePath;
        public GenerationManager()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            var userConfigFilePath = config.FilePath;
            var userConfigDirectory = Path.GetDirectoryName(userConfigFilePath);
            xmlFilePath = Path.Combine(userConfigDirectory, "generations.xml");
            DeserializeGenerationList();
        }

        public List<Generation> GetGenerationList()
        {
            return generationList;
        }

        public void AddGeneration(Generation generation)
        {
            generationList.Add(generation);
            SerializeGenerationList();
        }

        public void SerializeGenerationList()
        {
            using (var writer = new StreamWriter(xmlFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<Generation>));
                serializer.Serialize(writer, generationList);
            }
        }

        public void DeserializeGenerationList()
        {
            if (File.Exists(xmlFilePath))
            {
                using (var reader = new StreamReader(xmlFilePath))
                {
                    var serializer = new XmlSerializer(typeof(List<Generation>));
                    generationList = (List<Generation>)serializer.Deserialize(reader);
                }
            }
        }
    }
}
