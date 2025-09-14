using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;

namespace APP
{
    class EasyDownload
    {
        static void Main(string[] args)
        {
            // welcome screen
            WelcomeScreen();

            Console.Write("Enter the YouTube URL: ");
            string url = Console.ReadLine();

            Console.Write("Do you want (1) Video or (2) Audio? ");
            string choice = Console.ReadLine();

            if (choice == "2")
            {
                // Audio mode
                Console.Write("Which audio format do you want (mp3/m4a/wav/opus)? ");
                string audioFormat = Console.ReadLine().Trim().ToLower();
                DownloadAudio(url, audioFormat);
            }
            else if (choice == "1")
            {
                // Video mode
                Console.WriteLine("\nFetching available video resolutions\n");
                var resolutions = GetAvailableMP4Resolutions(url);

                if (resolutions.Count == 0)
                {
                    Console.WriteLine("No downloadable MP4 video formats found!");
                    return;
                }

                Console.WriteLine("Available video resolutions:");
                for (int i = 0; i < resolutions.Count; i++)
                    Console.WriteLine($"{i + 1}) {resolutions[i]}p");

                Console.Write("\nEnter choice number: ");
                if (!int.TryParse(Console.ReadLine(), out int choiceNum) || choiceNum < 1 || choiceNum > resolutions.Count)
                    choiceNum = resolutions.Count; // default to highest

                string selectedRes = resolutions[choiceNum - 1].ToString();
                Console.WriteLine($"\nDownloading {selectedRes}p video...\n");

                // Download best MP4 video+audio at chosen resolution
                DownloadVideo(url, $"bestvideo[ext=mp4][height<={selectedRes}]+bestaudio[ext=m4a]/best[ext=mp4]");
            }
            else
            {
                Console.WriteLine("Invalid choice. Exiting...");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nDownload finished!");
            Console.ResetColor();
        }

        static void WelcomeScreen()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            PrintWithDelay("############## Welcome to EasyDownload ##############", 30);
            Console.Write("download");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" YouTube ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("videos easily and without struggling");
            Console.WriteLine();
            Console.ResetColor();
        }

        static void PrintWithDelay(string text, int delayMS)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMS);
            }
            Console.WriteLine();
        }

        static List<int> GetAvailableMP4Resolutions(string url)
        {
            List<int> resolutions = new List<int>();

            Process process = new Process();
            process.StartInfo.FileName = "yt-dlp";
            process.StartInfo.Arguments = $"-F {url}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Match lines that contain MP4 video formats (ignore webm)
            var regex = new Regex(@"(?<id>\d+)\s+mp4\s+(?<res>\d+)x\d+");
            var matches = regex.Matches(output);

            HashSet<int> resSet = new HashSet<int>();
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups["res"].Value, out int h))
                    resSet.Add(h);
            }

            List<int> sortedRes = new List<int>(resSet);
            sortedRes.Sort();
            return sortedRes;
        }

        static void DownloadVideo(string url, string formatCode)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDownloading video...\n");
            Console.ResetColor();

            RunYtDlp($"-f {formatCode} {url}");
        }

        static void DownloadAudio(string url, string audioFormat)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nDownloading audio ({audioFormat})...\n");
            Console.ResetColor();

            RunYtDlp($"-x --audio-format {audioFormat} {url}");
        }

        static void RunYtDlp(string args)
        {
            Process process = new Process();
            process.StartInfo.FileName = "yt-dlp";
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    if (e.Data.Contains("%"))
                    {
                        int percentIndex = e.Data.IndexOf('%');
                        int start = e.Data.LastIndexOf(' ', percentIndex) + 1;
                        if (start > 0 && percentIndex > start)
                        {
                            string percent = e.Data.Substring(start, percentIndex - start + 1).Trim();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"\rDownloading... {percent}   ");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.WriteLine(e.Data);
                    }
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data) && !e.Data.Contains("%"))
                    Console.WriteLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            Console.WriteLine();
        }
    }
}