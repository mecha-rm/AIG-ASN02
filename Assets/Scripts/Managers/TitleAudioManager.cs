using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio manager for the title.
public class TitleAudioManager : AudioManager
{
    // plays the button sound effect.
    public void PlayButtonSfx()
    {
        PlaySoundEffect(0);
    }
}
