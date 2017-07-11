using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
       

        Connect();

	}
	
    void Connect()
    {
     //   PhotonNetwork.offlineMode = true;
        PhotonNetwork.ConnectUsingSettings("v1");
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() : No existing rooms ....");
        Debug.Log("Creating room...");
        PhotonNetwork.CreateRoom(null);
    }
    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        SpawnMyPlayer();
    }
    int teamId = 0;
    void SpawnMyPlayer()
    {
        SpawnSpot[] spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
        List<GameObject> myTeamsSpawnSpotsList = new List<GameObject>();
        if (teamId == 0)
        {
            PlayerTeam[] players = GameObject.FindObjectsOfType<PlayerTeam>();
            //Find a team for the player (Blue is first)
            int redTeam = 0, blueTeam = 0;
            for (int i = 0; i < players.Length - 1; i++)
            {

                if (players[i].teamid == 0)
                {
                }
                else if (players[i].teamid == 1)
                {
                    redTeam++;
                }
                else if (players[i].teamid == 2)
                {
                    blueTeam++;
                }
            }
            if (redTeam > blueTeam)
            {
                teamId = 2;
            }
            else
                teamId = 1;
        }
        for (int i = 0; i < spawnSpots.Length ; i++)
        {
            if (spawnSpots[i].teamId == teamId)
            {
                myTeamsSpawnSpotsList.Add( spawnSpots[i].gameObject);
            }
        }
        Debug.Log(myTeamsSpawnSpotsList.Count);

        GameObject mySpawnSpot = myTeamsSpawnSpotsList[Random.Range(0, myTeamsSpawnSpotsList.Count)];
        var go = PhotonNetwork.Instantiate("Player", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
        go.GetComponent<PlayerTeam>().teamid = teamId;
        go.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        go.transform.Find("Camera").gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
    }
}
