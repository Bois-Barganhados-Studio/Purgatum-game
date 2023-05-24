using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RogueLogic
{
    public enum States
    {
        STARTING = -1,
        RESTART = 0,
        NEW_LEVEL = 1,
        PLAYING = 2,
    };

    private States state = States.STARTING;
    private const int MAX_LEVELS = 7;
    private const int MAX_ROOMS = 16;
    private const int MIN_ROOMS = 2;
    private const int SZ_ROOM_STYLE = 5;
    private const int MAX_ORIGIN = 16;
    private const int MIN_ORIGIN = 0;
    private static int level = 0;
    private const int HUB_INDEX = 0;
    private const int BOSS_INDEX = 7;
    private GameObject renderLevelsObj = null;
    private ProceduralMapBuilder mapBuilder = null;
    //private AI ia = null;

    public RogueLogic()
    {
        //ia = new AI();
        state = States.STARTING;
    }

    /**
     * Cria um novo level para o jogo com as regras para inicio do game
     */
    private async Task<bool> NewLevel(LevelData levelData)
    {
        bool status = true;
        try
        {
            if (level == HUB_INDEX)
            {
                //StartHub();
            }
            else if (level < MAX_LEVELS)
            {
                if (renderLevelsObj == null)
                {
                    renderLevelsObj = GameObject.Find("renderLevels");
                    mapBuilder = renderLevelsObj.GetComponent<ProceduralMapBuilder>();
                }
                mapBuilder.SetLevelData(levelData);
                if (await mapBuilder.NewLevel())
                {
                    state = States.PLAYING;
                    level++;
                }
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

    private void ResetLevel()
    {
        level = 0;
        state = States.RESTART;
    }

    public async void DoAction()
    {
        switch ((int)state)
        {
            case -1:
                level = 1;
                await NewLevel(new LevelData(5, 0, 0, 5, null));
                break;
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

}