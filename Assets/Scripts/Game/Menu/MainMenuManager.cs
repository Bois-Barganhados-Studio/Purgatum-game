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

    public void Start()
    {
        soundController = FindObjectOfType<SoundControl>();
        if (soundController != null)
        {
            soundController.PlaySong(menuSong, loop: true);
        }
    }

    public void Jogar()
    {
        soundController.PlaySoundEffect(0);
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void AbrirOpcoes()
    {
        soundController.PlaySoundEffect(0);
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        soundController.PlaySoundEffect(0);
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void Sair()
    {
        soundController.PlaySoundEffect(0);
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }

}
