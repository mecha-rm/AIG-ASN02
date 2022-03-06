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
        public int score;

        // the tic-tac-toe board's state at this node.
        public boardSymbol[,] boardState;

        // the lost of related branches.
        // public List<MinMaxNode> nodes;

        // copy constructor
        public MinMaxNode(MinMaxNode copy)
        {
            score = copy.score;

            // pass-by reference
            // boardState = copy.boardState;

            // copies the contents.
            boardState = copy.boardState.Clone() as boardSymbol[,];

            // nodes = new List<MinMaxNode>(copy.nodes);

            // nodes = copy.nodes.CopyTo(;
        }

        // // initializes score, boardState, and nodes.
        // public MinMaxNode(int score, boardSymbol[,] boardState, List<MinMaxNode> nodes)
        // {
        //     this.score = score;
        //     this.boardState = boardState;
        //     this.nodes = nodes;
        // }
    }

    // if 'true', the computer player uses its AI.
    // TODO: change to default 'true' when A is complete.
    public bool useAI = false;

    // the maximum length of a branch.
    private const int maxBranchLen = 9;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        // defaults to o symbol.
        if (playerSymbol == boardSymbol.none)
            playerSymbol = boardSymbol.o;
    }


    // GAME AI //

    // checks if the board has a winning chain for the AI.
    private bool NodeHasWinner(boardSymbol[,] boardState, boardSymbol symbol)
    {
        // ROWS
        // row 0
        if (boardState[0, 0] == symbol && boardState[0, 1] == symbol && boardState[0, 2] == symbol)
        {
            // Debug.Log("Row 0 Win.");
            return true;
        }

        // row 1
        if (boardState[1, 0] == symbol && boardState[1, 1] == symbol && boardState[1, 2] == symbol)
        {
            // Debug.Log("Row 1 Win.");
            return true;
        }

        // row 2
        if (boardState[2, 0] == symbol && boardState[2, 1] == symbol && boardState[2, 2] == symbol)
        {
            // Debug.Log("Row 2 Win.");
            return true;
        }

        // COLUMNS
        // column 0
        if (boardState[0, 0] == symbol && boardState[1, 0] == symbol && boardState[2, 0] == symbol)
        {
            // Debug.Log("Column 0 Win.");
            return true;
        }

        // column 1
        if (boardState[0, 1] == symbol && boardState[1, 1] == symbol && boardState[2, 1] == symbol)
        {
            // Debug.Log("Column 1 Win.");
            return true;
        }

        // column 2
        if (boardState[0, 2] == symbol && boardState[1, 2] == symbol && boardState[2, 2] == symbol)
        {
            // Debug.Log("Column 2 Win.");
            return true;
        }

        // DIAGONALS
        // left-to-right
        if (boardState[0, 0] == symbol && boardState[1, 1] == symbol && boardState[2, 2] == symbol)
        {
            // Debug.Log("Diagonal Left-to-Right Win.");
            return true;
        }

        // right-to-left
        if (boardState[2, 0] == symbol && boardState[1, 1] == symbol && boardState[0, 2] == symbol)
        {
            // Debug.Log("Diagonal Right-to-Left Win.");
            return true;
        }

        // no matches.
        return false;
    }

    // checks if the node has a winning chain for the AI.
    private bool NodeHasWinner(MinMaxNode node, boardSymbol symbol)
    {
        return NodeHasWinner(node.boardState, symbol);
    }


    // runs the node and goes through all attached branches.
    // if there are no branches a value is returned.
    private int RunNode(MinMaxNode node, bool yourTurn, int depth)
    {
        // the list of nodes.
        List<MinMaxNode> nodes = new List<MinMaxNode>();

        // checking for winners
        {
            // default value.
            boardSymbol winSymbol = boardSymbol.none;

            // checks for a winner
            if (NodeHasWinner(node, boardSymbol.x)) // checks for the x-winner
            {
                winSymbol = boardSymbol.x;
            }
            else if (NodeHasWinner(node, boardSymbol.o)) // checks for the o-winner
            {
                winSymbol = boardSymbol.o;
            }
            
            // the board being filled is checked at the end of the function.
            if(winSymbol != boardSymbol.none)
            {
                // returns terminal value result.
                return maxBranchLen - depth + ((winSymbol == playerSymbol) ? 1 : -1);
            }

        }

        // goes through all indexes to get all combinations
        for (int row = 0; row < node.boardState.GetLength(0); row++) // goes through each row
        {
            for(int col = 0; col < node.boardState.GetLength(1); col++) // goes through each column
            {
                // it's an open space.
                if(node.boardState[row, col] == boardSymbol.none)
                {
                    // makes a new node.
                    MinMaxNode newNode = new MinMaxNode(node);
                    boardSymbol symbol = boardSymbol.none; // default.

                    // checks whose turn it is.
                    if(yourTurn) // it's the player's turn.
                    {
                        symbol = playerSymbol;
                    }
                    else // it's the opponent's turn.
                    {
                        // opposite symbol to the one this player uses.
                        symbol = (playerSymbol == boardSymbol.x) ? boardSymbol.x : boardSymbol.o;
                    }

                    // slot in the symbol.
                    newNode.boardState[row, col] = symbol;

                    // adds the new node to the list.
                    nodes.Add(newNode);
                }
            }
        }

        // checks nodes.
        if(nodes.Count == 0) // no nodes, so all spots are filled.
        {
            return maxBranchLen - depth + 0; // terminal value.
        }
        else
        {
            // goes through each node to update the scores.
            for(int i = 0; i < nodes.Count; i++)
            {
                // needed to set it up this way since it won't allow the score to change otherwise.
                MinMaxNode temp = nodes[i]; // get node.

                // grabs the result.
                int result = RunNode(nodes[i], !yourTurn, depth + 1);
                
                temp.score = result; // adds to the score.
                nodes[i] = temp; // put back in list.
            }

            // the value that will be returned. 
            int score = nodes[0].score; // grabs first score.

            // goes through each node again to find score.
            for (int i = 0; i < nodes.Count; i++)
            {
                if(yourTurn) // it's the computer player's turn (find max)
                {
                    // max
                    score = Mathf.Max(nodes[i].score, score);

                    // // larger score found.
                    // if (nodes[i].score > score)
                    //     score = nodes[i].score;
                }
                else // it's the opponent's turn (find min)
                {
                    // min
                    score = Mathf.Min(nodes[i].score, score);

                    // // smaller score found.
                    // if (nodes[i].score < score)
                    //     score = nodes[i].score;
                }
            }

            // returns the final score.
            return score;

        }
    }

    // runs the AI to find a space.
    private BoardIndex RunMinMaxAI(Board board)
    {
        // the root node.
        MinMaxNode rootNode = new MinMaxNode();

        // branch nodes.
        List<MinMaxNode> nodes = new List<MinMaxNode>();

        // the open indexes for the board. It's the same length as the nodes list.
        // (x) = row, (y) = column.
        List<Vector2Int> openIndexes = new List<Vector2Int>();

        // the highest value.
        int value = 0;

        // the chosen spot for the player.
        int nodeIndex = -1;

        // generates the board.
        rootNode.boardState = board.GenerateBoardSymbolArray();

        // checks all outcomes.
        for (int row = 0; row < rootNode.boardState.GetLength(0); row++) // row
        {
            for (int col = 0; col < rootNode.boardState.GetLength(1); col++) // column
            {
                // open space.
                if (rootNode.boardState[row, col] == boardSymbol.none)
                {
                    // makes a new node.
                    MinMaxNode newNode = new MinMaxNode(rootNode);
                    boardSymbol symbol = playerSymbol; // player symbol.
                    // boardSymbol symbol = boardSymbol.none; // default.

                    // since it's the player's turn, the next row will be the opponent's symbol.
                    // if (playerSymbol == boardSymbol.x) // computer uses (x) 
                    //     symbol = boardSymbol.o;
                    // 
                    // if (playerSymbol == boardSymbol.o) // computer uses (o) 
                    //     symbol = boardSymbol.x;

                    // slot in the symbol for this player's turn.
                    newNode.boardState[row, col] = symbol;

                    // calls the function to follow the branch through.
                    newNode.score = RunNode(newNode, false, 1); // going into level 1

                    // adds the new node and index to the lists.
                    nodes.Add(newNode);
                    openIndexes.Add(new Vector2Int(row, col));
                }
            }
        }

        // saves the list of nodes.
        // this doesn't really serve a purpose.
        // rootNode.nodes = nodes;

        // gets best score.
        if(nodes.Count > 0)
        {
            // grabs 0 index for initial score at the start.
            nodeIndex = 0;
            value = nodes[nodeIndex].score;

            // list of best options.
            // if there's more than 1 best option, a random one is chosen.
            List<int> indexBestOptions = new List<int>();

            // goes through each index.
            for (int i = 0; i < nodes.Count; i++)
            {
                if(nodes[i].score > value) // higher score found.
                {
                    nodeIndex = i; // saves the index.
                    value = nodes[i].score; // grab score.
                }
                else if(nodes[i].score == value) // value found.
                {
                    nodeIndex = i;
                    indexBestOptions.Add(nodeIndex);
                }
            }

            // more than one best option, so choose a random spot.
            if (indexBestOptions.Count > 1)
                nodeIndex = indexBestOptions[Random.Range(0, indexBestOptions.Count)];
            else
                nodeIndex = 0;
        }

        // returning a value.
        if(nodeIndex >= 0 && nodeIndex < openIndexes.Count) // node index found.
        {
            // returns the section in the board.
            return board.boardArray[openIndexes[nodeIndex].x, openIndexes[nodeIndex].y];
        }
        else // no selection.
        {
            return null;
        }
    }

    // SELECTION FUNCTION //

    // gets the chosen index from the computer player.
    public override BoardIndex GetChosenIndex()
    {
        // gets the board.
        Board board = manager.board;

        if (useAI) // if the AI should be used.
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
