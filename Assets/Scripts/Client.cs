﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

	private bool socketReady;
	private TcpClient socket;
	private NetworkStream stream;
	private StreamWriter writer;
	private StreamReader reader;
	public string clientName;
	public bool isHost = false;

	private List<GameClient> players = new List<GameClient>();

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
	private void Update(){
		if (socketReady) {
			if (stream.DataAvailable) {
				string data = reader.ReadLine ();
				if (data != null)
					OnIncomingData (data);
			}
		}
	}
	private void onApplicationQuit()
	{
		CloseSocket ();
	}
	private void OnDisabled(){
		CloseSocket ();
	}
	public bool ConnectToServer(string host, int port){
		if (socketReady)
			return false;
		try{
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);

			socketReady = true;
		}
		catch(Exception e){
			Debug.Log ("Socket error " + e.Message);
		}
		return socketReady;
	}

	//Sending Message to the server
	public void Send(string data){
		if (!socketReady)
			return;

		writer.WriteLine (data);
		writer.Flush ();
	}
	//Read messages from the server
	private void OnIncomingData(string data)
	{
		//Debug.Log ("Client: " + data);
		string[] aData = data.Split ('|');
		switch (aData [0]) 
		{
		case "SWHO":
			for (int i = 1; i < aData.Length - 1; i++) {
				UserConnected (aData [i], false);
			}
			Send ("CWHO|" + this.clientName + "|" + ((isHost)?1:0).ToString());
			break;
		case "SombodyConnected":
			UserConnected (aData [1], false);
			break;
		case "SMOVE":
			Debug.Log (aData [1] + " " + aData [2] + " " + aData [3] + " " + aData [4]);
			GameManager.Instance.AttemptToMove (int.Parse (aData [1]), int.Parse (aData [2]), int.Parse (aData [3]), int.Parse (aData [4]));
			Debug.Log ("Did I move?");
			break;
		}
	}

	private void CloseSocket(){
		if (!socketReady)
			return;
		writer.Close ();
		reader.Close ();
		socket.Close ();
		socketReady = false;
	}
	private void UserConnected(string name, bool host)
	{
		GameClient c = new GameClient ();
		c.name = name;
		players.Add (c);

		if (players.Count == 2)
			ManuManager.Instance.StartGame ();
	}
}
public class GameClient
{
	public string name;
	public bool isHost;
}
