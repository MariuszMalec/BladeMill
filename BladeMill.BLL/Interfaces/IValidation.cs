namespace BladeMill.BLL.Interfaces
{
    public interface IValidation
    {
        bool ValidationMachine(string selectedMachine, string mainProgram);

        bool ValidationNewNameProgram(string newNameProgram);

        bool ValidationMainProgram(string mainProgram);

    }
}
