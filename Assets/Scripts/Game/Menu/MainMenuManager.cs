using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioClip menuSong;
    private SoundControl soundController =  null;
    [SerializeField] private string nomeCenaJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelCreditos;

    public void Start()
    {
        soundController = FindObjectOfType<SoundControl>();
    }

    public void Jogar()
    {
        soundController.PlaySoundEffect("click");
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void AbrirOpcoes()
    {
        soundController.PlaySoundEffect("click");
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        Settings settings = DataSaver.LoadData<Settings>("settings.boi");
        settings.Volume = SoundControl.globalSoundVolume;
        DataSaver.SaveData("settings.boi", settings);

        soundController.PlaySoundEffect("click");        
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void AbrirCreditos()
    {
        soundController.PlaySoundEffect("click");
        painelMenuInicial.SetActive(false);
        painelCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        Settings settings = DataSaver.LoadData<Settings>("settings.boi");
        settings.Volume = SoundControl.globalSoundVolume;
        DataSaver.SaveData("settings.boi", settings);

        soundController.PlaySoundEffect("click");        
        painelCreditos.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void Sair()
    {
        soundController.PlaySoundEffect("click");
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }

}
