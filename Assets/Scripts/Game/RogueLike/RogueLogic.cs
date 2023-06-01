using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class RogueLogic
{
    public enum States
    {
        STARTING = -1,
        NEW_LEVEL = 0,
        RESTART = 1,
        PLAYING = 2,
        BOOT_PROCEDURAL = 3,
        EXIT = 4
    };
    private const int MAX_LEVELS = 7, HUB_INDEX = 0, BOSS_INDEX = 7, MIN_RUNS_TO_AI = 0;
    private ProceduralMapBuilder mapBuilder;
    private static int level = 0;
    private static States state;
    private int actualScene = 0, mainScene, hubScene;
    private LoadingScreen loading;

    WorldData currentWorldData = null;

    public RogueData rogueData = null;

    private AIController aiController = null;

    public RogueLogic(RogueData rogueData = null)
    {
        if(rogueData == null)
        {
            rogueData = new RogueData();
        }

        state = States.STARTING;
        this.rogueData = rogueData;
        aiController = new AIController();
        //this.rogueData.CreateSampleData();
        
        //aiController.SetData(this.rogueData);
        //aiController.Train();
        //aiController.TestAI();
    }

    #region Acoes do RogueLike
    //<summary>
    //  Inicia um novo level proceduralmente.
    //</summary>
    private async Task StartNewProceduralLevel(bool boot = true)
    {
        if (mapBuilder == null)
            mapBuilder = GameObject.FindObjectOfType<ProceduralMapBuilder>();
        
        Debug.Log("Contagem death: " + this.rogueData.GetDeathCount() + " contagem Vida: " + this.rogueData.GetSurviveCount());
        //Gera o mundo aleatoriamente ou por IA, dependendo de quantas runs o player já teve
        if(this.rogueData.GetDeathCount() <= (MIN_RUNS_TO_AI/2) || this.rogueData.GetSurviveCount() <= (MIN_RUNS_TO_AI/2))
        {
            Debug.Log("Gerando mundo aleatoriamente....");
            currentWorldData = aiController.GenerateRandomParams();
        }
        else
        {
            currentWorldData = aiController.GenerateWorldParams();
        }
        
        Debug.Log("Gerando novo level proceduralmente: " + currentWorldData.ToString());
        mapBuilder.SetLevelData(currentWorldData.GetLevelData());
        bool response = await mapBuilder.NewLevel(boot);
        if (response)
        {
            state = States.PLAYING;
            level++;
        }
    }

    //<summary>
    //  Cold Boot para iniciar o mapa procedural pela primeira vez
    //</summary>
    private async void StartLevelsFirstTime(Scene scene, LoadSceneMode mode)
    {
        if (mainScene == scene.buildIndex)
        {
            //Remover isso depois
            aiController.SetData(this.rogueData);
            aiController.Train();

            
            await StartNewProceduralLevel();
        }
    }

    //<summary>
    //  Inicializa a cena do hub do jogo.
    //</summary>
    private void StartHubScene()
    {
        actualScene = hubScene;
        if (loading == null)
            loading = GameObject.FindObjectOfType<LoadingScreen>();
        loading.LoadScene(hubScene);
    }

    //<summary>
    //  Inicia a cena principal do jogo.
    //</summary>
    private void StartMainScene()
    {
        actualScene = mainScene;
        if (loading == null)
            loading = GameObject.FindObjectOfType<LoadingScreen>();
        loading.LoadScene(mainScene, StartLevelsFirstTime);
    }

    //<summary>
    //  Reseta o level atual do jogo.
    //</summary>
    private async Task<bool> ResetLevel()
    {
        level = 1;
        state = States.RESTART;
        return await NewLevel();
    }

    
    #endregion

    #region Metodos do RogueLike
    //<summary>
    //  Realiza acoes do rogue like controller de forma geral/universal baseado no
    //  estado atual do jogo.
    //</summary>
    public async void DoAction()
    {
        switch ((int)state)
        {
            case -1:
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
            case 3:
                StartMainScene();
                break;
            case 4:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    /**
    * Cria um novo level para o jogo com as regras para inicio do game
    */
    private async Task<bool> NewLevel()
    {
        bool status = true;
        
        try
        {
            if(this.rogueData.GetDeathCount() > (MIN_RUNS_TO_AI/2) && this.rogueData.GetSurviveCount() > (MIN_RUNS_TO_AI/2))
            {
                Debug.Log("Treinando AI...");
                aiController.SetData(this.rogueData);
                aiController.Train();
            }
            if (level == HUB_INDEX)
            {
                StartHubScene();
            }
            else if (level < MAX_LEVELS)
            {
                await StartNewProceduralLevel(boot: false);
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

    //<summary>
    //  Define um estado para o rogueLike
    //</summary>
    public void SetState(States state)
    {
        RogueLogic.state = state;
    }
    public void SetMainScene(int mainScene)
    {
        this.mainScene = mainScene;
    }

    public void SetHubScene(int hubScene)
    {
        this.hubScene = hubScene;
    }
    #endregion
    
    #region Metodos do banco de Runs

    public void InsertData(RogueData rogueData)
    {
        this.rogueData = rogueData;
        aiController.SetData(this.rogueData);
    }

    public void AddPlayerSuccess()
    {
        this.AddRunData(true);
    }

    public void AddPlayerDeath()
    {
        this.AddRunData(false);
    }

    public void AddRunData(bool playerResult)
    {
        this.rogueData.AddPlayerRunData(currentWorldData, playerResult);
    }
    #endregion
    
}