using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Hosting;

namespace CSharpGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            RuntimeTextTemplate1 sft = new RuntimeTextTemplate1();
            var json = "{\"employee\": { \"name\": \"John Doe\",\"title\":\"Developer\",\"age\": \"30\"}}";
            dynamic data = JObject.Parse(json);

            string className = string.Empty;
            List<string> props = new List<string>();
            foreach (JProperty clss in data.Properties())
            {
                className = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(clss.Name.ToLower());
                foreach (JProperty parsedProperty in clss.Value)
                {
                    string propertyName = parsedProperty.Name;

                    //string propertyValue = (string)parsedProperty.Value;

                    props.Add(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propertyName.ToLower()));
                }

            }


            sft.Session = new Dictionary<string, object>();
            sft.Session.Add("_namespace", "CSharpGenerator.OutputClasses");
            sft.Session.Add("className", className);            
            sft.Session.Add("properties", props);
            sft.Initialize();
            string output = sft.TransformText();
            System.IO.File.WriteAllText( $"{AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "\\OutputClasses")}\\{className}.cs", output);
            Console.WriteLine(output);
            Console.ReadKey();
        }
    }
}
