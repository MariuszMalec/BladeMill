namespace BladeMillWithExcel.Logic.Models
{
    public class Tool
    {
        public Tool(int id, string batchFile, string description, string toolSet, string toolID, string toolIDPreLoad, string toollen, string toolDiam, string toolCrn, string spindle, string feedrate, string maxmilltime, string offsets, string machine, bool checkProload)
        {
            Id = id;
            BatchFile = batchFile;
            Description = description;
            ToolSet = toolSet;
            ToolID = toolID;
            ToolIDPreLoad = toolIDPreLoad;
            Toollen = toollen;
            ToolDiam = toolDiam;
            ToolCrn = toolCrn;
            Spindle = spindle;
            Feedrate = feedrate;
            MaxMillTime = maxmilltime;
            Offsets = offsets;
            Machine = machine;
            CheckProload = checkProload;
        }

        public int Id { get; set; }
        public string BatchFile { get; set; }
        public string Description { get; set; }
        public string ToolSet { get; set; }
        public string ToolID { get; set; }
        public string ToolIDPreLoad { get; set; }
        public string Toollen { get; set; }
        public string ToolDiam { get; set; }
        public string ToolCrn { get; set; }
        public string Spindle { get; set; }
        public string Feedrate { get; set; }
        public string MaxMillTime { get; set; }
        public string Offsets { get; set; }
        public string Machine { get; set; }
        public bool CheckProload { get; set; }

    }
}
