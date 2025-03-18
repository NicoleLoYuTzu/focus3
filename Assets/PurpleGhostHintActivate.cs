using UnityEngine;

public class PurpleGhostHintActivate : MonoBehaviour
{
    public GameObject hintUI; // 需要顯示的物件 (請在 Inspector 設定)

    private void Start()
    {
        if (hintUI != null)
        {
            hintUI.SetActive(false); // 遊戲開始時隱藏 hintUI
        }
        else
        {
            Debug.LogError("PurpleGhostHintActivate: hintUI 未指定！");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"PurpleGhostHintActivate: PlayerTextUpdate other {other.gameObject.name}");

        // 確保 MainCamera 進入
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 進入觸發區，顯示提示 UI");

            if (hintUI != null)
            {
                hintUI.SetActive(true); // 顯示提示 UI
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 當 MainCamera 離開時，隱藏提示 UI
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 離開觸發區，隱藏提示 UI");

            if (hintUI != null)
            {
                hintUI.SetActive(false);
            }
        }
    }
}
