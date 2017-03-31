using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Excutebat
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcuteBAT("test.bat");
            Console.ReadLine();
        }


        public static void ExcuteBAT(string path)
        {
            try
            {
                Process p = new Process();
                ProcessStartInfo pi = new ProcessStartInfo(path, "fdfs fdf sss");
                pi.UseShellExecute = false;
                pi.RedirectStandardOutput = true;
                pi.CreateNoWindow = false;
                p.StartInfo = pi;
                p.Start();
                
                p.WaitForExit();
                string output = p.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
