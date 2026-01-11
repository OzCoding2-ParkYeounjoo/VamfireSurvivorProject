using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Game Stat")]
    public bool isGameOver = false;
    public int killCount = 0;

    [Header("Level System")]
    public int level = 1;
    public int currentExp = 0;
    public int maxExp = 100;
    public GameObject expGmePrefab;

    public PlayerController player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        if (player == null) player = FindObjectOfType<PlayerController>();
        if(UIManager.Instance != null)
        {
            if (player != null)
                UIManager.Instance.UpdateHP(player.CurrentHP, player.playerData.maxHP);
            UIManager.Instance.UpdateExp(0, maxExp);
            UIManager.Instance.UpdateLevel(level);
            UIManager.Instance.ShowLevelUpUI(false);
        }
    }
    private void Update()
    {
        if (isGameOver || player == null) return;
        if(UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHP(player.CurrentHP, player.playerData.maxHP);
        }
    }
    public void GetExp(int amount)
    {
        currentExp += amount;
        UIManager.Instance.UpdateExp(currentExp, maxExp);
        if (currentExp >= maxExp) LevelUp();
    }
    void LevelUp()
    {
        level++;
        currentExp = 0;
        maxExp += 50;
        UIManager.Instance.UpdateLevel(level);
        UIManager.Instance.UpdateExp(0, maxExp);
        Time.timeScale = 0f;
        UIManager.Instance.ShowLevelUpUI(true);
    }
    public void SelectAugment(int type)
    {
        if (player == null) return;
        switch (type)
        {
            case 1:
                player.currentDamage += 5;
                Debug.Log($"공증 : 현재 공격력 {player.currentDamage}");
                break;
            case 2:
                player.currentMoveSpeed += 1f;
                Debug.Log($"이속 증 : 현재 이속 {player.currentMoveSpeed}");
                break;
            case 3:
                player.FullRecovery();
                if(UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateHP(player.CurrentHP, player.playerData.maxHP);
                }
                break;
        }
        UIManager.Instance.ShowLevelUpUI(false);
        Time.timeScale = 1f;
    }
    public void AddkillCount()
    {
        killCount++;
        UIManager.Instance.UpdateKillCount(killCount);
    }
    public void OnGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        DataManager.instance.SaveGame();
        UIManager.Instance.ShowGameOver();
    }
    public void RetryGame()
    {
        DG.Tweening.DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMain()
    {
        DG.Tweening.DOTween.KillAll();
        SceneManager.LoadScene("MainMenu");
    }

}
