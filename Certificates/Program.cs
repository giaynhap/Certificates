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
            design.setRect(5, 0, 500, 20);
            design.setTextColor(70, 70, 70);
            design.setPage(1);


            KMACertificate cert = new KMACertificate();
            cert.loadFromStore();

            design.appendText("@KÝ SỐ BỞI " + cert.getSubjectByKey("CN"));
      
            var inx = args[0];
            string fileName = Path.GetFileNameWithoutExtension(inx);
            string folderPath = Path.GetDirectoryName(inx);
            string outx = folderPath + "\\" + fileName + "_Signed.pdf";

        
           // InspectSignatures(outx);

            cert.SignPdf(inx, outx, design);
            Console.ReadKey();
            


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
