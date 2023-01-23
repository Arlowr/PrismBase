using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace PrismBase.Core.Generics
{
    public static class generics
    {
        public static string dataManagementType { get; set; }
        public static string databaseLoc { get; set; }
        private static int NumTasksRunning { get; set; }
        private static bool backgroundTasksRunning { get; set; }

        public async static void ErrorManagement(string errorType, string module, string error)
        {
            var fileLoc = System.Reflection.Assembly.GetEntryAssembly().Location;
            switch (errorType)
            {
                case "Database":
                    using (StreamWriter sw = new StreamWriter(Path.Combine(fileLoc, module + "databaseError.txt")))
                    {

                        await sw.WriteAsync(DateTime.Now.ToString("MM/dd/yy H:mm:ss") + "; " + error);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void UpdateTaskRunning(bool taskStarted)
        {
            if (taskStarted)
                NumTasksRunning += 1;
            else
                NumTasksRunning -= 1;

            if (NumTasksRunning > 0)
                backgroundTasksRunning = true;
            else if (NumTasksRunning <= 0)
            {
                backgroundTasksRunning = false;
                NumTasksRunning = 0;
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            var parent = parentObject as T;
            if (parent != null)
                return parent;

            return FindParent<T>(parentObject);

        }

    }
}
