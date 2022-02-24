using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a manager for the game audio.
public class AudioManager : MonoBehaviour
{
    // if 'true', the title manager is in the start function.
    // this is used to stop sound effects from going off when the game starts.
    protected bool inStart = true;

    // music
    [Header("Background Music")]

    // background music source. Only one bgm at a time is supported.
    public AudioSource bgmSource;

    // list of bgms
    public List<AudioClip> bgmList;

    // the sound effects
    [Header("Sound Effects")]
    public List<AudioSource> sfxList;

    // if 'true', sound effects can play while the game is starting.
    public bool allowSfxsOnStart = false;

    // jingles and fanfares
    [Header("Jingles/Fanfares")]
    public List<AudioSource> jngList;

    // if 'true', sound effects can play while the game is starting.
    public bool allowJngsOnStart = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // inStart = false;
    }

    // changes the background music.
    public void ChangeBackgroundMusic(int index)
    {
        // no source set.
        if (bgmSource == null)
            return;

        // index out of bounds.
        if (index < 0 || index >= bgmList.Count)
            return;

        // new audio clip.
        AudioClip newClip = bgmList[index];

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();
    }

    // plays a sound effect.
    public void PlaySoundEffect(int index)
    {
        // in the start function, so play nothing.
        if (inStart && !allowSfxsOnStart)
            return;

        // index out of bounds.
        if (index < 0 || index >= sfxList.Count)
            return;

        // plays the audio.
        sfxList[index].Play();
    }

    // plays a jingle.
    public void PlayJingle(int index)
    {
        // in the start function, so play nothing.
        if (inStart && !allowJngsOnStart)
            return;

        // index out of bounds.
        if (index < 0 || index >= jngList.Count)
            return;

        // plays the audio.
        jngList[index].Play();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // no longer in the start function, so set this to false.
        inStart = false;
    }
}
