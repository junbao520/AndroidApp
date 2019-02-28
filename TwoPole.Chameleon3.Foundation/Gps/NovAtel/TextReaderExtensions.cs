using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TwoPole.Chameleon3.Foundation.Gps
{
    internal static class TextReaderExtensions
    {
        private static string GetCommand(this string line)
        {
            if (line == null)
                return null;

            var index = line.IndexOf(",", StringComparison.Ordinal);
            if (index < 0)
                return null;

            var cmd = line.Substring(0, index);
            return cmd;
        }

        public static string[] ReadLines(this TextReader reader, int rows)
        {
            string[] lines = new string[rows];
            var index = 0;
            while (index < rows)
            {
                lines[index] = reader.ReadLine();
                index++;
            }
            return lines;
        }

        public static string[] ReadToCommand(this TextReader reader, string targetCmd)
        {
            var lines = new List<string>();
            while (true)
            {
                var line = reader.ReadLine();
                lines.Add(line);
                var cmd = line.GetCommand();
                if (cmd != null && cmd.Equals(targetCmd, StringComparison.OrdinalIgnoreCase))
                    break;
            }
            return lines.ToArray();
        }

        public static int TestCommandRows(this StreamReader reader)
        {
            reader.ReadLine();
            var rows = 0;
            string firstCmd = string.Empty;
            var queues = new Queue<Tuple<string, long>>(5);
            var sw = new Stopwatch();
            while (true)
            {
                sw.Reset();
                sw.Start();
                var line = reader.ReadLine();
                sw.Stop();
                var cmd = line.Substring(0, line.IndexOf(",", StringComparison.Ordinal));
                queues.Enqueue(new Tuple<string, long>(cmd, sw.ElapsedMilliseconds));
                if (rows == 0)
                {
                    firstCmd = cmd;
                }
                else if (firstCmd.Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                rows++;
            }

            var lastCmd = queues.GetLastCommand();
            SkipOverCommand(reader, lastCmd);

            return rows;
        }

        public static string GetLastCommand(this StreamReader reader)
        {
            reader.ReadLine();
            reader.ReadLine();
            var rows = 0;
            string firstCmd = string.Empty;
            var queues = new Queue<Tuple<string, long>>(5);
            var sw = new Stopwatch();
            while (true)
            {
                sw.Reset();
                sw.Start();
                var line = reader.ReadLine();
                sw.Stop();

                var cmd = line.Substring(0, line.IndexOf(",", StringComparison.Ordinal));
                if (rows == 0)
                {
                    firstCmd = cmd;
                }
                else if (firstCmd.Equals(cmd, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                queues.Enqueue(new Tuple<string, long>(cmd, sw.ElapsedMilliseconds));
                rows++;
            }

            var lastCmd = GetLastCommand(queues);
            return lastCmd;
        }

        private static string GetLastCommand(this Queue<Tuple<string, long>> queues)
        {
            Tuple<string, long> max = queues.First();
            int maxCommandIndex = 0;
            int index = 0;
            foreach (var queue in queues)
            {
                if (queue.Item2 > max.Item2)
                {
                    max = queue;
                    maxCommandIndex = index;
                }
                index++;
            }
            if (maxCommandIndex == 0)
                return queues.Last().Item1;
            else
                return queues.Skip(maxCommandIndex - 1).First().Item1;
        }

        public static void SkipOverCommand(this StreamReader reader, string skipCmd)
        {
            while (true)
            {
                var line = reader.ReadLine();
                var cmd = line.Substring(0, line.IndexOf(",", StringComparison.Ordinal));
                if (string.Equals(skipCmd, cmd, StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }
    }
}
