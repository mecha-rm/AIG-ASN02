using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages the gameplay.
public class GameplayManager : MonoBehaviour
{
    // the audio manager for the game.
    // audio components tied to buttons are attached to them.
    // audio components tied to game events are done in the scripts.
    public GameplayAudioManager audioManager;

    // the mouse from the gameplay manager.
    public Mouse2D mouse;

    // the board.
    public Board board;

    // the last index chosen by a player.
    private BoardIndex lastChosenIndex;

    // if 'true', the numbers are shown.
    public bool showNumbers = true;

    // if the game is running, this is true.
    public bool running = false;

    // cover for the board.
    public GameObject boardCover;

    // player variables.
    [Header("Players")]
    // players
    public Player p1;
    public Player p2;

    // toggle for player 1's symbol.
    public Toggle p1SymbolToggle;

    // cover for disabling
    public GameObject p1SymbolToggleCover;

    // toggle for player 1 going first.
    public Toggle p1StartsToggle;

    // if 'false', it's p2's turn.
    public bool p1Turn = true; 

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
    public Text p1WinCountText;
    public Text p2WinCountText;

    // Other UI components.
    [Header("Other UI")]
    // toggle for muting the audio.
    public Toggle muteToggle;

    // placeholder text for the status element.
    public Text statusPlaceholderText;

    // win text for the player.
    public Text playerWinText;

    // the text for showing a tie.
    public Text tieText;

    // Start is called before the first frame update
    void Start()
    {
        // finds the audio manager.
        if (audioManager == null)
            audioManager = FindObjectOfType<GameplayAudioManager>();

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
            if (p1.playerSymbol == boardSymbol.x) // using x symbol
                p1SymbolToggle.isOn = true;
            else if (p1.playerSymbol == boardSymbol.o) // using o symbol
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
        if (p1WinCountText != null)
            p1WinCountText.text = "Player 1 Wins: " + p1Wins.ToString("D3");

        // display p2 wins in text.
        if (p2WinCountText != null)
            p2WinCountText.text = "Player 2 Wins: " + p2Wins.ToString("D3");

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
            p1.playerSymbol = boardSymbol.x;
            p2.playerSymbol = boardSymbol.o;
        }
        else // o symbol
        {
            p1.playerSymbol = boardSymbol.o;
            p2.playerSymbol = boardSymbol.x;
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
            // p1StartsToggle.interactable = false; // can now be switched in-betwee
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

        // shows status text placeholder
        if (statusPlaceholderText != null)
            statusPlaceholderText.gameObject.SetActive(true);

        // hides the player win text.
        if (playerWinText != null)
            playerWinText.gameObject.SetActive(false);

        // hies the tie text.
        if (tieText != null)
            tieText.gameObject.SetActive(false);

        // clears out the board.
        board.ClearBoard();

        // shows or hides the numbers.
        if (showNumbers)
            board.ShowNumbers();
        else
            board.HideNumbers();

        // updates the display.
        UpdateDisplay();

        // change bgm.
        if (audioManager != null)
            audioManager.ChangeToRoundBgm();
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
        // if (p1StartsToggle != null)
        //     p1StartsToggle.interactable = true;

        // updates the display.
        UpdateDisplay();

        // change bgm.
        if (audioManager != null)
            audioManager.ChangeToRulesBgm();
    }

    // called when a player wins.
    public void OnWin(bool p1Won)
    {
        // shows which person wins.
        // Debug.Log((p1Won) ? "P1 Wins." : "P2/Computer Wins.");

        // updates win count.
        if (p1Won)
            p1Wins++;
        else
            p2Wins++;

        // if the player win text 
        if(statusPlaceholderText != null && playerWinText != null && tieText != null)
        {
            playerWinText.text = (p1Won) ? "Player 1 Wins!" : "Player 2 Wins!";

            // show proper text.
            statusPlaceholderText.gameObject.SetActive(false);
            playerWinText.gameObject.SetActive(true);
            tieText.gameObject.SetActive(false);
        }

        // stop the round.
        StopRound();

        // change bgm.
        if (audioManager != null)
            audioManager.ChangeToWinBgm();
    }

    // called when the game ends in a tie.
    public void OnTie()
    {
        // message to show tie.
        // Debug.Log("Tie.");

        // if the player win text 
        if (statusPlaceholderText != null && playerWinText != null && tieText != null)
        {
            statusPlaceholderText.gameObject.SetActive(false);
            playerWinText.gameObject.SetActive(false);
            tieText.gameObject.SetActive(true);
        }

        // stop the round.
        StopRound();

        // change bgm.
        if (audioManager != null)
            audioManager.ChangeToTieBgm();
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
            BoardIndex index = null;
            index = (p1Turn) ? p1.GetChosenIndex() : p2.GetChosenIndex(); // TODO: change players.

            // tries to get the door component from the object.
            if (index != null)
            {
                // the index is available.
                if (index.IsAvailable())
                {
                    // plays sound.
                    if (audioManager != null)
                    {
                        bool playSound = false;

                        // if it's a computer player the sound doesn't play.
                        // this is because the symbol is placed instantly.
                        if(p1Turn) // player 1's turn
                        {
                            playSound = !(p1 is ComputerPlayer);
                        }
                        else // player 2's turn
                        {
                            playSound = !(p2 is ComputerPlayer);
                        }

                        // plays the sound.
                        if (playSound)
                            audioManager.PlayBoardIndexSuccessSfx();
                    }
                        

                    // the current symbol
                    boardSymbol currSym = (p1Turn) ? p1.playerSymbol : p2.playerSymbol;
                    index.SetIndexSymbol(currSym); // set to x for now.

                    // checks if the board has a winner.
                    if (board.HasWinner(currSym))
                    {
                        // turn aligns with whoever won.
                        OnWin(p1Turn);
                    }
                    else if (board.IsFull()) // no winner, so check if the game is over.
                    {
                        OnTie();
                    }
                    else
                    {
                        // change the turn.
                        p1Turn = !p1Turn;
                    }
                }
                else
                {
                    // plays sound.
                    if (audioManager != null && index != lastChosenIndex)
                        audioManager.PlayBoardIndexFailSfx();
                }
            }

            lastChosenIndex = index;
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
