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
            // copies the score.
            score = copy.score;

            // copies the contents.
            boardState = copy.boardState.Clone() as boardSymbol[,];

            // copies the attached nodes.
            nodes = new List<MinMaxNode>(copy.nodes);
        }
    }

    // if 'true', the computer player uses its AI.
    public bool useAI = false;

    // if 'true' the computer's time to make a decision is printed in terms of ticks.
    [Tooltip("Prints the the time it took for the computer to make decision if true. Time is in ticks.")]
    public bool printDecisionTime = false;

    // the maximum length of a branch.
    private const int maxBranchLen = 9;

    // the maximum depth that the computer can descend down the tree.
    // if set to 0 or less it will be able to form a complete game tree.
    public int maxDepth = -1;

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

    // checks if a node's board is full.
    private bool NodeHasTie(MinMaxNode node)
    {
        // checks for a space.
        for(int row = 0; row < node.boardState.GetLength(0); row++) // row
        {
            for(int col = 0; col < node.boardState.GetLength(1); col++) // col
            {
                // space found, so it's not full, so return false.
                if (node.boardState[row, col] == boardSymbol.none)
                    return false;
            }
        }

        // no space.
        return true;
    }

    // outcome: the outcome of the board.
    //  * >=  1: player (computer) wins
    //  * ==  0: tie
    //  * <= -1: opponent wins
    public int GetNodeScore(int outcome, int depth)
    {
        // as long as the number is 10 or greater its fine.
        if(outcome > 0) // player (computer) wins
        {
            // the sooner the value, the higher it should be.
            return maxBranchLen - depth + 100;
        }
        else if(outcome < 0) // opponent wins
        {
            // the sooner the value, the lower it should be.
            return depth - maxBranchLen - 100;
        }
        else // tie
        {
            return maxBranchLen - depth + 0;
        }
    }


    // runs the node and goes through all attached branches.
    // if there are no branches a value is returned.
    private int RunNode(MinMaxNode node, int depth, bool yourTurn)
    {
        // Debug.Log("Depth: " + depth);

        // the list of nodes.
        // List<MinMaxNode> nodes = new List<MinMaxNode>();
        node.nodes = new List<MinMaxNode>(); // starts new list.

        // goes through all indexes to get all possible next nodes.
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
                        symbol = (playerSymbol == boardSymbol.x) ? boardSymbol.o : boardSymbol.x;
                    }

                    // slots in the symbol.
                    newNode.boardState[row, col] = symbol;

                    // adds the new node to the list.
                    node.nodes.Add(newNode);
                }
            }
        }

        // for checking game ends, the program assumes rounds end as fast as possible.
        // as such, outcomes farther down the branches have lower ratings.

        // checks if there are spaces available in the boards.
        // if no nodes were made, that means there was no space to put them.
        // if there are no spaces available, then that means it's a tie game (winning cases are checked on an earlier iteration).
        if(node.nodes.Count == 0) // no nodes available, so all spots are filled.
        {
            return GetNodeScore(0, depth);
        }
        else
        {
            // checks if there is a winning or tieing node.
            for(int i = 0; i < node.nodes.Count; i++)
            {
                // checks if the game is over.
                bool end = false;

                // WIN CHECK //
                // checks if this node has a game ending combination.
                // x wins
                end = NodeHasWinner(node.nodes[i], boardSymbol.x);

                // o wins
                if (end == false)
                    end = NodeHasWinner(node.nodes[i], boardSymbol.o);

                // checkes winning case (someone has won).
                // this outcome should either be sought after or avoided immediately.
                if (end == true)
                {
                    // the game ends on the turn of the user that either won or tied.
                    // if 'yourTurn' is true then the player (computer) won.
                    // if 'yourTurn' is false then the opponent won.
                    return GetNodeScore((yourTurn ? 1 : -1), depth);
                }

                // TIE CHECK and EARLY END //
                // checks if the board ended in a tie. Also checks if 
                // this isn't used because the check for attached nodes earlier already covers this.
                // if there are no attached nodes then that means the board has ended in a tie.
                if (end == false)
                {
                    end = NodeHasTie(node.nodes[i]);

                    // checks if the max depth has been reached.
                    // if maxDepth is 0 or less then there is no max depth.
                    // if max depth has been reached this node returns as a tie.
                    if (end == false)
                        end = (maxDepth > 0 && depth >= maxDepth);

                    // the game has tied.
                    if(end)
                    {
                        return GetNodeScore(0, depth);
                    }
                }

                // RUN NODE//
                // Runs the node since there are still spots available on the board.
                
                // Copies the node. This is needed because the 'score' parameter cannot be edited through accessing the list directly.
                // A copy is made to perserve the data so that it cannot be altered by travelling further down this branch of the tree.
                MinMaxNode currNode = new MinMaxNode(node.nodes[i]);

                // runs the branches attached to this node, and grabs the result.
                // it also provides the number of the branch.
                int result = RunNode(node.nodes[i], depth + 1, !yourTurn);

                // changes the score to the new result, and puts the new node back into the list.
                currNode.score = result; // replaces old score with new score.
                node.nodes[i] = currNode; // replaces item in the list.
            }

            // checks for the score to keep, which will be stored in this variable. 
            int score = node.nodes[0].score; // grabs first score.

            // goes through each node again to find score.
            for (int i = 0; i < node.nodes.Count; i++)
            {
                if(yourTurn) // it's the computer player's turn (find max)
                {
                    // max - finds largest score.
                    score = Mathf.Max(node.nodes[i].score, score);
                }
                else // it's the opponent's turn (find min)
                {
                    // min - finds smallest score.
                    score = Mathf.Min(node.nodes[i].score, score);
                }
            }

            // returns the final score.
            return score;

        }
    }

    // runs the AI to find the best space to be used.
    private BoardIndex RunMinMaxAI(Board board)
    {
        // the root node that all branches come from.
        MinMaxNode rootNode = new MinMaxNode();

        // initialize root node list.
        rootNode.nodes = new List<MinMaxNode>();

        // the open indexes for the board. It's the same length as the nodes list.
        // this is used for cases where multiple spots are equally as viable.
        // (x) = row, (y) = column.
        List<Vector2Int> openIndexes = new List<Vector2Int>();

        // generates the board.
        rootNode.boardState = board.GenerateBoardSymbolArray();

        // checks all outcomes to see which is the best one.
        for (int row = 0; row < rootNode.boardState.GetLength(0); row++) // row
        {
            for (int col = 0; col < rootNode.boardState.GetLength(1); col++) // column
            {
                // open space.
                if (rootNode.boardState[row, col] == boardSymbol.none)
                {
                    // makes a new node.
                    MinMaxNode newNode = new MinMaxNode(rootNode);

                    // since it's the player's turn, it'll always be a max node.
                    // as such, it just takes in the player's symbol.
                    boardSymbol symbol = playerSymbol; // player symbol.
                    
                    // slot in the symbol for this player's turn.
                    newNode.boardState[row, col] = symbol;

                    // if the computer can win in one move, they will instantly choose that option.
                    // if the game is one turn away from being a tie, the computer will instantly choose that option as well.
                    // basically if the game can be ended in one turn where the computer either wins or gets a tie they will do it.

                    // checks for a win and a tie.
                    if(NodeHasWinner(newNode, symbol) || NodeHasTie(newNode)) // if this is a win or a tie.
                    {
                        // it's the player's turn, so this must be a winning o
                        rootNode.nodes.Clear(); // only one node is needed.
                        rootNode.nodes.Add(newNode); // adds the single available node.

                        openIndexes.Clear(); // only one open index.
                        openIndexes.Add(new Vector2Int(row, col)); // give open index.
                        goto SKIP; // skip the rest.
                    }


                    // calls the function to follow the branch through.
                    newNode.score = RunNode(newNode, 1, false); // going into level 1

                    // adds the new node and index to the lists.
                    rootNode.nodes.Add(newNode);
                    openIndexes.Add(new Vector2Int(row, col));
                }
            }
        }

        // skips out on the rest of the checks since this is a winning or tieing combination.
        SKIP:

        // the available nodes to choose from.
        if(rootNode.nodes.Count > 0)
        {
            // index of the best option.
            int bestOptionIndex = -1;

            // set to the first score to start off.
            int bestScore = rootNode.nodes[0].score;

            // list of best options.
            // if there's more than 1 best option, a random one is chosen.
            List<int> bestOptions = new List<int>();

            // goes through each index.
            for (int i = 0; i < rootNode.nodes.Count; i++)
            {
                // checks for the highest score.
                if(rootNode.nodes[i].score >= bestScore) // same or higher score found.
                {
                    // new high score found, so clear out the list of saved scores.
                    if(rootNode.nodes[i].score > bestScore)
                        bestOptions.Clear();

                    // new best score (or same score).
                    bestScore = rootNode.nodes[i].score;

                    // adds the new best option or one of the best options.
                    bestOptions.Add(i); // save index
                }
                
            }

            
            // grabs the best option from the list.
            if (bestOptions.Count > 1) // multiple options.
            {
                bestOptionIndex = Random.Range(0, bestOptions.Count);
            }
            else if (bestOptions.Count == 0) // no options.
            {
                return null;
            }
            else // only one option.
            {
                bestOptionIndex = 0;
            }
                

            // grabs and returns the board index.
            BoardIndex bi = board.boardArray[
                openIndexes[bestOptions[bestOptionIndex]].x, 
                openIndexes[bestOptions[bestOptionIndex]].y
                ];
            
            // return the index.
            return bi;
        }
        else
        {
            // no nodes to choose from.
            return null;
        }
    }

    // SELECTION FUNCTION //
    // prints the decision time message (time is in ticks).
    private void PrintDecisionTimeMessage(long startTime, long endTime)
    {
        // gets the total time.
        long totalTime = endTime - startTime;
       
        // forms the message.
        string message = string.Format(
            "Start Time: {0} | End Time: {1} | Total Decision Time: {2}",
            startTime.ToString(), endTime.ToString(), totalTime.ToString());

        // prints the message.
        Debug.Log(message);
    }

    // gets the chosen index from the computer player.
    public override BoardIndex GetChosenIndex()
    {
        // gets the board.
        Board board = manager.board;

        // the start time and end time for the computer making a board spot decision.
        long startTime = 0, endTime = 0;

        // checks how the computer should act.
        if (useAI) // if the AI should be used.
        {
            // print the decision time.
            if(printDecisionTime)
                startTime = System.DateTime.Now.Ticks; // start time

            // runs the min-max AI.
            BoardIndex bi = RunMinMaxAI(board);

            // print the decision time.
            if (printDecisionTime)
            {
                endTime = System.DateTime.Now.Ticks; // end time
                PrintDecisionTimeMessage(startTime, endTime); // message
            }
                
            // return index.
            return bi;
        }
        else // AI should not be used.
        {
            // decision should be printed.
            if(printDecisionTime)
                startTime = System.DateTime.Now.Ticks; // start time.


            // goes through each index and picks the next available space.
            for (int i = 0; i < board.boardList.Count; i++)
            {
                // the index is available.
                if (board.boardList[i].IsAvailable())
                {
                    // the decision should be printed.
                    if(printDecisionTime)
                    {
                        endTime = System.DateTime.Now.Ticks; // end time.
                        PrintDecisionTimeMessage(startTime, endTime); // prints message.
                    }


                    return board.boardList[i];
                }
                    
            }
        }

        // no spots available.
        return null;
    }
}
