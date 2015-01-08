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
    public partial class Simulation : Form
    {
        Brain brain;
        List<CreationFrame> frames;
        List<ReceptorData> receptors;

        int length;
        int pace;

        Mode mode;
        FormWindowState state;
        Controller controller;

        public Simulation()
        {
            InitializeComponent();
            Config.load();
            initialize();
            prepareAnimation();

            radioButtonCreation.Checked = true;
            creation.Visible = true;
        }

        void initialize()
        {
            brain = new Brain();
            frames = new List<CreationFrame>();

            animation = new Animation(this);
            creation = new Creation(this, frames);
            sequence = new Sequence(this);
            chart = new Chart(this);

            Controls.Add(animation);
            Controls.Add(creation);
            Controls.Add(chart);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            mode = Mode.Creation;
            controller = creation;

            length = trackBarLength.Value * 10;
            pace = trackBarPace.Value * 100;

            animation.changePace(pace);
            animation.balanceStarted += new EventHandler(balanceStarted);
            animation.balanceFinished += new EventHandler(balanceFinished);
            animation.animationStop += new EventHandler(animationStop);
            animation.queryAccepted += new EventHandler(queryAccepted);
            animation.frameChanged += new EventHandler<FrameEventArgs>(frameChanged);

            creation.animationStop += new EventHandler(animationStop);
            creation.creationFinished += new EventHandler(creationFinished);
            creation.frameChanged += new EventHandler<FrameEventArgs>(frameChanged);
            
            load();
            simulate();
            animation.setSequence(sequence);
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
                brain.addSentence(xn.InnerText, frames);

            brain.addReceptors(receptors);

            animation.loadNeurons(brain.Neurons);
            animation.create(creation, frames);
            chart.setNeurons(animation.getNeurons());
        }

        void simulate()
        {
            brain.simulate(length);
            animation.load(length);
        }

        void clear()
        {
            animation.unload();
            brain.clear();
        }

        void prepareAnimation()
        {
            buttonOpen.Enabled = true;
            buttonSave.Enabled = true;

            buttonPlay.Enabled = true;
            buttonBalance.Enabled = true;

            buttonBack.Enabled = true;
            buttonForth.Enabled = true;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (controller.started())
            {
                prepareAnimation();
                controller.stop();
                return;
            }

            buttonBalance.Enabled = false;
            buttonOpen.Enabled = false;
            buttonSimulate.Enabled = true;

            buttonBack.Enabled = false;
            buttonForth.Enabled = false;
            buttonPlay.Text = "Stop";

            controller.changePace(pace);
            controller.start();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            simulate();
        }

        private void buttonPaceUp_Click(object sender, EventArgs e)
        {
            if (pace < 2000)
            {
                pace += 100;
                controller.changePace(pace);
            }

            labelPace.Text = pace.ToString();
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (pace > 200)
            {
                pace -= 100;
                controller.changePace(pace);
            }

            labelPace.Text = pace.ToString();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            brain.clear();
            animation.unload();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                brain.undo();

            controller.back();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                brain.tick();

            controller.forth();
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
            /*neurons = new List<AnimatedNeuron>(count);

            for (int i = 0; i < count; i++)
                neurons.Add(new AnimatedNeuron(reader, buffer.Graphics));

            count = reader.ReadInt32();
            synapses = new List<AnimatedSynapse>(count);

            for (int i = 0; i < count; i++)
                synapses.Add(new AnimatedSynapse(reader, neurons, buffer.Graphics));*/

            reader.Close();
            prepareAnimation();
        }

        private void saveFile_FileOk(object sender, CancelEventArgs e)
        {
            String path = saveFile.FileName;
            BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate));
            /*writer.Write(neurons.Count);

            foreach (AnimatedNeuron n in neurons)
                n.save(writer);

            writer.Write(synapses.Count);

            foreach (AnimatedSynapse s in synapses)
                s.save(writer);*/
            
            writer.Close();
        }

        private void buttonBalance_Click(object sender, EventArgs e)
        {
            animation.balance();
        }

        private void balanceStarted(object sender, EventArgs e)
        {
            layerMenu.Enabled = false;
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            if(mode != Mode.Manual)
                buttonPlay.Enabled = true;

            buttonBalance.Enabled = false;
            layerMenu.Enabled = true;
        }

        private void checkBoxLabel_CheckedChanged(object sender, EventArgs e)
        {
            animation.labelChanged(checkBoxLabel.Checked);
        }

        private void checkBoxState_CheckedChanged(object sender, EventArgs e)
        {
            animation.stateChanged(checkBoxState.Checked);
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
                case Keys.F8:
                    loadSimulation();
                    break;
                case Keys.F4:
                    if(animation.Visible)
                    {
                        animation.Visible = false;
                        sequence.Visible = false;
                        chart.Visible = true;
                    }
                    else
                    {
                        chart.Visible = false;
                        animation.Visible = true;
                        sequence.Visible = false;
                    }
                    break;
                case Keys.F11:
                    animation.stopBalance();
                    layerMenu.Enabled = true;
                    break;
                case Keys.Left:

                    break;
                case Keys.Right:

                    break;

                case Keys.Add:

                    break;

                case Keys.Subtract:

                    break;
                case Keys.Space:

                    break;
            }
        }

        private void resize(object sender, EventArgs e)
        {
            layerMenu.Left = Width - 148;

            if (WindowState != state)
            {
                animation.resize();
                sequence.resize();
                creation.resize();
                state = WindowState;
            }
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            animation.resize();
            sequence.resize();
            creation.resize();
        }

        private void radioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonCreation.Checked)
            {
                animation.create();

                buttonBack.Enabled = true;
                buttonForth.Enabled = true;

                animation.Visible = true;
                sequence.Visible = true;
                creation.Visible = false;
                return;
            }

            mode = Mode.Creation;

            creation.Visible = true;
            animation.Visible = false;

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = false;
            buttonBack.Enabled = false;
        }

        private void radioButtonAnimation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonAuto.Checked)
                return;

            mode = Mode.Auto;
            animation.setMode(Mode.Auto);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = false;
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonManual.Checked)
                return;

            mode = Mode.Manual;
            animation.setMode(Mode.Manual);

            buttonPlay.Enabled = false;
            buttonQuery.Enabled = false;
            /*
            if (MessageBox.Show("Reset all data?", "Data Reset", MessageBoxButtons.YesNo) == DialogResult.No)
                return;*/

            clear();
        }

        private void radioButtonQuery_CheckedChanged(object sender, EventArgs e)
        {
            newQuery();

            if (mode == Mode.Query)
                buttonQuery.Enabled = true;
            else
            {
                buttonQuery.Enabled = false;

                if (mode == Mode.Auto)
                    radioButtonAuto.Checked = true;
                else
                    radioButtonManual.Checked = true;
            }
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            newQuery();
        }

        private void newQuery()
        {
            if (!radioButtonQuery.Checked)
                return;

            mode = animation.newQuery(mode);
        }

        private void animationStop(object sender, EventArgs e)
        {
            buttonPlay.Text = "Play";
            buttonBalance.Enabled = true;
            buttonOpen.Enabled = true;
            buttonSimulate.Enabled = true;
        }

        private void frameChanged(object sender, FrameEventArgs e)
        {
            labelFrame.Text = e.Frame.ToString();
        }

        public void queryAccepted(object sender, EventArgs e)
        {
            clear();
            simulate();
        }

        public void creationFinished(object sender, EventArgs e)
        {
            buttonForth.Enabled = false;
        }

        private void checkBoxSequence_CheckedChanged(object sender, EventArgs e)
        {
            sequence.Visible = checkBoxSequence.Checked;
            animation.relocate(checkBoxSequence.Checked);
        }

        private void trackBarPace_Scroll(object sender, EventArgs e)
        {
            pace = trackBarPace.Value * 100;
            animation.changePace(pace);
            labelPace.Text = pace.ToString();
        }

        private void trackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }
    }
}