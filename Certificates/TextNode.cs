using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Certificates
{
    class TextNode
    {
        public string text = "";
        public int fontSize= 12;
        public int style = 0;
        public string ToString()
        {
            return this.text;
        }
        public TextNode(string text)
        {
            this.text = text;
        }
        public TextNode(string text, int style,int fontSize)
        {
            this.text = text;
            this.fontSize = fontSize;
            this.style = style;
        }
    }
}
