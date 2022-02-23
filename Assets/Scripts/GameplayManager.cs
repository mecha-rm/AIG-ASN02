using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages the gameplay.
public class GameplayManager : MonoBehaviour
{
    // the mouse from the gameplay manager.
    public Mouse2D mouse;

    // the board.
    public Board board;

    // if the game is running, this is true.
    public bool running;

    // the two players.
    [Header("Players")]
    public Player p1;
    public Player p2;

    public bool p1Turn = true; // if 'false', it's p2's turn.

    // Start is called before the first frame update
    void Start()
    {
        // finds and sets the mouse.
        if (mouse == null)
            mouse = FindObjectOfType<Mouse2D>();

        // finds and sets the board.
        if (board == null)
            board = FindObjectOfType<Board>();

        // checks for players
        if(p1 == null || p2 == null)
        {
            // finds all players in the scene. There should only be two.
            Player[] players = FindObjectsOfType<Player>(true);

            // checks for right amount of players.
            if (players.Length != 2)
            {
                Debug.LogError("Not enough players in the scene exist.");
            }
            else
            {
                // saves the players. For some reason the com player is in index 0, and the user player is in index 1.
                p1 = players[0];
                p2 = players[1];

                // makes sure the user-controlled player is p1.
                if(p2.GetType() == typeof(UserPlayer) && p1.GetType() == typeof(ComputerPlayer))
                {
                    Player p0 = p1;
                    p1 = p2;
                    p2 = p0;
                }
            }
        }
    }

    // quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        BoardIndex index;
        index = (p1Turn) ? p1.GetChosenIndex() : p2.GetChosenIndex(); // TODO: change players.

        // tries to get the door component from the object.
        if (index != null)
        {
            // the index is available.
            if(index.IsAvailable())
            {
                // the current symbol
                symbol currSym = (p1Turn) ? p1.playerSymbol : p2.playerSymbol;
                index.SetIndexSymbol(currSym); // set to x for now.

                // checks if the board has a winner.
                if (board.HasWinner(currSym))
                {
                    // checks
                    Debug.Log("WIN");
                }
                else if (board.IsFull()) // no winner, so check if the game is over.
                {
                    Debug.Log("Board Full.");
                }

                // change the turn.
                p1Turn = !p1Turn;
            }
        }

        // // player has clicked on something.
        // if ( mouse.clickedObject != null)
        // {
        //     BoardIndex index;
        // 
        //     // tries to get the door component from the object.
        //     if (mouse.clickedObject.TryGetComponent<BoardIndex>(out index))
        //     {
        //         // the current symbol (TODO: get from current player.)
        //         symbol currSym = symbol.x;
        // 
        //         index.SetIndexSymbol(currSym); // set to x for now.
        // 
        //         // checks if the board has a winner.
        //         if(board.HasWinner(currSym))
        //         {
        //             // checks
        //             Debug.Log("WIN");
        //         }
        //         else if(board.IsFull()) // no winner, so check if the game is over.
        //         {
        //             Debug.Log("Board Full.");
        //         }
        // 
        //     }
        // 
        // }
    }
}
