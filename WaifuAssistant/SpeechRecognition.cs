using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.SpeechRecognition;

namespace WaifuAssistant
{
    public class SpeechRecognition
    {

        List<string> cmds;
        List<string> paths;
        List<string> userCmds;
        List<string> userPaths;

        Choices choicesStart = new Choices();
        SpeechRecognitionEngine sysSpeech = new SpeechRecognitionEngine(new CultureInfo("fr-FR"));
        Windows.Media.SpeechRecognition.SpeechRecognizer mediaSpeech = new Windows.Media.SpeechRecognition.SpeechRecognizer();
        WinApi winApi;

        public SpeechRecognition(List<string> cmds, List<string> paths, List<string> userCmds, List<string> userPaths, WinApi winApi)
        {
            choicesStart.Add(new string[] { "Salut", "Teko" });
            GrammarBuilder builder = new GrammarBuilder();
            builder.Append(choicesStart);
            Grammar grammar = new Grammar(builder);
            sysSpeech.LoadGrammar(grammar);
            sysSpeech.SetInputToDefaultAudioDevice();
            sysSpeech.SpeechRecognized += firstRecognized;
            sysSpeech.RecognizeAsync(RecognizeMode.Multiple);

            this.cmds = cmds;
            this.paths = paths;
            this.userCmds = userCmds;
            this.userPaths = userPaths;

            this.winApi = winApi;
            IAsyncOperation<SpeechRecognitionCompilationResult> compiledSpeech = mediaSpeech.CompileConstraintsAsync();
        }


        private void firstRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            Console.WriteLine("e: " + e.Result.Text + " certitude: " + e.Result.Confidence + " autre: " + e.Result.Alternates.Count);
            for (int i = 0; i < e.Result.Alternates.Count; i++)
            {
                Console.Write(e.Result.Alternates.ToArray());
            }
            if (cmds.Contains(e.Result.Text) || userCmds.Contains(e.Result.Text))
            {
                Console.WriteLine("omg reconu");
            }
            if ((e.Result.Text == "Salut" || e.Result.Text == "Teko") && e.Result.Confidence > 0.001) //todo
            {
                Console.WriteLine("windows first ground");
                //winApi.MySetActiveWindow();
                //winApi.MySetForegroundWindow();
                winApi.MyShowWindowAsync(1);
                winApi.MySetFocus();
            }
        }

        public Windows.Media.SpeechRecognition.SpeechRecognizer getWindowsMedia()
        {
            return this.mediaSpeech;
        }

        public System.Speech.Recognition.SpeechRecognitionEngine getSystemSpeech()
        {
            return this.sysSpeech;
        }
    }
}
