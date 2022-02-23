using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// symbol enum
public enum symbol { none = 0, x = 1, o = 2 }

// spot in the board.
public class BoardIndex : MonoBehaviour
{
    // the symbol in this board index.
    // TODO: change permissions level.
    public symbol indexSymbol = symbol.none;

    // the sprite for the index.
    public SpriteRenderer sprite;

    // the default sprite.
    public Sprite defaultSprite;

    // the x-sprite
    public Sprite xSprite;

    // the o-sprite
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

    // gets the symbol as an integer.
    public int GetIndexSymbol()
    {
        return (int)indexSymbol;
    }

    // sets the symbol
    public void SetIndexSymbol(symbol newSymbol)
    {
        indexSymbol = newSymbol;

        // changes sprite based on new symbol.
        switch (indexSymbol)
        {
            case symbol.none: // none
                sprite.sprite = null;
                break;
            case symbol.x: // x
                sprite.sprite = xSprite;
                break;
            case symbol.o: // o
                sprite.sprite = oSprite;
                break;
        }
    }

    // sets the symbol
    public void SetIndexSymbol(int newSymbol)
    {
        indexSymbol = (symbol)newSymbol;
    }

    // checks if the board index is available.
    public bool IsAvailable()
    {
        return indexSymbol == symbol.none;
    }

    // hides the number for the board index.
    public void HideNumber()
    {
        // sets this to null so that the sprite is hidden.
        if (sprite.sprite == defaultSprite)
            sprite.sprite = null;
    }

    // shows the number for the board index if it is not set.
    public void ShowNumber()
    {
        // goes back to default sprite.
        if (sprite.sprite == null || sprite.sprite == defaultSprite)
            sprite.sprite = defaultSprite;
    }

    // toggles the visibility of the default sprite, if it should be showing.
    public void ToggleNumber()
    {
        if (sprite.sprite == null) // show
            sprite.sprite = defaultSprite;
        else if (sprite.sprite == defaultSprite) // hide
            sprite.sprite = null;
    }

    // resets the board index.
    public void ResetBoardIndex()
    {
        indexSymbol = symbol.none;
        sprite.sprite = defaultSprite;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
