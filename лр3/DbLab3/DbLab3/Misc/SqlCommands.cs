using System.Collections.Generic;

namespace DbLab3.Misc
{
    public static class SqlCommands
    {
        public static Dictionary<string, string> Commands = new ()
        {
            ["SelectAll"] = "SELECT * FROM Employees;",
            ["SelectNameTown"] = "SELECT FullName, Town FROM Employees ORDER BY Town;",
            ["SelectNameExperience"] = "SELECT FullName, WorkExperience FROM Employees WHERE WorkExperience > 4;"
        };
    }
}