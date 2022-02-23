using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player class for the game.
public abstract class Player : MonoBehaviour
{
    // the symbol used by the player
    public symbol playerSymbol;

    // the gameplay manager.
    public GameplayManager manager;

    // Start is called before the first frame update
    protected void Start()
    {
        // finds the gameplay manager if not in the scene.
        if (manager == null)
            manager = FindObjectOfType<GameplayManager>();
    }

    // gets the chosen index from the player.
    public abstract BoardIndex GetChosenIndex();
}
