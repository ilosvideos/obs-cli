using NReco.VideoConverter;
using obs_cli.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace obs_cli.Objects
{
	public class VideoMerge
	{
		public string VideoFileName { get; set; }

		public List<FileInfo> InputFiles { get; set; }
		public FileInfo OutputFile { get; set; }
		public FileInfo TempFileList { get; set; }

		public VideoMerge(List<FileInfo> inputFiles)
		{
			this.InputFiles = inputFiles;
		}

		/// <summary>
		/// Combines all of the InputFiles and writes it to disk.
		/// </summary>
		public void CombineAndWrite()
		{
			try
			{
				string fileOutputPath = Path.Combine(InputFiles.First().DirectoryName, Store.Data.Record.LastVideoName + ".mp4");

				if (InputFiles.Count == 1)
				{
					// Remove _part 1 from the file name, but don't concatenate
					InputFiles.First().MoveTo(fileOutputPath);
					OutputFile = new FileInfo(fileOutputPath);

					return;
				}

				TempFileList = new FileInfo(Path.GetTempFileName());
				using (var sw = new StreamWriter(TempFileList.FullName))
				{
					foreach (FileInfo file in InputFiles)
					{
						sw.WriteLine($"file '{file.FullName}'");
					}
				}

				FFMpegConverter ffMpegConverter = new FFMpegConverter();

				OutputFile = new FileInfo(fileOutputPath);

				string ffmpegArgs = $"-f concat -safe 0 -i \"{TempFileList.FullName}\" -movflags +faststart -c copy \"{OutputFile.FullName}\"";
				ffMpegConverter.Invoke(ffmpegArgs);

				DeleteInputFiles();
				DeleteTemporaryFiles();
			}
			catch
			{
				DeleteTemporaryFiles();
			}
		}

		/// <summary>
		/// Deletes input files after the final resulting file has been written.
		/// </summary>
		/// <returns></returns>
		private bool DeleteInputFiles()
		{
			foreach (FileInfo file in InputFiles)
			{
				file.Delete();
			}

			return true;
		}

		/// <summary>
		/// Deletes the temporary files.
		/// </summary>
		private void DeleteTemporaryFiles()
		{
			TempFileList?.Delete();
		}
	}
}
