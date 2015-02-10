namespace Brain
{
    partial class Query
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
            this.button1 = new System.Windows.Forms.Button();
            this.textQuery = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelInterval = new System.Windows.Forms.Label();
            this.temporalNumber = new System.Windows.Forms.NumericUpDown();
            this.temporalQuery = new System.Windows.Forms.ComboBox();
            this.labelQuery = new System.Windows.Forms.Label();
            this.temporalWeight = new System.Windows.Forms.NumericUpDown();
            this.labelIntensivity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.temporalNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.temporalWeight)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // textQuery
            // 
            this.textQuery.Location = new System.Drawing.Point(137, 180);
            this.textQuery.Name = "textQuery";
            this.textQuery.Size = new System.Drawing.Size(216, 20);
            this.textQuery.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(78, 294);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(230, 294);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelInterval
            // 
            this.labelInterval.AutoSize = true;
            this.labelInterval.Location = new System.Drawing.Point(75, 234);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(45, 13);
            this.labelInterval.TabIndex = 4;
            this.labelInterval.Text = "Interval:";
            // 
            // temporalNumber
            // 
            this.temporalNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.temporalNumber.Location = new System.Drawing.Point(137, 230);
            this.temporalNumber.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.temporalNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.temporalNumber.Name = "temporalNumber";
            this.temporalNumber.Size = new System.Drawing.Size(37, 21);
            this.temporalNumber.TabIndex = 5;
            this.temporalNumber.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // temporalQuery
            // 
            this.temporalQuery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.temporalQuery.FormattingEnabled = true;
            this.temporalQuery.Location = new System.Drawing.Point(78, 116);
            this.temporalQuery.Name = "temporalQuery";
            this.temporalQuery.Size = new System.Drawing.Size(121, 21);
            this.temporalQuery.TabIndex = 6;
            this.temporalQuery.Visible = false;
            // 
            // labelQuery
            // 
            this.labelQuery.AutoSize = true;
            this.labelQuery.Location = new System.Drawing.Point(75, 183);
            this.labelQuery.Name = "labelQuery";
            this.labelQuery.Size = new System.Drawing.Size(38, 13);
            this.labelQuery.TabIndex = 7;
            this.labelQuery.Text = "Query:";
            // 
            // temporalWeight
            // 
            this.temporalWeight.DecimalPlaces = 2;
            this.temporalWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.temporalWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.temporalWeight.Location = new System.Drawing.Point(278, 230);
            this.temporalWeight.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.temporalWeight.Name = "temporalWeight";
            this.temporalWeight.Size = new System.Drawing.Size(46, 21);
            this.temporalWeight.TabIndex = 8;
            this.temporalWeight.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // labelIntensivity
            // 
            this.labelIntensivity.AutoSize = true;
            this.labelIntensivity.Location = new System.Drawing.Point(216, 234);
            this.labelIntensivity.Name = "labelIntensivity";
            this.labelIntensivity.Size = new System.Drawing.Size(57, 13);
            this.labelIntensivity.TabIndex = 9;
            this.labelIntensivity.Text = "Intensivity:";
            // 
            // Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.labelIntensivity);
            this.Controls.Add(this.temporalWeight);
            this.Controls.Add(this.labelQuery);
            this.Controls.Add(this.temporalQuery);
            this.Controls.Add(this.temporalNumber);
            this.Controls.Add(this.labelInterval);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textQuery);
            this.Controls.Add(this.button1);
            this.Name = "Query";
            this.Text = "Query";
            ((System.ComponentModel.ISupportInitialize)(this.temporalNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.temporalWeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textQuery;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.NumericUpDown temporalNumber;
        private System.Windows.Forms.ComboBox temporalQuery;
        private System.Windows.Forms.Label labelQuery;
        private System.Windows.Forms.NumericUpDown temporalWeight;
        private System.Windows.Forms.Label labelIntensivity;
    }
}