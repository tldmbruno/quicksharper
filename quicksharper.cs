using System;
using System.Diagnostics;

class Program {
	public static void Main(string[] args) {
		// Remembers which folder the binary is in
		string binPath = Directory.GetCurrentDirectory();
		
		// Get default project folder path in quicksharper.conf
		string confPath = binPath + @"\quicksharper.conf"; // The config file location
		string lastPath = File.Exists(confPath) ?
			File.ReadAllText(confPath) // If the file exists, get the default path.
			: @"Documents\GitHub"; // If there isn't any, default to GitHub's folder.
		
		// Goes to the Users directory
		string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		Directory.SetCurrentDirectory(userDirectory);
		
		// A welcome text is shown
		Console.WriteLine("-------------------------------------------");
		Console.WriteLine("Quick Sharper - Creating a new .NET project");
		Console.WriteLine("    (You can cancel by pressing Ctrl+C)    ");
		Console.WriteLine("-------------------------------------------\n");		
		
		// Asks the user where the project file should go, defaulting to Documents\GitHub.
		Console.WriteLine($"Where you want to create the project? (ENTER: {lastPath})");
		string filePath = Console.ReadLine() ?? "";
		
		// If the user leaves the answer in blank, load the default values.
		if (String.IsNullOrWhiteSpace(filePath)) {			
			filePath = lastPath;
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
		
		string projectName = "";
		
		while (String.IsNullOrWhiteSpace(projectName)) {
			// Asks for the projects name
			Console.WriteLine("What should this project's name be? (ENTER: Project)");
			string input = Console.ReadLine() ?? "";
			
			// If the user leaves the answer in blank, load the default values.
			if (String.IsNullOrWhiteSpace(projectName)) {
				projectName = "Project";
			}
			
			// Checks if there's a project with the same name
			if(Directory.Exists(projectName)) {
				// Notifies the user about it
				Console.WriteLine($"There is already a project with the name \"{projectName}\".");
				
				// Erases the variable's value, keeping the loop going
				projectName = "";
			}
		}		
		
		// Create and enter the project's directory
		Directory.CreateDirectory(projectName);
		Directory.SetCurrentDirectory(projectName);
		
		// Invoke 'dotnet new' with the specified application template
		Console.WriteLine($"Running \"dotnet new {appType}... ");
		Process dotnetProcess = Process.Start("CMD.exe",$"/C dotnet new {appType}");
		dotnetProcess.WaitForExit(1000 * 60 * 2); // Wait up to two minutes for timeout
		
		// Rename the Program.cs file to projectName.cs
		System.IO.File.Move("Program.cs", projectName + ".cs");
		
		// Open the file in the user's preferred .cs text editor
		Process editorProcess = new Process();
		editorProcess.StartInfo.FileName = projectName + ".cs";
		editorProcess.StartInfo.UseShellExecute = true;
		editorProcess.Start();
		
		// Celebrate!
		Console.WriteLine($"Project \"{projectName}\"succesfully created!");
		
		// Register the path used for this project, becoming the default for any future projects.
		File.WriteAllText(binPath + @"\quicksharper.conf", filePath);
	}
}