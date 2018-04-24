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
using com.itextpdf.text.pdf.security;

namespace Certificates
{
    class Program
    {
        static void Main(string[] args)
        {

            DesignSign design = new DesignSign();

             design.setFont(@"D:\C++Project\Projects\times.ttf");
           // design.setFont("Tahoma", 12);
            design.getFont().Size = 10;
            design.setLocation("KMA");
            design.setTextColor(70, 70, 240);
            design.setPage(1);


            KMACertificate cert = new KMACertificate();
            cert.loadFromStore();

            design.appendText(new TextNode("Đã được ký điện tử bởi",1,11));
            design.appendText(new TextNode("(Signed digitally by)",0,8));
            design.appendText(new TextNode(" "));
            design.appendText(new TextNode(" "));
            design.appendText(new TextNode(cert.getSubjectByKey("CN"), 1, 11));

            var inx = @"C:/Users/GiayNhap/Desktop/HoangDuong_Mau1.pdf";//args[0];
            string fileName = Path.GetFileNameWithoutExtension(inx);
            string folderPath = Path.GetDirectoryName(inx);
            string outx = folderPath + "\\" + fileName + "_Signed.pdf";

        
           // InspectSignatures(outx);

            cert.SignPdf(inx, outx, design);
       


        }

        public static void InspectSignatures(String path)
        {
            Console.WriteLine(path);
            PdfReader reader = new PdfReader(path);
            AcroFields fields = reader.AcroFields;
            List<String> names = fields.GetSignatureNames();
          
            SignaturePermissions perms = null;
            foreach (String name in names)
            {
                Console.WriteLine("===== " + name + " =====");
        //        perms = InspectSignature(fields, name, perms);
            }
            Console.WriteLine();
        }

    }

}
