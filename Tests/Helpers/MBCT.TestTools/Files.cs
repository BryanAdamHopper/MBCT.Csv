using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MBCT.Helpers.TestTools
{
    public class Files
    {
        public static string GetTestDataPath(string testDataFile)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 2));
            return Path.Combine(projectPath, "TestData", testDataFile);
        }
    }
}
