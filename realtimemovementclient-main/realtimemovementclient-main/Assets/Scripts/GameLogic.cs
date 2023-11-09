using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    GameObject character;

    Vector2 characterPositionInPercent;
    Vector2 characterVelocityInPercent;
    //const float CharacterSpeed = 0.25f;
    //float DiagonalCharacterSpeed;

    void Start()
    {
        
        NetworkClientProcessing.SetGameLogic(this);

        Sprite circleTexture = Resources.Load<Sprite>("Circle");

        character = new GameObject("Character");

        character.AddComponent<SpriteRenderer>();
        character.GetComponent<SpriteRenderer>().sprite = circleTexture;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            //characterVelocityInPercent = Vector2.zero;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.UpRight, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.UpLeft, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.DownRight, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.DownLeft, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.Right, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.Left, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.Up, TransportPipeline.ReliableAndInOrder);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.Down, TransportPipeline.ReliableAndInOrder);
            }
            else
                NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.KeyboardInput + "," + KbInputDirections.NoInput, TransportPipeline.ReliableAndInOrder);
        }

        characterPositionInPercent += (characterVelocityInPercent * Time.deltaTime);

        Vector2 screenPos = new Vector2(characterPositionInPercent.x * (float)Screen.width, characterPositionInPercent.y * (float)Screen.height);
        Vector3 characterPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        characterPos.z = 0;
        character.transform.position = characterPos;

    }

    public void SetVelocityAndPosition(Vector2 vel, Vector2 pos)
    {
        characterVelocityInPercent = vel;
        characterPositionInPercent = pos;
    }

}
