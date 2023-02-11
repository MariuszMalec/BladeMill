namespace BladeMill.BLL.Models
{
    public class BmdXmlFileView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Flag { get; set; } = false;

        public BmdXmlFileView(int id, string name, string value, bool flag)
        {
            Id = id;
            Name = name;
            Value = value;
            Flag = flag;
        }
    }
}
