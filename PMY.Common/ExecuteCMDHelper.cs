using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common
{
    public class ExecuteCMDHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="DataReceivedEvent">输出重定向事件</param>
        /// <returns></returns>
        public static string ExecuteCMD(string command, Action<object, DataReceivedEventArgs> DataReceivedEvent=null)
        {
            try
            {
                Process p = new Process();

                //设置要启动的应用程序
                p.StartInfo.FileName = "cmd.exe";

                //是否使用操作系统shell启动
                p.StartInfo.UseShellExecute = false;

                // 接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardInput = true;

                //输出信息
                p.StartInfo.RedirectStandardOutput = true;

                // 输出错误
                p.StartInfo.RedirectStandardError = true;

                //不显示程序窗口
                p.StartInfo.CreateNoWindow = true;

                //启动程序
                p.Start();

                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(command + "&exit");

                p.StandardInput.AutoFlush = true;

                string strOuput = string.Empty;
                if (DataReceivedEvent != null)
                {
                    //输出重定向,即一行一行的输出
                    p.OutputDataReceived += new DataReceivedEventHandler(DataReceivedEvent);
                    p.BeginOutputReadLine();
                }
                else
                {
                    //获取输出信息
                    strOuput = p.StandardOutput.ReadToEnd();
                }
                             
                //等待程序执行完退出进程
                p.WaitForExit();
                p.Close();

                return strOuput;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
