using BladeMill.BLL.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BladeMill.BLL.Services
{
    public class DirectoryService
    {
        private static IEnumerable<SelectedDirectory> _selectedDirs = new List<SelectedDirectory>() { };
        private string _mainDirectory = string.Empty;

        public DirectoryService(string mainDirectory)
        {
            _mainDirectory = mainDirectory;
        }

        public IEnumerable<SelectedDirectory> GetAll()
        {
            string[] directories = Directory.GetDirectories(_mainDirectory);
            var dirList = new List<SelectedDirectory>() { };
            int count = 1;
            foreach (var item in directories.ToList())
            {
                dirList.Add(new SelectedDirectory(count++, item));
            }

            if (dirList.Count > 0)
            {
                return dirList;
            }
            return _selectedDirs;
        }
    }
}
