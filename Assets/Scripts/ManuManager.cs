using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ManuManager : MonoBehaviour {
	public static ManuManager Instance { set; get; }

	public GameObject mainMenu;
	public GameObject hostMenu;
	public GameObject connectMenu;

	public GameObject serverPrefab;
	public GameObject clientPrefab;

	public InputField nameInput;

	private void Awake()
	{
		Instance = this;
		hostMenu.SetActive (false);
		connectMenu.SetActive (false);
		DontDestroyOnLoad (gameObject);
	}

	public void OnConnectBtn()
	{
		mainMenu.SetActive (false);
		connectMenu.SetActive (true);
	}
	public void HostBtn()
	{
		try{
			Server s = Instantiate(serverPrefab).GetComponent<Server>();
			s.init();

			Client c = Instantiate(clientPrefab).GetComponent<Client>();
			///This part is broken
			c.clientName = nameInput.text;
			/////////////
			if(c.clientName == "")
				c.clientName = "Host";
			c.ConnectToServer("127.0.0.1", 6321);

		}catch(Exception e) {
			Debug.Log (e.Message);
		}
		mainMenu.SetActive (false);
		hostMenu.SetActive (true);
	}
	public void ConnectToServerBtn() 
	{
		string hostAddress = GameObject.Find ("HostInput").GetComponent<InputField> ().text;
		if(hostAddress == "")
			hostAddress = "127.0.0.1";

		try
		{
			Client c = Instantiate(clientPrefab).GetComponent<Client>();
			c.clientName = nameInput.text;
			if(c.clientName == "")
				c.clientName = "Player";
			c.ConnectToServer(hostAddress, 6321);
			connectMenu.SetActive(false);
		}
		catch(Exception e) {
			Debug.Log (e.Message);
		}
	}
	public void BackBtn()
	{
		hostMenu.SetActive (false);
		connectMenu.SetActive (false);
		mainMenu.SetActive (true);

		Server s = FindObjectOfType<Server> ();
		Client c = FindObjectOfType<Client> ();
		if(s != null)
			Destroy (s);
		if (c != null)
			Destroy (c);
	}
	public void StartGame()
	{
		SceneManager.LoadScene ("Game");
	}
}
