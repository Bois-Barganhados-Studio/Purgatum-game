using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skillpoints : MonoBehaviour
{
    public GameObject container;
    private Player player;
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
        player = GameObject.FindObjectOfType<PlayerObject>().player;
        container.SetActive(true);

        setUIPoints(player.SkillPoints);
        setVitalityUiPoints(player.Vitality, 100);
        setStrengthUiPoints(player.Strength, 100);
        setAgilityUiPoints(player.Agility, 100);
        setDefenseUiPoints(player.Defense, 100);
        setLuckUiPoints(player.Luck, 100);
        setSpeedUiPoints(player.Speed, 100);
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
        if (player == null || player.SkillPoints < 1) 
            return;

        player.Vitality += 1;
        setUIPoints(--player.SkillPoints);
        setVitalityUiPoints(player.Vitality, 100);
    }

    public void addStrengthPoint() {
        if (player == null || player.SkillPoints < 1)
            return;

        player.Strength += 1;
        setUIPoints(--player.SkillPoints);
        setStrengthUiPoints(player.Strength, 100);
    }

    public void addAgilityPoint()
    {
        if (player == null || player.SkillPoints < 1)
            return;

        player.Agility += 1;
        setUIPoints(--player.SkillPoints);
        setAgilityUiPoints(player.Agility, 100);
    }

    public void addDefensePoint()
    {
        if (player == null || player.SkillPoints < 1)
            return;

        player.Defense += 1;
        setUIPoints(--player.SkillPoints);
        setDefenseUiPoints(player.Defense, 100);
    }

    public void addLuckPoint()
    {
        if (player == null || player.SkillPoints < 1)
            return;

        player.Luck += 1;
        setUIPoints(--player.SkillPoints);
        setLuckUiPoints(player.Luck, 100);
    }

    public void addSpeedPoint()
    {
        if (player == null || player.SkillPoints < 1)
            return;

        player.Speed += 1;
        setUIPoints(--player.SkillPoints);
        setSpeedUiPoints(player.Speed, 100);
    }

}
