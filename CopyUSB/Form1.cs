using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyUSB
{
    public partial class CopyUSB : Form
    {
        public CopyUSB()
        {
            InitializeComponent();
           
        }

         String usb = "";
         String destination = "";
         bool isDestinaionSet=false;

        private const int WM_DEVICECHANGE = 0x219;
        private const int WM_DEVICEARRIVAL = 0x8000;
        private const int WM_DEVICEREMOVED = 0x8004;

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                location.Text = fbd.SelectedPath;
                
                var time = DateTime.Now;
                destination = Path.Combine(fbd.SelectedPath, "CopyUSB "+time.ToString("dd-MM-yyyy_HH-mm-ss"));
                
                Directory.CreateDirectory(destination);
                isDestinaionSet = true;
                
            }        
           
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (isDestinaionSet)
            {
                if ((int)m.WParam == WM_DEVICEARRIVAL)
                    setSource();
                else if ((int)m.WParam == WM_DEVICEREMOVED)
                    Application.Exit();
            }
        }


        private void setSource()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    usb = drive.ToString();
                    copy(usb, destination);

                }
            }
        }


        private void copy(string source, string destination)
        {
            
            

            String filename = "";
            String target = "";

            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

                 String[] files = Directory.GetFiles(source);
            foreach (String file in files)
            {
                filename = Path.GetFileName(file);
               
                    target = Path.Combine(destination, filename);
                    File.Copy(file, target);
                
            }

            String[] folders = Directory.GetDirectories(source);
            foreach(String folder in folders)
            {
                filename = Path.GetFileName(folder);
                if (filename != "System Volume Information")
                {
                    target = Path.Combine(destination, filename);
                    copy(folder, target);
                }
            }
           
            }

        private void hideBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (checkBox1.Checked == false)
                notifyIcon1.Visible = false;
                   
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void pwdText_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

