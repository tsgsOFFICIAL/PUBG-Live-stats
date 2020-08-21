using System;
using System.IO;
using System.Collections.Generic;

namespace PUBG_Live_stats__Framework_
    {
    public class TextProcessor
        {
        private static List<string> ReadOuts { get; set; } = new List<string>();

        /// <summary>
        /// Check the string for if it contains the required words, and if it does, check what it says
        /// </summary>
        /// <param name="_string">OCR Scanned text</param>
        public static void CheckString(string _string)
            {
            if (_string.ToLower().Contains("you") && _string.ToLower().Contains("killed") || _string.ToLower().Contains(@"k\\\ed") && _string.ToLower().Contains(@"vou") || _string.ToLower().Contains(@"k\\\ed") && _string.ToLower().Contains(@"you") || _string.ToLower().Contains("killed") && _string.ToLower().Contains(@"vou"))
                {
                if (_string.ToLower().StartsWith("you") || _string.ToLower().StartsWith("vou") || _string.ToLower().IndexOf("you") < 7 || _string.ToLower().IndexOf("vou") < 7)
                    {
                    Statistic.AddAKill();
                    }
                else
                    {
                    Statistic.AddADeath();
                    }
                //ReadOuts.Add(_string);
                }
            ReadOuts.Add(_string);
            }

        /// <summary>
        /// Output everything to a file when the program is stopped
        /// </summary>
        /// <param name="_outputPath"></param>
        public static void OutputToFile(string _outputPath)
            {
            StreamWriter writer = new StreamWriter($@"{_outputPath}\end.txt");
            for (int i = 0; i < ReadOuts.Count; i++)
                {
                writer.WriteLine(i.ToString() + " " + ReadOuts[i]);
                }
            ReadOuts.Clear();
            writer.Close();
            }

        }
    }
