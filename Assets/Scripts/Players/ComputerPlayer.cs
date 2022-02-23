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

        // goes through each index.
        for(int i = 0; i < board.boardList.Count; i++)
        {
            // the index is available.
            if (board.boardList[i].IsAvailable())
                return board.boardList[i];
        }


        // no spots available.
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
