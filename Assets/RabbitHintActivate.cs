using UnityEngine;

public class RabbitHintActivate : MonoBehaviour
{
    public GameObject hint;


    void Start() {

        hint.SetActive(false); // 顯示提示 UI

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"RabbitHintActivate: PlayerTextUpdate other {other.gameObject.name}");

        // 確保 MainCamera 進入
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 進入觸發區，顯示提示 UI");


            hint.SetActive(true); // 顯示提示 UI
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 當 MainCamera 離開時，隱藏提示 UI
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 離開觸發區，隱藏提示 UI");

            hint.SetActive(false); // 顯示提示 UI
        }
    }
}
