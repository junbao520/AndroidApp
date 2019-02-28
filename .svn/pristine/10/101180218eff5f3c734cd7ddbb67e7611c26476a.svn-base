using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace AppDataCreate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private byte[] allBytes { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
         
        }
        /// <summary>
        /// 数据库路径
        /// </summary>
        string path = string.Empty;
        /// <summary>
        /// xml文件路径
        /// </summary>
        string xmlpath = string.Empty;
        /// <summary>
        /// 选择的数据库名
        /// </summary>
        string dataName= string.Empty;
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            var t = Application.StartupPath;
            openFileDialog1.InitialDirectory = t;
            //openFileDialog1.Filter = "数据文件(*.db)|*.db";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               path = openFileDialog1.FileName;
                //获取数据库名
                 int index=path.LastIndexOf("\\");
                dataName = path.Substring(index,path.Length-index).Replace("\\","");

                
                this.txtpath.Text = path;
                xmlpath=ProcessPath(path);
                this.txtXmlpath.Text = xmlpath;
                
                if (!File.Exists(xmlpath))
                {
                    string str = "未检测到XML文件:" + xmlpath;
                    MessageBox.Show(str,"提示");
                    return;
                }
                XmlOperation.SelectedXmlPath = xmlpath;
            }
        }

        string xmldefault = "Resources\\drawable\\defaultSqliteData.xml";
        private string ProcessPath(string tempPath)
        {
          return  Application.StartupPath + "\\XML\\DefaultData.xml";
            //if (!string.IsNullOrEmpty(tempPath))
            //{
            //    if (tempPath.Contains("Data"))
            //    {
            //       int index= tempPath.IndexOf("Data");
            //        string p=tempPath.Substring(0, index);
            //        p += xmldefault;
            //        return p;
            //    }
            //}
            //return string.Empty;
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            allBytes = File.ReadAllBytes("breke.wav");
            string str = Convert.ToBase64String(allBytes);
            // var r1=XmlOperation.SetAppSetByKey("SqlDataName", string.Empty);
            File.WriteAllText("test.txt", str);
           // var r1=XmlOperation
            //var r2=XmlOperation.SetAppSetByKey("FileDataStream", str);
            //if ( r2 == false)
            //{
            //    MessageBox.Show("写入失败！");
            //}
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
               string filename="test.xml";
               saveFileDialog1.FileName = filename;
               //易考星
               //易考星
               //
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var str3 = XmlOperation.config.AppSettings.Settings["FileDataStream"].Value;
                    byte[] read = Convert.FromBase64String(str3);

                    File.WriteAllBytes(saveFileDialog1.FileName, read);
                    this.richTextBox1.Text = "导出路径："+ saveFileDialog1.FileName;
                }

            }
            catch (Exception ex)
            {

                string str = ex.Message;
            }

        }

        private void txtXmlpath_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
