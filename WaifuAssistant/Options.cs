using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaifuAssistant
{
    public partial class Options : Form
    {
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
            this.Close();
        }

        private void selectAudioFile_Click(object sender, EventArgs e)
        {
            //open file selection windows
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "c:\\";
            fileDialog.Filter = "Fichier audio|*.mp3;*.wav;*.m4a";
            fileDialog.ShowDialog();
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            this.mainWindowParent.EnableOptionButton(true);
            this.Close();
        }
    }
}
