using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace The_New_Order_Of_Discord
{
    public partial class TNOOD : Form
    {
        private bool Server = new bool();
        private bool DM = new bool();

        public TNOOD()
        {
            try
            {
                InitializeComponent();
            }
            catch { }
        }

        private void TNOOD_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch { }
        }

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    string Command = richTextBox2.Text.Split(' ').Take(1).First();
                    string[] Args = richTextBox2.Text.Split(' ').Skip(1).ToArray();

                    switch (Command)
                    {
                        case "/join":
                            {
                                string _1 = richTextBox1.Text;

                                label8.Text = $"Joining in server 'discord.gg/{Args[0]}' with '{_1.Split(new string[] { "\n" }, StringSplitOptions.None).Length} bot's'.";

                                new Thread(() =>
                                {
                                    WebClient WC = new WebClient();

                                    for (long i = 0; i < _1.Split(new string[] { "\n" }, StringSplitOptions.None).Length; i++)
                                    {
                                        if (!string.IsNullOrWhiteSpace(_1.Split(new string[] { "\n" }, StringSplitOptions.None)[i]))
                                        {
                                            try
                                            {
                                                WC.Headers["authorization"] = _1.Split(new string[] { "\n" }, StringSplitOptions.None)[i];
                                                WC.UploadValues(string.Concat("https://discordapp.com/api/v6/invite/", Args[0]), new NameValueCollection());
                                            }
                                            catch { }
                                        }
                                    }
                                }).Start();
                            }
                            break;

                        case "/server":
                            {
                                if (!Server)
                                {
                                    string _1 = richTextBox1.Text;

                                    Server = true;

                                    label8.Text = $"Sending '{_1.Split(new string[] { "\n" }, StringSplitOptions.None).Length} bot's' to flood in '{Args[0]}' with message:\n'{string.Join(" ", Args.Skip(1))}'";

                                    new Thread(() =>
                                    {
                                        while (Server)
                                        {
                                            using (WebClient WC = new WebClient())
                                            {
                                                for (long i = 0; i < _1.Split(new string[] { "\n" }, StringSplitOptions.None).Length; i++)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(_1.Split(new string[] { "\n" }, StringSplitOptions.None)[i]))
                                                    {
                                                        try
                                                        {
                                                            NameValueCollection NVC = new NameValueCollection();
                                                            NVC["content"] = string.Join(" ", Args.Skip(1));

                                                            WC.Headers["authorization"] = _1.Split(new string[] { "\n" }, StringSplitOptions.None)[i];
                                                            WC.UploadValues($"https://discordapp.com/api/v6/channels/{Args[0]}/messages", NVC);
                                                        }
                                                        catch { }
                                                    }
                                                }
                                            }
                                        }
                                    }).Start();
                                }
                                else
                                {
                                    Server = false;

                                    label8.Text = $"Server flood has disabled with success!";
                                }
                            }
                            break;

                        case "/dm":
                            {
                                if (!DM)
                                {
                                    string _1 = richTextBox1.Text;
                                    string Cache = string.Empty;

                                    DM = true;

                                    label8.Text = $"Sending '{_1.Split(new string[] { "\n" }, StringSplitOptions.None).Length} bot's' to flood to user '{Args[0]}' with message:\n'{string.Join(" ", Args.Skip(1))}'";

                                    new Thread(() =>
                                    {
                                        /* STEP 1 */
                                        if (DM)
                                        {
                                            using (WebClient WC = new WebClient())
                                            {
                                                for (long i = 0; i < _1.Split(new string[] { "\n" }, StringSplitOptions.None).Length; i++)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(_1.Split(new string[] { "\n" }, StringSplitOptions.None)[i]))
                                                    {
                                                        try
                                                        {
                                                            WC.Headers[HttpRequestHeader.ContentType] = "application/json";
                                                            WC.Headers["authorization"] = _1.Split(new string[] { "\n" }, StringSplitOptions.None)[i];

                                                            string Data = WC.UploadString("https://discordapp.com/api/v6/users/@me/channels", "POST", @"{ ""recipient_id"": " + Args[0] + " }");

                                                            Data = string.Join(" ", Data.Split('"').Skip(3));
                                                            Data = Data.Substring(0, Data.IndexOf(",") - 1);

                                                            if (string.IsNullOrWhiteSpace(Cache))
                                                            {
                                                                Cache = Data;
                                                            }
                                                            else
                                                            {
                                                                Cache += "\n" + Data;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            if (string.IsNullOrWhiteSpace(Cache))
                                                            {
                                                                Cache = "0";
                                                            }
                                                            else
                                                            {
                                                                Cache += "\n" + "0";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        /* STEP 2 */
                                        if (!string.IsNullOrWhiteSpace(Cache))
                                        {
                                            while (DM)
                                            {
                                                using (WebClient WC = new WebClient())
                                                {
                                                    for (long i = 0; i < _1.Split(new string[] { "\n" }, StringSplitOptions.None).Length; i++)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(_1.Split(new string[] { "\n" }, StringSplitOptions.None)[i]))
                                                        {
                                                            try
                                                            {
                                                                HttpWebRequest HTTP = (HttpWebRequest)WebRequest.Create(new Uri($"https://discordapp.com/api/v6/channels/{Cache.Split(new string[] { "\n" }, StringSplitOptions.None)[i]}/messages"));
                                                                HTTP.Accept = "application/json";
                                                                HTTP.ContentType = "application/json";
                                                                HTTP.Headers["authorization"] = _1.Split(new string[] { "\n" }, StringSplitOptions.None)[i];
                                                                HTTP.Method = "POST";

                                                                byte[] BYTES = new ASCIIEncoding().GetBytes(@"{ ""content"": """ + string.Join(" ", Args.Skip(1)) + @""", ""tts"": false }");

                                                                Stream NetworkStream = HTTP.GetRequestStream();

                                                                NetworkStream.Write(BYTES, 0, BYTES.Length);
                                                                NetworkStream.Close();

                                                                WebResponse Response = HTTP.GetResponse();
                                                                Stream ResponseStream = Response.GetResponseStream();

                                                                StreamReader SR = new StreamReader(ResponseStream);

                                                                string Data = SR.ReadToEnd();

                                                                SR.Dispose();
                                                                SR.Close();
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }).Start();
                                }
                                else
                                {
                                    DM = false;

                                    label8.Text = $"Direct Message flood has disabled with success!";
                                }
                            }
                            break;
                    }

                    richTextBox2.Text = string.Empty;
                }
            }
            catch { }
        }
    }
}
