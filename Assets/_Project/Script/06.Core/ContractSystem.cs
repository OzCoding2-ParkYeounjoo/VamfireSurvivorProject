using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContractSystem : MonoBehaviour
{
    public TMP_InputField nameInput;
    public void OnSignContract()
    {
        string nickName = nameInput.text;
        if (string.IsNullOrEmpty(nickName)) return;

        if(DataManager.instance != null)
        {
            DataManager.instance.playerName = nickName;
            DataManager.instance.SaveGame();
        }
        SceneManager.LoadScene("GameScene");
    }
}
