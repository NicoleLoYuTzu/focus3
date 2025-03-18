using UnityEngine;

public class PurpleGhostHintActivate : MonoBehaviour
{
    public GameObject hintUI1; // 需要顯示的物件 (請在 Inspector 設定)
    public GameObject hintUI2; // 需要顯示的物件 (請在 Inspector 設定)

   
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"PurpleGhostHintActivate: PlayerTextUpdate other {other.gameObject.name}");

        // 確保 MainCamera 進入
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 進入觸發區，顯示提示 UI");

           
            hintUI1.SetActive(true); // 顯示提示 UI
            hintUI2.SetActive(true); // 顯示提示 UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 當 MainCamera 離開時，隱藏提示 UI
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 離開觸發區，隱藏提示 UI");

            hintUI1.SetActive(false); // 顯示提示 UI
            hintUI2.SetActive(false); // 顯示提示 UI
        }
    }
}
