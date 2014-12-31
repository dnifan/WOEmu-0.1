using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WO.Core.Logger
{
    public static class Logger
    {
        /// <summary>
        /// Initializes the Logger module.
        /// </summary>
        /// <param name="filename">The filename to log to.</param>
        public static void InitLogger(string filename)
        {
            fs = new FileStream(filename, FileMode.Append);
            writer = new StreamWriter(fs);

            DateTime now = DateTime.Now;

            writer.WriteLine("----------------");
            writer.WriteLine("Session started on: " + now.ToShortDateString() + ", " + now.ToShortTimeString());
            writer.WriteLine("");
            writer.Flush();
        }

        /// <summary>
        /// Appends a line to the logfile.
        /// </summary>
        /// <param name="txt">The text to append.</param>
        public static void AppendLine(string txt)
        {
            writer.WriteLine(txt);
            writer.Flush();
        }

        /// <summary>
        /// Prints the message on-screen, and in the logfile.
        /// </summary>
        /// <param name="text">The text to log.</param>
        public static void printInfo(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            System.Console.Write("[Info]: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine(text);

            AppendLine(text);
        }

        /// <summary>
        /// Prints a debug message on-screen.
        /// </summary>
        /// <param name="text">The text to log.</param>
        public static void printDebug(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.Write("[Debug]: ");
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(text);
        }

        /// <summary>
        /// Prints a warning message on-screen, and in the logfile.
        /// </summary>
        /// <param name="text"></param>
        public static void printWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write("[Warning]: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(text);

            AppendLine("\tWarning: " + text);
        }

        private static FileStream fs;
        private static StreamWriter writer;
    }
}
