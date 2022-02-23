using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the board.
public class Board : MonoBehaviour
{
    // hides hte board object.
    [HideInInspector()] // doesn't show up in the inspector anyway.
    public BoardIndex[,] board;

    // the x-sprite
    public Sprite xSprite;

    // the o-sprite
    public Sprite oSprite;

    // Start is called before the first frame update
    void Start()
    {
        // initialize board.
        board = new BoardIndex[3, 3];

        // grabs the indexes
        BoardIndex[] indexes = FindObjectsOfType<BoardIndex>();
        
        // checks for proper count.
        if(indexes.Length == 9)
        {
            // inex in the list.
            int index = 0;

            // fills contents
            for(int r = 0; r < 3; r++) // checks rows
            {
                for(int c = 0; c < 3; c++) // checks columns
                {
                    // sets x-sprite if not preset.
                    if (indexes[index].xSprite == null)
                        indexes[index].xSprite = xSprite;

                    // sets o-sprite if not pre-set.
                    if (indexes[index].oSprite == null)
                        indexes[index].oSprite = oSprite;

                    board[r, c] = indexes[index];
                    index++;
                }
            }
        }
        else
        {
            Debug.LogError("Could not find 9 indexes in the scene.");
        }

    }

    // checks if the game is over.
    public bool IsFull()
    {
        // goes through each index.
        for(int r = 0; r < 3; r++)
        {
            for(int c = 0; c < 3; c++)
            {
                // open space available.
                if(board[r, c].indexSymbol == symbol.none)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // checks if the board has a winning chain.
    public bool HasWinner(symbol sym)
    {
        // ROWS
        // row 0
        if(board[0, 0].indexSymbol == sym && board[0, 1].indexSymbol == sym && board[0, 2].indexSymbol == sym)
        {
            return true;
        }

        // row 1
        if (board[1, 0].indexSymbol == sym && board[1, 1].indexSymbol == sym && board[1, 2].indexSymbol == sym)
        {
            return true;
        }

        // row 2
        if (board[2, 0].indexSymbol == sym && board[2, 1].indexSymbol == sym && board[2, 2].indexSymbol == sym)
        {
            return true;
        }

        // COLUMNS
        // column 0
        if (board[0, 0].indexSymbol == sym && board[1, 0].indexSymbol == sym && board[2, 0].indexSymbol == sym)
        {
            return true;
        }

        // column 1
        if (board[0, 1].indexSymbol == sym && board[1, 1].indexSymbol == sym && board[2, 1].indexSymbol == sym)
        {
            return true;
        }

        // column 2
        if (board[0, 2].indexSymbol == sym && board[1, 2].indexSymbol == sym && board[2, 2].indexSymbol == sym)
        {
            return true;
        }

        // DIAGONALS
        if (board[0, 0].indexSymbol == sym && board[1, 1].indexSymbol == sym && board[2, 2].indexSymbol == sym)
        {
            return true;
        }

        // column 1
        if (board[2, 1].indexSymbol == sym && board[1, 1].indexSymbol == sym && board[0, 2].indexSymbol == sym)
        {
            return true;
        }

        // no matches.
        return false;
    }

    // checks if a board index is available.
    public bool IsBoardIndexAvailable(BoardIndex index)
    {
        // BoardIndex checkedIndex = board.
        foreach(BoardIndex boardIndex in board)
        {
            if (boardIndex == index)
                return boardIndex.indexSymbol == symbol.none;
        }

        return false;
    }

    // sets a board index on the actual space.
    public bool SetBoardIndex(int row, int col, symbol sym, bool overwrite = false)
    {
        // out of bounds.
        if (row > 2 || row < 0 || col > 2 || col < 0)
            return false;

        // nothing set.
        if (board[row, col] == null)
            return false;

        // changes symbol.
        if(board[row, col].indexSymbol != symbol.none || overwrite == true)
        {
            board[row, col].SetIndexSymbol(sym);
            return true;
        }

        // no change.
        return false;
    }

    // sets a board index on a [1 - 9] scale.
    public bool SetBoardIndex(int space, symbol sym, bool overwrite = false)
    {
        // checks if board index set successful.
        bool success = false;

        // TODO: optimize this setup.
        switch (space)
        {
            case 0: // top left
            case 1:
                success = SetBoardIndex(0, 0, sym, overwrite);
                break;
            case 2: // top middle
                success = SetBoardIndex(0, 1, sym, overwrite);
                break;
            case 3: // top right
                success = SetBoardIndex(0, 2, sym, overwrite);
                break;
            case 4: // middle left
                success = SetBoardIndex(1, 0, sym, overwrite);
                break;
            case 5: // middle middle
                success = SetBoardIndex(1, 1, sym, overwrite);
                break;
            case 6: // middle right
                success = SetBoardIndex(1, 2, sym, overwrite);
                break;
            case 7: // bottom left
                success = SetBoardIndex(2, 0, sym, overwrite);
                break;
            case 8: // bottom middle
                success = SetBoardIndex(2, 1, sym, overwrite);
                break;
            case 9: // bottom right
                success = SetBoardIndex(2, 2, sym, overwrite);
                break;
        }

        return success;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
