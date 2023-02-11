using System.IO;

namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Do obslugi pliku
    /// </summary>
    public class SelectedFile
    {
        public int Id { get; set; }
        public string BatchFile { get; set; }
        public bool IsExist
        {
            get
            {
                return File.Exists(BatchFile);
            }
            private set
            {
                IsExist = File.Exists(BatchFile);
            }
        }

        public SelectedFile() {}
        public SelectedFile(string batchFile)
        {
            BatchFile = batchFile;
        }
        public SelectedFile(int id, string batchFile)
        {
            Id = id;
            BatchFile = batchFile;
        }
    }
}
