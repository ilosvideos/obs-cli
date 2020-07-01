using NReco.VideoConverter;
using obs_cli.Data;
using System;
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
		/// <returns>The final video's output path.</returns>
		public VideoMergeOutput CombineAndWrite()
		{
			var output = new VideoMergeOutput();

			try
			{
				output.FileOutputPath = Path.Combine(InputFiles.First().DirectoryName, Store.Data.Record.LastVideoName + ".mp4");

				if (InputFiles.Count == 1)
				{
					// Remove _part 1 from the file name, but don't concatenate
					InputFiles.First().MoveTo(output.FileOutputPath);
					OutputFile = new FileInfo(output.FileOutputPath);

					return output;
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

				OutputFile = new FileInfo(output.FileOutputPath);

				string ffmpegArgs = $"-f concat -safe 0 -i \"{TempFileList.FullName}\" -movflags +faststart -c copy \"{OutputFile.FullName}\"";
				ffMpegConverter.Invoke(ffmpegArgs);

				DeleteInputFiles();
				DeleteTemporaryFiles();
			}
			catch (Exception ex)
			{
				DeleteTemporaryFiles();
				output.IsSuccessful = false;
				output.MergeFailureReason = ex.Message;
			}

			return output;
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
