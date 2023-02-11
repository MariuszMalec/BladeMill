namespace BladeMill.BLL.Validators
{
    public interface IValidator
    {
        public (bool, string) CheckFile(string input);
    }
}