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
    class KMACertificate
    {
        X509Certificate2 cert = null;
        List<KeyValuePair<string, string>> info;
        public bool loadFromStore()
        {
            X509Store store = new X509Store(StoreName.My,StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(store.Certificates, null, null, X509SelectionFlag.SingleSelection);
            if (sel.Count > 0)
            {
                cert = sel[0];
                this.afterLoading();

                return true;
            }
            return false;
        }
        public bool loadFromFile(string path,string pass)
        {
            var cert = new X509Certificate2(path, pass, X509KeyStorageFlags.MachineKeySet);
            if (cert == null) return false;
            this.afterLoading();
            return true;
        }
        public string getCertificateName()
        {
            return cert.GetName();
        }
        public void afterLoading()
        {
     
            this.info = cert.Subject.Split(',').Select(x => new KeyValuePair<string, string>(x.Split('=')[0].TrimStart(' '), x.Split('=')[1])).ToList();
           
        }
        // get MST value
        public string getMST()
        {
            for (int k=0; k< info.Count;k++)
            {
                if (info[k].Value.IndexOf("MST:")>=0)
                     return info[k].Value;
            }
            return null;
        }
        // get MST only number
        public string getMST_EX()
        {
            var str = this.getMST();
            if (str == null) return null;
            return str.Substring(4);
        }

        public bool checkDate(object current )
        {
            if (current == null)
            {
                current = DateTime.Now;
            }
            var cur_Date = (DateTime)current;
            var strDate = this.cert.GetExpirationDateString();
            if (strDate == null) return false;
            var date = DateTime.Parse(strDate);
            if (cur_Date.CompareTo(date)>=0) return false;
            return true;
 
        }
        public KeyValuePair<string,string> getSubjectByIndex(int index)
        {
            return this.info[index];
        }

        public string getSubjectByKey(string key)
        {
           for (int i =0;i< this.info.Count;i++)
            {
                if (this.info[i].Key.Equals(key))
                return this.info[i].Value;
            }
            return null;
        }
        public string getSerialNumber()
        {
            return cert.GetSerialNumberString();
        }
        public PdfSignatureAppearance makePdfSignature(PdfSignatureAppearance signatureAppearance, DesignSign design)
        {

            /* signatureAppearance.Layer2Text = 
                "K\x00fd bởi: " + this.getSubjectByKey("CN") + 
                "\nK\x00fd ng\x00e0y: " + string.Format("{0:d/M/yyyy HH:mm:ss}"+
                "\nMã số thuế:"+this.getMST_EX()
                , DateTime.Now);
           */
            // iTextSharp.text.Font font2 = new iTextSharp.text.Font(BaseFont.CreateFont(@"D:\C++Project\Projects\times.ttf", "Identity-H", false));
            //    iTextSharp.text.Font font2 = new iTextSharp.text.Font(BaseFont.CreateFont("Tahoma", "UTF-8", false));

            signatureAppearance.Location = design.getLocation();
            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            signatureAppearance.SignDate = DateTime.Now;
            signatureAppearance.Layer2Font = design.getFont();
            signatureAppearance.Layer2Font.SetColor(design.getColor().r, design.getColor().g, design.getColor().b);
            signatureAppearance.Layer2Text = design.getString();
          //  signatureAppearance.SetVisibleSignature(design.getRect(),design.getPage(), null);


            return signatureAppearance;
        }
 
        public PdfFormField formField(PdfStamper stamper, int page, DesignSign design)
        {
            PdfWriter writer = stamper.Writer;
            PdfFormField sig = PdfFormField.CreateSignature(writer);
            
            sig.SetWidget(design.getRect(), null);
            sig.SetHighlighting(PdfAnnotation.HIGHLIGHT_NONE);
            sig.Color = new BaseColor(design.getColor().r, design.getColor().g, design.getColor().b);
            sig.FieldName = design.getString();
            TextField field = new TextField(stamper.Writer, new iTextSharp.text.Rectangle(40, 500, 360, 530), "some_text");

            sig.AddKid(field.GetTextField());
          



            sig.Title = design.getString(); 
            // sig.SetFlags(PdfAnnotation.FLAGS_READONLY | PdfAnnotation.FLAGS_PRINT);
            //sig.SetFieldName("sig3");
            //sig.SetPage(page);
            //   stamper.addAnnotation(sig, 1);

            stamper.AcroFields.SetFieldProperty("sign-kma", "textfont", design.getFont(),
        null);
            sig.SetFieldFlags(PdfAnnotation.FLAGS_READONLY | PdfAnnotation.FLAGS_PRINT);
           
            return sig; 
        }

        public bool SignPdf(string SourcePdfFileName,string DestPdfFileName, DesignSign design )
        {


            var chain = __makeChain();

            try { 
            IExternalSignature externalSignature = new iTextSharp.text.pdf.security.X509Certificate2Signature(cert, "SHA-1");

            PdfReader pdfReader = new PdfReader(SourcePdfFileName);
            FileStream signedPdf = new FileStream(DestPdfFileName, FileMode.Create );  
            PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
            PdfWriter writer = pdfStamper.Writer;
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;


            if (design!=null)
            signatureAppearance = makePdfSignature(signatureAppearance, design);



                //MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CADES);
                // MessageBox.Show("Done");
                for (int i = 0; i < pdfReader.NumberOfPages; ++i)
                {

                        __drawFooter(pdfStamper,design,i+1);
                    //PdfFormField form = formField(pdfStamper, i + 1,design);
                    //pdfStamper.AddAnnotation(form, i + 1);
                    
                }

                MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);


                signedPdf.Close();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        private Org.BouncyCastle.X509.X509Certificate[] __makeChain()
        {
           Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
           return  new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
        }

        private void __drawFooter(PdfStamper pdfStamper, DesignSign design,int page)
        {
            PdfContentByte overContent = pdfStamper.GetOverContent(page );
 
            ColumnText col = new ColumnText(overContent);
            col.SetSimpleColumn(design.getRect());
            col.Alignment = Element.ALIGN_BOTTOM;
        
           
            col.AddElement(  new Phrase(design.getString(), design.getFont()));
            col.Go();
             
        }








}


}
