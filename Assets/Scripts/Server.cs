using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {
	//which port game is going to be played on.
	public int port = 6321;

	private List<ServerClient> clients;
	private List<ServerClient> disconnectList;

	private TcpListener server;
	private bool serverStarted;

	private void Update(){
		if (!serverStarted)
			return;

		foreach (var c in clients) {
			//is the client still connected?
			if (!IsConnected (c.tcp)) {
				c.tcp.Close ();
				disconnectList.Add (c);
				continue;
			} else {
				//check if client writes something
				NetworkStream s = c.tcp.GetStream();
				if (s.DataAvailable) {
					StreamReader reader = new StreamReader (s, true);
					string data = reader.ReadLine ();

					if (data != null)
						OnIncomingData (c, data);
				}
			}
		}
		for (int i = 0; i < disconnectList.Count; i++) {
			clients.Remove (disconnectList [i]);
			disconnectList.RemoveAt (i);
		}
	}
	public void init(){
		DontDestroyOnLoad (gameObject);
		clients = new List<ServerClient> ();
		disconnectList = new List<ServerClient> ();

		try{
			server = new TcpListener(IPAddress.Any, port);
			server.Start();
			//listening for incomming connections
			StartListening();
		}
		catch(Exception e) {
			Debug.Log ("Socket error: " + e.Message);
		}
	}
	private void StartListening(){
		server.BeginAcceptTcpClient (AcceptTcpClient, server);
	}
	private void AcceptTcpClient(IAsyncResult ar){
		TcpListener listener = (TcpListener)ar.AsyncState;
		ServerClient sc = new ServerClient (listener.EndAcceptTcpClient (ar));
		clients.Add (sc);
		//tell server to comeback to listening, because after the ServerClient is added, server "forgets" to go back to listening
		StartListening();
		Debug.Log ("Somebody has connected!");
	}
	private bool IsConnected(TcpClient c){
		try{
			if(c != null && c.Client != null && c.Client.Connected){
				return true;
			}
			else
				return false;
		}
		catch{
			return false;
		}
	}
	//Send from server
	private void Boardcast(string data, List<ServerClient> cl)
	{
		foreach (var sc in cl) {
			try{
				StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
				writer.WriteLine(data);
				writer.Flush();
			}
			catch(Exception e){
				Debug.Log ("Write error :" + e.Message);
			}
		}
	}
	//Server Read
	private void OnIncomingData(ServerClient c, string data)
	{
		Debug.Log (c.clientName + " : " + data);
	}
}

public class ServerClient
{
	public string clientName;
	public TcpClient tcp;

	public ServerClient (TcpClient tcp)
	{
		this.tcp = tcp;
	}
}
