using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	private static int numPlayers = 0;
	public GameObject Player0Prefab;
	public GameObject Player1Prefab;

	public const string VOICE_TRANSLATE_SERVER = "127.0.0.1";
	public const int VOICE_TRANSLATE_PORT = 20001;

	const int MASTER_SERVER_MAX_CONNECTIONS = 2;
	const int MASTER_SERVER_PORT = 20000;
	bool localMasterServer = false; //Use only to use this device as master server
	private const string typeName = "CoolBeansGame";
	private const string gameName = "BestRoomEva";
	private HostData[] hostList;
	GameObject recoClient;
	
	
	private void StartServer()
	{
		GameObject recoClient;

		if (localMasterServer) {
			MasterServer.ipAddress = "127.0.0.1";
		}

		Globals.isServer = true;
		Network.InitializeServer (MASTER_SERVER_MAX_CONNECTIONS, MASTER_SERVER_PORT, !Network.HavePublicAddress());
		MasterServer.RegisterHost (typeName, gameName);

		recoClient = GameObject.Find ("VoiceTranslation");
		recoClient.GetComponentInChildren<UDP_RecoServer> ().StartServer ();
	}

	void OnPlayerConnected(NetworkPlayer player)
	{

	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);

		numPlayers--;
	}

	private void refreshHostList()
	{
		MasterServer.RequestHostList (typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}

	}
	

	private void JoinServer(HostData hostData)
	{
		Network.Connect (hostData);

		recoClient = GameObject.Find ("VoiceTranslation");;
		recoClient.GetComponentInChildren<UDP_RecoServer> ().StartServer ();
	}

	void OnConnectedToServer()
	{
		SpawnPlayer ();
	}

	void OnServerInitialised()
	{
		Debug.Log ("Server has been initialised!");
		SpawnPlayer ();
	}

	public static int nextPlayerID()
	{
		Debug.Log ("Alloc Player: " + numPlayers);
		return numPlayers++;
	}

	private void SpawnPlayer()
	{
		if (numPlayers % 2 == 0) {
			Network.Instantiate (Player1Prefab, new Vector3 (-1f, 5f, 0f), Quaternion.identity, 0);
		} else {
			Network.Instantiate(Player1Prefab, new Vector3(1f, 5f, 0f),Quaternion.identity, 0);
		}
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer) 
		{
			if(GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				StartServer();
			}

			if(GUI.Button(new Rect(500, 100, 250, 100), "Refresh Host List"))
			{
				refreshHostList();
			}

			if(hostList != null)
			{
				for(int i = 0; i < hostList.Length; i++)
				{
					if(GUI.Button(new Rect(500, 200 + (110 * i), 250, 50), hostList[i].gameName))
					{
						JoinServer(hostList[i]);
					}
				}
			}
		}
	}
}
