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
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonPaceDown = new System.Windows.Forms.Button();
            this.buttonPaceUp = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.labelPace = new System.Windows.Forms.Label();
            this.labelFrame = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForth = new System.Windows.Forms.Button();
            this.labelLength = new System.Windows.Forms.Label();
            this.buttonLengthDown = new System.Windows.Forms.Button();
            this.buttonLengthUp = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.buttonBalance = new System.Windows.Forms.Button();
            this.checkBoxLabel = new System.Windows.Forms.CheckBox();
            this.checkBoxState = new System.Windows.Forms.CheckBox();
            this.changeFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.layerMenu = new System.Windows.Forms.GroupBox();
            this.radioButtonQuery = new System.Windows.Forms.RadioButton();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.radioButtonAnimation = new System.Windows.Forms.RadioButton();
            this.layerAnimation = new System.Windows.Forms.GroupBox();
            this.layerChart = new System.Windows.Forms.GroupBox();
            this.layerSequence = new System.Windows.Forms.GroupBox();
            this.layerMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Location = new System.Drawing.Point(13, 327);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(75, 23);
            this.buttonSimulate.TabIndex = 0;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(14, 97);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonPaceDown
            // 
            this.buttonPaceDown.Enabled = false;
            this.buttonPaceDown.Location = new System.Drawing.Point(13, 271);
            this.buttonPaceDown.Name = "buttonPaceDown";
            this.buttonPaceDown.Size = new System.Drawing.Size(32, 24);
            this.buttonPaceDown.TabIndex = 2;
            this.buttonPaceDown.Text = "-";
            this.buttonPaceDown.UseVisualStyleBackColor = true;
            this.buttonPaceDown.Click += new System.EventHandler(this.buttonPaceDown_Click);
            // 
            // buttonPaceUp
            // 
            this.buttonPaceUp.Enabled = false;
            this.buttonPaceUp.Location = new System.Drawing.Point(56, 271);
            this.buttonPaceUp.Name = "buttonPaceUp";
            this.buttonPaceUp.Size = new System.Drawing.Size(32, 24);
            this.buttonPaceUp.TabIndex = 3;
            this.buttonPaceUp.Text = "+";
            this.buttonPaceUp.UseVisualStyleBackColor = true;
            this.buttonPaceUp.Click += new System.EventHandler(this.buttonPaceUp_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Enabled = false;
            this.buttonLoad.Location = new System.Drawing.Point(13, 418);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // labelPace
            // 
            this.labelPace.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPace.Location = new System.Drawing.Point(13, 233);
            this.labelPace.Name = "labelPace";
            this.labelPace.Size = new System.Drawing.Size(75, 20);
            this.labelPace.TabIndex = 6;
            this.labelPace.Text = "500";
            this.labelPace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFrame
            // 
            this.labelFrame.Font = new System.Drawing.Font("Raavi", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelFrame.Location = new System.Drawing.Point(15, 9);
            this.labelFrame.Name = "labelFrame";
            this.labelFrame.Size = new System.Drawing.Size(74, 34);
            this.labelFrame.TabIndex = 8;
            this.labelFrame.Text = "1";
            this.labelFrame.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonBack
            // 
            this.buttonBack.Enabled = false;
            this.buttonBack.Location = new System.Drawing.Point(14, 53);
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
            this.buttonForth.Location = new System.Drawing.Point(58, 53);
            this.buttonForth.Name = "buttonForth";
            this.buttonForth.Size = new System.Drawing.Size(32, 24);
            this.buttonForth.TabIndex = 10;
            this.buttonForth.Text = ">>";
            this.buttonForth.UseVisualStyleBackColor = true;
            this.buttonForth.Click += new System.EventHandler(this.buttonForth_Click);
            // 
            // labelLength
            // 
            this.labelLength.BackColor = System.Drawing.SystemColors.Control;
            this.labelLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLength.Location = new System.Drawing.Point(13, 152);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(75, 20);
            this.labelLength.TabIndex = 11;
            this.labelLength.Text = "200";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonLengthDown
            // 
            this.buttonLengthDown.Location = new System.Drawing.Point(13, 184);
            this.buttonLengthDown.Name = "buttonLengthDown";
            this.buttonLengthDown.Size = new System.Drawing.Size(32, 24);
            this.buttonLengthDown.TabIndex = 12;
            this.buttonLengthDown.Text = "-";
            this.buttonLengthDown.UseVisualStyleBackColor = true;
            this.buttonLengthDown.Click += new System.EventHandler(this.buttonLengthDown_Click);
            // 
            // buttonLengthUp
            // 
            this.buttonLengthUp.Location = new System.Drawing.Point(57, 184);
            this.buttonLengthUp.Name = "buttonLengthUp";
            this.buttonLengthUp.Size = new System.Drawing.Size(32, 24);
            this.buttonLengthUp.TabIndex = 13;
            this.buttonLengthUp.Text = "+";
            this.buttonLengthUp.UseVisualStyleBackColor = true;
            this.buttonLengthUp.Click += new System.EventHandler(this.buttonLengthUp_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(14, 654);
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
            this.buttonSave.Location = new System.Drawing.Point(14, 683);
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
            this.buttonBalance.Location = new System.Drawing.Point(13, 372);
            this.buttonBalance.Name = "buttonBalance";
            this.buttonBalance.Size = new System.Drawing.Size(75, 23);
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
            this.checkBoxLabel.Enabled = false;
            this.checkBoxLabel.Location = new System.Drawing.Point(17, 578);
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
            this.checkBoxState.Enabled = false;
            this.checkBoxState.Location = new System.Drawing.Point(17, 613);
            this.checkBoxState.Name = "checkBoxState";
            this.checkBoxState.Size = new System.Drawing.Size(51, 17);
            this.checkBoxState.TabIndex = 18;
            this.checkBoxState.Text = "State";
            this.checkBoxState.UseVisualStyleBackColor = true;
            this.checkBoxState.CheckedChanged += new System.EventHandler(this.checkBoxState_CheckedChanged);
            // 
            // layerMenu
            // 
            this.layerMenu.Controls.Add(this.radioButtonQuery);
            this.layerMenu.Controls.Add(this.buttonLoad);
            this.layerMenu.Controls.Add(this.buttonReset);
            this.layerMenu.Controls.Add(this.buttonOpen);
            this.layerMenu.Controls.Add(this.buttonSimulate);
            this.layerMenu.Controls.Add(this.buttonBalance);
            this.layerMenu.Controls.Add(this.buttonSave);
            this.layerMenu.Controls.Add(this.radioButtonManual);
            this.layerMenu.Controls.Add(this.radioButtonAnimation);
            this.layerMenu.Controls.Add(this.buttonBack);
            this.layerMenu.Controls.Add(this.checkBoxLabel);
            this.layerMenu.Controls.Add(this.checkBoxState);
            this.layerMenu.Controls.Add(this.labelFrame);
            this.layerMenu.Controls.Add(this.buttonPaceDown);
            this.layerMenu.Controls.Add(this.buttonPaceUp);
            this.layerMenu.Controls.Add(this.buttonLengthUp);
            this.layerMenu.Controls.Add(this.labelPace);
            this.layerMenu.Controls.Add(this.buttonLengthDown);
            this.layerMenu.Controls.Add(this.buttonForth);
            this.layerMenu.Controls.Add(this.labelLength);
            this.layerMenu.Location = new System.Drawing.Point(872, 12);
            this.layerMenu.Name = "layerMenu";
            this.layerMenu.Size = new System.Drawing.Size(100, 714);
            this.layerMenu.TabIndex = 19;
            this.layerMenu.TabStop = false;
            // 
            // radioButtonQuery
            // 
            this.radioButtonQuery.AutoSize = true;
            this.radioButtonQuery.Location = new System.Drawing.Point(17, 534);
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
            this.radioButtonManual.Location = new System.Drawing.Point(17, 502);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(60, 17);
            this.radioButtonManual.TabIndex = 20;
            this.radioButtonManual.Text = "Manual";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            this.radioButtonManual.CheckedChanged += new System.EventHandler(this.radioButtonManual_CheckedChanged);
            // 
            // radioButtonAnimation
            // 
            this.radioButtonAnimation.AutoSize = true;
            this.radioButtonAnimation.Checked = true;
            this.radioButtonAnimation.Location = new System.Drawing.Point(17, 470);
            this.radioButtonAnimation.Name = "radioButtonAnimation";
            this.radioButtonAnimation.Size = new System.Drawing.Size(47, 17);
            this.radioButtonAnimation.TabIndex = 19;
            this.radioButtonAnimation.TabStop = true;
            this.radioButtonAnimation.Text = "Auto";
            this.radioButtonAnimation.UseVisualStyleBackColor = true;
            this.radioButtonAnimation.CheckedChanged += new System.EventHandler(this.radioButtonAnimation_CheckedChanged);
            // 
            // layerAnimation
            // 
            this.layerAnimation.Location = new System.Drawing.Point(40, 40);
            this.layerAnimation.Margin = new System.Windows.Forms.Padding(0);
            this.layerAnimation.Name = "layerAnimation";
            this.layerAnimation.Size = new System.Drawing.Size(200, 200);
            this.layerAnimation.TabIndex = 20;
            this.layerAnimation.TabStop = false;
            this.layerAnimation.Visible = false;
            // 
            // layerChart
            // 
            this.layerChart.BackColor = System.Drawing.SystemColors.Control;
            this.layerChart.Location = new System.Drawing.Point(280, 40);
            this.layerChart.Name = "layerChart";
            this.layerChart.Size = new System.Drawing.Size(200, 200);
            this.layerChart.TabIndex = 21;
            this.layerChart.TabStop = false;
            this.layerChart.Visible = false;
            // 
            // layerSequence
            // 
            this.layerSequence.BackColor = System.Drawing.SystemColors.Control;
            this.layerSequence.Location = new System.Drawing.Point(520, 40);
            this.layerSequence.Name = "layerSequence";
            this.layerSequence.Size = new System.Drawing.Size(200, 200);
            this.layerSequence.TabIndex = 22;
            this.layerSequence.TabStop = false;
            this.layerSequence.Visible = false;
            // 
            // Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.layerSequence);
            this.Controls.Add(this.layerChart);
            this.Controls.Add(this.layerAnimation);
            this.Controls.Add(this.layerMenu);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1000, 800);
            this.Name = "Simulation";
            this.Text = "Animation";
            this.ResizeEnd += new System.EventHandler(this.resizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.Resize += new System.EventHandler(this.resize);
            this.layerMenu.ResumeLayout(false);
            this.layerMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonPaceDown;
        private System.Windows.Forms.Button buttonPaceUp;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label labelPace;
        private System.Windows.Forms.Label labelFrame;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForth;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Button buttonLengthDown;
        private System.Windows.Forms.Button buttonLengthUp;
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
        private System.Windows.Forms.RadioButton radioButtonAnimation;
        private System.Windows.Forms.RadioButton radioButtonQuery;
        private System.Windows.Forms.GroupBox layerAnimation;
        private System.Windows.Forms.GroupBox layerChart;
        private System.Windows.Forms.GroupBox layerSequence;
    }
}

