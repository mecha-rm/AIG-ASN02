using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages the gameplay.
public class GameplayManager : MonoBehaviour
{
    // the mouse from the gameplay manager.
    public Mouse2D mouse;

    // the board.
    public Board board;

    // if 'true', the numbers are shown.
    public bool showNumbers = true;

    // if the game is running, this is true.
    public bool running = false;

    // cover for the board.
    public GameObject boardCover;

    // the two players.
    [Header("Players")]
    public Player p1;
    public Player p2;

    public Toggle p1SymbolToggle; // toggle for player 1's symbol.
    public GameObject p1SymbolToggleCover; // cover for disabling
    public Toggle p1StartsToggle; // toggle for player 1 going first.
    public bool p1Turn = true; // if 'false', it's p2's turn.

    // statistics
    [Header("Statistics")]
    // current round.
    public int round = 0;

    // player wins
    public int p1Wins = 0;
    public int p2Wins = 0;

    // Statistics UI components.
    [Header("Statistics/UI")]
    public Text roundText;
    public Text p1WinsText;
    public Text p2WinsText;

    // Other UI components.
    [Header("Other UI")]
    // toggle for muting the audio.
    public Toggle muteToggle;

    // the list of the audio.
    // [Header("Audio")]
    // public AudioClip rulesBGM;
    // public AudioClip roundBGM;
    // public AudioClip winBGM;
    // public AudioClip loseBGM;

    // Start is called before the first frame update
    void Start()
    {
        // finds and sets the mouse.
        if (mouse == null)
            mouse = FindObjectOfType<Mouse2D>();

        // finds and sets the board.
        if (board == null)
            board = FindObjectOfType<Board>();

        if(board != null)
        {
            // shows or hides the numbers.
            if (showNumbers)
                board.ShowNumbers();
            else
                board.HideNumbers();
        }

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

        // symbol toggle set.
        if(p1SymbolToggle != null && p1 != null && p2 != null)
        {
            if (p1.playerSymbol == symbol.x) // using x symbol
                p1SymbolToggle.isOn = true;
            else if (p1.playerSymbol == symbol.o) // using o symbol
                p1SymbolToggle.isOn = false;
            else // calls to set value.
                OnPlayer1SymbolToggleChange();
        }

        // toggle for determining who goes first.
        if (p1StartsToggle != null)
            p1Turn = p1StartsToggle.isOn;

        // current setting for sound.
        if (muteToggle != null)
            muteToggle.isOn = AudioListener.pause;
    }

    // called when the mute toggle changes.
    public void OnMuteToggleChange()
    {
        AudioListener.pause = muteToggle.isOn;
    }

    // updates components in the canvas.
    public void UpdateDisplay()
    {
        // display round in text.
        if (roundText != null)
            roundText.text = "Round: " + round.ToString("D3");

        // display p1 wins in text.
        if (p1WinsText != null)
            p1WinsText.text = "Player 1 Wins: " + p1Wins.ToString("D3");

        // display p2 wins in text.
        if (p2WinsText != null)
            p2WinsText.text = "Player 2 Wins: " + p2Wins.ToString("D3");

        // hides the board cover.
        if (boardCover != null)
            boardCover.SetActive(!running);
    }

    // called when the player 1 symbol toggle changes.
    public void OnPlayer1SymbolToggleChange()
    {
        // can't change symbol when the game is running.
        if (running)
            return;

        // players not set.
        if (p1 == null || p2 == null)
            return;

        // change symbol.
        if (p1SymbolToggle.isOn) // x symbol
        {
            p1.playerSymbol = symbol.x;
            p2.playerSymbol = symbol.o;
        }
        else // o symbol
        {
            p1.playerSymbol = symbol.o;
            p2.playerSymbol = symbol.x;
        }
    }

    // starts the round.
    public void StartRound()
    {
        round++;
        running = true;

        // checks who should start first.
        if (p1StartsToggle != null)
        {
            p1Turn = p1StartsToggle.isOn;
            p1StartsToggle.interactable = false;
        }
        else
        {
            p1Turn = true; // player 1 starts.
        }

        // disables toggle.
        if (p1SymbolToggle != null)
        {
            p1SymbolToggle.interactable = false;

            // puts cover to show its disabled.
            if (p1SymbolToggleCover != null)
                p1SymbolToggleCover.SetActive(!p1SymbolToggle.interactable);
        }
            

        
        board.ClearBoard();

        // shows or hides the numbers.
        if (showNumbers)
            board.ShowNumbers();
        else
            board.HideNumbers();

        // updates the display.
        UpdateDisplay();
    }

    // stops the round.
    public void StopRound()
    {
        running = false;

        // enables toggle for player symbol.
        if (p1SymbolToggle != null)
        {
            // now can interact
            p1SymbolToggle.interactable = true;

            // hide cover to show its enabled.
            if (p1SymbolToggleCover != null)
                p1SymbolToggleCover.SetActive(!p1SymbolToggle.interactable);
        }
            

        // enables toggle for who goes first.
        if (p1StartsToggle != null)
            p1StartsToggle.interactable = true;

        // updates the display.
        UpdateDisplay();
    }

    // clears out the data
    public void ClearData()
    {
        round = 0;
        p1Wins = 0;
        p2Wins = 0;

        UpdateDisplay();
    }

    // returns to the title screen.
    public void ReturnToTitle()
    {
        SceneHelper.ChangeScene("TitleScene");
    }

    // quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        // checks to see if the game is running
        if(running)
        {
            BoardIndex index;
            index = (p1Turn) ? p1.GetChosenIndex() : p2.GetChosenIndex(); // TODO: change players.

            // tries to get the door component from the object.
            if (index != null)
            {
                // the index is available.
                if (index.IsAvailable())
                {
                    // the current symbol
                    symbol currSym = (p1Turn) ? p1.playerSymbol : p2.playerSymbol;
                    index.SetIndexSymbol(currSym); // set to x for now.

                    // checks if the board has a winner.
                    if (board.HasWinner(currSym))
                    {
                        Debug.Log((p1Turn) ? "Player 1 Wins" : "Player 2 Wins");

                        // updates win count.
                        if (p1Turn)
                            p1Wins++;
                        else
                            p2Wins++;

                        StopRound();
                    }
                    else if (board.IsFull()) // no winner, so check if the game is over.
                    {
                        Debug.Log("Tie.");
                        StopRound();
                    }
                    else
                    {
                        // change the turn.
                        p1Turn = !p1Turn;
                    }
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


        // toggles the visibility of the number sprites.
        if (Input.GetKeyDown(KeyCode.H))
        {
            showNumbers = !showNumbers;

            // shows/hides the numbers.
            if (showNumbers)
                board.ShowNumbers();
            else
                board.HideNumbers();
        }
            
        
    }
}
