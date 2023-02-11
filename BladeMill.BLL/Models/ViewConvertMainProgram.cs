using BladeMill.BLL.Enums;
using System.ComponentModel.DataAnnotations;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane dla Web application
    /// </summary>
    public class ViewConvertMainProgram
    {
        public string OrgMachine { get; set; }
        [Display(Name = "Select machine")]
        public MachineEnum MachineType { get; set; }
        public string ProgramName { get; set; }
        [Display(Name = "Fill new name program")]
        public string NewProgramName { get; set; }

    }
}
