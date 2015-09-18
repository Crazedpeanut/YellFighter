// *********************************************************
    // UDP SPEECH RECOGNITION
    // *********************************************************
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Net;
    using System.Text;
    using System.Net.Sockets;
    using System.Threading;
	using System.Collections.Generic;
 
    public class UDP_RecoServer : MonoBehaviour
    {
       Thread receiveThread;
       UdpClient client;
       public int port = 26000;
       string strReceiveUDP = "";
       string LocalIP = String.Empty;
       string hostname;
		public static List<string> messageList = new List<string>();
	
		public static void AddMessageToPrint(string message)
		{
			messageList.Add (message);
		}

	// Update is called once per frame
	void Update () {
		if (messageList.Count == 0) {
			foreach (string message in messageList)
			{
				Debug.Log(message);
			}

			messageList.Clear();
		}
	}

 
       public void StartServer()
       {
		Debug.Log ("Start Server: " + (Globals.inputType == Globals.InputTypes.VOICE) + " " + Globals.isServer);

			if (Globals.inputType == Globals.InputTypes.VOICE && !Globals.isServer) {
			SetUpServer();
		}

		if (Globals.inputType == Globals.InputTypes.VOICE && !Globals.isServer) {

				Application.runInBackground = true;
				init(); 
			} 
       }


	void SetUpServer()
	{
		Debug.Log("Current Loc: " + VoiceRec_Server.StartServer ());
	}

       // init
       private void init()
       {
          receiveThread = new Thread( new ThreadStart(ReceiveData));
          receiveThread.IsBackground = true;
          receiveThread.Start();
          hostname = Dns.GetHostName();
          IPAddress[] ips = Dns.GetHostAddresses(hostname);
          if (ips.Length > 0)
          {
             LocalIP = ips[0].ToString();
             Debug.Log(" MY IP : "+LocalIP);
          }
       }
 
       private  void ReceiveData()
       {
          client = new UdpClient(port);
		Debug.Log ("About to connect to the server on port: " + port);
          while (true)
          {
             try
             {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Broadcast, port);
                byte[] data = client.Receive(ref anyIP);
                strReceiveUDP = Encoding.UTF8.GetString(data);
                // ***********************************************************************
                // Simple Debug. Must be replaced with SendMessage for example.
                // ***********************************************************************
                Debug.Log(strReceiveUDP);
				Player.AddMessage(strReceiveUDP);
                // ***********************************************************************
             }
             catch (Exception err)
             {
                print(err.ToString());
             }
          }
       }
 
       public string UDPGetPacket()
       {
          return strReceiveUDP;
       }
 
       void OnDisable()
       {
			if (Globals.inputType == Globals.InputTypes.VOICE) {
				if ( receiveThread != null) receiveThread.Abort();
				client.Close();

				}
		VoiceRec_Server.QuitServer();
       }
    }
    // *********************************************************
