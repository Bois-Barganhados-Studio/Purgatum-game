using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NextLevelCaller : MonoBehaviour
{
    public void GoToNextLevel()
    {
        RogueLikeController rogueController = FindObjectOfType<RogueLikeController>();

        if (rogueController != null)
        {
            rogueController.OnGoingToNextLevel();
        }
    }
}
