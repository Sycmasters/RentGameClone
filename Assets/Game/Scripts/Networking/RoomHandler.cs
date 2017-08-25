using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class RoomHandler : Photon.MonoBehaviour
{
    [Header("Room Data")]
    public GameObject[] playerInfo;
    public List<int> roomPlayers;
    public int playersInRoom = 0;

    [Header("Required")]
    public LobbyHandler lobby;
    public InputField roomName;
    public Text errorMessage;
    public Button startGame;
    public Button createRoomBtn;

    public void CreateRoom ()
    {
        if(string.IsNullOrEmpty(roomName.text))
        {
            errorMessage.text = "Necesita un nombre para la sala obligatorio";
            return;
        }

        createRoomBtn.interactable = false;

        RoomOptions settings = new RoomOptions()
        {
            MaxPlayers = 5,
            PlayerTtl = 300000,
            EmptyRoomTtl = 0
        };

        PhotonNetwork.CreateRoom(roomName.text, settings, TypedLobby.Default);
    }

    private void OnJoinedRoom ()
    {
        // Show room wait
        lobby.roomNamePanel.SetActive(false);
        lobby.roomPanel.SetActive(true);

        // We entered the room we just created
        playersInRoom = PhotonNetwork.room.PlayerCount;
        playerInfo[playersInRoom-1].SetActive(true);

        // Set nick for everyone
        playerInfo[playersInRoom - 1].transform.Find("Name").GetComponent<Text>().text = PhotonNetwork.playerName;
        playerInfo[playersInRoom - 1].transform.Find("RightBtn").GetComponent<Button>().interactable = true;
        playerInfo[playersInRoom - 1].transform.Find("LeftBtn").GetComponent<Button>().interactable = true;

        roomPlayers.Add(PhotonNetwork.player.ID);

        if (playersInRoom > 2)
        {
            startGame.interactable = true;
        }
    }

    private void OnPhotonCreateRoomFailed (object[] codeAndMsg)
    {
        Debug.LogError("Create room failed: " + codeAndMsg[0]);
        createRoomBtn.interactable = true;
    }

    private void OnPhotonPlayerConnected (PhotonPlayer otherPlayer)
    {
        // We entered the room we just created
        playersInRoom = PhotonNetwork.room.PlayerCount;
        playerInfo[playersInRoom - 1].SetActive(true);

        // Set nick for everyone and buttons for avatar
        playerInfo[playersInRoom - 1].transform.Find("Name").GetComponent<Text>().text = otherPlayer.NickName;

        roomPlayers.Add(otherPlayer.ID);

        if (playersInRoom > 2)
        {
            startGame.interactable = true;
        }
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        int index = roomPlayers.IndexOf(otherPlayer.ID);

        playerInfo[index].SetActive(false);
        playersInRoom = PhotonNetwork.room.PlayerCount;

        // Set nick for everyone
        playerInfo[index].transform.Find("Name").GetComponent<Text>().text = "Buscando jugador";
    }
}
