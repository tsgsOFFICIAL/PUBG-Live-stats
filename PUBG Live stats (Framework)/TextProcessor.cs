using System;
using System.IO;
using System.Collections.Generic;

namespace PUBG_Live_stats__Framework_
    {
    public class TextProcessor
        {
        private static List<string> ReadOuts { get; set; }

        /// <summary>
        /// Check the string for if it contains the required words, and if it does, check what it says
        /// </summary>
        /// <param name="_string">OCR Scanned text</param>
        public static void CheckString(string _string)
            {
            if (_string.ToLower().Contains("you") && _string.ToLower().Contains("killed"))
                {
                if (_string.ToLower().StartsWith("you"))
                    {
                    Statistic.AddAKill();
                    }
                else
                    {
                    Statistic.AddADeath();
                    }
                ReadOuts.Add(_string);
                }
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
                writer.WriteLine(ReadOuts[i]);
                }
            ReadOuts.Clear();
            writer.Close();
            }

        }
    }
