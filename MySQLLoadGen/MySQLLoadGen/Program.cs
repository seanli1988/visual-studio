using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;

namespace MySQLLoadGen
{

    class Program
    {
        public struct mysqlslap
        {
            public Process p;
            public String args;
            public mysqlslap(Process p, string args)
            {
                this.p = p;
                this.args = args;
            }
        }

        static void Main(string[] args)
        {
            Process process1 = new Process();
            Process process2=null;
            int workload = Convert.ToInt32(ConfigurationManager.AppSettings["workload"]);

            string master = ConfigurationManager.AppSettings["master"];
            if (master == "localhost")
                master = "";
            string replica = ConfigurationManager.AppSettings["replica"];
            if (replica == "" || replica ==null)
            {
                Console.WriteLine("No replica detected. Starting load generation on Master.");
                mysqlslap m0 = new mysqlslap(process1, master + " --concurrency=" + workload + " --iterations=100  --query=\"select* from employees.current_dept_emp\"  --verbose --create-schema=employees");
                genload(m0);
            }
            else
            {
                Console.WriteLine("Starting load generation on Master and Replica.");
                process2 = new Process();
                workload = workload / 2;
                mysqlslap m1 = new mysqlslap(process1, master + " --concurrency=" + workload + " --iterations=100  --query=\"select* from employees.current_dept_emp\"  --verbose --create-schema=employees");
                mysqlslap m2 = new mysqlslap(process2, replica + " --concurrency=" + workload + " --iterations=100  --query=\"select* from employees.current_dept_emp\"  --verbose --create-schema=employees");
                genload(m1);
                genload(m2);
            }                        
            
            Console.WriteLine("\nPress any key to kill load test.");
            Console.ReadLine();
            process1.Kill();
            if (process2 !=null)
                process2.Kill();
            Console.WriteLine("\nPress any key to exit program.");
            Console.ReadLine();
        }

        public static void genload(mysqlslap m)
        {
            Program loadgen = new Program();
            Thread newThread = new Thread(loadgen.startmysqlslap);
            newThread.Start(m);
        }

        public void startmysqlslap(Object obj)
        {
            mysqlslap m = (mysqlslap)obj;
            Process p = m.p;
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WorkingDirectory = @"C:\Program Files\MySQL\MySQL Server 5.7\bin\";
            p.StartInfo.FileName = "mysqlslap.exe";
            p.StartInfo.Arguments = m.args;
            Console.WriteLine(m.args);
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
          //  Console.WriteLine(output);

        }
    }
}
