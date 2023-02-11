using BladeMill.BLL.Models;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Dane z pliku Application.xml.conf 
    /// Konfiguracja BladeMilla
    /// </summary>
    public class AppXmlConfService
    {
        private AppXmlConfDirectories _appXmlConfDirectories;
        public AppXmlConfService()
        {
            _appXmlConfDirectories = new AppXmlConfDirectories();
        }

        public string GetNcDir()
        {
            return _appXmlConfDirectories.NC_DIR;
        }
        public string GetRootEngDir()
        {
            return _appXmlConfDirectories.ENGINEERING_ORDER_DIR;
        }
        public string GetRootMfgDir()
        {
            return _appXmlConfDirectories.MFG_ORDER_DIR;
        }
        public string GetBladeMillScriptsDir()
        {
            return _appXmlConfDirectories.SCRIPTS_DIR;
        }
    }
}
