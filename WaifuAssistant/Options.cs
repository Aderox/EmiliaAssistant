using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaifuAssistant
{
    public partial class Options : Form
    {
        string initialPath = "";
        string fileName = "";
        bool isFileChoosen = false;
        mainWindow mainWindowParent;
        public Options(mainWindow mainWindowParent)
        {
            InitializeComponent();
            MyInitialize();
            this.mainWindowParent = mainWindowParent;
        }

        private void Options_Load(object sender, EventArgs e)
        {

        }

        //
        private void discardChanges_Click(object sender, EventArgs e)
        {
            //resume and close
            this.mainWindowParent.EnableOptionButton(true);
            this.mainWindowParent.Resume();
            this.Close();
        }

        private void selectAudioFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "c:\\";
            fileDialog.Filter = "Fichier audio|*.mp3;*.wav;*.m4a";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                initialPath = fileDialog.FileName;
                fileName = initialPath.Split('\\').Last();
                Console.WriteLine("filename: " + fileName);
                this.selectAudioFile.Text = fileName;
                isFileChoosen = true;
            }
            else
            {
                MessageBox.Show("Erreur: Le fichier n'a pas pu etre séléctioné !", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            if (isFileChoosen)
            {
                if (!Directory.Exists(mainWindow.clientResources + "\\USER_VOICE\\"))
                {
                    Directory.CreateDirectory(mainWindow.clientResources + "\\USER_VOICE\\");
                }
                if (!File.Exists(mainWindow.clientResources + "\\USER_VOICE\\" + fileName))
                {
                    File.Copy(initialPath, mainWindow.clientResources + "\\USER_VOICE\\" + fileName);
                    //TODO REGISTER TO JSON
                    MessageBox.Show("Le fichier à été copié avec succés !", "SUCCES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Le fichier existe déjà !", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                //pas de fichier audio
            }


            this.mainWindowParent.EnableOptionButton(true);
            this.mainWindowParent.Resume();
            this.Close();
        }

    }
}
