using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brain
{
    class Config
    {
        static String path;

        static float alpha;
        static float beta;

        static float diameter;
        static float halo;
        static float radius;

        public static void load()
        {
            StreamReader reader;

            try
            {
                reader = new StreamReader(File.Open("config.xml", FileMode.Open));
                XmlDocument xml = new XmlDocument();
                xml.Load(reader);
                reader.Close();
                XmlNode node;

                node = xml.FirstChild.NextSibling.FirstChild;
                path = node.InnerText;

                node = node.NextSibling;
                radius = Int32.Parse(node.InnerText);

                node = node.NextSibling;
                alpha = Single.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture);

                node = node.NextSibling;
                beta = Single.Parse(node.InnerText, System.Globalization.CultureInfo.InvariantCulture);

                halo = radius + 2;
                diameter = radius * 2;
            }
            catch(Exception)
            {
                loadDefault();
            }
        }

        public static void save()
        {
            StreamReader reader = new StreamReader(File.Open("config.xml", FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            StreamWriter writer = new StreamWriter(File.Open("config.xml", FileMode.Open));
            xml.FirstChild.NextSibling.FirstChild.InnerText = path;
            xml.Save(writer);
            writer.Close();
        }

        public static void changePath(String path)
        {
            Path = path;
            Config.save();

            if (File.Exists(System.IO.Path.Combine(path, "data.xml")))
                return;

            Directory.CreateDirectory(path);
            File.Create(System.IO.Path.Combine(path, "data.xml"));
            Directory.CreateDirectory(System.IO.Path.Combine(path, "Save"));
            Directory.CreateDirectory(System.IO.Path.Combine(path, "Simulation"));
        }

        static void loadDefault()
        {
            changePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Data"));
            radius = 24;
            diameter = 48;

            alpha = 0.9f;
            beta = 0.7f;
        }

        static String addPostfix(String postfix)
        {
            return System.IO.Path.Combine(path, postfix);
        }

        public static String SaveFolder()
        {
            return addPostfix("Save");
        }

        public static String SimulationFolder()
        {
            return addPostfix("Simulation");
        }

        public static String Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public static float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;
            }
        }

        public static float Beta
        {
            get
            {
                return beta;
            }
            set
            {
                beta = value;
            }
        }

        public static float Diameter
        {
            get
            {
                return diameter;
            }
            set
            {
                diameter = value;
            }
        }

        public static float Halo
        {
            get
            {
                return halo;
            }
            set
            {
                halo = value;
            }
        }

        public static float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
    }
}
