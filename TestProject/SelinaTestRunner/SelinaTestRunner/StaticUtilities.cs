using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Configuration;


namespace SelinaTestRunner
{

    public static class StaticUtilities
    {
        /**
         * Copies all the contents of source dir to destination dir 
         */
        public static void CopyDirectory(string sourceDir, string destDir)
        {
            if (!(Directory.Exists(sourceDir)))
            {
                Log("Directory " + sourceDir + " doesn't exist");
                return;
            }
            if (!(Directory.Exists(destDir)))
            {
                Log("Destination directory " + destDir + " doesn't exist, creating it");
                Directory.CreateDirectory(destDir);
            }
            try
            {
                var processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "xcopy";
                processStartInfo.Arguments = @"/E " + sourceDir + " " + destDir;
                processStartInfo.CreateNoWindow = true;

                var process = Process.Start(processStartInfo);
                process.WaitForExit();
            }catch(Exception ex){
                Log(ex);
            }
        }

        public static void DeleteDirectory(string dirPath) {
            var deletePath = Environment.CurrentDirectory + @"\Utilities\";

            if (Directory.Exists(deletePath) && File.Exists(deletePath + "delete.bat"))
            {
                try
                {
                    var processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = "delete.bat";
                    processStartInfo.Arguments = dirPath;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.WorkingDirectory = deletePath;

                    var process = Process.Start(processStartInfo);
                    process.WaitForExit();
                }
                catch (Exception ex) {
                    Log(ex);
                }
            }
            else {
                Log("delete.bat is not found, expected path : " + deletePath);
            }                   
        }

        public static void LogToFile(string message)
        {
            var logFileName = "selinaLog" + new Random().Next() + ".log";
            try
            {
                using (StreamWriter sWriter = new StreamWriter(logFileName, true))
                {
                    sWriter.WriteLine(message);
                    sWriter.Flush();
                };
            }
            catch (Exception ex) {
                Log(ex);
            }
        }

        public static void Log(string message) {
            Console.WriteLine(DateTime.Now.ToString() + " : " + message);
        }

        public static void Log(Exception ex) {
            Log("Message : " + ex.Message + " StackTrace : " + ex.StackTrace);
        }

        public static string GetWorkingDirectoryPath() {
            string workingDirectoryPath = "";
            try
            {
                workingDirectoryPath = ConfigurationManager.AppSettings["workingDirectoryPath"];
                //workingDirectoryPath = @"D:\Projects\Selina\RunTests\";
            }
            catch(Exception ex)
            {
                Log(ex);
            }
            return workingDirectoryPath;
        }

        public static string GetTestAssemblyPath()
        {            
            string testAssemblyPath = "";
            try
            {
                testAssemblyPath = ConfigurationManager.AppSettings["testAssemblyPath"];
                //testAssemblyPath = @"D:\Projects\Selina\RunTests\TestAssembly";
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            return testAssemblyPath;
        }
    }
}
