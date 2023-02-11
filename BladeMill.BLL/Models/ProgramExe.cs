using BladeMill.BLL.Enums;
using System.IO;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Obsluga programow pomocniczych
    /// </summary>
    public class ProgramExe//TODO dodac IsExist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string MainDir { get; set; }

        public string SubDir { get; set; }

        public string Extension { get; set; }

        public string FullName
        {
            get
            {
                return Path.Combine(MainDir, SubDir, Name + Extension);
            }
            private set
            {
                FullName = Path.Combine(MainDir, SubDir, Name + Extension);
            }
        }

        public bool IsExist
        {
            get
            {
                return File.Exists(FullName);
            }
            private set
            {
                IsExist = File.Exists(FullName);
            }
        }

        /// <summary>
        /// Programy pomocnicze
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mainDir"></param>
        /// <param name="subDir"></param>
        /// <param name="name"></param>
        /// <param name="extension"></param>
        public ProgramExe(int id, string mainDir, string subDir, string name, string extension)
        {
            Id = id;
            MainDir = mainDir;
            SubDir = subDir;
            Name = name;
            Extension = extension;
        }
    }
}
