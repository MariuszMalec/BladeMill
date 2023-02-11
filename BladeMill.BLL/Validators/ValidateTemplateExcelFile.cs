namespace BladeMill.BLL.Validators
{
    public class ValidateTemplateExcelFile : IValidator
    {
        private string GetErrorMessage(string input)
        {
            if (input == null)
                return $"{input} is empty, retry!";
            else if (!input.Contains(".xlsm"))
            {
                return $"{input} this is not correct xlsm file";
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
                return (false, message);
            return (true, message);
        }
    }
}
