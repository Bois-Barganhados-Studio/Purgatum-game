using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundControl : MonoBehaviour
{
    public static float globalSoundVolume = 0.0f;
    public static float globalSongVolume = 0.0f;

    public AudioClip[] soundEffects;
    public string[] soundEffectNames;
    private AudioSource[] audioSources; 

    public AudioClip[] songs;    
    public string[] songNames;
    public List<AudioClip> battleSongs = null;

    private AudioSource audioSource  = null;
    private AudioSource ambience  = null;
    private AudioSource battleSong  = null;

    public void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        ambience = gameObject.AddComponent<AudioSource>();
        battleSong = gameObject.AddComponent<AudioSource>();
        PlayAmbience();

        //Adiciona um AudioSource para cada efeito sonoro
        audioSources = new AudioSource[soundEffects.Length];
        for (int i = 0; i < soundEffects.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = soundEffects[i];
        }

        int qtdBattleSongs = 9;
        battleSongs = new List<AudioClip>();
        for (int i = 0; i < qtdBattleSongs; i++)
        {
            battleSongs.Add(Resources.Load<AudioClip>("Sounds/Songs/Battle_song_" + i));
            if(battleSongs[i] == null)
            {
                Debug.LogError("Battle song not found: " + i);
            }
        }
    }

    public void PlayEffect(int index, bool loop=false){
        if (index < 0 || index >= songs.Length)
        {
            Debug.LogError("Song index out of range: " + index);
            return;
        }
        audioSource.clip = soundEffects[index];
        audioSource.loop = loop;
        audioSource.volume = globalSoundVolume;
        audioSource.Play();
    }

    public void PlaySoundEffect(string name, bool loop = false)
    {
        int index = System.Array.IndexOf(soundEffectNames, name);
        if (index < 0)
        {
            Debug.LogError("SoundEffect name not found: " + name);
            return;
        }


        if(!audioSources[index].isPlaying)
        {
            audioSources[index].loop = loop;
            audioSources[index].volume = globalSoundVolume;
            audioSources[index].Play();
        }       

        //PlaySoundEffectAt(index, new Vector3(0, 0, 0));
    }

    public void PlaySoundEffect(int index)
    {
        PlaySoundEffectAt(index, new Vector3(0, 0, 0));
    }

    public void PlaySoundEffectAt(string name, Vector3 position)
    {
        int index = System.Array.IndexOf(soundEffectNames, name);
        if (index < 0)
        {
            Debug.LogError("SoundEffect name not found: " + name);
            return;
        }
        PlaySoundEffectAt(index, position);
        
    }
    
    public void PlaySoundEffectAt(int index, Vector3 position)
    {
        if (index < 0 || index >= soundEffects.Length)
        {
            Debug.LogError("SoundEffect index out of range: " + index);
            return;
        }
        if(!audioSources[index].isPlaying){
            audioSources[index].volume = globalSoundVolume;
            audioSources[index].loop = false;
            audioSources[index].Play();
        }
        
    }

    public void PlaySong(string name, bool loop = false)
    {
        int index = System.Array.IndexOf(songNames, name);
        if (index < 0)
        {
            Debug.LogError("SoundEffect name not found: " + name);
            return;
        }
        print("PlaySong: " + name);
        PlaySong(index, loop);
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

    public void PlayAmbience(bool loop = true)
    {
        AudioClip ambienceClip = songs[0];
        if (ambienceClip == null)
        {
            Debug.LogError("Ambience clip not found");
            return;
        }
        if(!ambience.isPlaying){   
        ambience.clip = ambienceClip;
        ambience.volume = globalSongVolume;
        ambience.loop = loop;
        ambience.Play();
        }
    }

    public void StopAmbience()
    {
        ambience.Stop();
    }

    public void PlayBattleSong(bool replaceSong = false,bool loop = true)
    {
        Debug.Log("Play battle song");
        AudioClip battle = battleSongs[Random.Range(0, battleSongs.Count)];
        if (battle == null)
        {
            Debug.LogError("BattleSong clip not found");
            return;
        }
        if(!battleSong.isPlaying || replaceSong){
            battleSong.clip = battle;
            battleSong.volume = globalSongVolume;
            battleSong.loop = loop;
            battleSong.Play();
            //StartCoroutine(FadeAudioSource.StartFade(battleSong, 1.0f, globalSongVolume));
        }
        // else if(replaceSong && battleSong.isPlaying)
        // {
        //     Debug.Log("Entrando na ChangeBattleSong");
        //     ChangeBattleSong(battle, loop);
            
        // }
        
        
    }
    public void StopBattleSong()
    {
        StartCoroutine(FadeAudioSource.StartFade(battleSong, 1.0f, 0.0f));
    }

    public void ChangeBattleSong(AudioClip song, bool loop = true)
    {
        Debug.Log("Change battle song");
        StartCoroutine(FadeChangeAudioSource.StartChange(battleSong, song , 1.0f));
        // StartCoroutine(FadeAudioSource.StartFade(battleSong, 1.0f, 0.0f));
        // Debug.Log("tmp: " + audioSource + " battleSong: " + battleSong + " song: " + song);
        // audioSource.clip = song;
        // audioSource.volume = 0.0f;;
        // audioSource.loop = loop;
        // //audioSource.Play();
        // StartCoroutine(FadeAudioSource.StartFade(battleSong, 1.0f, globalSongVolume));
        // battleSong = audioSource;

        // return song.name;
    }

    private void Update() {
        Debug.Log(battleSong.name + " " + battleSong.isPlaying);    
    }

    public void SetGlobalSoundVolume(float volume)
    {
        SoundControl.globalSoundVolume = volume;
        SoundControl.globalSongVolume = volume;
        audioSource.volume = volume;
        ambience.volume = volume;
        battleSong.volume = volume;
    }

}

public static class FadeAudioSource {
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if(targetVolume == 0.01f)
            audioSource.Stop();
        yield break;
    }
}

public static class FadeChangeAudioSource {
    public static IEnumerator StartChange(AudioSource audioSource, AudioClip changeAudio, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        float targetVolume = 0.0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.clip = changeAudio;
        audioSource.volume = start;
        audioSource.loop = true;
        audioSource.Play();
        yield break;
    }
}