using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace Brain
{
    class Brain
    {
        List<Neuron> neurons;  
        List<Synapse> synapses;
        List<Receptor> receptors;

        float alpha;
        float beta;

        public Brain()
        {
            neurons = new List<Neuron>();
            synapses = new List<Synapse>();
            receptors = new List<Receptor>();

            alpha = Config.Alpha;
            beta = Config.Beta;
        }

        public void addReceptors(List<ReceptorData> receptors)
        {
            if (receptors.Count == 0)
                return;

            foreach(ReceptorData rd in receptors)
            {
                Neuron n = neurons.Find(k => k.Word == rd.Word);
                Receptor r = new Receptor(rd);
                Synapse s = new Synapse(r, n);

                this.receptors.Add(r);
                synapses.Add(s);
                n.Sensin.Add(r);
                r.Output = s;
                s.Weight = (float)rd.Value;
            }
        }

        public void simulate(int length)
        {
            for (int i = 0; i < length; i++)
            {
                foreach (Receptor r in receptors)
                    r.tick();

                foreach (Synapse s in synapses)
                    s.tick();

                foreach (Neuron n in neurons)
                    n.tick();
            }
        }

        public void tick()
        {
            foreach (Receptor r in receptors)
                r.tick(false);

            foreach (Neuron n in neurons)
                n.tick();

            foreach (Synapse s in synapses)
                s.tick();
        }

        public void undo()
        {
            foreach (Receptor r in receptors)
                r.undo();

            foreach (Neuron n in neurons)
                n.undo();

            foreach (Synapse s in synapses)
                s.undo();
        }

        public void clear()
        {
            foreach (Neuron n in neurons)
                n.clear();

            foreach (Synapse s in synapses)
                s.clear();

            foreach (Receptor r in receptors)
                r.clear();
        }

        public void addSentence(String sentence)
        {
            addSentence(sentence.Split(' '));
        }

        public void addSentence(String[] words)
        {
            List<Neuron> fragment = new List<Neuron>();

            foreach (String word in words)
            {
                Neuron neuron = neurons.Find(i => i.Word == word);

                if (neuron == null)
                {
                    neuron = new Neuron(word, alpha, beta);
                    neurons.Add(neuron);
                }

                neuron.Count++;
                fragment.Add(neuron);
            }

            for (int i = 0; i < fragment.Count; i++)
            {
                for (int j = i + 1; j < fragment.Count; j++)
                {
                    Synapse synapse = fragment[i].Output.Find(k => k.Post.Equals(fragment[j]));

                    if (synapse == null)
                    {
                        synapse = new Synapse(fragment[i], fragment[j]);
                        synapses.Add(synapse);
                        fragment[i].Output.Add(synapse);
                        fragment[j].Input.Add(synapse);
                    }

                    synapse.Factor += 1 / (float)(j - i);
                }

                foreach (Synapse syn in fragment[i].Output)
                    syn.Weight = (2 * syn.Factor) / (((Neuron)syn.Pre).Count + syn.Factor);
            }
        }

        static void chart(Neuron n)
        {
            Application xlApp;
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object mV = System.Reflection.Missing.Value;

            xlApp = new Application();
            xlWorkBook = xlApp.Workbooks.Add(mV);

            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = n.Word;

            for (int i = 0; i < n.Activity.Count; i++)
                xlWorkSheet.Cells[i + 1, 1] = n.Activity[i];

            Range chartRange;

            ChartObjects xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            ChartObject myChart = (ChartObject)xlCharts.Add(200, 120, 1200, 400);
            Chart chartPage = myChart.Chart;

            chartRange = xlWorkSheet.get_Range("A1", "A" + n.Activity.Count);
            chartPage.SetSourceData(chartRange, mV);
            chartPage.ChartType = XlChartType.xlLine;

            //xlWorkBook.SaveAs("G:\\C#\\" + n.Word + ".xls", XlFileFormat.xlWorkbookNormal, mV, mV, mV, mV, XlSaveAsAccessMode.xlExclusive, mV, mV, mV, mV, mV);
            xlApp.Visible = true;
            xlWorkBook.Close(true, mV, mV);
            xlApp.Quit();
        }

        static void chart(List<Neuron> neurons, String name)
        {
            Application xlApp;
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            object mV = System.Reflection.Missing.Value;

            xlApp = new Application();
            xlWorkBook = xlApp.Workbooks.Add(mV);

            for (int i = 0; i < neurons.Count - 3; i++)
                xlWorkBook.Worksheets.Add();

            for (int k = 0; k < neurons.Count; )
            {
                Neuron n = neurons[k];
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(++k);
                xlWorkSheet.Name = n.Word;

                for (int i = 0; i < n.Activity.Count; i++)
                    xlWorkSheet.Cells[i + 1, 1] = n.Activity[i];

                Range chartRange;

                ChartObjects xlCharts = (ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
                ChartObject myChart = (ChartObject)xlCharts.Add(80, 20, 20 * n.Activity.Count, 400);
                Chart chartPage = myChart.Chart;

                chartRange = xlWorkSheet.get_Range("A1", "A" + n.Activity.Count);
                chartPage.SetSourceData(chartRange, mV);
                chartPage.ChartType = XlChartType.xlLine;
            }
            /*
            String file = name + ".xls";
            xlWorkBook.SaveAs(file, XlFileFormat.xlWorkbookNormal, mV, mV, mV, mV, XlSaveAsAccessMode.xlExclusive, XlListConflict.xlListConflictDiscardAllConflicts, mV, mV, mV, mV);
            xlWorkBook.Close(true, mV, mV);
            xlApp.Quit();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "EXCEL.EXE";
            startInfo.Arguments = file;
            Process.Start(startInfo);

            xlApp.Visible = true;*/
        }

        public Neuron getNeuron(String word)
        {
            return neurons.Find(k => k.Word == word);
        }

        public void load(String path)
        {
            StreamReader reader = new StreamReader(path);
            reader.ReadLine();
        }

        public void save(String path)
        {
            int id = 0;
            
            FileStream file = new FileStream(path, FileMode.Create);
            file.Close();
            StreamWriter writer = new StreamWriter(path);
         
            writer.WriteLine(alpha);
            writer.WriteLine(beta);
            writer.WriteLine(neurons.Count);

            foreach (Neuron n in neurons)
            {
                String line = "" + n.Word + ';' + n.Treshold + ';' + n.Value;
                writer.WriteLine(line);
                n.ID = id++;
            }
                

            writer.WriteLine(synapses.Count);

            foreach (Synapse s in synapses)
            {
                String line = "" + s.Factor + ';' + ';' + s.Weight;
                writer.WriteLine(line);
            }
        }
    }
}