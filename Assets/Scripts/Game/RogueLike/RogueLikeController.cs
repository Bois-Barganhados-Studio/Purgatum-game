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
        rogueLogic.rogueData.CreateSampleData();
        rogueLogic.SetMainScene(mainScene);
        rogueLogic.SetHubScene(hubScene);
        rogueLogic.SetState(RogueLogic.States.STARTING);
        //Invoke("OnGoingToNextLevel",15f);
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
        //Adicionar instancias no banco
        rogueLogic.AddPlayerSuccess();
        rogueLogic.SetState(RogueLogic.States.NEW_LEVEL);
        rogueLogic.DoAction();
    }

     public void OnFirstRun()
    {
        //Adicionar instancias no banco
        rogueLogic.AddPlayerSuccess();
        rogueLogic.SetState(RogueLogic.States.BOOT_PROCEDURAL);
        rogueLogic.DoAction();
    }

    public void OnGameRestart()
    {
        //Adicionar instancias no banco
        rogueLogic.ClearMap();
        rogueLogic.AddPlayerDeath();
        rogueLogic.SetState(RogueLogic.States.RESTART);
        rogueLogic.DoAction();
    }

    public void OnGameExit()
    {
        rogueLogic.SetState(RogueLogic.States.EXIT);
        rogueLogic.DoAction();
    }

    public int GetActualLevel(){
        return rogueLogic.GetLevel();
    }

    #endregion

}
