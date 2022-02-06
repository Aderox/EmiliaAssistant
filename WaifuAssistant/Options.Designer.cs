using System;
using System.Drawing;
using System.Windows.Forms;


namespace WaifuAssistant
{
    partial class Options
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


        //dragable windows
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void optionWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void MyInitialize()
        {
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.optionWindow_MouseDown);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveChanges = new System.Windows.Forms.Button();
            this.discardChanges = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textCommands = new System.Windows.Forms.RichTextBox();
            this.choosedFile = new System.Windows.Forms.Label();
            this.selectAudioFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveChanges
            // 
            this.saveChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(80)))));
            this.saveChanges.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveChanges.FlatAppearance.BorderSize = 0;
            this.saveChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveChanges.Location = new System.Drawing.Point(105, 271);
            this.saveChanges.Name = "saveChanges";
            this.saveChanges.Size = new System.Drawing.Size(75, 23);
            this.saveChanges.TabIndex = 1;
            this.saveChanges.Text = "Enrengistrer";
            this.saveChanges.UseVisualStyleBackColor = false;
            this.saveChanges.Click += new System.EventHandler(this.saveChanges_Click);
            // 
            // discardChanges
            // 
            this.discardChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.discardChanges.FlatAppearance.BorderSize = 0;
            this.discardChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.discardChanges.Location = new System.Drawing.Point(585, 271);
            this.discardChanges.Name = "discardChanges";
            this.discardChanges.Size = new System.Drawing.Size(75, 23);
            this.discardChanges.TabIndex = 2;
            this.discardChanges.Text = "Annuler";
            this.discardChanges.UseVisualStyleBackColor = false;
            this.discardChanges.Click += new System.EventHandler(this.discardChanges_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.panel1.Controls.Add(this.textCommands);
            this.panel1.Controls.Add(this.choosedFile);
            this.panel1.Controls.Add(this.selectAudioFile);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(82, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(606, 118);
            this.panel1.TabIndex = 3;
            // 
            // textCommands
            // 
            this.textCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.textCommands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textCommands.ForeColor = System.Drawing.Color.White;
            this.textCommands.Location = new System.Drawing.Point(349, 24);
            this.textCommands.Name = "textCommands";
            this.textCommands.Size = new System.Drawing.Size(193, 65);
            this.textCommands.TabIndex = 3;
            this.textCommands.Text = "";
            // 
            // choosedFile
            // 
            this.choosedFile.AutoSize = true;
            this.choosedFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.choosedFile.ForeColor = System.Drawing.Color.White;
            this.choosedFile.Location = new System.Drawing.Point(88, 92);
            this.choosedFile.Name = "choosedFile";
            this.choosedFile.Size = new System.Drawing.Size(77, 15);
            this.choosedFile.TabIndex = 2;
            this.choosedFile.Text = "Aucun fichier";
            // 
            // selectAudioFile
            // 
            this.selectAudioFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.selectAudioFile.FlatAppearance.BorderSize = 0;
            this.selectAudioFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectAudioFile.ForeColor = System.Drawing.Color.White;
            this.selectAudioFile.Location = new System.Drawing.Point(23, 68);
            this.selectAudioFile.Name = "selectAudioFile";
            this.selectAudioFile.Size = new System.Drawing.Size(216, 21);
            this.selectAudioFile.TabIndex = 1;
            this.selectAudioFile.Text = "Selectionner un fichier audio";
            this.selectAudioFile.UseVisualStyleBackColor = false;
            this.selectAudioFile.Click += new System.EventHandler(this.selectAudioFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(25, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enrengistrer une nouvelle commande vocal:";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(731, 369);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.discardChanges);
            this.Controls.Add(this.saveChanges);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Options";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button saveChanges;
        private System.Windows.Forms.Button discardChanges;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectAudioFile;
        private System.Windows.Forms.Label choosedFile;
        private System.Windows.Forms.RichTextBox textCommands;
    }
}