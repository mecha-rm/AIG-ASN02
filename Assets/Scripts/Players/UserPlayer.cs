using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the user player for the game.
public class UserPlayer : Player
{
    // saves the clicked object from the mouse every frame.
    private GameObject clickedObject;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        // defaults to x symbol.
        if (playerSymbol == symbol.none)
            playerSymbol = symbol.x;
    }

    // gets the chosen index from the user player.
    public override BoardIndex GetChosenIndex()
    {
        // player has clicked on something.
        if (clickedObject != null)
        {
            BoardIndex index;

            // tries to get the index component from the object.
            if (clickedObject.TryGetComponent<BoardIndex>(out index))
            {
                return index;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        // saves the clicked object. There's only one mouse.
        clickedObject = manager.mouse.lastClickedObject;

        // TODO: try to optimize this if possible.
        // checks if any key has been pressed down.
        if(Input.anyKeyDown)
        {
            // index
            BoardIndex index = null;

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) // spot 1
            {
                index = manager.board.GetBoardListIndex(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) // spot 2
            {
                index = manager.board.GetBoardListIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) // spot 3
            {
                index = manager.board.GetBoardListIndex(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) // spot 4
            {
                index = manager.board.GetBoardListIndex(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) // spot 5
            {
                index = manager.board.GetBoardListIndex(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) // spot 6
            {
                index = manager.board.GetBoardListIndex(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) // spot 7
            {
                index = manager.board.GetBoardListIndex(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) // spot 8
            {
                index = manager.board.GetBoardListIndex(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) // spot 9
            {
                index = manager.board.GetBoardListIndex(8);
            }

            // if the index was found.
            if (index != null)
                clickedObject = index.gameObject;
        }

        // didn't work.
        // // checks if the current even is a key press.
        // if(Event.current != null)
        // {
        //     // if this is a key event.
        //     if(Event.current.isKey)
        //     {
        //         // index
        //         BoardIndex index = null;
        // 
        //         // checks for selected key.
        //         switch (Event.current.keyCode)
        //         {
        //             case KeyCode.Alpha1:
        //             case KeyCode.Keypad1: // 1
        //                 index = manager.board.GetBoardListIndex(0);
        //                 break;
        // 
        //             case KeyCode.Alpha2:
        //             case KeyCode.Keypad2: // 2
        //                 index = manager.board.GetBoardListIndex(1);
        //                 break;
        // 
        //             case KeyCode.Alpha3:
        //             case KeyCode.Keypad3: // 3
        //                 index = manager.board.GetBoardListIndex(2);
        //                 break;
        // 
        //             case KeyCode.Alpha4:
        //             case KeyCode.Keypad4: // 4
        //                 index = manager.board.GetBoardListIndex(3);
        //                 break;
        // 
        //             case KeyCode.Alpha5:
        //             case KeyCode.Keypad5: // 5
        //                 index = manager.board.GetBoardListIndex(4);
        //                 break;
        // 
        //             case KeyCode.Alpha6:
        //             case KeyCode.Keypad6: // 6
        //                 index = manager.board.GetBoardListIndex(5);
        //                 break;
        // 
        //             case KeyCode.Alpha7:
        //             case KeyCode.Keypad7: // 7
        //                 index = manager.board.GetBoardListIndex(6);
        //                 break;
        // 
        //             case KeyCode.Alpha8:
        //             case KeyCode.Keypad8: // 8
        //                 index = manager.board.GetBoardListIndex(7);
        //                 break;
        // 
        //             case KeyCode.Alpha9:
        //             case KeyCode.Keypad9: // 9
        //                 index = manager.board.GetBoardListIndex(8);
        //                 break;
        //         }
        // 
        //         // if the index was found.
        //         if (index != null)
        //             clickedObject = index.gameObject;
        //     } 
        // }
    }
}
