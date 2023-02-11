using System.IO;

namespace BladeMill.BLL.Entities
{
    public class SubProgram : Entity
    {
        public string? SubProgramName
        {
            get
            {
                return Path.GetFileName(SubProgramNameWithDir);
            }
            private set
            {
                SubProgramName = Path.GetFileName(SubProgramNameWithDir);
            }
        }

        public string? SubProgramNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(SubProgramNameWithDir);
            }
            private set
            {
                SubProgramNameWithoutExtension = Path.GetFileNameWithoutExtension(SubProgramNameWithDir);
            }
        }

        public string? SubProgramNameWithDir { get; set; }

        public bool? IsSubProgram
        {
            get
            {
                return File.Exists(SubProgramNameWithDir);
            }
            private set
            {
                IsSubProgram = File.Exists(SubProgramNameWithDir);
            }
        }
    }
}
