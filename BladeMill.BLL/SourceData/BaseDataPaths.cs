using BladeMill.BLL.Models;
using System.Collections.Generic;

namespace BladeMill.BLL.SourceData
{
    public class BaseDataPaths
    {
        private static PathDataBase _pathDataBase = new PathDataBase();

        public static IEnumerable<BasePaths> Create()
        {
            var _datas = new BasePaths
            {
                DirOneDriveClever = _pathDataBase.GetDirOneDriveClever(),
                DirOrders= _pathDataBase.GetDirOrders(),
                DirVericutProjectTemplate = _pathDataBase.GetDirVericutProjectTemplate(),
                FileVericutToolsLibrary = _pathDataBase.GetFileVericutToolsLibrary(),
                DirProgramExe = _pathDataBase.GetDirProgramExe(),
                DirBladeMillScripts = _pathDataBase.GetDirBladeMillScripts(),
                DirTask= _pathDataBase.GetDirTask(),
                DirDrive= _pathDataBase.GetDirDrive(),
                DirCmm=_pathDataBase.GetDirCmm(),
                DirHtml= _pathDataBase.GetDirHtml(),
                DirIcon= _pathDataBase.GetDirIcon(),
                FileExcelTemplate= _pathDataBase.GetFileExcelTemplate()
            };
            return new[] { _datas };
        }
    }
}
