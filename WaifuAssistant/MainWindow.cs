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
using System.Runtime.InteropServices;
using System.Threading;
using System.Activities;

namespace WaifuAssistant
{
    public partial class mainWindow : Form
    {
        public bool isOptionShown { get; set; } = false;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        SpeechRecognitionEngine recognization = new SpeechRecognitionEngine();

        public mainWindow()
        {
            InitializeComponent();
            MyInizialize();

            richTextBox1.Text += "Initialisation...";
            Console.WriteLine("Début !");

            Choices cmds = new Choices();
            cmds.Add(new string[] { "Salut ça va", "Que fait tu", "Oui"});
            GrammarBuilder builder = new GrammarBuilder();
            builder.Culture = new CultureInfo("fr-FR");
            builder.Append(cmds);
            Grammar grammar = new Grammar(builder);
           

            recognization.LoadGrammar(grammar);
            recognization.SetInputToDefaultAudioDevice();
            recognization.SpeechRecognized += recognized_voice;

        }

        private void recognized_voice(object sender, SpeechRecognizedEventArgs e)
        {
            string resultat = e.Result.Text;
            Console.WriteLine("speech: " + resultat);
            switch (e.Result.Text)
            {
                case "Salut ça va":
                    richTextBox1.Text += "\n Je vais bien, merci.";
                    break;
                case "Que fait tu":
                    richTextBox1.Text += "\n Je suis en cours de maj";
                    break;
                default:
                    break;
            }
        }

        private void Pause()
        {
            //fait en sorte de ne plus pouvoir intéragir avec les bouttons
        }

        public void EnableOptionButton(bool action)
        {
            this.optionButton.Enabled = action;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "\nDebut de l'écoute.";
            recognization.RecognizeAsync(RecognizeMode.Multiple);
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            recognization.RecognizeAsyncStop();
            richTextBox1.Text += "\nFin de l'écoute";
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void optionButton_Click(object sender, EventArgs e)
        {
            //ouvrir les options
            Options option = new Options(this);
            option.StartPosition = FormStartPosition.Manual;
            Point center = new Point((int)Math.Round((float)this.Location.X + option.Size.Width/2),(int)Math.Round((float)this.Location.Y + option.Size.Height/2));
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
    }
}
