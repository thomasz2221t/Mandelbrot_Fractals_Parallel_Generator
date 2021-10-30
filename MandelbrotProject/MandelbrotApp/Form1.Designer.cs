
namespace MandelbrotApp
{
    partial class appWindow
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
            this.iterationLabel = new System.Windows.Forms.Label();
            this.iterationBox = new System.Windows.Forms.TextBox();
            this.iterationButton = new System.Windows.Forms.Button();
            this.threatsLabel = new System.Windows.Forms.Label();
            this.threatsUpDown = new System.Windows.Forms.NumericUpDown();
            this.threatsButton = new System.Windows.Forms.Button();
            this.asmCppLabel = new System.Windows.Forms.Label();
            this.asmBox = new System.Windows.Forms.CheckBox();
            this.cppBox = new System.Windows.Forms.CheckBox();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.resolutionXBox = new System.Windows.Forms.TextBox();
            this.xLabel = new System.Windows.Forms.Label();
            this.resolutionYBox = new System.Windows.Forms.TextBox();
            this.resolutionButton = new System.Windows.Forms.Button();
            this.outputLabel = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.proceedButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.threatsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // iterationLabel
            // 
            this.iterationLabel.AutoSize = true;
            this.iterationLabel.Location = new System.Drawing.Point(45, 43);
            this.iterationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.iterationLabel.Name = "iterationLabel";
            this.iterationLabel.Size = new System.Drawing.Size(338, 17);
            this.iterationLabel.TabIndex = 15;
            this.iterationLabel.Text = "Set a number of iterations that\'d generate the fractal";
            this.iterationLabel.Click += new System.EventHandler(this.iterationLabel_Click);
            // 
            // iterationBox
            // 
            this.iterationBox.Location = new System.Drawing.Point(48, 88);
            this.iterationBox.Margin = new System.Windows.Forms.Padding(4);
            this.iterationBox.Name = "iterationBox";
            this.iterationBox.Size = new System.Drawing.Size(329, 22);
            this.iterationBox.TabIndex = 16;
            this.iterationBox.TextChanged += new System.EventHandler(this.iterationBox_TextChanged);
            // 
            // iterationButton
            // 
            this.iterationButton.Location = new System.Drawing.Point(153, 148);
            this.iterationButton.Margin = new System.Windows.Forms.Padding(4);
            this.iterationButton.Name = "iterationButton";
            this.iterationButton.Size = new System.Drawing.Size(108, 27);
            this.iterationButton.TabIndex = 17;
            this.iterationButton.Text = "set iterator";
            this.iterationButton.UseVisualStyleBackColor = true;
            this.iterationButton.Click += new System.EventHandler(this.iterationButton_Click);
            // 
            // threatsLabel
            // 
            this.threatsLabel.AutoSize = true;
            this.threatsLabel.Location = new System.Drawing.Point(494, 43);
            this.threatsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.threatsLabel.Name = "threatsLabel";
            this.threatsLabel.Size = new System.Drawing.Size(208, 17);
            this.threatsLabel.TabIndex = 18;
            this.threatsLabel.Text = "Set a number of threts (max 64)";
            // 
            // threatsUpDown
            // 
            this.threatsUpDown.Location = new System.Drawing.Point(497, 88);
            this.threatsUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.threatsUpDown.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.threatsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threatsUpDown.Name = "threatsUpDown";
            this.threatsUpDown.Size = new System.Drawing.Size(201, 22);
            this.threatsUpDown.TabIndex = 19;
            this.threatsUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threatsUpDown.ValueChanged += new System.EventHandler(this.threatsUpDown_ValueChanged);
            // 
            // threatsButton
            // 
            this.threatsButton.Location = new System.Drawing.Point(541, 147);
            this.threatsButton.Margin = new System.Windows.Forms.Padding(4);
            this.threatsButton.Name = "threatsButton";
            this.threatsButton.Size = new System.Drawing.Size(100, 28);
            this.threatsButton.TabIndex = 20;
            this.threatsButton.Text = "set threats";
            this.threatsButton.UseVisualStyleBackColor = true;
            this.threatsButton.Click += new System.EventHandler(this.threatsButton_Click);
            // 
            // asmCppLabel
            // 
            this.asmCppLabel.AutoSize = true;
            this.asmCppLabel.Location = new System.Drawing.Point(791, 43);
            this.asmCppLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.asmCppLabel.Name = "asmCppLabel";
            this.asmCppLabel.Size = new System.Drawing.Size(117, 17);
            this.asmCppLabel.TabIndex = 21;
            this.asmCppLabel.Text = "Choose DLL type";
            // 
            // asmBox
            // 
            this.asmBox.AutoSize = true;
            this.asmBox.Location = new System.Drawing.Point(794, 79);
            this.asmBox.Margin = new System.Windows.Forms.Padding(4);
            this.asmBox.Name = "asmBox";
            this.asmBox.Size = new System.Drawing.Size(96, 21);
            this.asmBox.TabIndex = 22;
            this.asmBox.Tag = "";
            this.asmBox.Text = "Assembler";
            this.asmBox.UseVisualStyleBackColor = true;
            this.asmBox.CheckedChanged += new System.EventHandler(this.asmBox_CheckedChanged);
            // 
            // cppBox
            // 
            this.cppBox.AutoSize = true;
            this.cppBox.Location = new System.Drawing.Point(794, 108);
            this.cppBox.Margin = new System.Windows.Forms.Padding(4);
            this.cppBox.Name = "cppBox";
            this.cppBox.Size = new System.Drawing.Size(55, 21);
            this.cppBox.TabIndex = 23;
            this.cppBox.Text = "C++";
            this.cppBox.UseVisualStyleBackColor = true;
            this.cppBox.CheckedChanged += new System.EventHandler(this.cppBox_CheckedChanged);
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(191, 267);
            this.resolutionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(171, 17);
            this.resolutionLabel.TabIndex = 24;
            this.resolutionLabel.Text = "Set output file\'s resolution";
            // 
            // resolutionXBox
            // 
            this.resolutionXBox.Location = new System.Drawing.Point(147, 304);
            this.resolutionXBox.Margin = new System.Windows.Forms.Padding(4);
            this.resolutionXBox.Name = "resolutionXBox";
            this.resolutionXBox.Size = new System.Drawing.Size(113, 22);
            this.resolutionXBox.TabIndex = 25;
            this.resolutionXBox.TextChanged += new System.EventHandler(this.resolutionXBox_TextChanged);
            // 
            // xLabel
            // 
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(268, 309);
            this.xLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(14, 17);
            this.xLabel.TabIndex = 26;
            this.xLabel.Text = "x";
            // 
            // resolutionYBox
            // 
            this.resolutionYBox.Location = new System.Drawing.Point(290, 304);
            this.resolutionYBox.Margin = new System.Windows.Forms.Padding(4);
            this.resolutionYBox.Name = "resolutionYBox";
            this.resolutionYBox.Size = new System.Drawing.Size(116, 22);
            this.resolutionYBox.TabIndex = 27;
            this.resolutionYBox.TextChanged += new System.EventHandler(this.resolutionYBox_TextChanged);
            // 
            // resolutionButton
            // 
            this.resolutionButton.Location = new System.Drawing.Point(204, 334);
            this.resolutionButton.Margin = new System.Windows.Forms.Padding(4);
            this.resolutionButton.Name = "resolutionButton";
            this.resolutionButton.Size = new System.Drawing.Size(129, 30);
            this.resolutionButton.TabIndex = 28;
            this.resolutionButton.Text = "set resolution";
            this.resolutionButton.UseVisualStyleBackColor = true;
            this.resolutionButton.Click += new System.EventHandler(this.resolutionButton_Click);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(608, 234);
            this.outputLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(191, 17);
            this.outputLabel.TabIndex = 29;
            this.outputLabel.Text = "Choose output file\'s directory";
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(579, 267);
            this.outputBox.Margin = new System.Windows.Forms.Padding(4);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(244, 152);
            this.outputBox.TabIndex = 30;
            this.outputBox.Text = "";
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(593, 427);
            this.outputButton.Margin = new System.Windows.Forms.Padding(4);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(215, 30);
            this.outputButton.TabIndex = 31;
            this.outputButton.Text = "set output file directory";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // proceedButton
            // 
            this.proceedButton.Location = new System.Drawing.Point(899, 510);
            this.proceedButton.Margin = new System.Windows.Forms.Padding(4);
            this.proceedButton.Name = "proceedButton";
            this.proceedButton.Size = new System.Drawing.Size(111, 31);
            this.proceedButton.TabIndex = 32;
            this.proceedButton.Text = "proceed";
            this.proceedButton.UseVisualStyleBackColor = true;
            this.proceedButton.Click += new System.EventHandler(this.proceedButton_Click);
            // 
            // appWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1045, 567);
            this.Controls.Add(this.proceedButton);
            this.Controls.Add(this.outputButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.resolutionButton);
            this.Controls.Add(this.resolutionYBox);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.resolutionXBox);
            this.Controls.Add(this.resolutionLabel);
            this.Controls.Add(this.cppBox);
            this.Controls.Add(this.asmBox);
            this.Controls.Add(this.asmCppLabel);
            this.Controls.Add(this.threatsButton);
            this.Controls.Add(this.threatsUpDown);
            this.Controls.Add(this.threatsLabel);
            this.Controls.Add(this.iterationButton);
            this.Controls.Add(this.iterationBox);
            this.Controls.Add(this.iterationLabel);
            this.Name = "appWindow";
            this.Text = "Fraktale Mandelbrota";
            ((System.ComponentModel.ISupportInitialize)(this.threatsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label iterationLabel;
        private System.Windows.Forms.TextBox iterationBox;
        private System.Windows.Forms.Button iterationButton;
        private System.Windows.Forms.Label threatsLabel;
        private System.Windows.Forms.NumericUpDown threatsUpDown;
        private System.Windows.Forms.Button threatsButton;
        private System.Windows.Forms.Label asmCppLabel;
        private System.Windows.Forms.CheckBox asmBox;
        private System.Windows.Forms.CheckBox cppBox;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.TextBox resolutionXBox;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.TextBox resolutionYBox;
        private System.Windows.Forms.Button resolutionButton;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.RichTextBox outputBox;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.Button proceedButton;
    }
}

