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
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonPaceDown = new System.Windows.Forms.Button();
            this.buttonPaceUp = new System.Windows.Forms.Button();
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.labelPace = new System.Windows.Forms.Label();
            this.labelFrame = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForth = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.buttonBalance = new System.Windows.Forms.Button();
            this.checkBoxLabel = new System.Windows.Forms.CheckBox();
            this.checkBoxState = new System.Windows.Forms.CheckBox();
            this.changeFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.layerMenu = new System.Windows.Forms.GroupBox();
            this.radioButtonCreation = new System.Windows.Forms.RadioButton();
            this.trackBarLength = new System.Windows.Forms.TrackBar();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.trackBarPace = new System.Windows.Forms.TrackBar();
            this.checkBoxSequence = new System.Windows.Forms.CheckBox();
            this.buttonLengthUp = new System.Windows.Forms.Button();
            this.buttonLengthDown = new System.Windows.Forms.Button();
            this.labelLength = new System.Windows.Forms.Label();
            this.radioButtonQuery = new System.Windows.Forms.RadioButton();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.radioButtonAuto = new System.Windows.Forms.RadioButton();
            this.layerMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(20, 265);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(80, 23);
            this.buttonPlay.TabIndex = 10;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(20, 143);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(80, 23);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonPaceDown
            // 
            this.buttonPaceDown.Location = new System.Drawing.Point(16, 189);
            this.buttonPaceDown.Name = "buttonPaceDown";
            this.buttonPaceDown.Size = new System.Drawing.Size(20, 24);
            this.buttonPaceDown.TabIndex = 2;
            this.buttonPaceDown.Text = "-";
            this.buttonPaceDown.UseVisualStyleBackColor = true;
            this.buttonPaceDown.Click += new System.EventHandler(this.buttonPaceDown_Click);
            // 
            // buttonPaceUp
            // 
            this.buttonPaceUp.Location = new System.Drawing.Point(84, 189);
            this.buttonPaceUp.Name = "buttonPaceUp";
            this.buttonPaceUp.Size = new System.Drawing.Size(20, 24);
            this.buttonPaceUp.TabIndex = 3;
            this.buttonPaceUp.Text = "+";
            this.buttonPaceUp.UseVisualStyleBackColor = true;
            this.buttonPaceUp.Click += new System.EventHandler(this.buttonPaceUp_Click);
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Location = new System.Drawing.Point(22, 435);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(80, 23);
            this.buttonSimulate.TabIndex = 5;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // labelPace
            // 
            this.labelPace.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPace.Location = new System.Drawing.Point(36, 190);
            this.labelPace.Name = "labelPace";
            this.labelPace.Size = new System.Drawing.Size(48, 20);
            this.labelPace.TabIndex = 6;
            this.labelPace.Text = "800";
            this.labelPace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFrame
            // 
            this.labelFrame.Font = new System.Drawing.Font("Raavi", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelFrame.Location = new System.Drawing.Point(20, 55);
            this.labelFrame.Name = "labelFrame";
            this.labelFrame.Size = new System.Drawing.Size(80, 32);
            this.labelFrame.TabIndex = 8;
            this.labelFrame.Text = "1";
            this.labelFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.Location = new System.Drawing.Point(20, 100);
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
            this.buttonForth.Location = new System.Drawing.Point(68, 100);
            this.buttonForth.Name = "buttonForth";
            this.buttonForth.Size = new System.Drawing.Size(32, 24);
            this.buttonForth.TabIndex = 10;
            this.buttonForth.Text = ">>";
            this.buttonForth.UseVisualStyleBackColor = true;
            this.buttonForth.Click += new System.EventHandler(this.buttonForth_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(760, 726);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 14;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Visible = false;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(660, 726);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Visible = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // openFile
            // 
            this.openFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openFile_FileOk);
            // 
            // saveFile
            // 
            this.saveFile.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFile_FileOk);
            // 
            // buttonBalance
            // 
            this.buttonBalance.Enabled = false;
            this.buttonBalance.Location = new System.Drawing.Point(20, 315);
            this.buttonBalance.Name = "buttonBalance";
            this.buttonBalance.Size = new System.Drawing.Size(80, 23);
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
            this.checkBoxLabel.Location = new System.Drawing.Point(24, 671);
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
            this.checkBoxState.Location = new System.Drawing.Point(24, 703);
            this.checkBoxState.Name = "checkBoxState";
            this.checkBoxState.Size = new System.Drawing.Size(51, 17);
            this.checkBoxState.TabIndex = 18;
            this.checkBoxState.Text = "State";
            this.checkBoxState.UseVisualStyleBackColor = true;
            this.checkBoxState.CheckedChanged += new System.EventHandler(this.checkBoxState_CheckedChanged);
            // 
            // layerMenu
            // 
            this.layerMenu.Controls.Add(this.radioButtonCreation);
            this.layerMenu.Controls.Add(this.trackBarLength);
            this.layerMenu.Controls.Add(this.buttonQuery);
            this.layerMenu.Controls.Add(this.trackBarPace);
            this.layerMenu.Controls.Add(this.checkBoxSequence);
            this.layerMenu.Controls.Add(this.buttonLengthUp);
            this.layerMenu.Controls.Add(this.buttonLengthDown);
            this.layerMenu.Controls.Add(this.labelLength);
            this.layerMenu.Controls.Add(this.radioButtonQuery);
            this.layerMenu.Controls.Add(this.buttonSimulate);
            this.layerMenu.Controls.Add(this.buttonReset);
            this.layerMenu.Controls.Add(this.buttonPlay);
            this.layerMenu.Controls.Add(this.buttonBalance);
            this.layerMenu.Controls.Add(this.radioButtonManual);
            this.layerMenu.Controls.Add(this.radioButtonAuto);
            this.layerMenu.Controls.Add(this.buttonBack);
            this.layerMenu.Controls.Add(this.checkBoxLabel);
            this.layerMenu.Controls.Add(this.checkBoxState);
            this.layerMenu.Controls.Add(this.labelFrame);
            this.layerMenu.Controls.Add(this.buttonPaceDown);
            this.layerMenu.Controls.Add(this.buttonPaceUp);
            this.layerMenu.Controls.Add(this.labelPace);
            this.layerMenu.Controls.Add(this.buttonForth);
            this.layerMenu.Location = new System.Drawing.Point(860, 12);
            this.layerMenu.Name = "layerMenu";
            this.layerMenu.Size = new System.Drawing.Size(120, 737);
            this.layerMenu.TabIndex = 19;
            this.layerMenu.TabStop = false;
            // 
            // radioButtonCreation
            // 
            this.radioButtonCreation.AutoSize = true;
            this.radioButtonCreation.Checked = true;
            this.radioButtonCreation.Location = new System.Drawing.Point(26, 480);
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
            this.trackBarLength.Location = new System.Drawing.Point(10, 402);
            this.trackBarLength.Maximum = 50;
            this.trackBarLength.Minimum = 5;
            this.trackBarLength.Name = "trackBarLength";
            this.trackBarLength.Size = new System.Drawing.Size(100, 24);
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
            this.buttonQuery.Location = new System.Drawing.Point(18, 613);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(80, 23);
            this.buttonQuery.TabIndex = 25;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // trackBarPace
            // 
            this.trackBarPace.AutoSize = false;
            this.trackBarPace.Location = new System.Drawing.Point(10, 225);
            this.trackBarPace.Maximum = 20;
            this.trackBarPace.Minimum = 2;
            this.trackBarPace.Name = "trackBarPace";
            this.trackBarPace.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarPace.Size = new System.Drawing.Size(100, 24);
            this.trackBarPace.SmallChange = 2;
            this.trackBarPace.TabIndex = 0;
            this.trackBarPace.TickFrequency = 100;
            this.trackBarPace.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarPace.Value = 8;
            this.trackBarPace.Scroll += new System.EventHandler(this.trackBarPace_Scroll);
            // 
            // checkBoxSequence
            // 
            this.checkBoxSequence.AutoSize = true;
            this.checkBoxSequence.Checked = true;
            this.checkBoxSequence.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSequence.Location = new System.Drawing.Point(22, 22);
            this.checkBoxSequence.Name = "checkBoxSequence";
            this.checkBoxSequence.Size = new System.Drawing.Size(75, 17);
            this.checkBoxSequence.TabIndex = 25;
            this.checkBoxSequence.Text = "Sequence";
            this.checkBoxSequence.UseVisualStyleBackColor = true;
            this.checkBoxSequence.CheckedChanged += new System.EventHandler(this.checkBoxSequence_CheckedChanged);
            // 
            // buttonLengthUp
            // 
            this.buttonLengthUp.Location = new System.Drawing.Point(84, 365);
            this.buttonLengthUp.Name = "buttonLengthUp";
            this.buttonLengthUp.Size = new System.Drawing.Size(20, 24);
            this.buttonLengthUp.TabIndex = 24;
            this.buttonLengthUp.Text = "+";
            this.buttonLengthUp.UseVisualStyleBackColor = true;
            this.buttonLengthUp.Click += new System.EventHandler(this.buttonLengthUp_Click);
            // 
            // buttonLengthDown
            // 
            this.buttonLengthDown.Location = new System.Drawing.Point(16, 365);
            this.buttonLengthDown.Name = "buttonLengthDown";
            this.buttonLengthDown.Size = new System.Drawing.Size(20, 24);
            this.buttonLengthDown.TabIndex = 23;
            this.buttonLengthDown.Text = "-";
            this.buttonLengthDown.UseVisualStyleBackColor = true;
            this.buttonLengthDown.Click += new System.EventHandler(this.buttonLengthDown_Click);
            // 
            // labelLength
            // 
            this.labelLength.BackColor = System.Drawing.SystemColors.Control;
            this.labelLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLength.Location = new System.Drawing.Point(36, 366);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(48, 20);
            this.labelLength.TabIndex = 22;
            this.labelLength.Text = "250";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonQuery
            // 
            this.radioButtonQuery.AutoSize = true;
            this.radioButtonQuery.Location = new System.Drawing.Point(26, 576);
            this.radioButtonQuery.Name = "radioButtonQuery";
            this.radioButtonQuery.Size = new System.Drawing.Size(53, 17);
            this.radioButtonQuery.TabIndex = 21;
            this.radioButtonQuery.Text = "Query";
            this.radioButtonQuery.UseVisualStyleBackColor = true;
            this.radioButtonQuery.CheckedChanged += new System.EventHandler(this.radioButtonQuery_CheckedChanged);
            // 
            // radioButtonManual
            // 
            this.radioButtonManual.AutoSize = true;
            this.radioButtonManual.Location = new System.Drawing.Point(26, 544);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(60, 17);
            this.radioButtonManual.TabIndex = 20;
            this.radioButtonManual.Text = "Manual";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            this.radioButtonManual.CheckedChanged += new System.EventHandler(this.radioButtonManual_CheckedChanged);
            // 
            // radioButtonAuto
            // 
            this.radioButtonAuto.AutoSize = true;
            this.radioButtonAuto.Location = new System.Drawing.Point(26, 512);
            this.radioButtonAuto.Name = "radioButtonAuto";
            this.radioButtonAuto.Size = new System.Drawing.Size(47, 17);
            this.radioButtonAuto.TabIndex = 19;
            this.radioButtonAuto.Text = "Auto";
            this.radioButtonAuto.UseVisualStyleBackColor = true;
            this.radioButtonAuto.CheckedChanged += new System.EventHandler(this.radioButtonAnimation_CheckedChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.layerMenu);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonOpen);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 800);
            this.Name = "Simulation";
            this.Text = "AAS Simulation Tool";
            this.ResizeEnd += new System.EventHandler(this.resizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.Resize += new System.EventHandler(this.resize);
            this.layerMenu.ResumeLayout(false);
            this.layerMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPace)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonPaceDown;
        private System.Windows.Forms.Button buttonPaceUp;
        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.Label labelPace;
        private System.Windows.Forms.Label labelFrame;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForth;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.Button buttonBalance;
        private System.Windows.Forms.CheckBox checkBoxLabel;
        private System.Windows.Forms.CheckBox checkBoxState;
        private System.Windows.Forms.FolderBrowserDialog changeFolderDialog;
        private System.Windows.Forms.GroupBox layerMenu;
        private System.Windows.Forms.RadioButton radioButtonManual;
        private System.Windows.Forms.RadioButton radioButtonAuto;
        private System.Windows.Forms.RadioButton radioButtonQuery;
        private System.Windows.Forms.Button buttonLengthUp;
        private System.Windows.Forms.Button buttonLengthDown;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.CheckBox checkBoxSequence;
        private System.Windows.Forms.TrackBar trackBarPace;
        private System.Windows.Forms.TrackBar trackBarLength;
        private System.Windows.Forms.RadioButton radioButtonCreation;
        private Animation animation = null;
        private Creation creation = null;
        private Chart chart = null;
        private Sequence sequence = null;
    }
}

