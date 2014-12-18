using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brain
{
    class Simulation
    {
        Brain brain;
        List<ReceptorData> receptors;
        bool loaded = false;

        public Simulation()
        {
            brain = new Brain();
            load();
        }

        public void load()
        {
            DateTime date = DateTime.Now;
            String name = date.ToString("yyyyMMdd-HHmmss");

            StreamReader reader = new StreamReader(File.Open("data.xml", FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).ChildNodes.Item(1);
            receptors = new List<ReceptorData>();

            foreach (XmlNode xn in node.ChildNodes)
                receptors.Add(new ReceptorData(xn));

            node = node.NextSibling;

            foreach (XmlNode xn in node.ChildNodes)
                brain.addSentence(xn.InnerText);

            brain.addReceptors(receptors);
            loaded = true;
        }

        public bool start(int length)
        {
            if (!loaded)
                return false;

            brain.simulate(length);
            return true;
        }

        public void clear()
        {
            brain.clear();
        }

        public void tick()
        {
            brain.tick();
        }

        public void undo()
        {
            brain.undo();
        }

        public Neuron getNeuron(String word)
        {
            return brain.getNeuron(word);
        }
    }
}
