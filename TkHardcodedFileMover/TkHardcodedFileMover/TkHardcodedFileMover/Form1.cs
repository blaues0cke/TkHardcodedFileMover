using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TkHardcodedFileMover
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            moveToRaumfeld(@"C:\Dropbox\@home\Raumfeld");
            moveToNAS(@"C:\Dropbox\@home\Raumfeld");
            moveToNAS(@"C:\Dropbox\@home\NAS");



        }


        private void moveTo(String sDir, String tDir, bool deleteAfterCopy)
        {
            ProgressBar1.Maximum = 0;
            FileInfo theFileInfo = new FileInfo(sDir);
            string[] dirs = Directory.GetDirectories(sDir);
            ProgressBar1.Maximum += dirs.Length;
            addLog("Processing folder " + ProgressBar1.Value.ToString() + " of " + ProgressBar1.Maximum.ToString());

            foreach (string dirName in dirs)
            {
                string shortDirName = new DirectoryInfo(dirName).Name;

                ++ProgressBar1.Value;
                addLog("Processing folder " + ProgressBar1.Value.ToString() + " of " + ProgressBar1.Maximum.ToString() + ": " + dirName + " (" + shortDirName + ")");

                string dst = tDir + shortDirName;

                addLog("Copying " + dirName + " to " + dst);
                Application.DoEvents();
                copyDirectory(dirName, dst);

                Application.DoEvents();
                addLog("Done!");


                if (deleteAfterCopy)
                {
                    delteDir(dirName);
                }

            }

            
        }

        private void delteDir(String dir)
        {
            addLog("Deleting " + dir);
    
           
            try
            {
                Directory.Delete(dir, true);
            }
            catch (IOException)
            {
                Thread.Sleep(10);
                Directory.Delete(dir, true);
            }
        }

        private void moveToRaumfeld(String sDir)
        {
            moveTo(sDir, @"Z:\", false);
        }

        private void moveToNAS(String sDir)
        {
            moveTo(sDir, @"M:\", true);
        }

        private void addLog(String log)
        {
            TextBox1.Text = log + "\r\n" + TextBox1.Text;
        }

        private void copyDirectory(String from, String to)
        {
            
//Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(from, "*",
    SearchOption.AllDirectories))
            {
                addLog("Copying directory " + from + " to " + to);
                Application.DoEvents();
                Directory.CreateDirectory(dirPath.Replace(from, to));
            }

//Copy all the files
            foreach (string newPath in Directory.GetFiles(from, "*.*",
    SearchOption.AllDirectories))
            {


                addLog("Copying " + newPath + " to " + newPath.Replace(from, to));
                Application.DoEvents();

                File.Copy(newPath, newPath.Replace(from, to), true);
            }
        }

        // 
        //
    }
}
