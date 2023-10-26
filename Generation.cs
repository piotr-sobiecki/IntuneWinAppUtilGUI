using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuneWinAppUtilGUI
{
    public class Generation
    {
        public string SetupFilesDirectory { get; set; }
        public string SetupFile { get; set; }
        public string OutputDirectory { get; set; }
        public Guid GenerationGUID { get; set; }
        public DateTime GenerationDate { get; set; }

        public Generation() { }

        public Generation(string setupFilesDirectory, string setupFile, string outputDirectory)
        {
            SetupFilesDirectory = setupFilesDirectory;
            SetupFile = setupFile;
            OutputDirectory = outputDirectory;
            GenerationGUID = Guid.NewGuid();
            GenerationDate = DateTime.Now;
        }
    }
}
