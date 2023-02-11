using System.Diagnostics;
using System.Linq;

namespace BladeMill.BLL.Services
{
    public class ProcessService
    {
        Process[] Processes;
        public ProcessService()
        {
            Processes = Process.GetProcesses();
        }
        public Process GetProcess(string procesName)
        {
            var result = Processes.Where(p => p.ProcessName == procesName).FirstOrDefault();
            return result;
        }


    }
}
