using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSoldierGameActivate : MonoBehaviour
{
    public GameObject hintUI;   // 提示 UI
    public GameObject wandUI;   // 權杖 UI (檢查是否已經獲得權杖)
    public TextMeshProUGUI infoTextUI;  // 顯示文字的 UI (需拖入 TextMeshPro 物件)
    public GameObject gameArea;
    private void Start()
    {
        hintUI.SetActive(false);
        // 確保這個物件有 Collider 並啟用 Trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 確保 MainCamera 進入
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 進入觸發區");
            ShowHintUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 確保 MainCamera 離開
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 離開觸發區");
            HideHintUI();
        }
    }

    private void HideHintUI()  // ✅ 更改方法名稱，避免衝突
    {
        if (hintUI != null) hintUI.SetActive(false);
    }

    private void ShowHintUI()  // ✅ 更改方法名稱，避免衝突
    {
        if (gameArea.activeSelf)
        {
            // ✅ 遊戲已經開始，仍然要顯示提示 UI
            if (hintUI != null) hintUI.SetActive(true);
            return;  // **避免執行後續邏輯**
        }

        else if (wandUI != null && wandUI.activeSelf)
        {
            // 只有權杖 UI 被啟動時，才顯示提示 UI
            if (hintUI != null) hintUI.SetActive(true);
            infoTextUI.text = "你帶來了權杖啊！\n\n勇敢的玩家，請尋找紅心、黑桃、梅花和菱形四種撲克牌圖案。\n" +
                   "每種圖案都有其獨特的魅力，正確的卡片將揭示通往放大藥水的秘密。\n" +
                   "找到每張牌後，依照順序將數字輸入控制台。\n" +
                   "一旦完成，你將獲得真正的放大藥水，幫助你的朋友愛麗絲！祝你好運！";
        }
        else
        {
            // 沒有權杖，改變 infoTextUI 文字
            if (infoTextUI != null)
            {
                infoTextUI.text = "看來是還沒拿到皇后的權杖呢...";
            }
        }
    }
}
