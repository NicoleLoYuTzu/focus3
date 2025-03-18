using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerZone : MonoBehaviour
{
    public GameObject hintUI;   // 提示 UI

    private void Start()
    {
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
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 確保 MainCamera 離開
        if (other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("MainCamera 離開觸發區");
            HideUI();
        }
    }

    private void HideUI()
    {
        if (hintUI != null) hintUI.SetActive(false);
    }

    private void ShowUI()
    {
        hintUI.SetActive(true);
    }
}
