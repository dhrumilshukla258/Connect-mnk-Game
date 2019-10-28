using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{ 
    enum Piece
    {
        Empty = 0,
        Blue = 1,
        Red = 2
    }

    //m,n value
    public static int numRows = 4;
    public static int numColumns = 5;
    public static int numPiecesToWin = 4;
    
    /// <summary>
    /// The Game field.
    /// 0 = Empty
    /// 1 = Blue
    /// 2 = Red
    /// </summary>
    int[,] field;

    bool isPlayersTurn = true;
    bool isLoading = true;
    bool isDropping = false;
    bool mouseButtonPressed = false;
    bool gameOver = false;
    bool isCheckingForWinner = false;

    public float dropTime = 4f;

    // Gameobjects 
    GameObject gameObjectField;
    public GameObject pieceRed;
    public GameObject pieceBlue;
    public GameObject pieceField;

    public string playerWonText = "You Won!";
    public string playerLoseText = "You Lose!";
    public string drawText = "Draw!";

    // temporary gameobject, holds the piece at mouse position until the mouse has clicked
    GameObject gameObjectTurn;


    // Start is called before the first frame update
    void Start()
    {
        CreateField();
    }

    /// <summary>
    /// Creates the field.
    /// </summary>
    void CreateField()
    {
        gameObjectField = GameObject.Find("Field");
        if (gameObjectField != null)
        {
            DestroyImmediate(gameObjectField);
        }
        gameObjectField = new GameObject("Field");

        //gameObjectField.transform.parent = this.transform; 

        // create an empty field and instantiate the cells
        field = new int[numColumns, numRows];
        for (int x = 0; x < numColumns; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                field[x, y] = (int)Piece.Empty;
                GameObject g = Instantiate(pieceField, new Vector3(x, y * -1, -1), Quaternion.identity) as GameObject;
                g.transform.parent = gameObjectField.transform;
            }
        }


        // center camera
        Camera.main.transform.position = new Vector3(
            (numColumns - 1) /2.0f, -((numRows - 1) / 2.0f) , Camera.main.transform.position.z);

    }



    // Update is called once per frame
    void Update()
    {
        if (isLoading)
            return;

        if (isCheckingForWinner)
            return;

        if (gameOver)
        {
           

            return;
        }
    }
}
