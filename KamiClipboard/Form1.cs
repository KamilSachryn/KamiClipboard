using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Timer = System.Windows.Forms.Timer;

namespace KamiClipboard
{
    public partial class Form1 : Form
    {


        int counter = 0;
        const int NAME_LENGTH = 20;

        public Form1()
        {
            InitializeComponent();

            InitTimer();
 
        }

        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Clipboard.ContainsText())
            {
                String txt = Clipboard.GetText();




                ClipboardItem item = new ClipboardItem(Regex.Replace(Regex.Replace(txt, @"\t|\n|\r", ""), @"\s+", " ").Substring(0, txt.Length > NAME_LENGTH ? NAME_LENGTH : txt.Length).Trim(), txt);
                addToListBox(item);

            }

        }

        void addToListBox(ClipboardItem item)
        {
            if(!listBox1.Items.Contains(item))
            {
                listBox1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ClipboardItem temp = new ClipboardItem("name", counter++.ToString()) ;

            listBox1.Items.Add(temp);

            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            bool cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
            }



        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClipboardItem temp = (ClipboardItem)listBox1.SelectedItem;


            richTextBox1.Text = temp.getContent();
        }




    }
}