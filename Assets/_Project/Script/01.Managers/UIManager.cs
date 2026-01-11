using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI")]
    public Image hpBar;
    public Image expBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;

    [Header("Popups")]
    public GameObject gameOverPanel;
    public GameObject levelUpPanel;

    public void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void UpdateHP(float current, float max)
    {
        if (hpBar == null) return;
        float target = current / max;

        if (Mathf.Abs(hpBar.fillAmount - target) > 0.01f)
            hpBar.DOFillAmount(target, 0.2f);
        
    }
    public void UpdateExp(float current,float max)
    {
        if (expBar == null) return;
        expBar.fillAmount = current / max;
    }
    public void UpdateKillCount(int count)
    {
        if (scoreText == null) return;
        scoreText.text = $"Kills : {count}";
        scoreText.transform.DOKill();
        scoreText.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f, 10, 1);
    }
    public void UpdateLevel(int level)
    {
        if (levelText != null) levelText.text = $"Lv.{level}";
    }
    public void ShowGameOver() 
    {
        if (gameOverPanel == null) return;
        gameOverPanel.SetActive(false);

        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
    public void ShowLevelUpUI(bool show)
    {
        if(levelUpPanel == null) return;
        levelUpPanel.SetActive(show);
    }

}
