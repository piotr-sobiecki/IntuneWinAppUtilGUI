using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuneWinAppUtilGUI
{
    internal class Generation
    {
        public string SetupFilesDirectory { get; set; }
        public string SetupFile { get; set; }
        public string OutputDirectory { get; set; }

        public Guid GenerationGUID { get; set; }
        public DateTime GenerationDate { get; set; }

    }
}
