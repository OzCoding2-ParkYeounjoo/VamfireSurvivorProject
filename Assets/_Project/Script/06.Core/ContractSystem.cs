using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContractSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject contractPanel;
    public GameObject dicePanel;
    public TMP_InputField nameInput;
    public TextMeshProUGUI diceResultText;

    public TextMeshProUGUI totalGoldText;
    private void Start()
    {
        int gold = PlayerPrefs.GetInt("TotalGold", 0);
        if(totalGoldText != null)
        {
            totalGoldText.text = $"TOTAL GOLD : {gold:N0}";
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnSignContract()
    {
        string nickName = nameInput.text;
        if (string.IsNullOrEmpty(nickName)) return;
        PlayerPrefs.SetString("PlayerName", nickName);

        if(DataManager.instance != null)
        {
            DataManager.instance.playerName = nickName;
        }
        StartCoroutine(RollDiceAndStart());
    }
    IEnumerator RollDiceAndStart()
    {
        if (contractPanel != null) contractPanel.SetActive(false);
        if (dicePanel != null) dicePanel.SetActive(true);
        float duration = 2.0f;
        float timer = 0f;

        while(timer > duration)
        {
            timer += Time.deltaTime;
            if(diceResultText != null)
                 diceResultText.text = Random.Range(1, 7).ToString();
            yield return null;
        }
        int finalBonus = Random.Range(1, 7);
        if (diceResultText != null)
        {
            diceResultText.text = $"<color=red>ATK +{finalBonus}</color>";
            diceResultText.fontSize += 10;
        }

        // 4. 보너스 저장 (우체통에 넣기)
        PlayerPrefs.SetInt("BonusDamage", finalBonus);

        
        PlayerPrefs.Save();

        Debug.Log($"[계약 성립] 이름: {nameInput.text} / 보너스: {finalBonus}");

        yield return new WaitForSeconds(1.5f);

        // 5. 게임 씬으로 출발!
        SceneManager.LoadScene("GameScene");
    }
}
