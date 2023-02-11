namespace BladeMill.BLL.Validators
{
    public class ValidateTextBox
    {
        public string ValidateTB(string input, string axis)
        {
            if (input == null)
                return $"{input} is empty, retry!";
            else if (input.Length > 5)
            {
                return $"{input} is too long!";
            }
            else if (input.Contains("+") || input.Contains("-"))
            {
                return $"{input} can't contain sign + or -";
            }
            else if (!double.TryParse(input, out _))
            {
                return $"{input} is no a number!";
            }
            else
            {
                double maxValue = 690.0;
                double minValue = 500.0;
                if (axis == "Y")
                {
                    maxValue = 350.0;
                    minValue = 140.0;
                }
                double.TryParse(input, out double myvalue);
                if (myvalue > maxValue || myvalue < minValue)
                {
                    return $"{input} is wrong range!";
                }
            }
            return string.Empty;
        }
    }
}
