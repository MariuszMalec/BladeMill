using BladeMill.BLL.Entities;
using System;
using System.IO;

namespace BladeMill.BLL.Models
{
    public class NcMainProgram : Entity
    {
        public string MainProgramName
        {
            get
            {
                return GetName(MainProgram);
            }
            private set
            {
                MainProgramName = GetName(MainProgram);
            }
        }
        public string MainProgram { get; set; }
        public string? Clamping { get; set; }
        public string? Machine { get; set; }
        public bool IsMainProgram
        {
            get
            {
                return File.Exists(MainProgram);
            }
            private set
            {
                IsMainProgram = File.Exists(MainProgram);
            }
        }
        private string GetName(string mainProgram)
        {
            if (!mainProgram.Contains(".MPF") && !mainProgram.Contains(".NC") &&
                !mainProgram.Contains(".mpf") && !mainProgram.Contains(".nc")) 
            {
                throw new ArgumentException(GetName(mainProgram));
            }
            if (mainProgram == null)
            {
                throw new ArgumentNullException(nameof(mainProgram));   
            }
            if (mainProgram == string.Empty)
            {
                throw new ArgumentNullException(GetName(mainProgram));
            }
            mainProgram = Path.GetFileNameWithoutExtension(mainProgram);
            if (mainProgram.Length == 1) 
                return mainProgram;
            if (mainProgram.Length == 9)
                mainProgram = mainProgram.Remove(mainProgram.Length - 2);
            return mainProgram;
        }

    }
}
