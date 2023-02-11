using BladeMill.BLL.Models;
using BladeMill.BLL.SourceData;
using System;
using System.Diagnostics;
using System.IO;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Tworzenie pliku git
    /// </summary>
    public class GitService
    {
        private PathDataBase _pathService;
        private string _gITCommitfile = "";
        private string _gITInitfile = "";
        private string _order = "";
        private string _orderDir = "";
        private BMOrder _bMOrder;
        public GitService()
        {
            _pathService = new PathDataBase();
            _bMOrder = new BMOrder(_pathService.GetFileCurrentToolsXml());
            _gITCommitfile = _pathService.GetGITCommitfile();
            _gITInitfile = _pathService.GetGITInitfile();
            _order = _bMOrder.OrderName;
            _orderDir = Path.Combine(_pathService.GetDirOrders(), _order);
        }

        public string GetGITCommitfile()
        {
            return _gITCommitfile;
        }

        public string GetGITInitfile()
        {
            return _gITInitfile;
        }

        public void CopyGITCommitfile()
        {
            if (File.Exists(_gITCommitfile) && Directory.Exists(_orderDir))
            {
                EditCommitFile();
                string name = Path.GetFileName(_gITCommitfile);
                string dest = Path.Combine(_orderDir, name);
                File.Copy(_gITCommitfile, dest, true);
                //Copy files add.py and commit.py
                string gitDir = Path.GetDirectoryName(_gITCommitfile);
                string addFile = Path.Combine(gitDir, "add.py");
                name = Path.GetFileName(addFile);
                dest = Path.Combine(_orderDir, name);
                if (File.Exists(addFile))
                {
                    File.Copy(addFile, dest, true);
                }
                string commitFile = Path.Combine(gitDir, "commit.py");
                name = Path.GetFileName(commitFile);
                dest = Path.Combine(_orderDir, name);
                if (File.Exists(commitFile))
                {
                    File.Copy(commitFile, dest, true);
                }
            }
        }

        private void EditCommitFile()
        {
            var template = _gITCommitfile.Replace(".bat", "Template.bat");
            if (File.Exists(template))
            {
                var fileService = new FileService();
                var lines = fileService.GetLinesFromFile(template);
                using (var file = File.CreateText(_gITCommitfile))
                {
                    foreach (var line in lines)
                    {
                        file.WriteLine(line.Line.Replace("newOrder", _orderDir));
                    }
                }
            }
            else
            {
                throw new Exception($"Brak pliku {_gITCommitfile}!");
            }
        }

        public void CopyGITInitfile()
        {
            if (File.Exists(_gITInitfile) && Directory.Exists(_orderDir))
            {
                string name = Path.GetFileName(_gITInitfile);
                string dest = Path.Combine(_orderDir, name);
                //Console.WriteLine($"Copy from {_gITInitfile}");
                //Console.WriteLine($"to {dest}");
                File.Copy(_gITInitfile, dest, true);
            }
        }
        public void StartGitInitAsPowerShell()
        {
            var gitRepo = Path.Combine(_orderDir, ".git");
            if (File.Exists(_gITInitfile) && !Directory.Exists(gitRepo))
            {
                string name = Path.GetFileName(_gITInitfile);
                var startGitInitfile = "&" + "'" + _orderDir + "\\" + name + "'";
                Process.Start("powershell.exe", startGitInitfile);
            }
        }
        public void StartGitInitAsBat()
        {
            var gitRepo = Path.Combine(_orderDir, ".git");
            if (File.Exists(_gITInitfile) && !Directory.Exists(gitRepo))
            {
                string anyCommand = $"S: & cd {_orderDir}" +
                    $" & dir & .\\GitInitCmd.bat";
                //Console.WriteLine(anyCommand);
                string strCmdText;
                strCmdText = "/c " + anyCommand;
                Process.Start("cmd.exe", strCmdText);
            }
        }
    }
}
