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
            InitNotifyIcon();
          

            

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

        void InitNotifyIcon()
        {
            NotifyIcon icon = this.notifyIcon_trayIcon;


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
            listBox_itemList.Items.Remove(listBox_itemList.SelectedItem);
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


        //add item to gui and xml
        void recordItem(ClipboardItem item, bool fromXML = false)
        {
            //if the listbox doesnt contain the item and the item has content
            if(!listBox_itemList.Items.Contains(item) && cleanText(item.getName()).Length > 0)
            {

                //if the item comes from XML dont add it to XML again
                //else add it to both gui and xml
                if (fromXML)
                {
                    listBox_itemList.Items.Add(item);
                }
                else
                {
                    listBox_itemList.Items.Insert(0, item);
                    
                    WriteXML(item);
                }
            }
        }

        //Delete an item 
        private void button_Delete(object sender, EventArgs e)
        {

            //load item into memory
            ClipboardItem item = (ClipboardItem)listBox_itemList.SelectedItem;
            //if an item was selected
            if(item != null)
            {
                //remove entry from GUI and XML
                DeleteXML(item);
                //remove content from view
                textBox_Content.Clear();
            }



            
        }

        //when program is minimized send it to tray instead
        protected override void OnResize(EventArgs e)
        {
            //event call
            base.OnResize(e);

            //if we're minimized
            if (this.WindowState == FormWindowState.Minimized)
            {
                //send to tray
                SendTotray();
            }



        }

        //Send application to tray
        void SendTotray()
        {
            //make the notifyIcon visible in tray
            this.notifyIcon_trayIcon.Visible = true;
            //make the taskbar not show the program
            this.ShowInTaskbar = false;
            //hide gui
            this.Hide();
            

        }

        //same as MouseClick
        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        //if the tray icon is clicked make the gui appear
        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                showFromTray();
            }
        }

        //makes gui appear from tray
        private void showFromTray()
        {
            //set windowstate to be shown
            this.WindowState = FormWindowState.Normal;
            //make visible in taskbar
            this.ShowInTaskbar = true;
            //show gui
            this.Show();
        }

        //change content box when a new clipboard item is seletced
        private void itemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load selected clipboard item into memory
            ClipboardItem temp = (ClipboardItem)listBox_itemList.SelectedItem;

            //if an item was selected
            if (temp != null)
            {
                //display content
                textBox_Content.Text = temp.getContent();
            }
        }

        //copy selected clipboard item's content into clipboard
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            //load selected clipboard item into memory
            ClipboardItem item = (ClipboardItem)listBox_itemList.SelectedItem;
            //if an item was selected
            if (item != null)
            {
                //push text into clipboard
                Clipboard.SetText(item.getContent());
            }


        }

        //if we double click an intentory item then copy it into memory directly
        private void itemList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //load selected clipboard item into memory
            ClipboardItem item = (ClipboardItem)listBox_itemList.SelectedItem;
            //if an item was selected
            if (item != null)
            {
                //push text into clipboard
                Clipboard.SetText(item.getContent());
            }
        }

        //On GUI Show
        private void Form1_Shown(object sender, EventArgs e)
        {
            //if we're launching from .exe
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                
                //minimize window
                this.WindowState = FormWindowState.Minimized;

                //hide from taskbar
                this.Hide();
                return;
            }
           
        }

        //on clicking exit in tray
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();

        }

        //on clicking open in tray
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showFromTray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox_Content_TextChanged(object sender, EventArgs e)
        {

        }
    }
}