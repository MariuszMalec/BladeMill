using System.Xml.Serialization;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Baza katalogow do usuniecia
    /// </summary>
    [XmlType("Directory")]
    public class BasePaths
    {
        public string DirOneDriveClever { get; set; }
        public string DirOrders { get; set; }
        public string DirVericutProjectTemplate { get; set; }
        public string FileVericutToolsLibrary { get; set; }
        public string DirProgramExe { get; set; } // patrz ProgramExe.cs
        public string DirBladeMillScripts { get; set; }
        public string DirTask { get; set; } // patrz ProgramExeService.cs

        public string DirDrive { get; set; }

        public string DirCmm { get; set; }

        public string DirHtml { get; set; }
        public string DirIcon { get; set; }

        public string FileExcelTemplate { get; set; }

        public override string ToString()
        {
            int textPaddingWidth = 125;
            return "-".PadRight(textPaddingWidth, '-')
                   + "|\n" + DirOneDriveClever.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirOrders.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + FileVericutToolsLibrary.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirProgramExe.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirBladeMillScripts.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirDrive.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirCmm.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirHtml.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirIcon.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + FileExcelTemplate.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + DirVericutProjectTemplate.ToString().PadRight(textPaddingWidth, ' ')
                   + "|\n" + "-".ToString().PadRight(textPaddingWidth, '-') + "|"
                   ;
        }
    }
}
