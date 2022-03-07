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
        public List<MinMaxNode> nodes;

        // copy constructor
        public MinMaxNode(MinMaxNode copy)
        {
            score = copy.score;

            // pass-by reference
            // boardState = copy.boardState;

            // copies the contents.
            boardState = copy.boardState.Clone() as boardSymbol[,];

            // copies the attached nodes.
            nodes = new List<MinMaxNode>(copy.nodes);

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
    private int RunNode(MinMaxNode node, int depth, int branchNum, bool yourTurn)
    {
        // Debug.Log("Depth: " + depth);

        // the list of nodes.
        // List<MinMaxNode> nodes = new List<MinMaxNode>();
        node.nodes = new List<MinMaxNode>(); // starts new list.

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
            if(winSymbol != boardSymbol.none) // win/lose case
            {
                // returns terminal value result.
                // favours ending the game as fast as possible.
                // return branchNum + maxBranchLen - depth + ((winSymbol == playerSymbol) ? 1 : -1);

                // favours ending the game as slowly as possible.
                // return branchNum + ((winSymbol == playerSymbol) ? 1 : -1); // originally 10
                // return (branchNum % 10) + ((winSymbol == playerSymbol) ? 1 : -1);

                // favours sooner decisions.

                // yourTurn is currently the opposite of the game ending turn
                // if(yourTurn) // going to choose the lowest
                //     return maxBranchLen - depth * ((winSymbol == playerSymbol) ? 1 : -1);
                // else // going to choose the highest.
                //     return maxBranchLen + depth * ((winSymbol == playerSymbol) ? 1 : -1);
                // return maxBranchLen - depth * ((winSymbol != playerSymbol) ? 1 : -1);

                // return maxBranchLen + depth * ((winSymbol == playerSymbol) ? 1 : -1);
                // return (maxBranchLen - depth) * ((winSymbol == playerSymbol) ? 1 : -1);

                // yourTurn is currently the opposite of the row it will be checked in.
                // this is because turns are changed before this check is done.
                // since this is a turn based game, it will be the losing player's turn when a win is confirmed.

                // if the player wins, make this a high score that it will be chosen (check wll be for max)
                // if the opponent wins, make this a high score so that it won't be chosen (check will be for min)
                // if(yourTurn) // going to choose the lowest.
                //     return (maxBranchLen - depth) * ((winSymbol != playerSymbol) ? 1 : -1);
                // else // going to choose the highest.
                //     return (maxBranchLen - depth) * ((winSymbol == playerSymbol) ? 1 : -1);

                // it's the loser's turn right now.
                // if true, the check being done will be for the highest value. This is the outcome we want.
                // if(winSymbol == playerSymbol) // +1
                // {
                //     return branchNum * (maxBranchLen - depth);
                // }
                // // if false, then the check will be done for the lowest value. This is the outcome we don't want.
                // else // -1
                // {
                //     return  branchNum * (maxBranchLen - depth);
                // }

                // +1 for win, -1 for lose.
                // return branchNum * depth + maxBranchLen - depth + 3 * ((winSymbol == playerSymbol) ? 1 : -1);
                if (winSymbol == playerSymbol)
                    return maxBranchLen - depth + 10;
                else
                    return maxBranchLen - depth - 10;

            }

        }

        // goes through all indexes to get all possible next modes.
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
                    node.nodes.Add(newNode);
                }
            }
        }

        // checks node.nodes for availability.
        if(node.nodes.Count == 0) // no nodes available, so all spots are filled.
        {
            // tie-case
            // return maxBranchLen - depth + 0; // terminal value.
            // return (branchNum % 10) + depth + 0; // terminal value.

            // +0
            // since this is a tie, it should be neutral.
            // going to be lowest check if true.
            // if (yourTurn)
            //     return branchNum;
            // // going to be highest check if true.
            // else
            //     return branchNum * (maxBranchLen - depth / 2);

            // +0
            return maxBranchLen - depth + 0;
        }
        else
        {
            // goes through each node to update the scores.
            for(int i = 0; i < node.nodes.Count; i++)
            {
                // needed to set it up this way since it won't allow the score to change otherwise.
                MinMaxNode temp = node.nodes[i]; // get node.

                // grabs the result from running the node.
                // the branch number is the current branch number times 10, plus the node index plus 1.
                int result = RunNode(node.nodes[i], depth + 1, branchNum * 10 + i + 1, !yourTurn);
                
                temp.score = result; // sets the score.
                node.nodes[i] = temp; // replace item in list.
            }

            // the value that will be returned. 
            int score = node.nodes[0].score; // grabs first score.

            // goes through each node again to find score.
            for (int i = 0; i < node.nodes.Count; i++)
            {
                if(yourTurn) // it's the computer player's turn (find max)
                {
                    // max
                    score = Mathf.Max(node.nodes[i].score, score);

                    // // larger score found.
                    // if (node.nodes[i].score > score)
                    //     score = node.nodes[i].score;
                }
                else // it's the opponent's turn (find min)
                {
                    // min
                    score = Mathf.Min(node.nodes[i].score, score);

                    // // smaller score found.
                    // if (node.nodes[i].score < score)
                    //     score = node.nodes[i].score;
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
        // List<MinMaxNode> nodes = new List<MinMaxNode>();
        rootNode.nodes = new List<MinMaxNode>();// initialize list.

        // the open indexes for the board. It's the same length as the nodes list.
        // (x) = row, (y) = column.
        List<Vector2Int> openIndexes = new List<Vector2Int>();

        // the base score nodes are compared to.
        int baseScore = 0;

        // the chosen spot for the player.
        int nodeIndex = -1;

        // the branch number.
        int branchNum = 0;

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

                    // since it's the player's turn, it's a max node.
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
                    branchNum++; // increase branch number, which is [1, 9]
                    newNode.score = RunNode(newNode, 1, branchNum, false); // going into level 1

                    // adds the new node and index to the lists.
                    rootNode.nodes.Add(newNode);
                    openIndexes.Add(new Vector2Int(row, col));
                }
            }
        }

        // saves the list of rootNode.nodes.
        // this doesn't really serve a purpose.
        // rootNode.rootNode.nodes = rootNode.nodes;

        // gets best score.
        if(rootNode.nodes.Count > 0)
        {
            // grabs 0 index for initial score at the start.
            nodeIndex = 0;
            baseScore = rootNode.nodes[nodeIndex].score;

            // list of best options.
            // if there's more than 1 best option, a random one is chosen.
            List<int> indexBestOptions = new List<int>();

            // goes through each index.
            for (int i = 0; i < rootNode.nodes.Count; i++)
            {
                if(rootNode.nodes[i].score > baseScore) // higher score found.
                {
                    nodeIndex = i; // saves the index.
                    baseScore = rootNode.nodes[i].score; // grab score.
                    
                    // new best option found, so clear out list.
                    indexBestOptions.Clear();

                    // adds the new best option.
                    indexBestOptions.Add(nodeIndex);
                }
                else if(rootNode.nodes[i].score == baseScore) // value found.
                {
                    nodeIndex = i; // saves the index.
                    indexBestOptions.Add(nodeIndex); // add to list.
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
