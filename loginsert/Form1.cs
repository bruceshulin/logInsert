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
            //打开文件
            string content = ReadFile(textBox1.Text);
            //解析文件
            //替换文件
            if (content == "")
            {
                return;
            }
            string ext = Path.GetExtension(textBox1.Text);


            if (ext.ToLower().EndsWith(".java") == true)
            {
                content = JavaReplaceContent(content);
            }
            else if (ext.ToLower().EndsWith(".c") == true)
            {
                content = JavaReplaceContent(content);
            }
            

            string filename =  "content"+ DateTime.Now.Ticks.ToString().Substring(7,3)+".txt";
            if (checkBox1.Checked)
            {
                System.IO.File.WriteAllText(textBox1.Text, content);
            }
            else 
            {
                System.IO.File.WriteAllText("D:\\" + filename, content);
            }
            MessageBox.Show("完成");
            
        }
        private void EvnInit()
        {
            if (textBox2.Text.Length > 0)
            {
                Tag = textBox2.Text;
            }
            else
            {
                Tag = "TAG";
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
        private string CReplaceContent(string content)
        {
            EvnInit();
            //正则表达  [^ ^.]{1,}\([ ,A-Za-z]*\)[\s]*{
            MessageBox.Show("尚未开发");
            return "";
        }

        private string JavaReplaceContent(string content)
        {
            EvnInit();
            //正则表达  [^ ^.]{1,}\([ ,A-Za-z]*\)[\s]*{
            string[] split = content.Split('\n');

            string pattern = "(?<fun>[^ ^.]{1,})\\([ ,A-Za-z\\<\\>]*\\)[\\s]*{";
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
                string log = "\r\n\tLog.d(" + Tag + ", \""+Name+" " + item.Groups["fun"] + "() + index " + index + "\");\r\n";
                index++;
                if( CheckContentItem(ref tempold,split))
                {
                    string tempnew = tempold + log;
                    content = content.Replace(tempold, tempnew);
                }
                
	        }

            System.Console.WriteLine("总行数：" + mc.Count);
            //需要对内容进行过滤
            return content;
        }

        private bool CheckContentItem(ref string tempold, string[] split)
        {
            int  i = 0;
            foreach (string item in split)
            {
                i++;
                if (item.Contains(tempold) && item.Contains("new ") == false)
                {
                    if (i < split.Length && split[i].Contains("super("))
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
    }
}
