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
        #region deklaracje

        Brain brain;
        List<CreationSequence> sequences;

        Animation animation;
        Creation creation;
        Charting charting;

        Display display;
        Presentation presentation;
        StateBar stateBar;

        int frames;
        int length;
        int pace;

        Mode mode;
        FormWindowState state;
        Drawable visible;

        #endregion

        public Simulation()
        {
            InitializeComponent();
            Constant.load();
            initialize();
            prepareAnimation();
        }

        #region funkcje inicjalizacji dla poszczególnych trybów

        void animate()
        {
            presentation = animation;
            visible = animation;
            animation.show();
            mode = Mode.Query;
        }

        void create()
        {
            presentation = creation;
            visible = creation;
            creation.show();
            mode = Mode.Creation;
        }

        void chart()
        {
            presentation = creation;
            visible = charting;
            charting.show();
            mode = Mode.Chart;
        }

        #endregion

        #region inicjalizacja

        void initialize()
        {
            brain = new Brain();
            sequences = new List<CreationSequence>();

            charting = new Charting();
            display = new Display();

            animation = new Animation(display);
            creation = new Creation(display);
            stateBar = new StateBar();

            Controls.Add(display);
            Controls.Add(charting);

            Padding margin = new Padding(10, 50, 10, 10);

            animation.setBar(stateBar);
            animation.setMargin(margin);
            creation.setMargin(margin);

            margin = new Padding(0, 0, rightPanel.Width + 20, 100);
            charting.setMargin(margin);
            display.setMargin(margin);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);

            trackBarDensity.KeyDown += keySuppress;
            trackBarFrame.KeyDown += keySuppress;
            trackBarLength.KeyDown += keySuppress;
            trackBarPace.KeyDown += keySuppress;
            trackBarScale.KeyDown += keySuppress;

            rightPanel.Controls.Add(stateBar);
            stateBar.show();

            length = trackBarLength.Value * 10;
            pace = trackBarPace.Value * 100;

            animation.changePace(pace);
            animation.animationStop += new EventHandler(animationStop);
            animation.balanceFinished += new EventHandler(balanceFinished);
            animation.queryAccepted += new EventHandler(queryAccepted);
            animation.frameChanged += new EventHandler(frameChanged);
            animation.framesChanged += new EventHandler(framesChanged);
            
            creation.animationStop += new EventHandler(animationStop);
            creation.creationFinished += new EventHandler(creationFinished);
            creation.frameChanged += new EventHandler(frameChanged);
            creation.framesChanged += new EventHandler(framesChanged);

            Presentation.factorChanged += new EventHandler(factorChanged);
            Presentation.sizeChanged += new EventHandler(sizeChanged);
            
            resize();
        }

        void simulate()
        {
            brain.simulate(length);
            animation.load(length);
        }

        void prepareAnimation()
        {
            buttonPlay.Enabled = true;
            buttonBack.Enabled = true;
            buttonForth.Enabled = true;
        }

        #endregion

        #region obsługa przycisków

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (presentation.started())
            {
                prepareAnimation();
                presentation.stop();
                return;
            }

            buttonSimulate.Enabled = true;

            buttonBack.Enabled = false;
            buttonForth.Enabled = false;
            buttonPlay.Text = "Stop";

            presentation.changePace(pace);
            presentation.start();
        }

        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            simulate();
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            if (!radioButtonQuery.Checked)
                return;

            presentation.stop();
            animation.newQuery();
        }

        #endregion

        #region obsługa przycisków sterujących

        private void buttonPaceUp_Click(object sender, EventArgs e)
        {
            if (pace < 2000)
            {
                pace += 100;
                presentation.changePace(pace);
            }

            labelPace.Text = pace.ToString();
            trackBarPace.Value = pace / 100;
        }

        private void buttonPaceDown_Click(object sender, EventArgs e)
        {
            if (pace > 200)
            {
                pace -= 100;
                presentation.changePace(pace);
            }

            labelPace.Text = pace.ToString();
            trackBarPace.Value = pace / 100;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                brain.undo();

            presentation.back();
        }

        private void buttonForth_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Manual)
                brain.tick();

            presentation.forth();
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

        #endregion

        #region obsługa checkBox

        private void checkBoxLabel_CheckedChanged(object sender, EventArgs e)
        {
            animation.labelChanged(checkBoxLabel.Checked);
        }

        private void checkBoxState_CheckedChanged(object sender, EventArgs e)
        {
            animation.stateChanged(checkBoxState.Checked);
        }

        #endregion

        #region obsługa klawiatury

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:
                    visible.save();
                    break;
                case Keys.F4:

                    break;
                case Keys.F11:
                    animation.stopBalance();
                    break;
                case Keys.Left:
                    presentation.back();
                    break;
                case Keys.Right:
                    presentation.forth();
                    break;
                case Keys.Add:
                    //if (e.Modifiers == Keys.Control)

                    break;
                case Keys.Subtract:
                    break;

                case Keys.Space:
                    presentation.space();
                    break;
                case Keys.Back:
                    presentation.erase();
                    break;
                case Keys.Enter:
                    presentation.enter();
                    break;
                default:
                    int code = msg.WParam.ToInt32();

                    if (code > 64 && code < 91)
                    {
                        if((keyData & Keys.Shift) != Keys.Shift)
                            code += 32;

                        presentation.add((char)code);
                    }
                    break;
            }
            return false;
        }

        void keySuppress(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        #endregion

        #region obsługa zmian rozmiaru

        private void resize(object sender, EventArgs e)
        {
            resize();

            if (WindowState != state)
            {
                resizeEnd(this, null);
                state = WindowState;
            }
        }

        private void resize()
        {
            int px = Width - rightPanel.Width - 20;
            int py = Height - 100;

            rightPanel.Left = px;
            rightPanel.Height = py + 40;

            labelFrame.Top = py;

            scrollHorizontal.Top = py - 12;
            scrollHorizontal.Width = px - 32;

            scrollVertical.Left = px - 20;
            scrollVertical.Height = py - 48;

            bottomPanel.Top = py + 4;
            bottomPanel.Width = px - 10;

            trackBarFrame.Width = px - 100;
            buttonForth.Left = px - 50;
        }

        private void resizeEnd(object sender, EventArgs e)
        {
            charting.resize();
            display.resize();
        }
        #endregion

        #region obsługa zmiany trybu pracy

        private void radioButtonCreation_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonCreation.Checked)
                return;

            mode = Mode.Creation;
            change(creation);
            presentation = creation;

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
            presentation = animation;

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
            presentation = animation;

            mode = Mode.Query;
            animation.setMode(Mode.Query);

            buttonPlay.Enabled = true;
            buttonQuery.Enabled = true;
        }

        private void change(Drawable area)
        {
            if (visible.Equals(area))
                return;

            presentation.stop();
            visible.hide();
            area.show();
            visible = area;
        }
        #endregion

        #region obsługa zdarzeń

        private void Simulation_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            String name = date.ToString("yyyyMMdd-HHmmss");

            StreamReader reader = new StreamReader(File.Open("Files\\data.xml", FileMode.Open));
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;
            Dictionary<Neuron, SequenceNeuron> map = new Dictionary<Neuron, SequenceNeuron>();

            foreach (XmlNode xn in node.ChildNodes)
                sequences.Add(brain.addSentence(xn.InnerText.ToLower()));

            creation.load(sequences);
            animation.loadBrain(brain);
            animation.create(creation);
            charting.loadBrain(brain);

            display.show();
            create();
            //animate();
        }

        private void animationStop(object sender, EventArgs e)
        {
            buttonPlay.Text = "Play";
            buttonSimulate.Enabled = true;
        }

        private void balanceFinished(object sender, EventArgs e)
        {
            if (mode != Mode.Manual)
                buttonPlay.Enabled = true;
        }

        private void factorChanged(object sender, EventArgs e)
        {
            trackBarScale.Minimum = (int)sender;
            trackBarScale.Value = (int)sender;

            AnimatedElement.Factor = (float)(int)sender / 100;
            animation.rescale(trackBarScale.Value);
        }

        private void frameChanged(object sender, EventArgs e)
        {
            labelFrame.Text = ((int)sender).ToString() + "/" + frames.ToString();
            trackBarFrame.Value = (int)sender;
        }

        private void framesChanged(object sender, EventArgs e)
        {
            frames = (int)sender;
            trackBarFrame.Maximum = frames;
        }

        private void sizeChanged(object sender, EventArgs e)
        {
            Presentation presentation = (Presentation)sender;
            float factor = AnimatedElement.Factor;

            scrollHorizontal.LargeChange = presentation.Width / 20 + 1;
            scrollHorizontal.Maximum = (int)(factor * presentation.Size.Width / 20);

            scrollVertical.LargeChange = presentation.Height / 20 + 1;
            scrollVertical.Maximum = (int)(factor * presentation.Size.Height / 20);
        }

        private void queryAccepted(object sender, EventArgs e)
        {
            simulate();
            charting.addQuery((QuerySequence)sender);
        }

        private void creationFinished(object sender, EventArgs e)
        {
            buttonForth.Enabled = false;
        }

        #endregion

        #region obsługa suwaków

        private void trackBarPace_Scroll(object sender, EventArgs e)
        {
            pace = trackBarPace.Value * 100;
            presentation.changePace(pace);
            labelPace.Text = pace.ToString();
        }

        private void trackBarLength_Scroll(object sender, EventArgs e)
        {
            length = trackBarLength.Value * 10;
            labelLength.Text = length.ToString();
        }

        private void trackBarFrame_Scroll(object sender, EventArgs e)
        {
            presentation.changeFrame(trackBarFrame.Value);
            labelFrame.Text = trackBarFrame.Value.ToString() + "/" + frames.ToString();
        }

        private void trackBarScale_Scroll(object sender, EventArgs e)
        {
            float factor = (float)trackBarScale.Value / 100;

            animation.rescale(trackBarScale.Value);
            AnimatedElement.Factor = factor;

            scrollHorizontal.LargeChange = presentation.Width / 20 + 1;
            scrollHorizontal.Maximum = (int)(factor * presentation.Size.Width / 20);

            scrollVertical.LargeChange = presentation.Height / 20 + 1;
            scrollVertical.Maximum = (int)(factor * presentation.Size.Height / 20);
        }

        private void trackBarDensity_Scroll(object sender, EventArgs e)
        {
            presentation.changeDensity(trackBarDensity.Value);
        }

        private void scrollHorizontal_ValueChanged(object sender, EventArgs e)
        {
            animation.scroll(scrollHorizontal.Value, scrollVertical.Value);
        }
       
        private void scrollVertical_ValueChanged(object sender, EventArgs e)
        {
            animation.scroll(scrollHorizontal.Value, scrollVertical.Value);
        }

        #endregion
    }
}