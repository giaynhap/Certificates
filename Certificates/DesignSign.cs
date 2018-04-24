using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text;

namespace Certificates
{
    public struct color
    {
        public int r;
        public int g;
        public int b;
    };
    class DesignSign
    {

       public List<TextNode> texts;
        string location;
        iTextSharp.text.Font font;
        iTextSharp.text.Rectangle rect;
        int page;
        
 
   
        color mColor;
        public void setPage(int page)
        {
            this.page = page;
        }
        public int getPage()
        {
            return this.page;
        }
        public void setLocation(string loc)
        {
            this.location = loc;
        }
        public color getColor()
        {
            return this.mColor;
        }
        public string getLocation()
        {
            return this.location;
        }
       public void setFont(string filefont,string indent = "Identity-H" )
        {
           this.font= new iTextSharp.text.Font(BaseFont.CreateFont(filefont, indent, false));
        }
        public void setFont(String fontname,int size)
        {
            this.font = FontFactory.GetFont(fontname, size, BaseColor.BLACK); ;
        }
        public iTextSharp.text.Font getFont()
        {
            return this.font;
        }
        public void setRect(float x,float y,float width,float height)
        {
            this.rect = new iTextSharp.text.Rectangle(x, y, x + width, y + height);
        }
        public iTextSharp.text.Rectangle getRect()
        {
            return this.rect;
        }
        public void appendText(TextNode text)
        {
            if (texts == null) texts = new List<TextNode>();
            texts.Add(text);
        }
        public void setTextColor(int r,int g,int b)
        {
            mColor.r = r;
            mColor.g = g;
            mColor.b = b;
        }
        // make string
       

    }
}
