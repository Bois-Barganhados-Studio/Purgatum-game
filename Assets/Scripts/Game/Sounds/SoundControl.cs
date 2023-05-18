using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public static float globalSoundVolume = 1f;
    public static float globalSongVolume = 1f;

    public AudioClip[] soundEffects;
    public AudioClip[] songs;

    private AudioSource audioSource  = null;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(int index)
    {
        PlaySoundEffectAt(index, new Vector3(0, 0, 0));
    }

    public void PlaySoundEffectAt(int index, Vector3 position)
    {
        if (index < 0 || index >= soundEffects.Length)
        {
            Debug.LogError("SoundEffect index out of range: " + index);
            return;
        }

        AudioSource.PlayClipAtPoint(soundEffects[index], position, globalSoundVolume);
    }

    public void PlaySong(int index, bool loop = false)
    {
        if (index < 0 || index >= songs.Length)
        {
            Debug.LogError("Song index out of range: " + index);
            return;
        }

        PlaySong(songs[index], loop);
    }

    public void PlaySong(AudioClip song, bool loop = false)
    {
        
        audioSource.clip = song;
        audioSource.volume = globalSongVolume;
        audioSource.loop = loop;
        audioSource.Play();
    }

}
