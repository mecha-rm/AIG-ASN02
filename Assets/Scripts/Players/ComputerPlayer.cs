using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for the computer player.
public class ComputerPlayer : Player
{
    // the tree node.
    private struct MinMaxNode
    {
        // the min and max score for this node.
        public int minScore, maxScore;

        // the tic-tac-toe board's state at this node.
        public boardSymbol[,] boardState;

        // the lost of related branches.
        public List<MinMaxNode> branches;
    }

    // if 'true', the computer player uses its AI.
    // TODO: change to default 'true' when A is complete.
    public bool useAI = false;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        // defaults to o symbol.
        if (playerSymbol == boardSymbol.none)
            playerSymbol = boardSymbol.o;
    }

    // gets the chosen index from the computer player.
    public override BoardIndex GetChosenIndex()
    {
        // gets the board.
        Board board = manager.board;

        if(useAI) // if the AI should be used.
        {
            // runs the min-max AI.
            return RunMinMaxAI(board);
        }
        else // AI should not be used.
        {
            // goes through each index and picks the next available space.
            for (int i = 0; i < board.boardList.Count; i++)
            {
                // the index is available.
                if (board.boardList[i].IsAvailable())
                    return board.boardList[i];
            }
        } 

        // no spots available.
        return null;
    }

    // runs the AI to find a space.
    private BoardIndex RunMinMaxAI(Board board)
    {
        MinMaxNode rootNode = new MinMaxNode();
        rootNode.boardState = board.GenerateBoardSymbolArray();

        // TODO: implement AI

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
