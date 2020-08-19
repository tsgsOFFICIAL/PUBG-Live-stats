using System;
using IronOcr;
using System.IO;
using System.Net;
using System.Windows;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PUBG_Live_stats__Framework_
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public static string Output_path_text_string;
        public static string OutputPath = null;
        public static bool runScanner = false;

        public MainWindow()
            {
            InitializeComponent();
            Output_path.Text = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\PUBG Live stats";
            OutputPath = Output_path.Text;
            Output_path_text_string = Output_path_text.Text;
            MainScanner();
            }

        //EventHandlers

        /// <summary>
        /// State ON event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Program_state_Checked(object sender, RoutedEventArgs e)
            {
            //Create directory if it doesn't exist.
            if (!Directory.Exists(Output_path.Text))
                {
                Directory.CreateDirectory(Output_path.Text);
                }

            //Change to a brighter color
            Program_state.Foreground = new SolidColorBrush(Colors.Orange);

            //Lock output path
            Output_path.Visibility = Visibility.Hidden;
            Output_path_text.Text = $"Your files will be saved here: \"{Output_path.Text}\"";

            if (Uploadtoftp_check.IsChecked == true)
                {
                //Check if FTP is valid
                if (!FTP_URL.Text.Trim().Equals(""))
                    {
                    WebClient ftpClient = new WebClient();
                    ftpClient.Credentials = new NetworkCredential(FTP_User.Text, FTP_Passwordbox.Password);
                    try
                        {
                        ftpClient.OpenWrite($"ftp://{FTP_URL.Text}/connection.con");
                        ftpClient.Dispose();
                        }
                    catch (Exception)
                        {
                        Uploadtoftp_check.IsChecked = false;
                        MessageBox.Show("Failed to login, check your credentials", "PUBG Live stats", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    ftpClient.Dispose();
                    }
                else
                    {
                    MessageBox.Show("Failed to open connection, no URL was specified!", "PUBG Live stats", MessageBoxButton.OK, MessageBoxImage.Error);
                    Uploadtoftp_check.IsChecked = false;
                    }
                }
            Program_state.Content = "Live tracker (ON)";
            runScanner = true;
            }

        /// <summary>
        /// State OFF event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Program_state_Unchecked(object sender, RoutedEventArgs e)
            {
            //Change to a darker color
            Program_state.Foreground = new SolidColorBrush(Colors.OrangeRed);

            //Unlock output path
            Output_path.Visibility = Visibility.Visible;
            Output_path_text.Text = Output_path_text_string;

            Program_state.Content = "Live tracker (OFF)";
            runScanner = false;
            }

        /// <summary>
        /// FTP Checked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uploadtoftp_check_Checked(object sender, RoutedEventArgs e)
            {
            //Check if program_state is checked
            if (Program_state.IsChecked == true)
                {
                if (MessageBox.Show("You can't have the stattracker running while configuring your FTP, press OK to end the tracker now or press Cancel to keep it running (without FTP)", "PUBG Live stats", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                    Uploadtoftp_check.IsChecked = false;
                    return;
                    }
                Program_state.IsChecked = false;
                }

            //Show FTP fields
            FTP_User_text.Visibility = Visibility.Visible;
            FTP_User.Visibility = Visibility.Visible;

            FTP_Password_text.Visibility = Visibility.Visible;
            FTP_Passwordbox.Visibility = Visibility.Visible;

            FTP_URL_text.Visibility = Visibility.Visible;
            FTP_URL.Visibility = Visibility.Visible;

            //Change to a brighter color
            Uploadtoftp_check.Foreground = new SolidColorBrush(Colors.Orange);

            return;
            }

        /// <summary>
        /// FTP Unchecked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uploadtoftp_check_Unchecked(object sender, RoutedEventArgs e)
            {
            //Hide FTP fields//Show FTP fields
            FTP_User_text.Visibility = Visibility.Hidden;
            FTP_User.Visibility = Visibility.Hidden;

            FTP_Password_text.Visibility = Visibility.Hidden;
            FTP_Passwordbox.Visibility = Visibility.Hidden;

            FTP_URL_text.Visibility = Visibility.Hidden;
            FTP_URL.Visibility = Visibility.Hidden;

            //Change to a darker color
            Uploadtoftp_check.Foreground = new SolidColorBrush(Colors.OrangeRed);
            }

        /// <summary>
        /// Discord button onClick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Discord_Button_Click(object sender, RoutedEventArgs e)
            {
            //Launch browser to join Discord
            LaunchWeb(@"https://discord.gg/Cddu5aJ");
            }

        /// <summary>
        /// Player lookup click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerLookupClick(object sender, RoutedEventArgs e)
            {
            PlayerLookup();
            }

        /// <summary>
        /// Player lookup keyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerLookupKeyDown(object sender, KeyEventArgs e)
            {
            if (e.Key == Key.Return)
                {
                PlayerLookup();
                }
            }


        //Functions

        /// <summary>
        /// Launch a web URL on Windows, Linux and OSX
        /// </summary>
        /// <param name="url">The URL to open in the standard browser</param>
        public void LaunchWeb(string url)
            {
            try
                {
                Process.Start(url);
                }
            catch
                {
                //Hack for running the above line in DOT net Core...
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                    }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                    Process.Start("xdg-open", url);
                    }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                    Process.Start("open", url);
                    }
                else
                    {
                    throw;
                    }
                }
            }

        /// <summary>
        /// PlayerLookup
        /// </summary>
        private void PlayerLookup()
            {
            //If playerID is not empty, launch
            if (!(PlayerID.Text.Equals("")))
                {
                //string url = $@"https://pubg-stats.com/profile/{PlayerID.Text}";
                string url = $@"https://pubglookup.com/players/steam/{PlayerID.Text}";
                LaunchWeb(url);
                }
            else
                {
                //Give error message if playerID is empty
                MessageBox.Show("You need to enter a playername above", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        /// <summary>
        /// Uploads to any FTP server
        /// </summary>
        /// <param name="server">Your FTP server url, include if any specific folder on your server, eg. ftp.stackcp.com/data</param>
        /// <param name="username">Your FTP username</param>
        /// <param name="password">Your FTP password</param>
        /// <param name="fileDir">The files location on YOUR pc, without the file!</param>
        /// <param name="file">the filename and extension</param>
        public static void UploadToFTP(string server, string username, string password, string fileDir, string file)
            {

            WebClient client = new WebClient();

            try
                {
                client.Credentials = new NetworkCredential(username, password);
                client.UploadFile($"ftp://{server}/{file}", $@"{fileDir}\{file}");
                }
            catch (WebException)
                { }
            }

        /// <summary>
        /// Main loop for running the scanner
        /// </summary>
        public static async void MainScanner()
            {
            System.Collections.Generic.List<string> readOuts = new System.Collections.Generic.List<string>(); //Initialize a new List string, adding all of the OCR readouts to it
            await Task.Delay(500);
            while (!runScanner)
                {
                Console.WriteLine("OFF");
                await Task.Delay(500);

                while (runScanner)
                    {
                    Console.WriteLine("ON");
                    ImageProcessor img = new ImageProcessor();
                    OcrResult result = img.ReadOCR(img.GrayscaleImage(img.CaptureScreen()));
                    result.SaveAsTextFile($@"{OutputPath}\ocr.txt");
                    readOuts.Add(result.Text);
                    await Task.Delay(500);
                    if (!runScanner)
                        {
                        StreamWriter writer = new StreamWriter($@"{OutputPath}\end.txt");
                        for (int i = 0; i < readOuts.Count; i++)
                            {
                            writer.WriteLine(readOuts[i]);
                            }
                        readOuts.Clear();
                        writer.Close();
                        }
                    }

                }
            }

        }
    }