using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueLikeController : MonoBehaviour
{
    private static RogueLikeController instance;
    private RogueLogic rogueLogic;
    [SerializeField] private int mainScene;
    [SerializeField] private int hubScene;

    
    void Start()
    {
        //Carrega os dados de configurações
        Settings settings = DataSaver.LoadData<Settings>("settings.boi", createIfNotExists: true);
        SoundControl soundControl = FindObjectOfType<SoundControl>();
        soundControl.SetGlobalSoundVolume(settings.Volume);

        //*********************************************//

            
        rogueLogic = new RogueLogic();
        rogueLogic.SetMainScene(mainScene);
        rogueLogic.SetHubScene(hubScene);
        rogueLogic.SetState(RogueLogic.States.BOOT_PROCEDURAL);
        //Invoke("OnGoingToNextLevel", 70f);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Eventos

    public void OnGameStart()
    {
        rogueLogic.DoAction();
    }
    public void OnGoingToNextLevel()
    {
        Debug.Log("GOING TO NEXT LEVEL");
        rogueLogic.SetState(RogueLogic.States.NEW_LEVEL);
        rogueLogic.DoAction();
    }

    public void OnGameRestart()
    {
        rogueLogic.SetState(RogueLogic.States.RESTART);
        rogueLogic.DoAction();
    }

    public void OnGameExit()
    {
        rogueLogic.SetState(RogueLogic.States.EXIT);
        rogueLogic.DoAction();
    }

    #endregion

}
