using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Brain
{
    public partial class Animation : Form
    {
        enum Mode {Auto, Manual, Query}

        BufferedGraphics buffer;
        BufferedGraphicsContext context;

        System.Windows.Forms.Timer timer;

        Simulation simulation;
        ShiftedNeuron shift;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedReceptor> receptors;

        int length;
        int pace = 0;
        int frame = 0;

        int interval = 0;
        int arrival = 0;
        int count = 1;

        bool animation = false;
        bool loaded = false;
        Mode mode;

        public Animation()
        {
            InitializeComponent();
            Config.load();
            initiate();
            prepareAnimation();
        }

        void initiate()
        {
            simulation = new Simulation();
            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
            receptors = new List<AnimatedReceptor>();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            initiateGraphics();

            changePace(500);
            mode = Mode.Auto;
            Graphics g = buffer.Graphics;
            
            if (!simulation.start(250))
            {
                MessageBox.Show("Simulation not loaded", "Error");
                simulation.load();
            }

            loadNeurons(g);
            loaded = true;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
            timer.Start();
        }

        void initiateGraphics()
        {
            Graphics visible = CreateGraphics();
            visible.FillRectangle(SystemBrushes.Control, visible.VisibleClipBounds);

            int height = (int)visible.VisibleClipBounds.Height - 20;
            int width = (int)visible.VisibleClipBounds.Width - 128;

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(width + 1, height + 1);

            buffer = context.Allocate(CreateGraphics(), new Rectangle(10, 10, width, height));
            buffer.Graphics.FillRectangle(Brushes.White, new Rectangle(10, 10, width, height));   
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        PointF randomPoint(Random random)
        {
            float x = 40 + random.Next() % (buffer.Graphics.VisibleClipBounds.Width - 90);
            float y = 40 + random.Next() % (buffer.Graphics.VisibleClipBounds.Height - 90);
            return new PointF(x, y);
        }

        void loadNeurons(Graphics g)
        {
            StreamReader reader = new StreamReader(File.Open("data.xml", FileMode.Open));
            Random random = new Random(DateTime.Now.Millisecond);

            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;
            int counter = 0;
            
            try
            {
                length = Int32.Parse(xml.ChildNodes.Item(1).Attributes[0].Value);
                labelLength.Text = length.ToString();
            }
            catch(Exception)
            {
                length = 250;
            }

            foreach(XmlNode xn in node.ChildNodes)
            {
                PointF position = randomPoint(random);

                try
                {
                    if(xn.Attributes.Count != 0)
                    {
                        int x = Int32.Parse(xn.Attributes[0].Value);
                        int y = Int32.Parse(xn.Attributes[1].Value);

                        position = new PointF(x, y);
                    }
                }
                catch(Exception) { }

                Neuron n = simulation.getNeuron(xn.InnerText);
                AnimatedNeuron an = new AnimatedNeuron(n, g, position);
                neurons.Add(an);

                if (n.Sensin.Count > 0)
                {
                    Receptor r = n.Sensin[0];
                    AnimatedReceptor ar = new AnimatedReceptor(r, an, counter++ % 4);
                    synapses.Add(new AnimatedSynapse(ar, an, r.Output, g));
                    receptors.Add(ar);
                }
            }

            foreach (AnimatedNeuron start in neurons)
                foreach (Synapse s in start.getNeuron().Output)
                    foreach (AnimatedNeuron n in neurons)
                        if (n.getNeuron() == s.Post)
                        {
                            bool single = true;

                            foreach (AnimatedSynapse syn in synapses)
                                if (syn.Pre == n && syn.Post == start)
                                {
                                    syn.setDuplex(s);
                                    single = false;
                                    break;
                                }

                            if (single)
                                synapses.Add(new AnimatedSynapse(start, n, s, g));

                            break;
                        }
        }

        void stop()
        {
            timer.Stop();
            animation = false;
            buttonLoad.Text = "Start";

            buttonBalance.Enabled = true;
            buttonOpen.Enabled = true;
        }

        void tick(object sender, EventArgs e)
        {
            if(frame == length)
            {
                animation = false;
                return;
            }

            if (animation)
                animate();
            else
                redraw();
        }

        void redraw()
        {
            if (!loaded)
                return;

            buffer.Graphics.Clear(SystemColors.Control);

            foreach (AnimatedSynapse s in synapses)
                s.draw(frame);

            foreach (AnimatedSynapse s in synapses)
                s.drawState(frame, false);

            foreach (AnimatedNeuron an in neurons)
                an.draw(frame);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw(frame);

            buffer.Render(CreateGraphics());
        }

        void animate()
        {
            if (count++ == interval)
            {
                frame++;
                labelFrame.Text = frame.ToString();
                count = 1;
            }

            buffer.Graphics.Clear(SystemColors.Control);

            int number = count - interval / 2;

            if (number >= 0 && number <= interval / 4)
            {
                foreach (AnimatedSynapse s in synapses)
                    s.animate(frame, ((float)number * 4) / interval);
            }
            else foreach (AnimatedSynapse s in synapses)
                    s.draw(frame);

            foreach (AnimatedSynapse s in synapses)
                s.drawState(frame, true);

            foreach (AnimatedNeuron an in neurons)
                an.animate(frame, count, (double)count / interval);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw(frame);

            buffer.Render(CreateGraphics());
        }

        void prepareAnimation()
        {
            loaded = true;
            frame = 1;

            buttonOpen.Enabled = true;
            buttonSave.Enabled = true;
            buttonBack.Enabled = true;
            buttonForth.Enabled = true;
            buttonPaceUp.Enabled = true;
            buttonPaceDown.Enabled = true;
            buttonSimulate.Enabled = true;
            buttonBalance.Enabled = true;
            buttonLoad.Enabled = false;

            checkBoxLabel.Enabled = true;
            checkBoxState.Enabled = true;
        }

        void changePace(int diff)
        {
            pace += diff;
            interval = pace / 25;
            arrival = (interval * 3) / 4;
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            if (animation)
            {
                prepareAnimation();
                stop();
                return;
            }

            buttonBalance.Enabled = false;
            buttonOpen.Enabled = false;

            animation = true;
            buttonSimulate.Text = "Stop";
            timer.Start();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        private void buttonPaceUp_Click(object sender, EventArgs e)
        {
            if (pace < 2000)
                changePace(100);

            labelPace.Text = pace.ToString();
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (pace > 200)
                changePace(-100);

            labelPace.Text = pace.ToString();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                simulation.clear();

            frame = 1;
            labelFrame.Text = "1";
            redraw();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual && frame > 1)
                simulation.undo();

            if (frame > 1)
                frame--;

            labelFrame.Text = frame.ToString();
            redraw();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                simulation.tick();

            if (frame < length)
                frame++;

            labelFrame.Text = frame.ToString();
            redraw();
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (shift != null && mode == Mode.Manual)
                shift.activate();
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            foreach(AnimatedNeuron neuron in neurons)
            {
                if(neuron.click(e.Location))
                {
                    shift = new ShiftedNeuron(neuron, new PointF(e.X, e.Y), neurons);
                    neurons[neurons.IndexOf(neuron)] = shift;
                    
                    break;
                }
            }

            redraw();
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.move(e.X, e.Y);
            redraw();
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            buttonBalance.Enabled = true;

            shift.save(checkBoxLabel.Checked);
            shift = null;
            redraw();
        }

        private void buttonLengthDown_Click(object sender, EventArgs e)
        {
            if (length > 50)
                length -= 10;

            labelLength.Text = length.ToString();
        }

        private void buttonLengthUp_Click(object sender, EventArgs e)
        {
            if (length < 500)
                length += 10;

            labelLength.Text = length.ToString();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            openFile.FileName = "";
            openFile.Filter = "Save Files (*.abs)|*.abs";
            openFile.InitialDirectory = Config.SaveFolder();
            openFile.ShowDialog();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            String name = "";
            FileInfo[] info = new DirectoryInfo(Config.SaveFolder()).GetFiles();

            if (info.Length == 0)
                name = "save001";
            else
            {
                String last = info.Last<FileInfo>().Name;
                int number = Int32.Parse(last.Substring(4, 3));
                name = "save" + (number + 1).ToString("D3");
            }

            saveFile.Filter = "Save Files (*.abs)|*.abs";
            saveFile.InitialDirectory = Config.SaveFolder();
            saveFile.FileName = name;
            saveFile.ShowDialog();
        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
            String path = openFile.FileName;
            BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));

            int count = reader.ReadInt32();
            neurons = new List<AnimatedNeuron>(count);

            for (int i = 0; i < count; i++)
                neurons.Add(new AnimatedNeuron(reader, buffer.Graphics));

            count = reader.ReadInt32();
            synapses = new List<AnimatedSynapse>(count);

            for (int i = 0; i < count; i++)
                synapses.Add(new AnimatedSynapse(reader, neurons, buffer.Graphics));

            reader.Close();
            prepareAnimation();
        }

        private void saveFile_FileOk(object sender, CancelEventArgs e)
        {
            String path = saveFile.FileName;
            BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate));
            writer.Write(neurons.Count);

            foreach (AnimatedNeuron n in neurons)
                n.save(writer);

            writer.Write(synapses.Count);

            foreach (AnimatedSynapse s in synapses)
                s.save(writer);

            writer.Close();
        }

        private void buttonBalance_Click(object sender, EventArgs e)
        {
            balanceStart();
        }

        private void balanceStart()
        {
            buttonSimulate.Enabled = false;
            buttonBalance.Enabled = false;

            MinimumSize = Size;
            MaximumSize = Size;

            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = false;

            GraphDrawing drawing = new GraphDrawing(neurons, synapses, receptors, 120);
            drawing.balanceFinished += balanceFinished;
            drawing.drawing += (this.draw);
            drawing.animate(buffer.Graphics);
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            if(mode != Mode.Manual)
                buttonSimulate.Enabled = true;

            buttonBalance.Enabled = false;

            MinimumSize = new Size(800, 600);
            MaximumSize = new Size(0, 0);

            if (!checkBoxState.Checked)
                return;

            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = true;

            redraw();
        }

        private void draw(object sender, EventArgs e)
        {
            redraw();
        }

        private void checkBoxLabel_CheckedChanged(object sender, EventArgs e)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.Label = checkBoxLabel.Checked;

            redraw();
        }

        private void checkBoxState_CheckedChanged(object sender, EventArgs e)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = checkBoxState.Checked;

            redraw();
        }

        private void changeFolder()
        {
            changeFolderDialog.SelectedPath = Config.Path;
            DialogResult result = changeFolderDialog.ShowDialog();

            if (result == DialogResult.OK)
                Config.changePath(changeFolderDialog.SelectedPath);
        }

        private void loadSimulation()
        {
            openFile.FileName = "";
            openFile.Filter = "Save Files (*.abs)|*.abs";
            openFile.InitialDirectory = Config.SimulationFolder();
            DialogResult result = openFile.ShowDialog();

            if (result == DialogResult.OK)
            {

            }
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F1:
                    changeFolder();
                    break;
                case Keys.F5:
                    loadSimulation();
                    break;
                case Keys.F4:
                    redraw();
                    break;
            }
        }

        private void resize()
        {
            initiateGraphics();

            if(!loaded)
                return;

            foreach (AnimatedNeuron n in neurons)
                n.setGraphics(buffer.Graphics);

            foreach (AnimatedSynapse s in synapses)
                s.setGraphics(buffer.Graphics);

            foreach (AnimatedReceptor r in receptors)
                r.setGraphics(buffer.Graphics);

            balanceStart();
        }

        private void resize(object sender, EventArgs e)
        {
            groupBox.Left = Width - 128;

            if (WindowState == FormWindowState.Maximized)
                resize();
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            resize();
        }

        private void radioButtonAnimation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonAnimation.Checked)
                return;

            mode = Mode.Auto;
            buttonSimulate.Enabled = true;
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonManual.Checked)
                return;

            mode = Mode.Manual;
            buttonSimulate.Enabled = false;
            animation = false;
            
            if (MessageBox.Show("Reset all data?", "Data Reset", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            simulation.clear();
            labelFrame.Text = "1";
            frame = 1;
            redraw();
        }

        private void radioButtonQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            Mode old = mode;
            mode = Mode.Query;

            Query query = new Query();
            DialogResult result = query.ShowDialog();

            if(result == DialogResult.Cancel)
            {
                mode = old;
                return;
            }

            foreach (AnimatedReceptor r in receptors)
                r.setInterval(1000);

            AnimatedReceptor ar = receptors[query.Index];
            ar.setInterval(query.Interval);

            simulation.clear();
            simulation.start(length);
        }
    }
}

