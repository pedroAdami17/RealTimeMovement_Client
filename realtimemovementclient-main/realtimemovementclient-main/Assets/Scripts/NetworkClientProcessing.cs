using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkClientProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.VelocityAndPosition)
        {
            Vector2 vel = new Vector2(float.Parse(csv[1]), float.Parse(csv[2]));
            Vector2 pos = new Vector2(float.Parse(csv[3]), float.Parse(csv[4]));

            gameLogic.GetComponent<GameLogic>().SetVelocityAndPosition(vel, pos);
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

}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int KeyboardInput = 1;
}

static public class ServerToClientSignifiers
{
    public const int VelocityAndPosition = 1;
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
#endregion

