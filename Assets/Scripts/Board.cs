using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the board.
public class Board : MonoBehaviour
{
    // hides the board object.
    [HideInInspector()] // doesn't show up in the inspector anyway.
    public BoardIndex[,] boardArray;

    // the board list.
    [Tooltip("The board indexes in a single list format.")]
    public List<BoardIndex> boardList = new List<BoardIndex>();

    // the x-sprite
    public Sprite xSprite;

    // the o-sprite
    public Sprite oSprite;

    // Start is called before the first frame update
    void Start()
    {
        // initialize board.
        boardArray = new BoardIndex[3, 3];

        // grabs the indexes
        // won't be listed in the proper order.
        BoardIndex[] indexes = FindObjectsOfType<BoardIndex>();
        
        // if the board list has been filled.
        if(boardList.Count == 9) // fills array based on list.
        {
            int row = 0, col = 0;

            // goes through every index.
            foreach(BoardIndex index in boardList)
            {
                // sets x-sprite if not preset.
                if (index.xSprite == null)
                    index.xSprite = xSprite;

                // sets o-sprite if not pre-set.
                if (index.oSprite == null)
                    index.oSprite = oSprite;

                boardArray[row, col] = index;

                // increases column count.
                col++;

                // move onto next row.
                if(col >= boardArray.GetLength(1))
                {
                    row++;
                    col = 0;
                }

            }
        }
        else if(indexes.Length == 9) // checks for proper count, and fills list based on array.
        {
            // inex in the list.
            int index = 0;

            // empties board list.
            boardList = new List<BoardIndex>();

            // fills contents
            for (int row = 0; row < boardArray.GetLength(0); row++) // checks rows
            {
                for(int col = 0; col < boardArray.GetLength(1); col++) // checks columns
                {
                    // sets x-sprite if not preset.
                    if (indexes[index].xSprite == null)
                        indexes[index].xSprite = xSprite;

                    // sets o-sprite if not pre-set.
                    if (indexes[index].oSprite == null)
                        indexes[index].oSprite = oSprite;

                    boardArray[row, col] = indexes[index];
                    boardList.Add(boardArray[row, col]);

                    index++;
                }
            }
        }
        else
        {
            Debug.LogError("Could not find 9 indexes in the scene.");
        }

        Debug.Log("Board Generated.");
    }

    // checks if the game is over.
    public bool IsFull()
    {
        // goes through each index.
        for(int r = 0; r < boardArray.GetLength(0); r++)
        {
            for(int c = 0; c < boardArray.GetLength(1); c++)
            {
                // open space available.
                if(boardArray[r, c].indexSymbol == boardSymbol.none)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // checks if the board has a winning chain.
    public bool HasWinner(boardSymbol sym)
    {
        // ROWS
        // row 0
        if(boardArray[0, 0].indexSymbol == sym && boardArray[0, 1].indexSymbol == sym && boardArray[0, 2].indexSymbol == sym)
        {
            Debug.Log("Row 0 Win.");
            return true;
        }

        // row 1
        if (boardArray[1, 0].indexSymbol == sym && boardArray[1, 1].indexSymbol == sym && boardArray[1, 2].indexSymbol == sym)
        {
            Debug.Log("Row 1 Win.");
            return true;
        }

        // row 2
        if (boardArray[2, 0].indexSymbol == sym && boardArray[2, 1].indexSymbol == sym && boardArray[2, 2].indexSymbol == sym)
        {
            Debug.Log("Row 2 Win.");
            return true;
        }

        // COLUMNS
        // column 0
        if (boardArray[0, 0].indexSymbol == sym && boardArray[1, 0].indexSymbol == sym && boardArray[2, 0].indexSymbol == sym)
        {
            Debug.Log("Column 0 Win.");
            return true;
        }

        // column 1
        if (boardArray[0, 1].indexSymbol == sym && boardArray[1, 1].indexSymbol == sym && boardArray[2, 1].indexSymbol == sym)
        {
            Debug.Log("Column 1 Win.");
            return true;
        }

        // column 2
        if (boardArray[0, 2].indexSymbol == sym && boardArray[1, 2].indexSymbol == sym && boardArray[2, 2].indexSymbol == sym)
        {
            Debug.Log("Column 2 Win.");
            return true;
        }

        // DIAGONALS
        // left-to-right
        if (boardArray[0, 0].indexSymbol == sym && boardArray[1, 1].indexSymbol == sym && boardArray[2, 2].indexSymbol == sym)
        {
            Debug.Log("Diagonal Left-to-Right Win.");
            return true;
        }

        // right-to-left
        if (boardArray[2, 0].indexSymbol == sym && boardArray[1, 1].indexSymbol == sym && boardArray[0, 2].indexSymbol == sym)
        {
            Debug.Log("Diagonal Right-to-Left Win.");
            return true;
        }

        // no matches.
        return false;
    }

    // checks if a board index is available.
    public bool IsBoardIndexAvailable(BoardIndex index)
    {
        // BoardIndex checkedIndex = board.
        foreach(BoardIndex boardIndex in boardArray)
        {
            if (boardIndex == index)
                return boardIndex.indexSymbol == boardSymbol.none;
        }

        return false;
    }

    // gets the board index.
    public BoardIndex GetBoardArrayIndex(int row, int col)
    {
        // out of bounds.
        if (row < 0 || row >= boardArray.GetLength(0) || col < 0 || col >= boardArray.GetLength(1))
            return null;

        // returns the board value.
        return boardArray[row, col];
    }

    // gets the board index from the board list.
    public BoardIndex GetBoardListIndex(int index)
    {
        if (index > 0 || index < boardList.Count)
            return boardList[index];
        else
            return null;
    }

    // sets a board index on the actual space.
    public bool SetBoardArrayIndexSymbol(int row, int col, boardSymbol sym, bool overwrite = false)
    {
        // out of bounds.
        if (row < 0 || row >= boardArray.GetLength(0) || col < 0|| col >= boardArray.GetLength(1))
            return false;

        // nothing set.
        if (boardArray[row, col] == null)
            return false;

        // changes symbol.
        if(boardArray[row, col].indexSymbol != boardSymbol.none || overwrite == true)
        {
            boardArray[row, col].SetIndexSymbol(sym);
            return true;
        }

        // no change.
        return false;
    }

    // sets the index of the board list.
    public bool SetBoardListIndexSymbol(int index, boardSymbol sym, bool overwrite = false)
    {
        // bounds check.
        if (index < 0 || index >= boardList.Count)
            return false;

        // checks if change allowed.
        if (boardList[index].indexSymbol != boardSymbol.none || overwrite == true)
        {
            boardList[index].SetIndexSymbol(sym);
            return true;
        }

        return false;
    }

    // generates a board array of board symbols.
    public boardSymbol[,] GenerateBoardSymbolArray()
    {
        // makes the array
        boardSymbol[,] arr = new boardSymbol[boardArray.GetLength(0), boardArray.GetLength(1)];

        // all rows
        for(int row = 0; row < arr.GetLength(0); row++)
        {
            // all columns
            for(int col = 0; col < arr.GetLength(1); col++)
            {
                // gets the symbol
                arr[row, col] = boardArray[row, col].indexSymbol;
            }
        }

        // returns the array.
        return arr;
    }

    // clears the board.
    public void ClearBoard()
    {
        // resets the board list.
        foreach(BoardIndex index in boardList)
        {
            index.SetIndexSymbol(boardSymbol.none);
        }
    }

    // hides the default board index sprites.
    public void HideNumbers()
    {
        foreach (BoardIndex index in boardList)
            index.HideNumber();
    }

    // show the default board index sprites.
    public void ShowNumbers()
    {
        foreach (BoardIndex index in boardList)
            index.ShowNumber();

    }

    // toggle the default board index sprite visibilities.
    public void ToggleNumbers()
    {
        foreach (BoardIndex index in boardList)
            index.ToggleNumber();

    }

    // Update is called once per frame
    void Update()
    {
    }
}
