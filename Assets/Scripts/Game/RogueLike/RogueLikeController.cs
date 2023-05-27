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
        Debug.Log("RogueLikeController iniciou");
        rogueLogic = new RogueLogic();
        rogueLogic.SetMainScene(mainScene);
        rogueLogic.SetHubScene(hubScene);
        //carregar dados da IA e estatisticas do jogador da memoria
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

    public void OnGameStart()
    {
        Debug.Log("Iniciando o jogo");
        rogueLogic.DoAction();
    }

    //Criar eventos para cada ação do jogador

}
