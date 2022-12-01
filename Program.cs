using System;
using System.Diagnostics;

class Program {
	public static void Main(string[] args) {
		// Goes to the Users directory
		string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		Directory.SetCurrentDirectory(userDirectory);
		
		// A welcome text is shown
		Console.WriteLine("Quick Sharper - Creating a new .NET project");
		Console.WriteLine("-------------------------------------------");
		Console.WriteLine("     You can cancel by pressing Ctrl+C     ");
		Console.WriteLine("    For more information, invoke with -h   ");
		Console.WriteLine("-------------------------------------------\n");		
		
		// Asks the user where the project file should go, defaulting to Documents\GitHub.
		Console.WriteLine(@"Where you want to create the project? (ENTER: Documents\GitHub)");
		string filePath = Console.ReadLine() ?? "";
		
		// If the user leaves the answer in blank, load the default values.
		if (String.IsNullOrWhiteSpace(filePath)) {
			filePath = @"Documents\GitHub";
		}
		
		// Checks if the Path is valid, if not, create folders.
		try {
			Directory.SetCurrentDirectory(filePath);
		}
		catch (DirectoryNotFoundException dirEx) {
			// Let the user know that the directory did not exist.
			Console.WriteLine("Directory not found: " + dirEx.Message);
			
			// Tries to create it
			try {
				Directory.CreateDirectory(filePath);
				Directory.SetCurrentDirectory(filePath);
			}
			catch (UnauthorizedAccessException unaEx) {
				// The program is unauthorized to create files in this Path
				Console.WriteLine("Unauthorized Access: " + unaEx);
			}
		}
		
		// Asks what type of .NET template application the user wants, defaulting to console
		Console.WriteLine("What type of .NET application you want to create? (ENTER: console)");
		string appType = Console.ReadLine() ?? "";
		
		// If the user leaves the answer in blank, load the default values.
		if (String.IsNullOrWhiteSpace(appType)) {
			appType = "console";
		}
		
		// Asks for the projects name
		Console.Write("Project name: ");
		string projectName = Console.ReadLine() ?? "";
		
		// Create and enter the project's directory
		Directory.CreateDirectory(projectName);
		Directory.SetCurrentDirectory(projectName);
		
		// Invoke 'dotnet new' with the specified application template
		Console.WriteLine(
			$"Running \"dotnet new {appType}... " +
			"A new terminal window should popup. ");
		Process dotnetProcess = Process.Start("CMD.exe",$"/C dotnet new {appType}");
		dotnetProcess.WaitForExit(1000 * 60 * 2); // Wait up to two minutes
		
		// Rename the Program.cs file to projectName.cs
		System.IO.File.Move("Program.cs", projectName + ".cs");
		
		// Open the file in the user's preferred .cs text editor
		Process editorProcess = new Process();
		editorProcess.StartInfo.FileName = projectName + ".cs";
		editorProcess.StartInfo.UseShellExecute = true;
		editorProcess.Start();
	}
}