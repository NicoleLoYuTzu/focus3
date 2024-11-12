using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{

    private Color originalColor; // 用於儲存按鈕的初始顏色
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        // 確保按鈕有 Image 組件並儲存初始顏色
        if (button != null)
        {
            originalColor = button.GetComponent<Image>().color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColorToBlue()
    {
        Color blueColor = Color.blue;
        button.GetComponent<Image>().color = blueColor;
    }

    public void ResetColor()
    {
        button.GetComponent<Image>().color = originalColor;
    }
}
