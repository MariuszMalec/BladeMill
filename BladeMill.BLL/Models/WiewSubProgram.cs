namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane dla Web application
    /// </summary>
    public class WiewSubProgram
    {
        public int Id { get; set; }
        public string? SubProgramName { get; set; }
        public string? SubProgramNameWithDir { get; set; }
        public bool? IsSubProgram { get; set; }
    }
}
