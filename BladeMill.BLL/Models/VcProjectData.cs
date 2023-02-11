using System.Collections.Generic;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane z pliku ..VcProjetct.xml
    /// </summary>
    public class VcProjectData
    {
        public string ControlCtl { get; set; }
        public string MachineMch { get; set; }
        public string ToolLibrary { get; set; }
        public string NcProgram { get; set; }
        public List<string> Subroutine { get; set; }
        public List<string> StlGeometry { get; set; }
    }
}
