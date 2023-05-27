using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RogueLogic
{
    public enum States
    {
        STARTING = -1,
        NEW_LEVEL = 0,
        RESTART = 1,
        PLAYING = 2,
    };
    private const int MAX_LEVELS = 7, HUB_INDEX = 0, BOSS_INDEX = 7;
    private ProceduralMapBuilder mapBuilder;
    private static int level = 0;
    private static States state;
    private int actualScene = 0, mainScene, hubScene;
    private LoadingScreen loading;

    //private AI ia = null;

    public RogueLogic()
    {
        //ia = new AI();
        state = States.STARTING;
    }

    /**
     * Cria um novo level para o jogo com as regras para inicio do game
     */
    private async Task<bool> NewLevel()
    {
        bool status = true;
        try
        {
            if (level == HUB_INDEX)
            {
                Debug.Log("Poço scene");
                StartHubScene();
            }
            else if (level < MAX_LEVELS)
            {
                await StartNewProceduralLevel();
            }
            else if (level == BOSS_INDEX)
            {
                //StartBossBattle();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            status = false;
        }
        return status;
    }

    private async Task<bool> StartNewProceduralLevel()
    {
        if (mapBuilder == null)
            mapBuilder = GameObject.FindObjectOfType<ProceduralMapBuilder>();
        //gerar dados da IA ou passar dados gerados aqui!
        mapBuilder.SetLevelData(new LevelData(numOfRooms: 4, roomStyle: 0, origin: 0, randFactor: 0, blueprints: new int[] { 0, 1, 2, 3 }));
        bool response = await mapBuilder.NewLevel();
        if (response)
        {
            state = States.PLAYING;
            level++;
        }
        return response;
    }

    private void StartHubScene()
    {
        actualScene = hubScene;
        if (loading == null)
            loading = GameObject.FindObjectOfType<LoadingScreen>();
        loading.LoadScene(hubScene);
    }

    private void StartMainScene()
    {
        actualScene = mainScene;
        if (loading == null)
            loading = GameObject.FindObjectOfType<LoadingScreen>();
        loading.LoadScene(hubScene);
    }

    private async Task<bool> ResetLevel()
    {
        level = 0;
        state = States.RESTART;
        return await NewLevel();
    }

    public async void DoAction()
    {
        Debug.Log("Doing some action");
        switch ((int)state)
        {
            case -1:
                Debug.Log("poço");
                level = 0;
                await NewLevel();
                break;
            case 0:
                await NewLevel();
                break;
            case 1:
                await ResetLevel();
                break;
            case 2:
                //LogData();
                break;
            default:
                break;
        }
    }

    public void SetMainScene(int mainScene)
    {
        this.mainScene = mainScene;
    }

    public void SetHubScene(int hubScene)
    {
        this.hubScene = hubScene;
    }

}