using BladeMill.BLL.Entities;

namespace BladeMill.BLL.Models
{
    public class Machine : Entity
    {
        public string MachineName { get; set; }
        public string MachineControl { get; set; }
        public string MachineVericutTemplate { get; set; }
    }
}
