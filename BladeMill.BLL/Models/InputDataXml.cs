using BladeMill.BLL.Enums;
using System.Xml.Serialization;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane z pliku inputdata.xml dla 3DViewer.exe , patrz tutaj C:/Temp
    /// </summary>
    //[Serializable, XmlRoot("DANE")]
    //[XmlType("NewTypeName")]//zmiana nazwy z DANE na inna
    [XmlType("DANE")]//to musi byc aby czytac xml deserialize!!
    public class InputDataXml
    {
        //[XmlElement("MASZYNA")]//rozne warianty tworzenie w strukturze xmla
        //[XmlAttribute("Id")]//
        //[XmlIgnore]//nie wtsawia elementu
        public string machine { get; set; }
        public string outfile { get; set; }
        public string catpart { get; set; }

        public string xmlpart { get; set; }

        public string xlspart { get; set; }

        public string catpartfirst { get; set; }

        public string catpartend { get; set; }

        public string xmlpartfirst { get; set; }
        public string xmlpartend { get; set; }

        public string Clampingmethod { get; set; }

        public string pinwelding { get; set; }

        public string millshroud { get; set; }

        public string readxls { get; set; }
        public string runconfiguration { get; set; }

        public string runbm { get; set; }

        public string runcmm { get; set; }

        public string createvcproject { get; set; }

        public string selectlanguage { get; set; } //enum
        public string Prerawbox { get; set; }

        public string createraport { get; set; }

        public string RootMfgDir { get; set; }

        public string clickcancel { get; set; }

        public string BMTemplate { get; set; }

        public string BMTemplateFile { get; set; }

        public string IsXML { get; set; }

        public string TypeBlade { get; set; } //enum

        public string middleTol { get; set; }

        public string admin { get; set; }

        public string ClampFromTemplate { get; set; }

        public string FIG_N { get; set; }

        public string infile { get; set; }
        public string TypeOfProcess { get; set; }
        public override string ToString()
        {
            int textPaddingWidth = 95;
            return machine.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + outfile.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + catpart.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + xmlpart.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + xlspart.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + catpartfirst.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + xmlpartfirst.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + xmlpartend.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + Clampingmethod.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + pinwelding.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + millshroud.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + readxls.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + runconfiguration.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + runbm.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + runcmm.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + createvcproject.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + selectlanguage.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + Prerawbox.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + createraport.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + RootMfgDir.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + clickcancel.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + BMTemplate.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + BMTemplateFile.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + IsXML.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + middleTol.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + admin.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + ClampFromTemplate.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + FIG_N.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + infile.ToString().PadRight(textPaddingWidth, ' ') + "|"
                ;
        }
    }

}
