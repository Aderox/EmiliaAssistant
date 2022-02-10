using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace WaifuAssistant
{
    public class MyFileReader
    {
        string clientResources;
        string baseCmds;


        public enum RegisterCommand
        {
            OK = 1,
            ALREADY_EXIST = 2,
            CAN_NOT_WRITE = 3,
            UNKOWN = 0
        }

        public MyFileReader(string clientResources, string baseCmds)
        {
            this.clientResources = clientResources;
            this.baseCmds = baseCmds;

        }


        private void checkRessourceDir()
        {
            if (!Directory.Exists(this.clientResources))
            {
                Directory.CreateDirectory(this.clientResources);
            }
        }

        private void checkCmds(bool userCmds = false)
        {
            string localCmds = this.baseCmds;
            if (userCmds)
            {
                localCmds = this.clientResources + "\\clientCmds.json";
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
            }
        }

        public JsonNode readCmds(bool userCmds = false)
        {
            string localCmds = baseCmds;
            if (userCmds)
            {
                localCmds = clientResources + "\\clientCmds.json";
            }
            checkCmds(userCmds);
            using (StreamReader sr = File.OpenText(localCmds))
            {
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
                return node;
            }

        }

        public void LoadCmds(List<string> cmds, List<string> paths, bool userCmds = false)
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
            var CommandsDict = CommandsNode.AsObject().ToDictionary(p => p.Key);

            //for each KEYS:
            foreach (string key in CommandsDict.Keys)
            {
                var temp = CommandsNode[key].AsArray();

                for (int i = 0; i < temp.Count; i++)
                {
                    cmds.Add(temp[i][0].ToString());
                    paths.Add("\\\\" + temp[i][1].ToString());
                }


            }
        }

        public RegisterCommand WriteClientJson(string cmd, string path, List<string> userCmds, List<string> userPaths)
        {
            string localCmds = clientResources + "\\clientCmds.json";

            if (userCmds.Contains(cmd))
            {
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

            ;

            //ajoute au commandes existante avant le reboot
            userCmds.Add(cmd);
            userPaths.Add(path);

            //ecrit le nouvel objet
            string nodeToJson = JsonSerializer.Serialize(node);

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
    }
}
