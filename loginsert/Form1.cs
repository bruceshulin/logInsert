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
            content = replaceContent(content);
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

        private string replaceContent(string content)
        {
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
                string tempnew = item.Value + log;
                index++;
                if( CheckContentItem(tempold,split))
                {
                    content = content.Replace(tempold, tempnew);
                }
                
	        }

            System.Console.WriteLine("总行数：" + mc.Count);
            //需要对内容进行过滤
            return content;
        }

        private bool CheckContentItem(string tempold, string[] split)
        {
            foreach (string item in split)
            {
                if (item.Contains(tempold) && item.Contains("new ") == false)
                {
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
