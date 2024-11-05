using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    public ScrollRect scrollRect; // 參考到 ScrollRect
    public Image[] images; // 存放三張圖片

    void Update()
    {
        // 獲取 Content 的 Y 位置
        float scrollPosition = scrollRect.content.anchoredPosition.y;

        // 計算每個圖片顯示的範圍
        float height = scrollRect.content.rect.height / 3;

        // 確保在範圍內
        int index = Mathf.Clamp(Mathf.FloorToInt(scrollPosition / height), 0, images.Length - 1);

        // 更新圖片顯示
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i == index);
        }
    }
}
