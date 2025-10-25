using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("############## Welcome to EasyDownload ##############");
        Console.WriteLine("Download YouTube videos easily and without struggling\n");

        Console.Write("Enter the YouTube URL: ");
        string url = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Enter the download directory (e.g., C:\\Users\\Name\\Videos or ~/Downloads): ");
        string directory = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Optional: Enter browser to use for cookies (chrome, firefox, edge) or press Enter to skip: ");
        string browser = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Do you want (1) Video or (2) Audio? ");
        string choice = Console.ReadLine()?.Trim() ?? "1";

        string type = choice == "2" ? "Audio" : "Video";
        string format;

        if (type == "Video")
        {
            Console.WriteLine("\nChoose preferred resolution:");
            Console.WriteLine("1. 144p\n2. 240p\n3. 360p\n4. 480p\n5. 720p\n6. 1080p\n7. Best Available");
            Console.Write("Enter choice (1-7): ");
            string resChoice = Console.ReadLine()?.Trim() ?? "7";

            string resolution = resChoice switch
            {
                "1" => "144",
                "2" => "240",
                "3" => "360",
                "4" => "480",
                "5" => "720",
                "6" => "1080",
                _ => "9999"
            };

            format = resolution == "9999"
                ? "bestvideo+bestaudio/best"
                : $"bestvideo[height<={resolution}]+bestaudio/best[height<={resolution}]";
        }
        else
        {
            format = "bestaudio";
        }

        string cookiesOption = string.IsNullOrEmpty(browser) ? "" : $"--cookies-from-browser {browser}";
        Directory.CreateDirectory(directory);

        Console.WriteLine($"\nDownloading {type.ToLower()} at selected quality...\n");

        string arguments =
            $"{cookiesOption} -f \"{format}\" -o \"{directory}/%(title)s.%(ext)s\" \"{url}\" " +
            "--merge-output-format mp4 --no-abort-on-error --newline --ignore-errors";

        var psi = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process { StartInfo = psi };
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                Console.WriteLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                Console.WriteLine(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        Console.WriteLine("\n✅ Download finished!");
    }

    static string ExtractTitle(string line)
    {
        var match = Regex.Match(line, @"Destination:\s*(.*)\.\w+$");
        if (match.Success)
            return Path.GetFileNameWithoutExtension(match.Groups[1].Value);
        return "";
    }
}
