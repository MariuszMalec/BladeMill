using BladeMill.BLL.Enums;
using BladeMill.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace BladeMill.BLL.Services
{
    /// <summary>
    /// Maszyna jako wzorzec fabryka
    /// </summary>
    public class GetMachineFromVarpool : MachineSettings
    {
        public override Machine GetMachine(string file)
        {
            if (File.Exists(file) && (file.Contains("_varpool.")))
            {
                return new Machine()
                {
                    Id = 1,
                    Created = DateTime.Now,
                    MachineName = GetMachineName(file),
                    MachineControl = GetControl(file),
                    MachineVericutTemplate = ""
                };
            }
            else
            {
                if (!File.Exists(file))
                    throw new Exception($"Warning, doesn't exist file! {file}");
                throw new Exception($"Warning, wrong input file! {file}");
            }
        }
        private string GetControl(string file)
        {
            var control = "unknow";
            var machine = GetFromFileValue(file, "MfgSystem");
            if (!string.IsNullOrEmpty(machine))
            {
                if (machine.Contains("SIM840D"))
                    return "SIM840D";
            }
            return control;
        }

        private string GetMachineName(string file)
        {
            return GetShortName(GetFromFileValue(file, "MfgSystem"));
        }

        private string GetFromFileValue(string fileNamewithDir, string findtext)
        {
            try
            {
                string Value = string.Empty;
                if (File.Exists(fileNamewithDir))
                {
                    List<string> listvarpoolNames = new List<string>(new string[] { });
                    List<string> listvarpoolValues = new List<string>(new string[] { });
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileNamewithDir);
                    XPathNavigator navigator = doc.CreateNavigator();
                    XPathNodeIterator nodes = navigator.Select("/VarPool/Overview");
                    string Name = "";
                    XPathNodeIterator nodesName = navigator.Select("/VarPool/Var/Name");
                    foreach (XPathNavigator oCurrent in nodesName)
                    {
                        Name = oCurrent.InnerXml;//Name
                        listvarpoolNames.Add(Name);
                    }
                    XPathNodeIterator nodesValue = navigator.Select("/VarPool/Var/Value");
                    foreach (XPathNavigator oCurrent in nodesValue)
                    {
                        Value = oCurrent.InnerXml;//Name
                        listvarpoolValues.Add(Value);
                    }
                    Value = string.Empty;
                    int count = 0;
                    foreach (string element in listvarpoolNames)
                    {
                        //if(element.Contains(textfind))
                        if (element == findtext)
                        {
                            Value = listvarpoolValues[count].ToString().Replace(" ", "");
                        }
                        count++;
                    }
                    return $"{Value}";
                }
                return $"{Value}";
            }
            catch (Exception e)
            {
                throw new Exception("check function GetFromFileValue", e);
            }
        }
        private string GetShortName(string machine)
        {
            switch (machine)
            {
                case ("HM_HSTM_300_SIM840D"):
                    machine = MachineEnum.HSTM300.ToString();
                    break;
                case ("SH_HX151_24_SIM840D"):
                    machine = MachineEnum.HX151.ToString();
                    break;
                case ("HM_HSTM_500M_SIM840D"):
                    machine = MachineEnum.HSTM500M.ToString();
                    break;
                case ("HURON_EX20_SIM840D"):
                    machine = MachineEnum.HURON.ToString();
                    break;
                case ("HM_HSTM_1000_SIM840D"):
                    machine = MachineEnum.HSTM1000.ToString();
                    break;
                case ("HM_HSTM_300HD_SIM840D"):
                    machine = MachineEnum.HSTM300HD.ToString();
                    break;
                case ("HM_HSTM_500_SIM840D"):
                    machine = MachineEnum.HSTM500.ToString();
                    break;
                default:
                    machine = "unkown";
                    break;
            }
            return machine;
        }
    }
}
