using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
//using InputManager;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Activities;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Windows.Media.SpeechRecognition;
using Windows.Media.Capture;
using System.Threading.Tasks.Sources;
using Windows;
using Windows.System.Threading.Core;
using System.Runtime.InteropServices;
using Windows.Foundation;
using System.Media;
using Windows.UI.Xaml;
using WMPLib;
using Windows.ApplicationModel.ExtendedExecution;

namespace WaifuAssistant
{

    public partial class mainWindow : Form
    {

      

        public static string clientResources { set; get; } = System.Environment.CurrentDirectory + "\\Resources\\ClientResources";
        public static string baseCmds { set; get; } = clientResources + "\\baseCmds.json";

        public static bool isListening { set; get; } = false;

        WindowsMediaPlayer Player = new WMPLib.WindowsMediaPlayer(); 
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        /// 


        List<string> cmds = new List<string>();
        List<string> paths = new List<string>();

        List<string> userCmds = new List<string>();
        List<string> userPaths = new List<string>();

        public MyFileReader fileReader;
        public SpeechRecognition speechRecognition;
        public WinApi winApi;

        private async void testTODELETE()
        {
            this.winApi.MyShowWindowAsync(1);
            await Task.Delay(1000);
            this.winApi.MyShowWindowAsync(2);
            await Task.Delay(1000);
                        this.winApi.MyShowWindowAsync(1);

        }
        public mainWindow()
        {
            InitializeComponent();
            MyInizialize();

            fileReader = new MyFileReader(clientResources, baseCmds);
            winApi = new WinApi(this.Handle);
            testTODELETE();

            try
            {
                fileReader.LoadCmds(cmds, paths);
                //this.LoadCmds(cmds, paths);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DialogResult result = MessageBox.Show("Erreur lors de la lecture des commandes par défaut !", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    Environment.Exit(1);
                }
            }
            try
            {
                fileReader.LoadCmds(cmds, paths, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                DialogResult result = MessageBox.Show("Erreur lors de la lecture des commandes client !", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    Environment.Exit(1);
                }
            }


            speechRecognition = new SpeechRecognition(cmds, paths, userCmds, userPaths, winApi);

            this.button2.Enabled = false;
            label1.Text = "Initialisation...";
            // Create an instance of SpeechRecognizer.


            // Compile the dictation grammar by default
            //var contraite = new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(new string[] { "canapé", "pomme de terre", "vitres"});


        }

        private async void handleVocal(bool fromButton = false)
        {
           
            Console.Write("vocal !");
            if (!isListening)
            {
                return;
            }

            string command = "";

            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognition.getWindowsMedia().RecognizeAsync();

            command = speechRecognitionResult.Text;
            //if(command.ToLower() in cmds)
            for (int i = 0; i < cmds.Count; i++)
            {
                if (command.ToLower() == cmds[i].ToString().ToLower())
                {
                    PlayFile(clientResources + paths[i].ToString());
                }
                else if (command.ToString() != "")
                {
                    //Console.WriteLine("la commande '" + command.ToString() + "' ne correspond pas à '" + cmds[i].ToString() + "'");
                }
            }
            for (int i = 0; i < userCmds.Count; i++)
            {
                if (command.ToLower() == userCmds[i].ToString().ToLower())
                {
                    //c'est une commande ajouté par le client
                    if (command.ToLower() == userCmds[i].ToString().ToLower())
                    {
                        if (userPaths[i].ToString() != "")
                        {
                            //il y a du son
                            PlayFile(clientResources + userPaths[i].ToString());
                        }
                    }
                }
            }

            handleVocal();
        }

        #region getter
        public List<string> getDefaultCmds()
        {
            return this.cmds;
        }
        public List<string> getDefaultPath()
        {
            return this.paths;
        }
        public List<string> getClientCmds()
        {
            return this.userCmds;
        }
        public List<string> getClientPath()
        {
            return this.userPaths;
        }
        #endregion getter
        public void Pause()
        {
            this.button1.Enabled = false;
            this.button2.Enabled = false;
        }
        public void Resume()
        {
            this.button1.Enabled = true;
            this.button2.Enabled = true;
        }

        public void EnableOptionButton(bool action)
        {
            this.optionButton.Enabled = action;
        }

        
        private async void button1_Click(object sender, EventArgs e)
        {
            label1.Text += "\nDebut de l'écoute.";
            isListening = true;
            try
            {
                handleVocal(true);
                Console.WriteLine("set click throught:");
                //invisible();
                //runInBackground();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }


            button1.Enabled = false;
            button2.Enabled = true;
        }

        //disable lisening
        private async void button2_Click(object sender, EventArgs e)
        {

            isListening = false;
            try
            {
                await speechRecognition.getWindowsMedia().StopRecognitionAsync();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            label1.Text += "\nFin de l'écoute.";
            Player.controls.stop();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void optionButton_Click(object sender, EventArgs e)
        {
            //ouvrir les options
            Options option = new Options(this);
            option.StartPosition = FormStartPosition.Manual;
            System.Drawing.Point center = new System.Drawing.Point((int)Math.Round((float)this.Location.X + option.Size.Width / 2), (int)Math.Round((float)this.Location.Y + option.Size.Height / 2));
            option.Location = center;
            option.Show();
            this.optionButton.Enabled = false;
            this.button2_Click(sender, e);
            this.Pause();
        }

        private void PlayFile(String url)
        {
            Player.PlayStateChange += Player_PlayStateChange;
            Player.URL = url;
            Player.controls.play();
        }

        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            {
            }
        }

        #region uselessEvent

        private void mainWindow_Load(object sender, EventArgs e)
        {

        }

        private void optionButton_Click_1(object sender, EventArgs e)
        {

        }

        private void mainWindow_Load_1(object sender, EventArgs e)
        {

        }

        #endregion uselessEvent
    }
}
