using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeUIPanels : MonoBehaviour
{
    public float spacing = 1.5f;  // 設定面板間距
    public bool arrangeOnUpdate = true; // 是否每幀更新排列（可選）

    void Start()
    {
        ArrangePanels();
    }

    void Update()
    {
        if (arrangeOnUpdate)
        {
            ArrangePanels();
        }
    }

    public void ArrangePanels()
    {
        // 取得當前 UIContainer 內的所有子物件
        int panelCount = transform.childCount;
        if (panelCount == 0) return;

        Vector3 basePosition = transform.position;
        Vector3 rightDirection = transform.right; // 確保水平排列

        for (int i = 0; i < panelCount; i++)
        {
            Transform panel = transform.GetChild(i);
            Vector3 targetPosition = basePosition + rightDirection * (i * spacing);
            panel.position = targetPosition;
        }
    }
}
