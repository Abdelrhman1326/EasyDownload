# EasyDownload
YouTube video/audio downloader

Before anything, please make sure .NET is installed on your pc.
This .NET app uses extra external libraries, so you need to install them before running.

---

## 1. Install FFmpeg

1. Download the full FFmpeg build package:  
   [ffmpeg-2025-09-08-git-45db6945e9-full_build.7z](https://www.gyan.dev/ffmpeg/builds/packages/ffmpeg-2025-09-08-git-45db6945e9-full_build.7z)

2. Extract the archive.

3. Copy the folder inside the extracted archive (it will have a name like `ffmpeg-2025-09-08-git-xxxx-full_build`) and paste it into:  `C:\Program Files`


4. Go inside the folder you just pasted.  
You will find a subfolder named `bin`.

5. Copy the full path of the `bin` folder.  
Example:  `C:\Program Files\ffmpeg-2025-09-10-git-c1dc2e2b7c-full_build\bin`

6. Add it to Windows Environment Variables in Path

---

## 2. Install yt-dlp

1. Download the executable "named: yt-dlp.exe" from the official GitHub release page:  
[yt-dlp.exe](https://github.com/yt-dlp/yt-dlp/releases/tag/2025.09.05)

2. Create a new folder named: yt-dlp

3. Move the downloaded `yt-dlp.exe` file into the `yt-dlp` folder.

4. Copy the `yt-dlp` folder into: C:\Program Files

---

## 3. Run the Application

### Download the full repo or clone it to your local pc storage

- **From Visual Studio 2022:**  
Open the solution in Visual Studio and run it directly.

- **From the executable file:**  
Navigate to: EasyDownload\EasyDownload\bin\Debug\net8.0\EasyDownload.exe
and run the `.exe` file.


## Note: you will find the downloaded videos and audio files in the same directory of EasyDownload.exe, in: EasyDownload\EasyDownload\bin\Debug\net8.0\
