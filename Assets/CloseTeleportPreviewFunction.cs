using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTeleportPreviewFunction : MonoBehaviour
{
    public List<GameObject> objectsToDisable = new List<GameObject>(); // 可在 Inspector 拖入多個物件

    void Start()
    {
        // 遍歷列表並禁用所有物件
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
