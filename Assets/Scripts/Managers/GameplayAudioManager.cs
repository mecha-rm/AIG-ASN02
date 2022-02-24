using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio manager for gameplay title.
public class GameplayAudioManager : AudioManager
{
    // BACKGROUND MUSIC //
    // changes to the rules BGM.
    public void ChangeToRulesBgm()
    {
        ChangeBackgroundMusic(0);
    }
    
    // changes to the round BGM.
    public void ChangeToRoundBgm()
    {
        ChangeBackgroundMusic(1);
    }

    // changes to the win BGM.
    public void ChangeToWinBgm()
    {
        ChangeBackgroundMusic(2);
    }

    // changes to the tie BGM.
    public void ChangeToTieBgm()
    {
        ChangeBackgroundMusic(3);
    }


    // SOUND EFFECTS //
    // plays the button sound effect.
    public void PlayButtonSfx()
    {
        PlaySoundEffect(0);
    }

    // plays the board index success effect.
    public void PlayBoardIndexSuccessSfx()
    {
        PlaySoundEffect(1);
    }

    // plays the board index fail effect.
    public void PlayBoardIndexFailSfx()
    {
        PlaySoundEffect(2);
    }


    // JINGLES //
    // plays the win jingle effect.
    public void PlayWinJng()
    {
        PlayJingle(0);
    }

    // plays the tie jingle effect.
    public void PlayTieJng()
    {
        PlayJingle(1);
    }
}
