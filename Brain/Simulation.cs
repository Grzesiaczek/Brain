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
        Animation animation;
        Chart chart;
        Sequence sequence;

        int length = 250;
        int pace = 500;

        Mode mode;
        FormWindowState state;

        public Simulation()
        {
            InitializeComponent();
            Config.load();
            initialize();
            prepareAnimation();
        }

        void initialize()
        {
            Show();

            animation = new Animation(layerAnimation);
            chart = new Chart(layerChart);
            sequence = new Sequence(layerSequence);

            layerSequence.Visible = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            mode = Mode.Auto;

            animation.changePace(500);
            animation.setSequence(sequence);
            animation.balanceFinished += new EventHandler(balanceFinished);
            animation.animationStop += new EventHandler(animationStop);
            animation.neuronShifted += new EventHandler(neuronShifted);
            animation.dataCleared += new EventHandler(dataCleared);
            animation.frameChanged += new EventHandler<FrameEventArgs>(frameChanged);
            
            chart.setNeurons(animation.getNeurons());
        }

        void prepareAnimation()
        {
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

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            if (animation.started())
            {
                prepareAnimation();
                animation.stop();
                return;
            }

            buttonBalance.Enabled = false;
            buttonOpen.Enabled = false;

            buttonSimulate.Text = "Stop";
            animation.start();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            animation.simulate(length);
            buttonLoad.Enabled = false;
        }

        private void buttonPaceUp_Click(object sender, EventArgs e)
        {
            if (pace < 2000)
            {
                pace += 100;
                animation.changePace(pace);
                
            }

            labelPace.Text = pace.ToString();
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (pace > 200)
            {
                pace -= 100;
                animation.changePace(pace);
            }

            labelPace.Text = pace.ToString();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                animation.clear();

            labelFrame.Text = "1";
            animation.redraw();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                animation.undo();

            animation.back();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                animation.tick();

            animation.forth();
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
            balance();
        }

        private void balance()
        {
            buttonSimulate.Enabled = false;
            buttonBalance.Enabled = false;

            MinimumSize = Size;
            MaximumSize = Size;

            animation.balance();
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            if(mode != Mode.Manual)
                buttonSimulate.Enabled = true;

            buttonBalance.Enabled = false;

            MinimumSize = new Size(1000, 800);
            MaximumSize = new Size(0, 0);
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
                    if(layerAnimation.Visible)
                    {
                        layerAnimation.Visible = false;
                        layerSequence.Visible = false;
                        layerChart.Visible = true;
                    }
                    else
                    {
                        layerChart.Visible = false;
                        layerAnimation.Visible = true;
                        layerSequence.Visible = false;
                    }
                    break;
            }
        }

        private void resize(object sender, EventArgs e)
        {
            layerMenu.Left = Width - 128;

            if (WindowState != state)
            {
                animation.resize();
                sequence.resize();
                state = WindowState;
            }
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            animation.resize();
            sequence.resize();
        }

        private void radioButtonAnimation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonAnimation.Checked)
                return;

            mode = Mode.Auto;
            animation.setMode(Mode.Auto);
            buttonSimulate.Enabled = true;
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonManual.Checked)
                return;

            mode = Mode.Manual;
            animation.setMode(Mode.Manual);
            buttonSimulate.Enabled = false;
            
            if (MessageBox.Show("Reset all data?", "Data Reset", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            animation.clear();
        }

        private void radioButtonQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            mode = animation.newQuery(mode);
        }

        private void animationStop(object sender, EventArgs e)
        {
            buttonLoad.Text = "Start";
            buttonBalance.Enabled = true;
            buttonOpen.Enabled = true;
        }

        private void frameChanged(object sender, FrameEventArgs e)
        {
            labelFrame.Text = e.Frame.ToString();
        }

        private void neuronShifted(object sender, EventArgs e)
        {
            buttonBalance.Enabled = true;
        }

        private void dataCleared(object sender, EventArgs e)
        {
            buttonLoad.Enabled = true;
        }
    }
}