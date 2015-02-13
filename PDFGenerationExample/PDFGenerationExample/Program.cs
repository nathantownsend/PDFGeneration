using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFGenerationExample
{
    class Program
    {
        static void Main(string[] args)
        {

            string templatePath  = @"C:\Users\cb0290\Documents\GitHub\PDFGeneration\PDFGenerationExample\PDFGenerationExample\Template.html";

            PDFGenerator generator = new PDFGenerator(templatePath);

            string content = GenerateContentFromDatabase();

            string html = generator.MergeTemplate(content);
            
            generator.CreatePDF(html, @"C:\temp\test.pdf");

        }


        private static string GenerateContentFromDatabase()
        {
            string[] sections = new string[] { "First", "Second", "Third" };
            string[] pages = new string[] { "Page 1", "Page 2", "Page 3" };
            Dictionary<string, string> rules = new Dictionary<string, string>();
            rules.Add("303(1)(a)", "the name, permanent and temporary post office addresses, telephone numbers, and, as applicable, social security numbers and employer identification numbers of the applicant, applicant's resident agent, and the person who will pay the abandoned mine reclamation fee pursuant to 30 USC 1232");
            rules.Add("303(1)(b)", "the names and addresses of any persons who are engaged in strip or underground mining on behalf of the applicant and any person who will conduct such operations should the permit be granted;");

            StringBuilder sb = new StringBuilder();
            foreach (string section in sections)
            {
                sb.AppendFormat("<h2>{0}</h2>", section);
                foreach (string page in pages)
                {
                    sb.AppendFormat("<h3>{0}</h3>", page);

                    sb.Append("<h4>Instructions</h4>");
                    sb.Append("<div class=\"instructions\">");
                    sb.Append("Instructions go here"); // instructios from the help screen
                    sb.Append("</div>");

                    sb.Append("<h4>Rules</h4>");
                    sb.Append("<div class=\"subsections\">");
                    foreach (KeyValuePair<string, string> rule in rules)
                    {
                        string subsection = rule.Key;
                        string description = rule.Value;
                        sb.AppendFormat("<p><strong>{0}:</strong>{1}</p>", subsection, description); // append rules like this
                    }
                    sb.Append("</div>");
                }

            }

            return sb.ToString();

        }

    }
}
