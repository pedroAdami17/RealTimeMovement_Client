using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class NetworkClientProcessing
{
    static List<int> spawnedPlayers = new List<int>();

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        for (int i = 0; i < csv.Length; i++)
        {
            Debug.Log("Element " + i + ": " + csv[i]);
        }

        // Check array length before accessing elements
        if (csv.Length < 7)
        {
            Debug.LogError("Invalid CSV format. Expected at least 7 elements.");
            return;
        }

        if (signifier == ServerToClientSignifiers.VelocityAndPosition)
        {
            int playerId = int.Parse(csv[1]);
            int playerIdentifier = int.Parse(csv[2]);
            Vector2 vel = new Vector2(float.Parse(csv[3]), float.Parse(csv[4]));
            Vector2 pos = new Vector2(float.Parse(csv[5]), float.Parse(csv[6]));

            if (!PlayerExists(playerId))
            {
                SpawnNewPlayer(playerId, playerIdentifier, pos);
            }
            else
            {
                gameLogic.GetComponent<GameLogic>().SetVelocityAndPosition(vel, pos);
            }
        }
        else if (signifier == ServerToClientSignifiers.SpawnPlayer)
        {
            int playerId = int.Parse(csv[1]);
            int playerIdentifier = int.Parse(csv[2]);

            // Spawn a new player based on the spawn message
            SpawnNewPlayer(playerId, playerIdentifier, Vector2.zero);
        }

    }

    static public void SendMessageToServer(string msg, TransportPipeline pipeline)
    {
        networkClient.SendMessageToServer(msg, pipeline);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    static public void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkClient.IsConnected();
    }
    static public void ConnectToServer()
    {
        networkClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        networkClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkClient networkClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkClient NetworkClient)
    {
        networkClient = NetworkClient;
    }
    static public NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion


    static private bool PlayerExists(int playerId)
    {
        return spawnedPlayers.Contains(playerId);
    }

    static private void SpawnNewPlayer(int playerId, int playerIdentifier, Vector2 spawnPosition)
    {
        GameObject newPlayer = InstantiatePlayerPrefab(playerIdentifier, spawnPosition);
        spawnedPlayers.Add(playerId);
    }

    static private GameObject InstantiatePlayerPrefab(int playerIdentifier, Vector2 spawnPosition)
    {
        GameObject newPlayer = new GameObject("Player");

        newPlayer.AddComponent<SpriteRenderer>();

        newPlayer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("YourSprite");
        newPlayer.transform.position = spawnPosition;

        // Attach the PlayerIdentifier component directly to the GameObject
        PlayerIdentifier playerIdentifierComponent = newPlayer.AddComponent<PlayerIdentifier>();
        playerIdentifierComponent.Identifier = playerIdentifier;

        return newPlayer;
    }
}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int KeyboardInput = 1;
}

static public class ServerToClientSignifiers
{
    public const int VelocityAndPosition = 1;
    public const int SpawnPlayer = 2;
}

static public class KbInputDirections
{
    public const int Up = 1;
    public const int Down = 2;
    public const int Right = 3;
    public const int Left = 4;

    public const int UpRight = 5;
    public const int UpLeft = 6;
    public const int DownRight = 7;
    public const int DownLeft = 8;

    public const int NoInput = 9;

}

public class PlayerIdentifier : MonoBehaviour
{
    public int Identifier { get; set; }
}
#endregion

