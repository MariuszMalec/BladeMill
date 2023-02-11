namespace BladeMill.BLL.Models
{
    /// <summary>
    /// Dane dla Web application
    /// </summary>
    public class ViewMainProgram
    {
        public int Id { get; set; }
        public string? MainPogramWithDir { get; set; }
        public string? Clamping { get; set; }
        public string? Machine { get; set; }
    }
}
