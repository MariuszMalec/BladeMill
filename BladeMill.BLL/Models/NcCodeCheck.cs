using System.Collections.Generic;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Lista bledow dla danego programu NC
    /// </summary>
    public class NcCodeCheck
    {
        public string NcProgramCheck { get; set; }
        public int Nmb { get; set; }
        public string Message { get; set; }
        public List<string> ListErrors { get; set; }
        public NcCodeCheck(int nmb, string ncProgramCheck, string message, List<string> listErrors)
        {
            Nmb = nmb;
            NcProgramCheck = ncProgramCheck;
            Message = message;
            ListErrors = listErrors;
        }
    }
}
