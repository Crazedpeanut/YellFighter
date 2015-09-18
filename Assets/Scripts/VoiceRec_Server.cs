using System.Threading;
using System.Collections;
using System.IO;
using System.Diagnostics;

public class VoiceRec_Server {
	public static string server = "127.0.0.1";
	public static int port = 26000;
	public static string fileLocation = @".\Assets\RecoServer\VoiceRec2.exe";
	static Thread serverThread;
	static Process serverProcess;

	public static string StartServer()
	{
		serverThread = new Thread (new ThreadStart (ServerThread));
		serverThread.Start ();

		return Directory.GetCurrentDirectory ();
	}

	public static void QuitServer()
	{
		serverProcess.Kill ();
	}


	static void ServerThread()
	{
		serverProcess = new Process ();
		serverProcess.StartInfo.FileName = fileLocation;
		serverProcess.StartInfo.UseShellExecute = true;
		//serverProcess.StartInfo.RedirectStandardOutput = true;
		//serverProcess.StartInfo.RedirectStandardError = true;
		//serverProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
		//serverProcess.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
		serverProcess.Start();
		//serverProcess.BeginOutputReadLine();
		//serverProcess.BeginErrorReadLine();
		serverProcess.WaitForExit();
	}

	static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) {
		//* Do your stuff with the output (write to console/log/StringBuilder)
		UDP_RecoServer.AddMessageToPrint(outLine.Data);
	}


}
