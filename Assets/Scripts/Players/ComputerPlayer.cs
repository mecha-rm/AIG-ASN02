using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for the computer player.
public class ComputerPlayer : Player
{
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        // defaults to o symbol.
        if (playerSymbol == symbol.none)
            playerSymbol = symbol.o;
    }

    // gets the chosen index from the computer player.
    public override BoardIndex GetChosenIndex()
    {
        // gets the board.
        Board board = manager.board;

        // chooses indexes from left to right.
        for(int r = 0; r < board.board.GetLength(0); r++) // rows
        {
            for (int c = 0; c < board.board.GetLength(1); c++) // columns
            {
                // returns the index.
                if (board.board[r, c].IsAvailable())
                    return board.board[r, c];
            }

        }

        // no spots available.
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
