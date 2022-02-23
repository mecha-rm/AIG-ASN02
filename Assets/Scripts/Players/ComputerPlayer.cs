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
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
