using BladeMill.BLL.Services;

namespace BladeMill.ConsoleApp.GitTests
{
    static class Git
    {
        public static void Tests()
        {
            var gitService = new GitService();
            gitService.CopyGITCommitfile();
            gitService.CopyGITInitfile();
            gitService.StartGitInitAsPowerShell();
            gitService.StartGitInitAsBat();
        }
    }
}
