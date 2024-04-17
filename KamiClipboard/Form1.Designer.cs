using System.Windows.Forms;

namespace KamiClipboard
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_delete = new System.Windows.Forms.Button();
            this.notifyIcon_trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox_itemList = new System.Windows.Forms.ListBox();
            this.textBox_Content = new System.Windows.Forms.RichTextBox();
            this.button_copy = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(30, 373);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(75, 23);
            this.button_delete.TabIndex = 0;
            this.button_delete.Text = "Remove";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_Delete);
            // 
            // notifyIcon_trayIcon
            // 
            this.notifyIcon_trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.notifyIcon_trayIcon.BalloonTipText = "TestText";
            this.notifyIcon_trayIcon.BalloonTipTitle = "TestTitle";
            this.notifyIcon_trayIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon_trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon_trayIcon.Icon")));
            this.notifyIcon_trayIcon.Text = "notifyIcon1";
            this.notifyIcon_trayIcon.Visible = true;
            this.notifyIcon_trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
            this.notifyIcon_trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // listBox_itemList
            // 
            this.listBox_itemList.FormattingEnabled = true;
            this.listBox_itemList.ItemHeight = 15;
            this.listBox_itemList.Location = new System.Drawing.Point(30, 12);
            this.listBox_itemList.Name = "listBox_itemList";
            this.listBox_itemList.Size = new System.Drawing.Size(212, 334);
            this.listBox_itemList.TabIndex = 1;
            this.listBox_itemList.SelectedIndexChanged += new System.EventHandler(this.itemList_SelectedIndexChanged);
            this.listBox_itemList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.itemList_MouseDoubleClick);
            // 
            // textBox_Content
            // 
            this.textBox_Content.Location = new System.Drawing.Point(316, 12);
            this.textBox_Content.Name = "textBox_Content";
            this.textBox_Content.ReadOnly = true;
            this.textBox_Content.Size = new System.Drawing.Size(445, 334);
            this.textBox_Content.TabIndex = 2;
            this.textBox_Content.Text = "";
            // 
            // button_copy
            // 
            this.button_copy.Location = new System.Drawing.Point(111, 373);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(75, 23);
            this.button_copy.TabIndex = 3;
            this.button_copy.Text = "Copy";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.textBox_Content);
            this.Controls.Add(this.listBox_itemList);
            this.Controls.Add(this.button_delete);
            this.Name = "Form1";
            this.Text = "KamiClipboard";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button button_delete;
        private NotifyIcon notifyIcon_trayIcon;
        private ListBox listBox_itemList;
        private RichTextBox textBox_Content;
        private Button button_copy;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}