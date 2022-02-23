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
        clickedObject = manager.mouse.clickedObject;
    }
}
