using System;
using System.IO;

namespace BladeMillWithExcel.Logic.Models
{
    public class SubProgram
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public string SubProgramName
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

        public string SubProgramNameWithoutExtension
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

        public string SubProgramNameWithDir { get; set; }

        public bool IsSubProgram
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
