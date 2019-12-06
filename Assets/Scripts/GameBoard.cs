using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour
{ 
    enum Piece
    {
        Empty = 0,
        Yellow = 1,
        Red = 2
    }

    //m,n value
    public static int numRows = 6;
    public static int depthValue = 4;
    public static int numColumns = 7;
    public static int numPiecesToWin = 4;
    public bool allowDiagonally = true;
    public static int tempadasd=0;


    /// <summary>
    /// The Game field.
    /// 0 = Empty
    /// 1 = Yellow
    /// 2 = Red
    /// </summary>
    int[,] field;

    bool isPlayersTurn = false;
    bool isLoading = true;
    bool isDropping = false;
    bool mouseButtonPressed = false;
    bool gameOver = false;
    bool isCheckingForWinner = false;
    public float dropTime = 4f;

    // Gameobjects 
    GameObject gameObjectField = null;
    public GameObject pieceRed = null;
    public GameObject pieceYellow = null;
    public GameObject pieceField = null;

    public string playerWonText = "You Won!";
    public string playerLoseText = "You Lose!";
    public string drawText = "Draw!";

    // temporary gameobject, holds the piece at mouse position until the mouse has clicked
    GameObject gameObjectTurn;

    // Pause Menu Logic
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI = null;
    public GameObject mainMenuConfirmUI = null;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateField();
        Time.timeScale = 1;
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

        isLoading = false;
        gameOver = false;

        // center camera
        Camera.main.transform.position = new Vector3(
            (numColumns - 1) /2.0f, -((numRows - 1) / 2.0f) , Camera.main.transform.position.z );

    }

    /// <summary>
    /// Gets all the possible moves.
    /// </summary>
    /// <returns>The possible moves.</returns>
    public List<int> GetPossibleMoves(int[,] tempField )
    {
        List<int> possibleMoves = new List<int>();
        for (int x = 0; x < numColumns; x++)
        {
            for (int y = numRows - 1; y >= 0; y--)
            {
                if (tempField[x, y] == (int)Piece.Empty)
                {
                    possibleMoves.Add(x);
                    break;
                }
            }
        }
        return possibleMoves;
    }

    private float Evaluate(int[] window, int start, int end, int piece)
    {
        int opp_piece = 1;
        if (piece == 1)
        {
            opp_piece = 2;
        }
        float score = 0;

        int total_piece = 0;
        int total_opp_piece = 0;
        int total_empty = 0;
        for (int i = start; i < end; ++i) {
            if (window[i] == piece)
            {
                ++total_piece;
            }
            if (window[i] == opp_piece)
            {
                ++total_opp_piece;
            }
            if (window[i] == 0)
            {
                ++total_empty;
            }
        }

        //for (int i = 0; i < numPiecesToWin; ++i)
        //{
        //    if (total_piece == numPiecesToWin - i  && total_empty == i)
        //    {
        //        score += ( numPiecesToWin / (numPiecesToWin - i) ) * 100.0f;
        //    }
        //}

        if (total_piece == 4) { score += 100; }
        else if (total_piece == 3 && total_empty == 1) { score += 5; }
        else if (total_piece == 2 && total_empty == 2) { score += 2; }
        
        if (total_opp_piece == 3 && total_empty ==1) { score -= 90; }


        return score;
    }

    private float ScoreBoard(int [,] tempBoard, int piece) 
    {
        float score = 0;
        int r, c;
        for (r = 0; r < numRows; ++r)
        {
            int[] row_array = new int[numColumns];
            for (c = 0; c < numColumns; ++c){ row_array[c] = tempBoard[c, r]; }
            for (c = 0; c < numColumns - numPiecesToWin; ++c)
            {
                score += Evaluate(row_array, c, c + numPiecesToWin, piece);
            }
        }

        for (c = 0; c < numColumns; ++c)
        {

            int[] col_array = new int[numRows];

            for (r = numRows - 1; r >= 0; --r) { col_array[r] = tempBoard[c, r]; }
            for (r = 0; r < numRows - numPiecesToWin; ++r)
            {
                score += Evaluate(col_array, r, r + numPiecesToWin, piece);
            }
        }


        for (c=numColumns-1-numPiecesToWin; c>=0; --c)
        {
            for(r=numRows-1- numPiecesToWin; r>=0; --r)
            {
                int[] rowcol_array = new int[numPiecesToWin];
                for( int i =0; i<numPiecesToWin; ++i)
                {
                    rowcol_array[i] = tempBoard[c + i, r + i];
                }
                score += Evaluate(rowcol_array, 0, numPiecesToWin, piece);
            }
        }
        for (c = numColumns - 1 - numPiecesToWin; c >= 0; --c)
        {
            for (r = numRows - 1 - numPiecesToWin; r >= 0; --r)
            {
                int[] rowcol_array = new int[numPiecesToWin];
                for (int i = 0; i < numPiecesToWin; ++i)
                {
                    rowcol_array[i] = tempBoard[c + i, r + numPiecesToWin - i];
                }
                score += Evaluate(rowcol_array, 0, numPiecesToWin, piece);
            }
        }


        return score;
    }

    private int PickBestMove()
    {
        List<int> moves = GetPossibleMoves(field);
        float best_score = -1;
        int best_col = -1;
        for (int i = 0; i < moves.Count; ++i)
        {
            int[,] temp = field.Clone() as int[,];
            //Dropping Piece in temp field
            for (int j = numRows - 1; j >= 0; --j)
            {
                if (temp[moves[i], j] == 0)
                {
                    temp[moves[i], j] = (int)Piece.Red;
                    break;
                }
            }

            float score = ScoreBoard(temp, (int)Piece.Red);
            if (score > best_score)
            {
                best_col = moves[i];
                best_score = score;
            }
        }

        print(best_score);
        return best_col;
    }

    private int GetNextOpenRow(int[,] tempField, int col)
    {
        int r = -1;
        for ( r =numRows-1; r>=0; --r)
        {
            if (tempField[col, r] == 0)
                return r;
        }
        return r;
    }

    private bool WinningBoard(int[,] tempField, int piece)
    {
        int r, c, d;
        

        
        for (c = 0; c < numColumns - numPiecesToWin + 1; ++c) {
            for (r = 0; r < numRows; ++r) {
                bool ans = true;
                for ( d=0; d< numPiecesToWin && ans; ++d)
                    ans = ( tempField[c+d, r] == piece );
                if (ans) { return true; }
            }
        }

        for (c = 0; c < numColumns; ++c) {
            for (r = 0; r < numRows - numPiecesToWin + 1; ++r) {
                bool ans = true;
                for (d = 0; d < numPiecesToWin && ans; ++d)
                    ans = (tempField[c, r+d] == piece);
                
                if (ans) { return true; }
            }
        }

        for (c = 0; c < numColumns - numPiecesToWin + 1; ++c) {
            for (r = 0; r < numRows - numPiecesToWin + 1; ++r) {
                bool ans = true;
                for (d = 0; d < numPiecesToWin && ans; ++d)
                    ans = (tempField[c + d, r + d] == piece);
                if (ans) { return true; }
            }
        }

        for (c = 0; c < numColumns - numPiecesToWin + 1; ++c){
            for (r = numPiecesToWin-1; r < numRows; ++r) {
                bool ans = true;
                for (d = 0; d < numPiecesToWin && ans; ++d)
                    ans = (tempField[c + d, r - d] == piece);
                if (ans) { return true; }
            }
        }


        return false;
    }

    private bool IsFinal(int[,] tempField)
    {
        return WinningBoard(tempField, (int)Piece.Red) || WinningBoard(tempField, (int)Piece.Yellow);
    }

    //Doesn't take depth value
    private List<float> MiniMax(int[,] tempField,int depth,float alpha, float beta, bool maximizingPlayer)
    {
        
        List<float> ans = new List<float>();
        ans.Add(-1);
        ans.Add(-1);
        List<int> moves = GetPossibleMoves(tempField);
        if (WinningBoard(tempField, (int)Piece.Red))
        {
            ans[0]= -1; ans[1]= 10000000 + depth;
        }
        else if (WinningBoard(tempField, (int)Piece.Yellow))
        {
            ans[0] = -1; ans[1] = -10000000 - depth; 
        }
        else if ( moves.Count == 0)
        {
            ans[0] = -1; ans[1] = 0; 
        }
        else if (depth == 0)
        {
            ans[0] = -1;
            if ( maximizingPlayer )
                ans[1] = ScoreBoard(tempField, (int)Piece.Red) + depth;
            else
                ans[1] = -ScoreBoard(tempField, (int)Piece.Yellow) - depth;
        }

        else if (maximizingPlayer)
        {
            float value = float.MinValue;
            int column = moves[0];
            foreach (int c in moves)
            {
                int row = GetNextOpenRow(tempField, c);
                int[,] field_copy = tempField.Clone() as int[,];
                field_copy[c, row] = (int)Piece.Red;
                float newScore = MiniMax(field_copy, depth-1,alpha, beta, false)[1];
                if (newScore > value)
                {
                    value = newScore;
                    column = c;
                }
                alpha = Mathf.Max(alpha, value);
                if (beta <= alpha)
                {
                    break;
                }
            }
            ans[0] = column;
            ans[1] = value;
            
        }
        else
        {
            float value = float.MaxValue;
            int column = moves[0];
            foreach (int c in moves)
            {
                int row = GetNextOpenRow(tempField, c);
                int[,] field_copy = tempField.Clone() as int[,];
                field_copy[c, row] = (int)Piece.Yellow;
                float newScore = MiniMax(field_copy, depth-1, alpha, beta, true)[1];
                if (newScore < value)
                {
                    value = newScore;
                    column = c;
                }
                beta = Mathf.Min(beta, value);
                if (beta <= alpha)
                {
                    break;
                }
            }
            ans[0] = column;
            ans[1] = value;
        }
        return ans;

    }

    GameObject SpawnPiece()
    {
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!isPlayersTurn)
        {
            //List<int> moves = GetPossibleMoves(field);
            //int column = moves[0];

            //tempadasd=0;
            List<float> ans = MiniMax(field, depthValue, float.MinValue, float.MaxValue, true);
            //print(ans.Count);
            int column = (int)ans[0];

            //print(tempadasd);
            //int column = PickBestMove();
            spawnPos = new Vector3(column, 0, 0);
        }
        //else
        //{
        //    List<float> ans = MiniMax(field, depthValue, float.MinValue, float.MaxValue, false);
        //    //print(ans.Count);
        //    int column = (int)ans[0];

        //    //int column = PickBestMove();
        //    spawnPos = new Vector3(column, 0, 0);
        //}

        GameObject g = Instantiate(
                                    isPlayersTurn ? pieceYellow : pieceRed, // is players turn = spawn yellow, else spawn red
                                    new Vector3( Mathf.Clamp(spawnPos.x, 0, numColumns - 1), gameObjectField.transform.position.y + 1, 0 ), // spawn it above the first row
                                    Quaternion.identity 
                                  ) as GameObject;
        g.transform.parent = gameObjectField.transform;
        return g;
    }

    /// <summary>
    /// This method searches for a empty cell and lets 
    /// the object fall down into this cell
    /// </summary>
    /// <param name="gObject">Game Object.</param>
    IEnumerator dropPiece(GameObject gObject)
    {
        isDropping = true;

        Vector3 startPosition = gObject.transform.position;
        Vector3 endPosition = new Vector3();

        // round to a grid cell
        int x = Mathf.RoundToInt(startPosition.x);
        startPosition = new Vector3(x, startPosition.y, startPosition.z);

        // is there a free cell in the selected column?
        bool foundFreeCell = false;
        for (int i = numRows - 1; i >= 0; i--)
        {
            if (field[x, i] == 0)
            {
                foundFreeCell = true;
                field[x, i] = isPlayersTurn ? (int)Piece.Yellow : (int)Piece.Red;
                endPosition = new Vector3(x, i * -1, startPosition.z);

                break;
            }
        }

        if (foundFreeCell)
        {
            // Instantiate a new Piece, disable the temporary
            GameObject g = Instantiate(gObject) as GameObject;
            gameObjectTurn.GetComponent<Renderer>().enabled = false;

            float distance = Vector3.Distance(startPosition, endPosition);

            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime * dropTime * ((numRows - distance) + 1);
                
                //if (GameIsPaused) { g.SetActive(false); }
                //else { g.SetActive(true); }
                
                g.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
            //g.SetActive(true);

            g.transform.parent = gameObjectField.transform;

            // remove the temporary gameobject
            DestroyImmediate(gameObjectTurn);

            isPlayersTurn = !isPlayersTurn;
        }

        isDropping = false;

        yield return 0;
    }

    /// <summary>
    /// check if the field contains an empty cell
    /// </summary>
    /// <returns><c>true</c>, if it contains empty cell, <c>false</c> otherwise.</returns>
    bool FieldContainsEmptyCell()
    {
        for (int x = 0; x < numColumns; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                if (field[x, y] == (int)Piece.Empty)
                    return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuConfirmUI.activeSelf && !isDropping)
        {
            
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        if (IsFinal(field))
            return;
        
        if (GameIsPaused)
            return;

        if (gameOver)
            return;

        if (isPlayersTurn)
        {
            
            if (gameObjectTurn == null)
            {
                gameObjectTurn = SpawnPiece();
            }
            else
            {
                // update the objects position
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gameObjectTurn.transform.position = new Vector3(
                    Mathf.Clamp(pos.x, 0, numColumns - 1),
                    gameObjectField.transform.position.y + 1, 0);

                // click the left mouse button to drop the piece into the selected column
                if (Input.GetMouseButtonDown(0) && !mouseButtonPressed && !isDropping)
                {
                    mouseButtonPressed = true;
                    StartCoroutine(dropPiece(gameObjectTurn));
                }
                else
                {
                    mouseButtonPressed = false;
                }
            }
            /*
            if (gameObjectTurn == null)
            {
                gameObjectTurn = SpawnPiece();
            }
            else
            {
                if (!isDropping)
                    StartCoroutine(dropPiece(gameObjectTurn));
            }
            */
        }
        else
        {
            if (gameObjectTurn == null)
            {
                gameObjectTurn = SpawnPiece();
            }
            else
            {
                if (!isDropping)
                    StartCoroutine(dropPiece(gameObjectTurn));
            }
        }

    }

    void Pause()
    {
        
        gameObjectField.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameObjectField.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void HowToPlay()
    {
    }

    public void MainMenu()
    {
        pauseMenuUI.SetActive(false);
        mainMenuConfirmUI.SetActive(true);
    }

    public void Yes()
    {
        SceneManager.LoadScene("Scenes/main_menu");
    }

    public void No()
    {
        mainMenuConfirmUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}


//if ( tempField[0,0] == 0 &&
//    tempField[0, 1] == 0 &&
//    tempField[0, 2] == 2 &&
//    tempField[0, 3] == 2 &&
//    tempField[1, 0] == 0 &&
//    tempField[1, 1] == 0 &&
//    tempField[1, 2] == 0 &&
//    tempField[1, 3] == 1 &&
//    tempField[2, 0] == 0 &&
//    tempField[2, 1] == 0 &&
//    tempField[2, 2] == 0 &&
//    tempField[2, 3] == 1 &&
//    tempField[3, 0] == 0 &&
//    tempField[3, 1] == 0 &&
//    tempField[3, 2] == 0 &&
//    tempField[3, 3] == 1)
//{
//    print("apple");
//}