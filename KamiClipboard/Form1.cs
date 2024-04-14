using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.LinkLabel;
using Timer = System.Windows.Forms.Timer;
using System.Xml;
using System.Text;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Diagnostics;

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

          

            

            //set xml file location in mydocuments
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "KamiClipboard");

            //create dir if doesnt exist
            Directory.CreateDirectory(path);

            xmlFile = path + "\\XMLDB.xml";

            //WriteXML();


            //create base file
            CreateXML();

            //get data from xml
            ReadXML();
           
            //begin timer loop to check clipboard
            InitTimer();


        }

        //Creates or updates the db file
        void CreateXML()
        {


           
            //if db either doesnt exists or isnt formatted properly
            if (!XMlFileIsReady())
            {
                //set indents for human readability
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;


                //write empty database with root element
                using (XmlWriter writer = XmlWriter.Create(xmlFile, settings))
                {
                    writer.WriteStartElement("ClipboardEntries");
                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
                
            }

           
        }

        //checks if the DB file exists and is formatted properly
        bool XMlFileIsReady()
        {
            if(!File.Exists(xmlFile))
            {
                return false;
            }
            else
            {
                long length = new System.IO.FileInfo(xmlFile).Length;

                if(length < 41) //size of xml header
                {
                    return false;
                }
            }

            return true;
        }

        //writes a new item to the database
        void WriteXML(ClipboardItem item)
        {

          
            //load db file into memory
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            //find root element
            XmlNode root = xmlDoc.DocumentElement;

            //Create parent node to be added
            XmlNode newEntry = xmlDoc.CreateNode("element", "ClipboardEntry", "");

            //Create children nodes to hold entry name and entry content
            XmlNode newItemName = xmlDoc.CreateNode("element", "name", "");
            XmlNode newItemContent = xmlDoc.CreateNode("element", "content", "");
                
            //append name child to parent
            newEntry.AppendChild(newItemName);

            //add name to the db entry
            newItemName.InnerText = item.getName();

            //append content child to parent
            newEntry.AppendChild(newItemContent);

            //add content to the db entry
            newItemContent.InnerText = item.getContent();

            //append new node to the database
            root.AppendChild(newEntry);

            //write to db
            xmlDoc.Save(fileName);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(xmlFile, settings);
            xmlDoc.WriteTo(writer);
            writer.Close();
           
        }

        //read all entries from db and push to memory
        void ReadXML()
        {
            //load db into memory
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            //find root
            XmlNode root = xmlDoc.DocumentElement;

            //create stack to reverse output later on
            Stack<ClipboardItem> stack = new Stack<ClipboardItem>();

            //loop through nodes, get name and content, add to stack
            foreach (XmlNode node in root.ChildNodes)
            {
                XmlNode subNode = node;
                string name = node.SelectSingleNode("name").InnerText;
                string content = node.SelectSingleNode("content").InnerText;


                stack.Push(new ClipboardItem(name, content));
                

            }

            //reverse list so newest item shows up ontop
            while(stack.Count > 0)
            {
                recordItem(stack.Pop(), true);
            }


        }

        //remove 1 item from the list and db
        void DeleteXML(ClipboardItem item)
        {

            //load db into memory
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            //find root
            XmlNode root = xmlDoc.DocumentElement;

            //loop through nodes until we find matching content
            foreach (XmlNode node in root.ChildNodes)
            {
                XmlNode subNode = node;
                string name = node.SelectSingleNode("name").InnerText;
                string content = node.SelectSingleNode("content").InnerText;

                //remove node from parent
                if(content == item.getContent())
                {
                    root.RemoveChild(node);
                }
                

            }

            //save to file
            SaveXMLToFile(xmlDoc);
            
            //remove from gui list
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        void SaveXMLToFile(XmlDocument xmlDoc)
        {
            xmlDoc.Save(fileName);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(xmlFile, settings);
            xmlDoc.WriteTo(writer);
            writer.Close();
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
            String txt = Clipboard.GetText().Trim();
            String name = cleanText(txt);
            name = name.Substring(0, name.Length > NAME_LENGTH ? NAME_LENGTH : name.Length); //cut to max NAME_LENGTH size
            name = name.Trim(); //trim leading and trailing spaces
            ClipboardItem item = new ClipboardItem(name, txt);

            return item;
        }

        string cleanText(string str)
        {
            str = Regex.Replace(str, @"\t|\n|\r", ""); //remove tabs and newlines
            str = Regex.Replace(str, @"\s+", " "); //replace repeating spaces with one space
            str = str.Trim(); //trim leading and trailing spaces
            return str;
        }

        void recordItem(ClipboardItem item, bool fromXML = false)
        {
            if(!listBox1.Items.Contains(item) && cleanText(item.getName()).Length > 0)
            {

                

                //listBox1.Items.Add(item);
                if (fromXML)
                {

                    listBox1.Items.Add(item);
                }
                else
                {
                    listBox1.Items.Insert(0, item);
                    
                    WriteXML(item);
                }

                using(outputFile)
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ClipboardItem item = (ClipboardItem)listBox1.SelectedItem;
            if(item != null)
            {
                DeleteXML(item);
                richTextBox1.Clear();
            }



            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.WindowState == FormWindowState.Minimized)
            {
                SendTotray();
            }



        }

        void SendTotray()
        {
            bool cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            this.notifyIcon1.Visible = true;
           // this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
            

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

            if (temp != null)
            {
                richTextBox1.Text = temp.getContent();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClipboardItem item = (ClipboardItem)listBox1.SelectedItem;
            if (item != null)
            {
                Clipboard.SetText(item.getContent());
            }


        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClipboardItem item = (ClipboardItem)listBox1.SelectedItem;
            if (item != null)
            {
                Clipboard.SetText(item.getContent());
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            if (!System.Diagnostics.Debugger.IsAttached)
            {
                
                //to minimize window
                this.WindowState = FormWindowState.Minimized;

                //to hide from taskbar
                this.Hide();
                return;
            }
           
        }
    }
}