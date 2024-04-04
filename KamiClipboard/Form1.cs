using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.LinkLabel;
using Timer = System.Windows.Forms.Timer;
using System.Xml;
using System.Text;
using System.IO;

namespace KamiClipboard
{
    public partial class Form1 : Form
    {


        int counter = 0;
        const int NAME_LENGTH = 20;
        StreamWriter outputFile;
        StreamReader inputFile;
        string fileName = "XMLDB.xml";
        string xmlFile;

        public Form1()
        {
            InitializeComponent();

            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "KamiClipboard");

            Directory.CreateDirectory(path);
            //outputFile = new StreamWriter(Path.Combine(path, fileName));
            //inputFile = new StreamReader(Path.Combine(path, fileName));
            xmlFile = path + "\\XMLDB.xml";

            //WriteXML();

            CreateXML();

            ReadXML();
           
            InitTimer();


 
        }

        void CreateXML()
        {


           

            if (!XMlFileIsReady())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;


                using (XmlWriter writer = XmlWriter.Create(xmlFile, settings))
                {
                    writer.WriteStartElement("ClipboardEntries");
                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
                
            }

           
        }

        bool XMlFileIsReady()
        {
            if(!File.Exists(xmlFile))
            {
                return false;
            }
            else
            {
                long length = new System.IO.FileInfo(xmlFile).Length;

                if(length < 41)
                {
                    return false;
                }
            }

            return true;
        }

        void WriteXML(ClipboardItem item)
        {

          

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            XmlNode root = xmlDoc.DocumentElement;
            XmlNode newEntry = xmlDoc.CreateNode("element", "ClipboardEntry", "");


            XmlNode newItemName = xmlDoc.CreateNode("element", "name", "");
            XmlNode newItemContent = xmlDoc.CreateNode("element", "content", "");
                
            newEntry.AppendChild(newItemName);

            newItemName.InnerText = item.getName();

            newEntry.AppendChild(newItemContent);

            newItemContent.InnerText = item.getContent();

            root.AppendChild(newEntry);
            xmlDoc.Save(fileName);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(xmlFile, settings);
            xmlDoc.WriteTo(writer);
            writer.Close();
           
        }

        void ReadXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);
            XmlNode root = xmlDoc.DocumentElement;

            foreach (XmlNode node in root.ChildNodes)
            {
                XmlNode subNode = node;
                string name = node.SelectSingleNode("name").InnerText;
                string content = node.SelectSingleNode("content").InnerText;

                recordItem(new ClipboardItem(name, content), true);

            }


        }


        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 100; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Clipboard.ContainsText())
            {
                ClipboardItem item = getClipboard();
                
                recordItem(item);
                

            }

        }

        ClipboardItem getClipboard()
        {
            String txt = Clipboard.GetText();
            String name = Regex.Replace(txt, @"\t|\n|\r", ""); //remove tabs and newlines
            name = Regex.Replace(name, @"\s+", " "); //replace repeating spaces with one space
            name = name.Substring(0, name.Length > NAME_LENGTH ? NAME_LENGTH : name.Length); //cut to max NAME_LENGTH size
            name = name.Trim(); //trim leading and trailing spaces
            ClipboardItem item = new ClipboardItem(name, txt);

            return item;
        }

        void recordItem(ClipboardItem item, bool fromXML = false)
        {
            if(!listBox1.Items.Contains(item))
            {
                listBox1.Items.Add(item);
                if (!fromXML)
                {
                    WriteXML(item);
                }

                using(outputFile)
                {

                }
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