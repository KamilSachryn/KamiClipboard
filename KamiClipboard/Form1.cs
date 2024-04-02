using System.Windows.Forms.VisualStyles;

namespace KamiClipboard
{
    public partial class Form1 : Form
    {

        int counter = 0;

        public Form1()
        {
            InitializeComponent();
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