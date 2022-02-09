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

namespace WaifuAssistant
{

    public partial class mainWindow : Form
    {
        public static string clientResources { set; get; } = System.Environment.CurrentDirectory + "\\Resources\\ClientResources";
        public static string baseCmds { set; get; } = clientResources + "\\baseCmds.json";

        public static bool isListening { set; get; } = false;
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        /// 
        public enum RegisterCommand
        {
            OK = 1,
            ALREADY_EXIST = 2,
            CAN_NOT_WRITE = 3,
            UNKOWN = 0
        }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        //SpeechRecognitionEngine recognization = new SpeechRecognitionEngine();
        Windows.Media.SpeechRecognition.SpeechRecognizer speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

        List<string> cmds = new List<string>();
        List<string> paths = new List<string>();

        List<string> userCmds = new List<string>();
        List<string> userPaths = new List<string>();

        public mainWindow()
        {
            InitializeComponent();
            MyInizialize();
           //TODO var contraite = new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(cmds);
           // speechRecognizer.CompileConstraintsAsync();
            this.button2.Enabled = false;

            Console.WriteLine("test:");
            richTextBox1.Text += "Initialisation...";
            Console.WriteLine("Début !");

            Dictionary<string, Choices> initialCmdsList = new Dictionary<string, Choices>();
            //initialCmds.Add(new string[] { "Salut ça va", "Que fait tu", "Oui"});
            //this.LoadInitialCmds(initialCmds); 

            // Create an instance of SpeechRecognizer.


            // Compile the dictation grammar by default
            IAsyncOperation<SpeechRecognitionCompilationResult> compiledSpeech = speechRecognizer.CompileConstraintsAsync();

            //Dictionary<string, Choices> cmds = new Dictionary<string, Choices>();
            //Dictionary<string, Choices> userCmds = new Dictionary<string, Choices>();
            
            try
            {
                this.LoadCmds(cmds, paths);
            } catch (Exception e)
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
                this.LoadCmds(userCmds, userPaths, true);
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
            //TODO ADD USER CMDS READER


            /*Choices tempChoice = new Choices();
            tempChoice.Add(new string[] { "Salut ça va", "Canapé" });

            GrammarBuilder builder = new GrammarBuilder();
            //builder.Culture = new CultureInfo("fr-FR");
            builder.Append(tempChoice);
            //builder.Append(cmds["Default"]);

            //Console.WriteLine("CMDS:" + cmds["Default"].ToString());
            //Grammar grammar = new Grammar(builder);

            
            recognization.LoadGrammarAsync(grammar);
            recognization.SetInputToDefaultAudioDevice();
            recognization.SpeechRecognized += recognized_voice;*/

        }

        private void checkRessourceDir()
        {
            if (!Directory.Exists(clientResources))
            {
                Directory.CreateDirectory(clientResources);
            }
        }

        private void checkCmds(bool userCmds = false)
        {
            string localCmds = baseCmds;
            if (userCmds)
            {
                localCmds = clientResources + "\\clientCmds.json";
            }

            if (!File.Exists(localCmds))
            {
                //le fichier existe pas on le crée
                using (FileStream fs = File.Create(localCmds))
                {
                    Byte[] info;
                    if (!userCmds)
                    {
                        info =
                            new UTF8Encoding(true).GetBytes(@"{""Commands"":{""Default"":[[""Salut ça va"",""TEKOS_VOICE\\Hey!.mp3""]],""Fun"":[[""Fais moi une blague"",""TEKOS_VOICE\\No.mp3""]]}}");

                    }
                    else
                    {
                        info =
                            new UTF8Encoding(true).GetBytes(@"{""Commands"":{""Default"":[[""Test Client"", ""TEKOS_VOICE\\Hey!.mp3""]]}}");
                    }

                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                Console.WriteLine("localCmds exist");
            }
        }

        private JsonNode readCmds(bool userCmds = false)
        {
            string localCmds = baseCmds;
            if (userCmds)
            {
                localCmds = clientResources + "\\clientCmds.json";
            }
            checkCmds(userCmds);
            using (StreamReader sr = File.OpenText(localCmds))
            {
                Console.WriteLine("on lit le fichie client: " + userCmds);
                string s = "";
                string jsonString = "";
                while ((s = sr.ReadLine()) != null)
                {
                    jsonString += s;
                }

                //jsonObject allCmdsFromFile = System.Text.Json.JsonSerializer.Deserialize<jsonObject>(jsonString);
                JsonNode node = JsonNode.Parse(jsonString);
                var options = new JsonSerializerOptions { WriteIndented = true };
                //clg: node.ToJsonString(options);
                var CommandsNode = node["Commands"];
                Console.WriteLine("ras on debug");
                Console.WriteLine(CommandsNode);
                return node;
            }

        }

        private void LoadCmds(List<string> cmds, List<string> paths, bool userCmds = false)
        {
            //for each cmds in the file appends 
            string localCmds = baseCmds;
            if (userCmds)
            {
                localCmds = clientResources + "\\clientCmds.json";
            }

                checkRessourceDir();
                var node = readCmds(userCmds);
                var CommandsNode = node["Commands"];
                Console.WriteLine("on transfrome en dict");
                Console.WriteLine(CommandsNode.ToString());
                var CommandsDict = CommandsNode.AsObject().ToDictionary(p => p.Key);

                Console.WriteLine("foreach");
                //for each KEYS:
                foreach (string key in CommandsDict.Keys)
                {
                    //Console.WriteLine("key: "+ key);
                    var temp = CommandsNode[key].AsArray();
                    
                    for (int i = 0; i < temp.Count; i++)
                    {
                        Console.WriteLine("Element: " + i + " commande vocal: " + temp[i][0] + " son à joué: " + temp[i][1] + " pour la catégorie " + key + " client: " + userCmds);
                    //on a ici TOUT !
                    Console.WriteLine("on ajoute à la liste des cmds:");
                        cmds.Add(temp[i][0].ToString());
                    Console.WriteLine("on ajoute à la liste des path:" + temp[i][1].ToString());
                        paths.Add("\\\\" + temp[i][1].ToString());
                    }
                    
                
                //Console.WriteLine("JSON DESERIALISZED:");
                //Console.WriteLine(allCmdsFromFile.getAllCategorieAsString());
                }
        }

        public RegisterCommand WriteClientJson(string cmd, string path)
        {
            string localCmds = clientResources + "\\clientCmds.json";

            if (userCmds.Contains(cmd)){
                return RegisterCommand.ALREADY_EXIST;
            }

            checkRessourceDir();
            JsonNode node = readCmds(true);
            JsonObject obj = node["Commands"].AsObject();

            List<string> list = new List<string>();
            list.Add(cmd.ToString());
            list.Add(path.ToString());


            obj["Default"].AsArray().Add(new JsonArray());
            int count = obj["Default"].AsArray().Count;
            obj["Default"].AsArray()[count - 1].AsArray().Add(cmd);
            obj["Default"].AsArray()[count - 1].AsArray().Add(path);

            Console.WriteLine("new node:");
            Console.WriteLine(node);
;

            //ajoute au commandes existante avant le reboot
            userCmds.Add(cmd);
            userPaths.Add(path);

            //ecrit le nouvel objet
            string nodeToJson = JsonSerializer.Serialize(node);
            Console.WriteLine(nodeToJson);

            if (File.Exists(localCmds))
            {
                File.Delete(localCmds);

            }
            
            using (FileStream fs = File.Create(localCmds))
            {
                try
                {
                    Byte[] info;
                    info =
                        new UTF8Encoding(true).GetBytes(nodeToJson);
                    fs.Write(info, 0, info.Length);
                    return RegisterCommand.OK;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return RegisterCommand.UNKOWN;
                }

            }

        }

        private async void handleVocal(bool fromButton = false)
        {
            if (!isListening)
            {
                return;
            }
            if (fromButton)
            {
                richTextBox1.Text += "\nRe !";
            }

            Console.WriteLine("rappel contre");
            string command = "";

            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeAsync();
            command = speechRecognitionResult.Text;
            //if(command.ToLower() in cmds)
            for(int i = 0; i<cmds.Count; i++)
            {
                if(command.ToLower() == cmds[i].ToString().ToLower())
                {
                    //Console.WriteLine("command de base");
                    //la commande est une commande de bas
                    //Console.WriteLine("on joue le fichier: " + clientResources + paths[i].ToString());
                    PlayFile(clientResources + paths[i].ToString());
                }
                else
                {
                    Console.WriteLine("la commande '" + command.ToString() + "' ne correspond pas à '" + cmds[i].ToString() + "'");
                }
            }
            for(int i = 0; i<userCmds.Count; i++)
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
        public void Pause()
        {
            //fait en sorte de ne plus pouvoir intéragir avec les bouttons
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

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "\nDebut de l'écoute.";
            //recognization.RecognizeAsync(RecognizeMode.Multiple);

            // Start recognition.
            isListening = true;
            try
            {
                handleVocal(true);
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
            //recognization.RecognizeAsyncStop();

            isListening = false;
            try
            {
                await speechRecognizer.StopRecognitionAsync();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            richTextBox1.Text += "\nFin de l'écoute.";
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
            this.Pause();
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {

        }

        private void optionButton_Click_1(object sender, EventArgs e)
        {

        }

        private void mainWindow_Load_1(object sender, EventArgs e)
        {

        }

        WMPLib.WindowsMediaPlayer Player;

        private void PlayFile(String url)
        {
            Console.WriteLine("play a sound: " + url);
            Player = new WMPLib.WindowsMediaPlayer();
            Player.PlayStateChange += Player_PlayStateChange;
            Player.URL = url;
            Player.controls.play();
        }

        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
            {
                Console.WriteLine("stop sound");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WriteClientJson("j'adore les pates", "\\Alert.mp3");
        }
    }
}
//C:\Users\Ady\Documents\Programmation\C#\WaifuAssistant\EmiliaAssistant\WaifuAssistant\bin\Debug\Resources\ClientResources\TEKOS_VOICE