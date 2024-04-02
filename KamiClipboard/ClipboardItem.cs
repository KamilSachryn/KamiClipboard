using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace KamiClipboard
{
    internal class ClipboardItem
    {
        String name;
        String content;

        public ClipboardItem(string name, string content)
        {
            this.name = name;
            this.content = content;
        }

        public String getName()
        {

            return name;
        }

        public String getContent()
        {
            return content;
        }

        public override string ToString()
        {
            return name;
        }

    }
}
