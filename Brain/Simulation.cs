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
        List<CreationSequence> sequences;

        Animation animation;
        Creation creation;
        Charting charting;
        SequenceBar sequenceBar;

        int length;
        int pace;

        Mode mode;
        FormWindowState state;
        Controller controller;
        Drawable visible;

        public Simulation()
        {
            InitializeComponent();
            Constant.load();
            initialize();
            prepareAnimation();

            //animate();
            create();
            //chart();
        }

        void animate()
        {
            controller = animation;
            visible = animation;
            animation.show();
            mode = Mode.Query;
        }

        void create()
        {
            controller = creation;
            visible = creation;
            creation.show();
            mode = Mode.Creation;
        }

        void chart()
        {
            controller = creation;
            visible = charting;
            charting.show();
            mode = Mode.Chart;
        }

        void initialize()
        {
            brain = new Brain();
            sequences = new List<CreationSequence>();

            sequenceBar = new SequenceBar();
            MainLayer.SequenceBar = sequenceBar;
            Controls.Add(sequenceBar);
            sequenceBar.show();

            animation = new Animation();
            creation = new Creation(sequences);
            charting = new Charting();

            Controls.Add(animation);
            Controls.Add(charting);
            Controls.Add(creation);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

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
            Dictionary<Neuron, SequenceNeuron> map = new Dictionary<Neuron, SequenceNeuron>();

            foreach (XmlNode xn in node.ChildNodes)
                sequences.Add(brain.addSentence(xn.InnerText));

            animation.loadBrain(brain);
            animation.create(creation);
            charting.loadBrain(brain);
        }

        void simulate()
        {
            brain.simulate(length);
            animation.load(length);
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
            trackBarPace.Value = pace / 100;
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (pace > 200)
            {
                pace -= 100;
                controller.changePace(pace);
            }

            labelPace.Text = pace.ToString();
            trackBarPace.Value = pace / 100;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            brain.clear(false);
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
            trackBarLength.Value = length / 10;
        }

        private void buttonLengthUp_Click(object sender, EventArgs e)
        {
            if (length < 500)
                length += 10;

            labelLength.Text = length.ToString();
            trackBarLength.Value = length / 10;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            openFile.FileName = "";
            openFile.Filter = "Save Files (*.abs)|*.abs";
            openFile.InitialDirectory = Constant.SaveFolder();
            openFile.ShowDialog();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            String name = "";
            FileInfo[] info = new DirectoryInfo(Constant.SaveFolder()).GetFiles();

            if (info.Length == 0)
                name = "save001";
            else
            {
                String last = info.Last<FileInfo>().Name;
                int number = Int32.Parse(last.Substring(4, 3));
                name = "save" + (number + 1).ToString("D3");
            }

            saveFile.Filter = "Save Files (*.abs)|*.abs";
            saveFile.InitialDirectory = Constant.SaveFolder();
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
            buttonBalance.Enabled = false;
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            if(mode != Mode.Manual)
                buttonPlay.Enabled = true;

            buttonBalance.Enabled = true;
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
            changeFolderDialog.SelectedPath = Constant.Path;
            DialogResult result = changeFolderDialog.ShowDialog();

            if (result == DialogResult.OK)
                Constant.changePath(changeFolderDialog.SelectedPath);
        }

        private void loadSimulation()
        {
            openFile.FileName = "";
            openFile.Filter = "Save Files (*.abs)|*.abs";
            openFile.InitialDirectory = Constant.SimulationFolder();
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
                    visible.save();
                    break;
                case Keys.F12:
                    changeFolder();
                    break;
                case Keys.F8:
                    loadSimulation();
                    break;
                case Keys.F4:

                    break;
                case Keys.F11:
                    animation.stopBalance();
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
                resizeEnd(this, null);
                state = WindowState;
            }
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            animation.resize();
            sequenceBar.resize();
            creation.resize();
            charting.resize();
        }

        private void radioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonCreation.Checked)
                return;

            mode = Mode.Creation;
            change(creation);
            controller = creation;

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = false;
        }

        private void radioButtonChart_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonChart.Checked)
                return;

            mode = Mode.Chart;
            change(charting);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = false;
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonManual.Checked)
                return;

            change(animation);
            controller = animation;

            mode = Mode.Manual;
            animation.setMode(Mode.Manual);

            buttonPlay.Enabled = false;
            buttonQuery.Enabled = false;
            /*
            if (MessageBox.Show("Reset all data?", "Data Reset", MessageBoxButtons.YesNo) == DialogResult.No)
                return;*/

            animation.unload();
            brain.clear(true);
        }

        private void radioButtonQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            change(animation);
            controller = animation;

            mode = Mode.Query;
            animation.setMode(Mode.Query);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = true;
        }

        private void change(Drawable area)
        {
            if (visible.Equals(area))
                return;

            controller.stop();
            visible.hide();
            area.show();
            visible = area;
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            controller.stop();
            animation.newQuery();
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
            simulate();
            charting.addQuery((QuerySequence)sender);
        }

        public void creationFinished(object sender, EventArgs e)
        {
            buttonForth.Enabled = false;
        }

        private void checkBoxSequence_CheckedChanged(object sender, EventArgs e)
        {
            sequenceBar.Active = checkBoxSequence.Checked;
            animation.relocate();
            creation.relocate();
            charting.relocate();
        }

        private void trackBarPace_Scroll(object sender, EventArgs e)
        {
            pace = trackBarPace.Value * 100;
            controller.changePace(pace);
            labelPace.Text = pace.ToString();
        }

        private void trackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }
    }
}