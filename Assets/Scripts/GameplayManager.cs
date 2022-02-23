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

    // Start is called before the first frame update
    void Start()
    {
        // finds and sets the mouse.
        if (mouse == null)
            mouse = FindObjectOfType<Mouse2D>();

        // finds and sets the board.
        if (board == null)
            board = FindObjectOfType<Board>();
    }

    // quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        // player has clicked on something.
        if (mouse.clickedObject != null)
        {
            BoardIndex index;

            // tries to get the door component from the object.
            if (mouse.clickedObject.TryGetComponent<BoardIndex>(out index))
            {
                // the current symbol (TODO: get from current player.)
                symbol currSym = symbol.x;

                index.SetIndexSymbol(currSym); // set to x for now.

                // checks if the board has a winner.
                if(board.HasWinner(currSym))
                {
                    // checks
                    Debug.Log("WIN");
                }
                else if(board.IsFull()) // no winner, so check if the game is over.
                {
                    Debug.Log("Board Full.");
                }

            }

        }
    }
}
