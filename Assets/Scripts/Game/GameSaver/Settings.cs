using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Settings
{
    private float volume;

    public float Volume
    {
        get => volume;
        set {
            if(volume < 0)
                volume = 0;
            else if(volume > 1)
                volume = 1;
            else
                volume = value;
        }
    }

    public Settings()
    {
        volume = 0.5f;
    }

    public Settings(float volume)
    {
        this.volume = volume;
    }
    

    public static Settings Default()
    {
        return new Settings();
    }
}