using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML
{
    struct GenConfig
    {
        public string name;
        public string source; 
    }
    class Program
    {
        static void Main(string[] args)
        {

            LoadXML("");
            System.Console.ReadLine();

        }

        public static void LoadXML(string xmlPath)
        {
            List<string> refDllList = new List<string>();
            List<GenConfig> genConfigList = new List<GenConfig>();
            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList refList;
            XmlNodeList genDllList;
            try
            {
                xmldoc.Load("csc.xml");
                refList = xmldoc.SelectNodes("/root/refList/ref");
                genDllList = xmldoc.SelectNodes("/root/genDllList/genDll");
                foreach (XmlNode node in refList)
                {
                    string path = ((XmlElement)node).GetAttribute("path");
                    refDllList.Add(path);
                }

                foreach (XmlElement elem in genDllList)
                {
                    
                    GenConfig genConfig = new GenConfig();
                    string name = elem.GetAttribute("name");
                    string source = elem.GetAttribute("source");
                    genConfig.name = name;
                    genConfig.source = source;
                    genConfigList.Add(genConfig);
                }
                genConfigList.ToString();
                refDllList.ToString();
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e);
            }
        }
    }
}
