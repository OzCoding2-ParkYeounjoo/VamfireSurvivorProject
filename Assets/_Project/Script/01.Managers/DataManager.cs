using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("Data")]
    public string playerName = "Player";
    public int totalGold = 0;
    public int currentStageGold = 0;
    private void Awake()
    {
       if(instance == null)
       {
           instance = this;
           DontDestroyOnLoad(gameObject);
           LoadData();
       }
       else
       {
           Destroy(gameObject);
       }
    }
    public void AddGold(int gold)
    {
        currentStageGold = gold;
    }
    public void SaveGame()
    {
        totalGold += currentStageGold;
        currentStageGold = 0;

        PlayerPrefs.SetInt("TotalGold", totalGold);
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log($"ÀúÀå ¿Ï·á! ÃÑ °ñµå : {totalGold}");
    }
    public void LoadData()
    {
        totalGold = PlayerPrefs.GetInt("TotalGold", totalGold);
        playerName = PlayerPrefs.GetString("PlayerName", playerName);
    }
}

