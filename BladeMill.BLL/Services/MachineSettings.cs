using BladeMill.BLL.Models;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Maszyna jako wzorzec fabryka
    /// przyklad klasy abstrakcyjnej
    /// </summary>
    public abstract class MachineSettings
    {
        public abstract Machine GetMachine(string file);
    }
}
