namespace BladeMill.BLL.Validators
{
    public class ValidateNcFile : IValidator
    {
        private string GetErrorMessage(string input)
        {
            if (input == null)
                return $"{input} is empty, retry!";
            else if (!input.Contains(".SPF") && !input.Contains(".MPF") && !input.Contains(".NC")
                && !input.Contains(".spf") && !input.Contains(".mpf") && !input.Contains(".nc"))
            {
                return $"{input} this is not correct nc file";
            }
            else if (!System.IO.File.Exists(input))
            {
                return $"{input} file not exist!";
            }
            else
            {
            }
            return string.Empty;
        }
        public (bool, string) CheckFile(string input)
        {
            var message = GetErrorMessage(input);
            if (string.IsNullOrEmpty(message))
                return (true, message);
            return (false, message);
        }
    }
}
