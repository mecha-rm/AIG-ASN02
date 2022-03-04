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

    /// <summary>
    /// NOTE: there was a problem where the fail SFX would play a split moment after the success SFX.
    /// I assume it happens because the chosen index is still selected, but is now filled, causing a failed selection.
    /// Since the computer player choses instantly after the user player, the fail sound initiates basically right away.
    /// I made it so that the two sounds are not allowed to overlap.
    /// It's not the best fix, but it works.
    /// </summary>


    // plays the board index success effect.
    public void PlayBoardIndexSuccessSfx()
    {
        // if the fail sound is playing, don't play this.
        if (!sfxList[2].isPlaying)
            PlaySoundEffect(1);
    }

    // plays the board index fail effect.
    public void PlayBoardIndexFailSfx()
    {
        // if the success sound is playing, don't play this.
        if(!sfxList[1].isPlaying)
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
