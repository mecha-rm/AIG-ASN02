using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// board symbol enum
public enum boardSymbol { none = 0, x = 1, o = 2 }

// spot in the board.
public class BoardIndex : MonoBehaviour
{
    // the symbol in this board index. Use the SetIndexSymbol() function to change both the symbol and its sprite.
    [Tooltip("The index symbol for this board index. Use the SetIndexSymbol() function to change the sprite and this variable.")]
    public boardSymbol indexSymbol = boardSymbol.none;

    // the sprite for the index.
    [Tooltip("The sprite renderer for this board index.")]
    public SpriteRenderer sprite;

    // the default sprite.
    [Tooltip("The default sprite used for this board index.")]
    public Sprite defaultSprite;

    // the x-sprite
    [Tooltip("The sprite for the (X) symbol.")]
    public Sprite xSprite;

    // the o-sprite
    [Tooltip("The sprite for the (O) symbol.")]
    public Sprite oSprite;

    // Start is called before the first frame update
    void Start()
    {
        // tries to find sprite.
        if(sprite == null)
        {
            // tries to grab the sprite component from the gameObject.
            if(!gameObject.TryGetComponent<SpriteRenderer>(out sprite))
            {
                // no sprite component found, so check the children.
                sprite = GetComponentInChildren<SpriteRenderer>(true);
            }
        }

        // saves the default sprite for this index.
        if (sprite != null)
            defaultSprite = sprite.sprite;
    }

    // gets the board symbol.
    public boardSymbol GetIndexSymbol()
    {
        return indexSymbol;
    }

    // gets the symbol as an integer.
    public int GetIndexSymbolAsInt()
    {
        return (int)indexSymbol;
    }

    // sets the symbol
    public void SetIndexSymbol(boardSymbol newSymbol)
    {
        indexSymbol = newSymbol;

        // changes sprite based on new symbol.
        switch (indexSymbol)
        {
            case boardSymbol.none: // none
                sprite.sprite = defaultSprite;
                break;
            case boardSymbol.x: // x
                sprite.sprite = xSprite;
                break;
            case boardSymbol.o: // o
                sprite.sprite = oSprite;
                break;
        }
    }

    // sets the symbol
    public void SetIndexSymbol(int newSymbol)
    {
        SetIndexSymbol((boardSymbol)newSymbol);
    }

    // checks if the board index is available.
    public bool IsAvailable()
    {
        return indexSymbol == boardSymbol.none;
    }

    // hides the number for the board index.
    public void HideNumber()
    {
        // sprite object does not exist.
        if (sprite == null)
            return;

        // sets this to null so that the sprite is hidden.
        if (sprite.sprite == defaultSprite)
            sprite.sprite = null;
    }

    // shows the number for the board index if it is not set.
    public void ShowNumber()
    {
        // sprite object does not exist.
        if (sprite == null)
            return;

        // goes back to default sprite.
        if (sprite.sprite == null || sprite.sprite == defaultSprite)
            sprite.sprite = defaultSprite;
    }

    // toggles the visibility of the default sprite, if it should be showing.
    public void ToggleNumber()
    {
        if (sprite.sprite == null) // show
            ShowNumber();
        else if (sprite.sprite == defaultSprite) // hide
            HideNumber();
    }

    // resets the board index.
    public void ResetBoardIndex()
    {
        indexSymbol = boardSymbol.none;
        sprite.sprite = defaultSprite;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
