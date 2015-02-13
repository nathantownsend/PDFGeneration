using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.IO;
using iTextSharp.tool.xml;
using System.Text.RegularExpressions;

namespace PDFGenerationExample
{
    public class PDFGenerator : PdfPageEventHelper 
    {
        string _templatepath;

        public PDFGenerator(string templatePath)
        {
            _templatepath = templatePath;

        }

        public void CreatePDF(string html, string outputPath)
        {
            WritePDF(html, outputPath);
        }

        public string MergeTemplate(string GeneratedContent)
        {
            string template = File.ReadAllText(_templatepath);
            return template.Replace("#ContentBody#", GeneratedContent);
        }


        private void WritePDF(string html, string outputPath)
        {
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, fs))
                    {
                        //Open our document for writing
                        doc.Open();

                        //Fully qualified path to our font
                        var verdana = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Verdana.TTF");
                        var times = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Times.ttf ");

                        //Register our font and give it an alias to reference in CSS
                        var fontProv = new XMLWorkerFontProvider();
                        fontProv.Register(verdana, "Verdana");
                        fontProv.Register(times, "Times New Roman");

                        //Extract the CSS from the html document
                        Regex re = new Regex("<style type=\"text/css\">[^</style>]+</style>", RegexOptions.IgnoreCase| RegexOptions.Multiline);
                        string css = re.Match(html).Value.Replace("<style type=\"text/css\">", "").Replace("</style>", "");
                        

                        //Create a stream to read our HTML
                        using (var htmlMS = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                        {
                            //Create a stream to read our CSS
                            using (var cssMS = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(css)))
                            {
                                //Get an instance of the generic XMLWorker
                                var xmlWorker = XMLWorkerHelper.GetInstance();

                                //Parse our HTML using everything setup above
                                xmlWorker.ParseXHtml(writer, doc, htmlMS, cssMS, System.Text.Encoding.UTF8, fontProv);
                            }
                        }

                        //Close and cleanup
                        doc.Close();

                    }
                }
            }
        }



    }
}

