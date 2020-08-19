using System;
using System.IO;

namespace PUBG_Live_stats__Framework_
    {
    public class Statistic
        {
        private static int Kills { get; set; }
        private static int Deaths { get; set; }
        private static int Wins { get; set; }
        private static int Losses { get; set; }
        private static double KD { get; set; }
        private static double WR { get; set; }

        /// <summary>
        /// Add a kill to your total kills, and calculate your KD
        /// </summary>
        public static void AddAKill()
            {
            Kills++;
            CalculateKD();
            try
                {
                File.WriteAllText($@"{MainWindow.OutputPath}\kills.txt", Kills.ToString());
                }
            catch (Exception) { }
            }

        /// <summary>
        /// Add a death to your total deaths, and calculate your KD
        /// </summary>
        public static void AddADeath()
            {
            Deaths++;
            CalculateKD();
            try
                {
                File.WriteAllText($@"{MainWindow.OutputPath}\deaths.txt", Deaths.ToString());
                }
            catch (Exception) { }
            }

        /// <summary>
        /// Calculate your kill per death ratio
        /// </summary>
        public static void CalculateKD()
            {
            if (Deaths == 0)
                {
                KD = Convert.ToDouble(Kills) / Convert.ToDouble(1);
                }
            else
                {
                KD = Convert.ToDouble(Kills) / Convert.ToDouble(Deaths);
                }
            try
                {
                File.WriteAllText($@"{MainWindow.OutputPath}\kd.txt", KD.ToString());
                }
            catch (Exception) { }
            }

        /// <summary>
        /// Add a win to your total wins, and calculate your WR
        /// </summary>
        public static void AddAWin()
            {
            Wins++;
            CalculateWR();
            try
                {
                File.WriteAllText($@"{MainWindow.OutputPath}\wins.txt", Wins.ToString());
                }
            catch (Exception) { }
            }

        /// <summary>
        /// Add a loss to your total losses, and calculate your WR
        /// </summary>
        public static void AddALoss()
            {
            Losses++;
            CalculateWR();
            try
                {
                File.WriteAllText($@"{MainWindow.OutputPath}\losses.txt", Losses.ToString());
                }
            catch (Exception) { }
            }

        /// <summary>
        /// Calculate your winrate (games played / wins)
        /// </summary>
        public static void CalculateWR()
            {
            if (Wins == 0)
                {
                WR = 0;
                }
            else
                {
                WR = (Convert.ToDouble(Wins) / Convert.ToDouble(Losses + Wins)) * 100;
                }
            try
                {
                File.WriteAllText($@"{MainWindow.OutputPath}\wr.txt", WR.ToString());
                }
            catch (Exception) { }
            }


        }
    }
