using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace obs_cli.Objects
{
	public class UploadQueueItem
	{
		public Guid UUID;
		public bool cancelRequested;

		public string title;

		public string videoFileName;
		public List<FileInfo> inputFiles = new List<FileInfo>();
		public FileInfo uploadFile;
		public FileInfo outputFile;
		public FileInfo tmpListFile;

		public Int64 inputFileSizeTotal = 0;
		public Int64 videoDuration = 0;

		public bool obsIsDoneWithOutputFile = false;
		public bool hasAutomaticallyOpenedInBrowser = false;
		public bool mustDoVideoUpdate = false;
		public bool deleteWhenFinishedProcessing = false;
		public bool userHasSubmittedTitle = false;
		public bool shouldUpdateTitleAfterReserving = false;
		public bool deleteFilesOnCancel = true;

		public UploadQueueItem()
		{
			this.UUID = Guid.NewGuid();
		}

		public void combineInputFiles()
		{
			try
			{
				string fileOutputPath = Path.Combine(inputFiles.First().DirectoryName, videoFileName + ".mp4");

				if (inputFiles.Count == 1)
				{
					Console.WriteLine("Only 1 input file. Renaming File.");

					// Remove _part 1 from the file name, but don't concatenate
					inputFiles.First().MoveTo(fileOutputPath);
					outputFile = new FileInfo(fileOutputPath);
					uploadFile = outputFile;

					return;
				}

				Console.WriteLine("Concatenating " + inputFiles.Count.ToString() + " files");

				tmpListFile = new FileInfo(Path.GetTempFileName());
				using (var sw = new StreamWriter(tmpListFile.FullName))
				{
					foreach (FileInfo file in inputFiles)
					{
						sw.WriteLine($"file '{file.FullName}'");
					}
				}

				FFMpegConverter ffMpegConverter = new FFMpegConverter();

				outputFile = new FileInfo(fileOutputPath);

				string ffmpegArgs = $"-f concat -safe 0 -i \"{tmpListFile.FullName}\" -movflags +faststart -c copy \"{outputFile.FullName}\"";
				ffMpegConverter.Invoke(ffmpegArgs);

				uploadFile = outputFile;

				deleteInputFiles();
				deleteTmpFile();
			}
			catch (Exception ex)
			{
				deleteTmpFile();
			}
		}

		public bool deleteInputFiles()
		{
			foreach (FileInfo file in inputFiles)
			{
				file.Delete();
			}

			return true;
		}

		public void deleteTmpFile()
		{
			tmpListFile?.Delete();
		}
	}
}
