using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject uiElement; // 要顯示或隱藏的 UI 元素
    private void LogWithName(string message)
    {
        Debug.Log($"Nicole: {message}");
    }

    // 顯示 UI
    public void Show()
    {
        uiElement.SetActive(true); // 將 UI 設置為可見
        Debug.Log("UI 顯示了！"); // 日誌輸出
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();

        // 使用 LogWithName 函数打印位置信息
        LogWithName($"Preview : MonoBehaviour UI Panel Position: {rectTransform.anchoredPosition}, Scale rectTransform: {rectTransform.localScale}");


     


    }

    // 隱藏 UI
    public void Hide()
    {
        uiElement.SetActive(false); // 將 UI 設置為隱藏
        Debug.Log("UI 隱藏了！"); // 日誌輸出
    }
}
