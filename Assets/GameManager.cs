using UnityEngine;
using TMPro;  // 使用 TextMeshPro 來顯示數量

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 單例模式，方便其他腳本呼叫

    private int wingCount = 0;  // 翅膀數量
    private int heartCount = 0; // 愛心數量

    public TextMeshProUGUI wingText;   // 翅膀數量顯示
    public TextMeshProUGUI heartText;  // 愛心數量顯示

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectItem(string itemType)
    {
        if (itemType == "Wing" && wingCount < 3)
        {
            wingCount++;
        }
        else if (itemType == "Heart" && heartCount < 1)
        {
            heartCount = 1;  // 只會有一顆愛心
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (wingText != null) wingText.text = $"*{wingCount}";
        if (heartText != null) heartText.text = $"*{heartCount}";
    }
}
