using System.IO;

namespace BladeMill.BLL.Models
{
    public class SelectedDirectory
    {
        public int Id { get; set; }
        public string DirectoryDir { get; set; }
        public string DirectoryName
        {
            get
            {
                return GetDirectoryName(DirectoryDir);
            }
            private set
            {
                DirectoryName = GetDirectoryName(DirectoryDir);
            }
        }
        public bool IsExist
        {
            get
            {
                return Directory.Exists(DirectoryDir);
            }
            private set
            {
                IsExist = Directory.Exists(DirectoryDir);
            }
        }
        public SelectedDirectory(string batchFile)
        {
            DirectoryDir = batchFile;
        }
        public SelectedDirectory(int id, string batchFile)
        {
            Id = id;
            DirectoryDir = batchFile;
        }

        private string GetDirectoryName (string directoryDir)
        {
            var lastPos = directoryDir.Split('\\').Length;
            var name = directoryDir.Split('\\')[lastPos - 1];
            return name;
        }
    }
}
