using UnityEngine;
using SWNetwork; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// Basic lobby matchmaking implementation.
/// </summary>
public class Lobby : MonoBehaviour
{

    public enum LobbyState
    {
        Default,
        JoinedRoom
    }
    public LobbyState State = LobbyState.Default;

    void Start()
    {
        // Add an event handler for the OnRoomReadyEvent
        NetworkClient.Lobby.OnRoomReadyEvent += Lobby_OnRoomReadyEvent;

        // Add an event handler for the OnFailedToStartRoomEvent
        NetworkClient.Lobby.OnFailedToStartRoomEvent += Lobby_OnFailedToStartRoomEvent;

        // Add an event handler for the OnLobbyConnectedEvent
        NetworkClient.Lobby.OnLobbyConnectedEvent += Lobby_OnLobbyConncetedEvent;

        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent += OnNewPlayerJoinRoomEven;

    }

    private void OnNewPlayerJoinRoomEven(SWJoinRoomEventData eventData)
    {
        if (NetworkClient.Lobby.IsOwner)
        {
            // Actualizar

            // Cargar datos de jugador 2
            DigiCartas.QuickPlayer roomCustomData = new DigiCartas.QuickPlayer();
            roomCustomData = JsonUtility.FromJson<DigiCartas.QuickPlayer>(eventData.data);
            MultiPlayer.instance.CargarPlayer2(roomCustomData);

            MultiPlayer.instance.StartGame.gameObject.SetActive(true);
            MultiPlayer.instance.StartGame.interactable = true;
        }
    }

    void onDestroy()
    {
        // remove the handlers
        NetworkClient.Lobby.OnRoomReadyEvent -= Lobby_OnRoomReadyEvent;
        NetworkClient.Lobby.OnFailedToStartRoomEvent -= Lobby_OnFailedToStartRoomEvent;
        NetworkClient.Lobby.OnLobbyConnectedEvent -= Lobby_OnLobbyConncetedEvent;
    }

    public void RemoveAll()
    {
        NetworkClient.Lobby.OnRoomReadyEvent -= Lobby_OnRoomReadyEvent;
        NetworkClient.Lobby.OnFailedToStartRoomEvent -= Lobby_OnFailedToStartRoomEvent;
        NetworkClient.Lobby.OnLobbyConnectedEvent -= Lobby_OnLobbyConncetedEvent;
    }


    /* Lobby events handlers */
    void Lobby_OnRoomReadyEvent(SWRoomReadyEventData eventData)
    {
        Debug.Log("Room is ready: roomId= " + eventData.roomId);
        // Room is ready to join and its game servers have been assigned.
        ConnectToRoom();
    }

    void Lobby_OnFailedToStartRoomEvent(SWFailedToStartRoomEventData eventData)
    {
        Debug.Log("Failed to start room: " + eventData);
    }

    void Lobby_OnLobbyConncetedEvent()
    {
        Debug.Log("Lobby connected");
        RegisterPlayer();
    }

    /* UI event handlers */
    /// <summary>
    /// Register button was clicked
    /// </summary>
    public void Register()
    {
        ResetData();
        string customPlayerId = PlayerManager.instance.Jugador.Nombre;
        Debug.Log("ENTRO");
        if(customPlayerId != null && customPlayerId.Length > 0)
        {
            Debug.Log("ENTRO1");
            // use the user entered playerId to check into SocketWeaver. Make sure the PlayerId is unique.
            NetworkClient.Instance.CheckIn(customPlayerId,(bool ok, string error) =>
            {
                if (!ok)
                {
                    Debug.LogError("Check-in failed: " + error);

                }
            });
       
        }
        else
        {
            Debug.Log("ENTRO2");
            // use a randomly generated playerId to check into SocketWeaver.
            NetworkClient.Instance.CheckIn((bool ok, string error) =>
            {
                if (!ok)
                {
                    Debug.LogError("Check-in failed: " + error);
                  
                }
            });
        }

    }



    public void ResetData()
    {
        MultiPlayer.instance.Reset();
    }

    /// <summary>
    /// Play button was clicked
    /// </summary>


    /* Lobby helper methods*/
    /// <summary>
    /// Register the player to lobby
    /// </summary>
    void RegisterPlayer()
    {
        // conver Quick data
        DigiCartas.QuickPlayer qp = new DigiCartas.QuickPlayer();
        qp.Nombre = PlayerManager.instance.Jugador.Nombre;
        qp.IDCartasMazo = PlayerManager.instance.Jugador.IDCartasMazo;
        qp.Nivel = PlayerManager.instance.Jugador.Nivel;
        qp.Photo= PlayerManager.instance.Jugador.Photo;
        qp.Tablero = PlayerManager.instance.Jugador.Tablero;

        NetworkClient.Lobby.Register(qp,(successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Registered " + reply);

                if (string.IsNullOrEmpty(reply.roomId))
                {
                    JoinOrCreateRoom();
                }
                else if(reply.started)
                {
                    State = LobbyState.JoinedRoom;
                    ConnectToRoom();
                }
                else
                {
                    State = LobbyState.JoinedRoom;
                    ShowJoinedRoomPopOver();
                    //
                    GetPlayersInRoom();
                }
            }
            else
            {
                Debug.Log("Failed to register " + error);
            }
        });
    }



    void JoinOrCreateRoom()
    {
        NetworkClient.Lobby.JoinOrCreateRoom(false, 2, 0, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Joined room randomly " + reply);
                State = LobbyState.JoinedRoom;
                // 
                ShowJoinedRoomPopOver();
                GetPlayersInRoom();
            }
            else
            {
                Debug.Log("Failed to Join room randomly" + error);
            }
        });
    }

    void GetPlayersInRoom()
    {
        NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) => {
            if (successful)
            {
                Debug.Log("Got players " + reply);
                if (reply.players.Count==1)
                {

                    // Cargar datos de jugador 1
                    DigiCartas.QuickPlayer roomCustomData = new DigiCartas.QuickPlayer();
                    roomCustomData = JsonUtility.FromJson<DigiCartas.QuickPlayer>(reply.players[0].data);
                    Debug.Log("Owo " + roomCustomData.Nombre+":"+roomCustomData.Photo);
                    MultiPlayer.instance.CargarPlayer1(roomCustomData);

                }
                else
                {
                    //Cargar Datos de Jugador 1

                    DigiCartas.QuickPlayer roomCustomData1 = new DigiCartas.QuickPlayer();
                    roomCustomData1 = JsonUtility.FromJson<DigiCartas.QuickPlayer>(reply.players[0].data);
                    MultiPlayer.instance.CargarPlayer1(roomCustomData1);

                    // Cargar datos de jugador 2
                    DigiCartas.QuickPlayer roomCustomData = new DigiCartas.QuickPlayer();
                    roomCustomData = JsonUtility.FromJson<DigiCartas.QuickPlayer>(reply.players[1].data);
                    MultiPlayer.instance.CargarPlayer2(roomCustomData);

                    if (NetworkClient.Lobby.IsOwner)
                    {
                        //Marcar boton de empezar
                        MultiPlayer.instance.StartGame.gameObject.SetActive(true);
                        MultiPlayer.instance.StartGame.interactable = true;
                    }
                }
            }
            else
            {
                Debug.Log("Failed to get players " + error);
            }
        });
    }
    /// <summary>
    /// Callback method for NetworkClient.Lobby.JoinOrCreateRoom().
    /// </summary>
    /// <param name="successful">If set to <c>true</c> <paramref name="successful"/>, the player has joined or created a room.</param>
    /// <param name="reply">Reply.</param>
    /// <param name="error">Error.</param>
    void HandleJoinOrCreatedRoom(bool successful, SWJoinRoomReply reply, SWLobbyError error)
    {
        if (successful)
        {
            Debug.Log("Joined or created room " + reply);

            // the player has joined a room which has already started.
            if (reply.started)
            {
                ConnectToRoom();
            }
            else if (NetworkClient.Lobby.IsOwner)
            {
                // the player did not find a room to join
                // the player created a new room and became the room owner.
                StartRoom();
            }
        }
        else
        {
            Debug.Log("Failed to join or create room " + error);
        }
    }

    /// <summary>
    /// Start local player's current room. Lobby server will ask SocketWeaver to assign suitable game servers for the room.
    /// </summary>
    void StartRoom()
    {
        NetworkClient.Lobby.StartRoom((okay, error) =>
        {
            if (okay)
            {
                // Lobby server has sent request to SocketWeaver. The request is being processed.
                // If socketweaver finds suitable server, Lobby server will invoke the OnRoomReadyEvent.
                // If socketweaver cannot find suitable server, Lobby server will invoke the OnFailedToStartRoomEvent.
                Debug.Log("Started room");
            }
            else
            {
                Debug.Log("Failed to start room " + error);
            }
        });
    }

    /// <summary>
    /// Connect to the game servers of the room.
    /// </summary>
    void ConnectToRoom()
    {
        NetworkClient.Instance.ConnectToRoom(HandleConnectedToRoom);
    }

    /// <summary>
    /// Callback method NetworkClient.Instance.ConnectToRoom();
    /// </summary>
    /// <param name="connected">If set to <c>true</c>, the client has connected to the game servers successfully.</param>
    void HandleConnectedToRoom(bool connected)
    {
        if (connected)
        {
            Debug.Log("Connected to room");
            SceneManager.LoadScene("VsTamer");
        }
        else
        {
            Debug.Log("Failed to connect to room");
        }
    }


    public void OnCancelClicked()
    {
        Debug.Log("cANCEL");
        if (State == LobbyState.JoinedRoom)
        {
            // Cerrar Room
            Debug.Log("2");
            LeaveRoom();
        }
        else
        {
            Debug.Log(State);
        }
    }

    private void LeaveRoom()
    {
        NetworkClient.Lobby.LeaveRoom((successful, error) =>
        {
            if (successful)
            {
                Debug.Log("Left Room");
                State = LobbyState.Default;
                MultiPlayer.instance.LeaveGame();
            }
            else
            {
                Debug.Log("fAILED TO LEAVE ROOM"+ error);
            }
        });
    }


    public void ShowJoinedRoomPopOver()
    {
        MultiPlayer.instance.Inicio.gameObject.SetActive(false);
        MultiPlayer.instance.Room.gameObject.SetActive(true);
    }

}