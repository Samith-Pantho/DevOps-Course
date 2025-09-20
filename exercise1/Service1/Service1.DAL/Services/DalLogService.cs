using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Service1.DAL.Repositories;

namespace Service1.DAL.Services
{
    public class DalLogService : IDalLogRepository
    {
        public void SaveLog(string message)
        {
            try
            {
                string messageFormat = $"{message}";

                StreamWriter sw = null;
                string path = "/app/vStorage";
                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);
                }

                string File = path + "/vStorage.txt";
                sw = new StreamWriter(File, true);


                sw.WriteLine(messageFormat);
                sw.Flush();
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Stacktrace: {ex.StackTrace}");
            }
        }

    }
}
