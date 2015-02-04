namespace Brain
{
    partial class Simulation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonPaceDown = new System.Windows.Forms.Button();
            this.buttonPaceUp = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.labelPace = new System.Windows.Forms.Label();
            this.labelFrame = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForth = new System.Windows.Forms.Button();
            this.buttonBalance = new System.Windows.Forms.Button();
            this.checkBoxLabel = new System.Windows.Forms.CheckBox();
            this.checkBoxState = new System.Windows.Forms.CheckBox();
            this.changeFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.rightPanel = new System.Windows.Forms.GroupBox();
            this.radioButtonTree = new System.Windows.Forms.RadioButton();
            this.trackBarScale = new System.Windows.Forms.TrackBar();
            this.radioButtonCreation = new System.Windows.Forms.RadioButton();
            this.trackBarLength = new System.Windows.Forms.TrackBar();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.trackBarPace = new System.Windows.Forms.TrackBar();
            this.buttonLengthUp = new System.Windows.Forms.Button();
            this.radioButtonChart = new System.Windows.Forms.RadioButton();
            this.buttonLengthDown = new System.Windows.Forms.Button();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.labelLength = new System.Windows.Forms.Label();
            this.radioButtonQuery = new System.Windows.Forms.RadioButton();
            this.trackBarFrame = new System.Windows.Forms.TrackBar();
            this.bottomPanel = new System.Windows.Forms.GroupBox();
            this.scrollVertical = new System.Windows.Forms.VScrollBar();
            this.scrollHorizontal = new System.Windows.Forms.HScrollBar();
            this.rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(37, 484);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(100, 25);
            this.buttonPlay.TabIndex = 10;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonPaceDown
            // 
            this.buttonPaceDown.Location = new System.Drawing.Point(28, 405);
            this.buttonPaceDown.Name = "buttonPaceDown";
            this.buttonPaceDown.Size = new System.Drawing.Size(25, 25);
            this.buttonPaceDown.TabIndex = 2;
            this.buttonPaceDown.Text = "-";
            this.buttonPaceDown.UseVisualStyleBackColor = true;
            this.buttonPaceDown.Click += new System.EventHandler(this.buttonPaceDown_Click);
            // 
            // buttonPaceUp
            // 
            this.buttonPaceUp.Location = new System.Drawing.Point(119, 404);
            this.buttonPaceUp.Name = "buttonPaceUp";
            this.buttonPaceUp.Size = new System.Drawing.Size(25, 25);
            this.buttonPaceUp.TabIndex = 3;
            this.buttonPaceUp.Text = "+";
            this.buttonPaceUp.UseVisualStyleBackColor = true;
            this.buttonPaceUp.Click += new System.EventHandler(this.buttonPaceUp_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Location = new System.Drawing.Point(38, 672);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(100, 25);
            this.buttonSimulate.TabIndex = 5;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // labelPace
            // 
            this.labelPace.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPace.Location = new System.Drawing.Point(64, 405);
            this.labelPace.Name = "labelPace";
            this.labelPace.Size = new System.Drawing.Size(50, 25);
            this.labelPace.TabIndex = 6;
            this.labelPace.Text = "800";
            this.labelPace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFrame
            // 
            this.labelFrame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelFrame.Font = new System.Drawing.Font("Perpetua", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelFrame.Location = new System.Drawing.Point(729, 10);
            this.labelFrame.Name = "labelFrame";
            this.labelFrame.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelFrame.Size = new System.Drawing.Size(80, 40);
            this.labelFrame.TabIndex = 8;
            this.labelFrame.Text = "1";
            this.labelFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.Location = new System.Drawing.Point(6, 19);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(32, 24);
            this.buttonBack.TabIndex = 9;
            this.buttonBack.Text = "<<";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonForth
            // 
            this.buttonForth.Enabled = false;
            this.buttonForth.Location = new System.Drawing.Point(745, 19);
            this.buttonForth.Name = "buttonForth";
            this.buttonForth.Size = new System.Drawing.Size(32, 24);
            this.buttonForth.TabIndex = 10;
            this.buttonForth.Text = ">>";
            this.buttonForth.UseVisualStyleBackColor = true;
            this.buttonForth.Click += new System.EventHandler(this.buttonForth_Click);
            // 
            // buttonBalance
            // 
            this.buttonBalance.Location = new System.Drawing.Point(37, 531);
            this.buttonBalance.Name = "buttonBalance";
            this.buttonBalance.Size = new System.Drawing.Size(100, 25);
            this.buttonBalance.TabIndex = 16;
            this.buttonBalance.Text = "Balance";
            this.buttonBalance.UseVisualStyleBackColor = true;
            this.buttonBalance.Click += new System.EventHandler(this.buttonBalance_Click);
            // 
            // checkBoxLabel
            // 
            this.checkBoxLabel.AutoSize = true;
            this.checkBoxLabel.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxLabel.Checked = true;
            this.checkBoxLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLabel.Location = new System.Drawing.Point(68, 267);
            this.checkBoxLabel.Name = "checkBoxLabel";
            this.checkBoxLabel.Size = new System.Drawing.Size(52, 17);
            this.checkBoxLabel.TabIndex = 17;
            this.checkBoxLabel.Text = "Label";
            this.checkBoxLabel.UseVisualStyleBackColor = false;
            this.checkBoxLabel.CheckedChanged += new System.EventHandler(this.checkBoxLabel_CheckedChanged);
            // 
            // checkBoxState
            // 
            this.checkBoxState.AutoSize = true;
            this.checkBoxState.Checked = true;
            this.checkBoxState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxState.Location = new System.Drawing.Point(68, 310);
            this.checkBoxState.Name = "checkBoxState";
            this.checkBoxState.Size = new System.Drawing.Size(51, 17);
            this.checkBoxState.TabIndex = 18;
            this.checkBoxState.Text = "State";
            this.checkBoxState.UseVisualStyleBackColor = true;
            this.checkBoxState.CheckedChanged += new System.EventHandler(this.checkBoxState_CheckedChanged);
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.radioButtonTree);
            this.rightPanel.Controls.Add(this.trackBarScale);
            this.rightPanel.Controls.Add(this.radioButtonCreation);
            this.rightPanel.Controls.Add(this.trackBarLength);
            this.rightPanel.Controls.Add(this.buttonQuery);
            this.rightPanel.Controls.Add(this.trackBarPace);
            this.rightPanel.Controls.Add(this.buttonLengthUp);
            this.rightPanel.Controls.Add(this.radioButtonChart);
            this.rightPanel.Controls.Add(this.buttonLengthDown);
            this.rightPanel.Controls.Add(this.radioButtonManual);
            this.rightPanel.Controls.Add(this.labelLength);
            this.rightPanel.Controls.Add(this.radioButtonQuery);
            this.rightPanel.Controls.Add(this.buttonSimulate);
            this.rightPanel.Controls.Add(this.buttonPlay);
            this.rightPanel.Controls.Add(this.buttonBalance);
            this.rightPanel.Controls.Add(this.checkBoxLabel);
            this.rightPanel.Controls.Add(this.checkBoxState);
            this.rightPanel.Controls.Add(this.buttonPaceDown);
            this.rightPanel.Controls.Add(this.buttonPaceUp);
            this.rightPanel.Controls.Add(this.labelPace);
            this.rightPanel.Location = new System.Drawing.Point(812, 10);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(160, 739);
            this.rightPanel.TabIndex = 19;
            this.rightPanel.TabStop = false;
            // 
            // radioButtonTree
            // 
            this.radioButtonTree.AutoSize = true;
            this.radioButtonTree.Location = new System.Drawing.Point(74, 157);
            this.radioButtonTree.Name = "radioButtonTree";
            this.radioButtonTree.Size = new System.Drawing.Size(47, 17);
            this.radioButtonTree.TabIndex = 31;
            this.radioButtonTree.Text = "Tree";
            this.radioButtonTree.UseVisualStyleBackColor = true;
            // 
            // trackBarScale
            // 
            this.trackBarScale.Location = new System.Drawing.Point(18, 19);
            this.trackBarScale.Maximum = 16;
            this.trackBarScale.Minimum = 6;
            this.trackBarScale.Name = "trackBarScale";
            this.trackBarScale.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarScale.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBarScale.Size = new System.Drawing.Size(45, 133);
            this.trackBarScale.TabIndex = 30;
            this.trackBarScale.Value = 12;
            this.trackBarScale.Scroll += new System.EventHandler(this.trackBarScale_Scroll);
            // 
            // radioButtonCreation
            // 
            this.radioButtonCreation.AutoSize = true;
            this.radioButtonCreation.Checked = true;
            this.radioButtonCreation.Location = new System.Drawing.Point(74, 29);
            this.radioButtonCreation.Name = "radioButtonCreation";
            this.radioButtonCreation.Size = new System.Drawing.Size(64, 17);
            this.radioButtonCreation.TabIndex = 26;
            this.radioButtonCreation.TabStop = true;
            this.radioButtonCreation.Text = "Creation";
            this.radioButtonCreation.UseVisualStyleBackColor = true;
            this.radioButtonCreation.CheckedChanged += new System.EventHandler(this.radioButtonCreation_CheckedChanged);
            // 
            // trackBarLength
            // 
            this.trackBarLength.AutoSize = false;
            this.trackBarLength.Location = new System.Drawing.Point(19, 627);
            this.trackBarLength.Maximum = 50;
            this.trackBarLength.Minimum = 5;
            this.trackBarLength.Name = "trackBarLength";
            this.trackBarLength.Size = new System.Drawing.Size(127, 24);
            this.trackBarLength.SmallChange = 2;
            this.trackBarLength.TabIndex = 0;
            this.trackBarLength.TickFrequency = 10;
            this.trackBarLength.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLength.Value = 25;
            this.trackBarLength.Scroll += new System.EventHandler(this.trackBarLength_Scroll);
            // 
            // buttonQuery
            // 
            this.buttonQuery.Enabled = false;
            this.buttonQuery.Location = new System.Drawing.Point(38, 191);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(100, 25);
            this.buttonQuery.TabIndex = 25;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // trackBarPace
            // 
            this.trackBarPace.AutoSize = false;
            this.trackBarPace.Location = new System.Drawing.Point(19, 440);
            this.trackBarPace.Maximum = 20;
            this.trackBarPace.Minimum = 2;
            this.trackBarPace.Name = "trackBarPace";
            this.trackBarPace.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarPace.Size = new System.Drawing.Size(136, 24);
            this.trackBarPace.SmallChange = 2;
            this.trackBarPace.TabIndex = 0;
            this.trackBarPace.TickFrequency = 100;
            this.trackBarPace.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarPace.Value = 8;
            this.trackBarPace.Scroll += new System.EventHandler(this.trackBarPace_Scroll);
            // 
            // buttonLengthUp
            // 
            this.buttonLengthUp.Location = new System.Drawing.Point(112, 580);
            this.buttonLengthUp.Name = "buttonLengthUp";
            this.buttonLengthUp.Size = new System.Drawing.Size(25, 25);
            this.buttonLengthUp.TabIndex = 24;
            this.buttonLengthUp.Text = "+";
            this.buttonLengthUp.UseVisualStyleBackColor = true;
            this.buttonLengthUp.Click += new System.EventHandler(this.buttonLengthUp_Click);
            // 
            // radioButtonChart
            // 
            this.radioButtonChart.AutoSize = true;
            this.radioButtonChart.Location = new System.Drawing.Point(74, 61);
            this.radioButtonChart.Name = "radioButtonChart";
            this.radioButtonChart.Size = new System.Drawing.Size(50, 17);
            this.radioButtonChart.TabIndex = 19;
            this.radioButtonChart.Text = "Chart";
            this.radioButtonChart.UseVisualStyleBackColor = true;
            this.radioButtonChart.CheckedChanged += new System.EventHandler(this.radioButtonChart_CheckedChanged);
            // 
            // buttonLengthDown
            // 
            this.buttonLengthDown.Location = new System.Drawing.Point(28, 580);
            this.buttonLengthDown.Name = "buttonLengthDown";
            this.buttonLengthDown.Size = new System.Drawing.Size(25, 25);
            this.buttonLengthDown.TabIndex = 23;
            this.buttonLengthDown.Text = "-";
            this.buttonLengthDown.UseVisualStyleBackColor = true;
            this.buttonLengthDown.Click += new System.EventHandler(this.buttonLengthDown_Click);
            // 
            // radioButtonManual
            // 
            this.radioButtonManual.AutoSize = true;
            this.radioButtonManual.Location = new System.Drawing.Point(74, 93);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(60, 17);
            this.radioButtonManual.TabIndex = 20;
            this.radioButtonManual.Text = "Manual";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            this.radioButtonManual.CheckedChanged += new System.EventHandler(this.radioButtonManual_CheckedChanged);
            // 
            // labelLength
            // 
            this.labelLength.BackColor = System.Drawing.SystemColors.Control;
            this.labelLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLength.Location = new System.Drawing.Point(64, 581);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(50, 25);
            this.labelLength.TabIndex = 22;
            this.labelLength.Text = "250";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonQuery
            // 
            this.radioButtonQuery.AutoSize = true;
            this.radioButtonQuery.Location = new System.Drawing.Point(74, 125);
            this.radioButtonQuery.Name = "radioButtonQuery";
            this.radioButtonQuery.Size = new System.Drawing.Size(53, 17);
            this.radioButtonQuery.TabIndex = 21;
            this.radioButtonQuery.Text = "Query";
            this.radioButtonQuery.UseVisualStyleBackColor = true;
            this.radioButtonQuery.CheckedChanged += new System.EventHandler(this.radioButtonQuery_CheckedChanged);
            // 
            // trackBarFrame
            // 
            this.trackBarFrame.AutoSize = false;
            this.trackBarFrame.Location = new System.Drawing.Point(44, 19);
            this.trackBarFrame.Maximum = 250;
            this.trackBarFrame.Name = "trackBarFrame";
            this.trackBarFrame.Size = new System.Drawing.Size(695, 24);
            this.trackBarFrame.SmallChange = 2;
            this.trackBarFrame.TabIndex = 27;
            this.trackBarFrame.TickFrequency = 10;
            this.trackBarFrame.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarFrame.Scroll += new System.EventHandler(this.trackBarFrame_Scroll);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.buttonBack);
            this.bottomPanel.Controls.Add(this.trackBarFrame);
            this.bottomPanel.Controls.Add(this.buttonForth);
            this.bottomPanel.Location = new System.Drawing.Point(12, 699);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(789, 50);
            this.bottomPanel.TabIndex = 28;
            this.bottomPanel.TabStop = false;
            // 
            // scrollVertical
            // 
            this.scrollVertical.Location = new System.Drawing.Point(792, 50);
            this.scrollVertical.Name = "scrollVertical";
            this.scrollVertical.Size = new System.Drawing.Size(17, 653);
            this.scrollVertical.TabIndex = 29;
            this.scrollVertical.ValueChanged += new System.EventHandler(this.scrollVertical_ValueChanged);
            // 
            // scrollHorizontal
            // 
            this.scrollHorizontal.Location = new System.Drawing.Point(9, 679);
            this.scrollHorizontal.Name = "scrollHorizontal";
            this.scrollHorizontal.Size = new System.Drawing.Size(780, 17);
            this.scrollHorizontal.TabIndex = 30;
            this.scrollHorizontal.ValueChanged += new System.EventHandler(this.scrollHorizontal_ValueChanged);
            // 
            // Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.scrollHorizontal);
            this.Controls.Add(this.scrollVertical);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.labelFrame);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 800);
            this.Name = "Simulation";
            this.Text = "AAS Simulation Tool";
            this.ResizeEnd += new System.EventHandler(this.resizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.Resize += new System.EventHandler(this.resize);
            this.rightPanel.ResumeLayout(false);
            this.rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonPaceDown;
        private System.Windows.Forms.Button buttonPaceUp;
        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.Label labelPace;
        private System.Windows.Forms.Label labelFrame;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForth;
        private System.Windows.Forms.Button buttonBalance;
        private System.Windows.Forms.CheckBox checkBoxLabel;
        private System.Windows.Forms.CheckBox checkBoxState;
        private System.Windows.Forms.FolderBrowserDialog changeFolderDialog;
        private System.Windows.Forms.GroupBox rightPanel;
        private System.Windows.Forms.RadioButton radioButtonManual;
        private System.Windows.Forms.RadioButton radioButtonChart;
        private System.Windows.Forms.RadioButton radioButtonQuery;
        private System.Windows.Forms.Button buttonLengthUp;
        private System.Windows.Forms.Button buttonLengthDown;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.TrackBar trackBarPace;
        private System.Windows.Forms.TrackBar trackBarLength;
        private System.Windows.Forms.RadioButton radioButtonCreation;
        private System.Windows.Forms.TrackBar trackBarFrame;
        private System.Windows.Forms.GroupBox bottomPanel;
        private System.Windows.Forms.VScrollBar scrollVertical;
        private System.Windows.Forms.RadioButton radioButtonTree;
        private System.Windows.Forms.TrackBar trackBarScale;
        private System.Windows.Forms.HScrollBar scrollHorizontal;
    }
}

