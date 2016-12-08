using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace loginsert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string Tag = "TAG";
        string Name = "bruce";
        
        private void button1_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            Boolean isdeath = false;
            if (System.IO.File.Exists(path))
            {
                //说明是文件
                ChangeFile(path);
                isdeath = true;
            } else if (System.IO.Directory.Exists(path))
            {
                if (!path.EndsWith("\\"))
                {
                    path += "\\";
                }
                //说明是目录
                showDirFile(path);
                isdeath = true;
            }

            if(isdeath)
            {
                
                MessageBox.Show("完成");
            }
            
        }

        private void ChangeFile(string file)
        {
             //打开文件
            //解析文件
            //替换文件
            string content = "";
            string ext = Path.GetExtension(file);

            if (ext.ToLower().EndsWith(".java") == true)
            {
                content = ReadFile(file);
                if (content == "")
                {
                    return;
                }
                string fileTagname = Path.GetFileName(file);
                content = JavaReplaceContent(content, fileTagname);
            }
            else if (ext.ToLower().EndsWith(".c") == true)
            {
                //content = C_ReplaceContent(content);
            }
            else
            {
                return;
            }

            string filename =  "content"+ DateTime.Now.Ticks.ToString().Substring(7,3)+".txt";
            if (checkBox1.Checked)
            {
                if(content.Length>0)
                {
                    System.IO.File.WriteAllText(file, content);
                }
            }
            else 
            {
                System.IO.File.WriteAllText("D:\\" + filename, content);
            }
        }

        private void showDirFile(string dir)
        {
            IList<FileInfo> lst = GetFiles(dir);
            foreach (var item in lst)
            {
                ChangeFile(item.FullName);
            }
        }
/*
        private void button3_Click123(object sender, EventArgs e)
        {
            Console.WriteLine("STT");
            string str = @"D:";
            if (!str.EndsWith("\\"))
            {
                str += "\\";
            }
            IList<FileInfo> lst = GetFiles(str);
            if (!Directory.Exists(str))
            {
                try
                {
                    Directory.CreateDirectory(str);
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    Console.ReadKey();
                    return;
                }
            }
            if (File.Exists(str + "test.txt"))
            {
                File.Delete(str + "test.txt");
            }
            FileInfo file = new FileInfo(str + "test.txt");
            if (!file.Directory.Exists)
            {
                Directory.CreateDirectory(file.DirectoryName);
            }
            using (StreamWriter outFileWriter = new StreamWriter(str + "test.txt", false, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                foreach (FileInfo item in lst)
                {
                    sb.Append("\"");
                    sb.Append(item.FullName);
                    sb.Append("\"");
                    sb.Append(",");
                    sb.Append("\r\n");
                }
                sb.Remove(sb.Length - 2, 2);
                outFileWriter.WriteLine(sb.ToString());
                outFileWriter.Flush();
                outFileWriter.Close();
            }
            Console.WriteLine("END");
        }
        */
        private static void GetDirectorys(string strPath, ref List<string> lstDirect)
        {
            DirectoryInfo diFliles = new DirectoryInfo(strPath);
            DirectoryInfo[] diArr = diFliles.GetDirectories();
            //DirectorySecurity directorySecurity = null;  
            foreach (DirectoryInfo di in diArr)
            {
                try
                {
                    //directorySecurity = new DirectorySecurity(di.FullName, AccessControlSections.Access);  
                    //if (!directorySecurity.AreAccessRulesProtected)  
                    //{  
                    lstDirect.Add(di.FullName);
                    GetDirectorys(di.FullName, ref lstDirect);
                    //}  
                }
                catch
                {
                    continue;
                }
            }
        }
        /// <summary>  
        /// 遍历当前目录及子目录  
        /// </summary>  
        /// <param name="strPath">文件路径</param>  
        /// <returns>所有文件</returns>  
        private static IList<FileInfo> GetFiles(string strPath)
        {
            List<FileInfo> lstFiles = new List<FileInfo>();
            List<string> lstDirect = new List<string>();
            lstDirect.Add(strPath);
            DirectoryInfo diFliles = null;
            GetDirectorys(strPath, ref lstDirect);
            foreach (string str in lstDirect)
            {
                try
                {
                    diFliles = new DirectoryInfo(str);
                    lstFiles.AddRange(diFliles.GetFiles());
                }
                catch
                {
                    continue;
                }
            }
            return lstFiles;
        }  






        private void EvnInit(string filename)
        {
            if (textBox2.Text.Length > 0)
            {
                Tag = textBox2.Text;
            }
            else
            {
                Tag = filename;
            }
            if (textBox3.Text.Length > 0)
            {
                Name = textBox3.Text;
            }
            else
            {
                Name = "bruce";
            }
        }
        private string CReplaceContent(string content,string filename)
        {
            EvnInit(filename);
            //正则表达  [^ ^.]{1,}\([ ,A-Za-z]*\)[\s]*{
            MessageBox.Show("尚未开发");
            return "";
        }

        /// <summary>
        /// 1如果没有加log包，那么添加包－－－－－
        //  2跳过switch
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string JavaReplaceContent(string content,string filename)
        {
            EvnInit(filename);

            //正则表达  [^ ^.]{1,}\([ ,A-Za-z]*\)[\s]*{
            string[] split = content.Split('\n');

            //string pattern = "(?<fun>[^ ^.]{1,})[ ]*\\([ ,A-Za-z\\<\\>\\s\\[\\]]*\\)[\\s]*{";
            string pattern = "(?<fun>[^ ^.]{1,})[ ]*\\([^{^;]*{";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(content, pattern);
            int index = 0;
            List<string> listFun = new List<string>(); 

            foreach (Match item in mc)
	        {
                System.Console.WriteLine(item.Value);
                if (listFun.Contains(item.Value))
                {
                    continue;
                }
                else 
                {
                    listFun.Add(item.Value);
                }
                string tempold = item.Value;
                string log = "\r\n\tLog.d(\"" + Tag + "\", \""+Name+" " + item.Groups["fun"] + "() + index " + index + "\");\r\n";
                
                if( CheckContentItem(ref tempold,split))
                {
                    string tempnew = tempold + log;
                    content = content.Replace(tempold, tempnew);
                    index++;
                }
                
	        }

            System.Console.WriteLine("总行数：" + mc.Count);
            //需要对内容进行过滤
            //1如果没有加log包，那么添加包－－－－－
            content =  isImportLog(content, split);
            return content;
        }

        private string isImportLog(string content, string[] split)
        {
            string import = "android.util.Log";
            //import android.util.Log;
            //如果没有导　log 包，是编不过的
            if (content.Contains(import) == true)
            {
                return content;
            }
            else
            {
                foreach (string item in split)
                {
                    if (item.Trim().StartsWith("import ") == true)
                    {
                        string itemNew = item + "\r\nimport android.util.Log;\r\n";
                        content = content.Replace(item, itemNew);
                        break;
                    }
                }
            }

            return content;

        }
        private bool checkKey(string item)
        {
            if (item.Contains("new ") == true || item.Contains("switch") == true || item.Contains("if") == true || item.Contains("while") == true || item.Contains("synchronized") == true)
            {
                return false;
            }
            return true;
        }

        private bool CheckContentItem(ref string tempold, string[] split)
        {
            if (checkKey(tempold) == false)
            {
                return false ;
            }
            if (tempold.Contains("\n"))
            {
                return true;
            }
            

            int  i = 0;
            foreach (string item in split)
            {
                i++;
                if (item.Contains(tempold))
                {
                    if (checkKey(item) == false)
                    {
                        continue;
                    }
                    //下一行判断
                    string nextstr =  jumpSpaceRow(ref i, split);
                    if (i < split.Length && nextstr.Contains("super("))
                    {
                        tempold = split[i]; //改变old字符串，在下一行添加log
                    }
                    if (i < split.Length && split[i-1].Trim().StartsWith("//"))
                    {
                        return false;
                    }
                    return true;
                }
            }

            return false;
        }

        private void testRunCmdroot()
        {

            //string str = @"adb devices&&adb root&&adb remount&&adb push W:\W406A\7731C_6.0_23.6\out\debug\target\product\itel_it1409\system\app\Calendar\Calendar.apk system/app/Calendar&&exit";
            string output = "";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine("adb devices");
            p.StandardInput.WriteLine("adb root");
            p.StandardInput.WriteLine("adb remount");
            p.StandardInput.WriteLine("exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            output = p.StandardOutput.ReadToEnd();
            txtCmd.Text = output;

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine(output);
        }
        private string jumpSpaceRow(ref int i, string[] split)
        {
            while (true)
            {
                if (i >= split.Length)
                {
                    return "";
                }
                if (split[i].Trim() == "" || String.IsNullOrWhiteSpace(split[i].Trim().ToString()))
                {
                    i++;
                    continue;
                }
                else
                {
                    break;
                }
            }

            return split[i];
        }


        private string ReadFile(string p)
        {
            if (System.IO.File.Exists(p) == true)
            {
                return System.IO.File.ReadAllText(p);
            }
            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            testRunCmdroot();
            //testRunCmd();
            //test2();
        }
        private void test2()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(p_Exited);
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.Start();//启动程序
            //开始异步读取输出
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
          
           string strOutput = "ping www.baidu.com";
            p.StandardInput.WriteLine(strOutput);
            p.StandardInput.WriteLine("exit");
            while (!p.StandardOutput.EndOfStream)
            {
                strOutput = p.StandardOutput.ReadLine();
                Console.WriteLine(strOutput);
            }
            //调用WaitForExit会等待Exited事件完成后再继续往下执行。
            p.WaitForExit();
            p.StandardInput.WriteLine(strOutput);
            p.StandardInput.WriteLine("exit");
            while (!p.StandardOutput.EndOfStream)
            {
                strOutput = p.StandardOutput.ReadLine();
                Console.WriteLine(strOutput);
            }
             
        }
        void p_Exited(Object sender, EventArgs e)
        {
            Console.WriteLine("finish");

        }
        void p_OutputDataReceived(Object sender, DataReceivedEventArgs e)
        {
            //这里是正常的输出
            Console.WriteLine(e.Data);

        }

        void p_ErrorDataReceived(Object sender, DataReceivedEventArgs e)
        {
            //这里得到的是错误信息
            Console.WriteLine(e.Data);

        }

    
        private void testRunCmd()
        {

            //string str = @"adb devices&&adb root&&adb remount&&adb push W:\W406A\7731C_6.0_23.6\out\debug\target\product\itel_it1409\system\app\Calendar\Calendar.apk system/app/Calendar&&exit";
             string output = "";
 
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine("adb devices");
            p.StandardInput.WriteLine("adb root");
            p.StandardInput.WriteLine("adb remount");
            string type = "";
            if (radioButton1.Checked)
            {
                type = "push";
            }
            else
            {
                type = "install -r";
            }
            p.StandardInput.WriteLine("adb " + type + " " + textBox4.Text + " " + textBox5.Text);

            p.StandardInput.WriteLine("adb reboot");
            p.StandardInput.WriteLine("exit");
            
            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            output = p.StandardOutput.ReadToEnd();
            txtCmd.Text = output;
                     
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine(output);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Boolean isRecord = false;
            string DevicesPath = "";
            string strPath = textBox4.Text.Replace("/","\\");
            textBox4.Text = strPath;
            if (strPath == "")
            {
                return;
            }
            string dirPath = Path.GetDirectoryName(strPath);
            string[] dirs = dirPath.Split('\\');
            foreach (var item in dirs)
            {
                if (item.Contains("system") == true || isRecord)
                {
                    isRecord = true;
                    DevicesPath += item + "/";
                }
            }
            this.textBox5.Text = DevicesPath.Trim('/');

        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\Administrator\Desktop\IT1409-W406A-6.0-20161028-DEBUG\IT1409-W406A-6.0-20161028-DEBUG\ResearchDownload.exe");
        }
    }
}
