using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skillpoints : MonoBehaviour
{
    public GameObject container;
    private PlayerObject player;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI vitText;
    public TextMeshProUGUI strText;
    public TextMeshProUGUI agiText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI luckText;
    public TextMeshProUGUI speedText;

    public /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Debug.Log("Starting UI");
    }
    public void open()
    {
        player = GameObject.FindObjectOfType<PlayerObject>();
        container.SetActive(true);

        //setUIPoints(pontos disponiveis)
        setVitalityUiPoints(player.player.Vitality, 100);
        setStrengthUiPoints(player.player.Strength, 100);
        setAgilityUiPoints(player.player.Agility, 100);
        setDefenseUiPoints(player.player.Defense, 100);
        setLuckUiPoints(player.player.Luck, 100);
        setSpeedUiPoints(player.player.Speed, 100);
    }

    public void close()
    {
        container.SetActive(false);
    }

    public void setUIPoints(int points)
    {
        pointsText.text = points.ToString();
    }

    public void setVitalityUiPoints(int points, int max)
    {
        string value = points.ToString() + "/" + max.ToString();

        vitText.text = value;
    }

    public void setStrengthUiPoints(int points, int max)
    {
        string value = points.ToString() + "/" + max.ToString();

        strText.text = value;
    }
    public void setAgilityUiPoints(int points, int max)
    {
        string value = points.ToString() + "/" + max.ToString();

        agiText.text = value;
    }
    public void setDefenseUiPoints(int points, int max)
    {
        string value = points.ToString() + "/" + max.ToString();

        defText.text = value;
    }
    public void setLuckUiPoints(int points, int max)
    {
        string value = points.ToString() + "/" + max.ToString();

        luckText.text = value;
    }
    public void setSpeedUiPoints(int points, int max)
    {
        string value = points.ToString() + "/" + max.ToString();

        speedText.text = value;
    }

    public void addVitalityPoint() {
        if (!player) return;

        //checar se tem ponto
        player.player.Vitality += 1;
    }

    public void addStrengthPoint() {
        if (!player) return;

        //checar se tem ponto
        player.player.Strength += 1;

        //setUIPoints(novos pontos)
    }

    public void addAgilityPoint()
    {
        if (!player) return;

        //checar se tem ponto
        player.player.Agility += 1;

        //setUIPoints(novos pontos)
    }

    public void addDefensePoint()
    {
        if (!player) return;

        //checar se tem ponto
        player.player.Defense += 1;

        //setUIPoints(novos pontos)
    }

    public void addLuckPoint()
    {
        if (!player) return;

        //checar se tem ponto
        player.player.Luck += 1;

        //setUIPoints(novos pontos)
    }

    public void addSpeedPoint()
    {
        if (!player) return;

        //checar se tem ponto
        player.player.Speed += 1;

        //setUIPoints(novos pontos)
    }

}
