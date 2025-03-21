using UnityEngine;

public class PrefabHoverHandler : MonoBehaviour
{
    private GameObject rabbit;
    private GameObject purpleGhost;
    private GameObject caterpillar;
    private GameObject redPrincess;
    private GameObject cardSoldier;
    private GameObject alice;

    void Start()
    {
        Debug.Log("PrefabHoverHandler Start method called.");

        // 嘗試找到 Canvas 並從中查找 UI 預覽物件
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null)
        {
            Debug.LogError("Canvas not found in the scene!");
            return;
        }

        Debug.Log("Canvas found. Searching for preview objects...");
        Transform canvasTransform = canvasObject.transform;

        rabbit = FindAndLog(canvasTransform, "RabbitPreview");
        purpleGhost = FindAndLog(canvasTransform, "PurpleGhostPreview");
        caterpillar = FindAndLog(canvasTransform, "CaterpillarPreview");
        redPrincess = FindAndLog(canvasTransform, "RedPrincessPreview");
        cardSoldier = FindAndLog(canvasTransform, "CardSoldierPreview");
        alice = FindAndLog(canvasTransform, "AlicePreview");
    }

    private GameObject FindAndLog(Transform parent, string name)
    {
        Transform foundTransform = parent.Find(name);
        if (foundTransform != null)
        {
            Debug.Log($"Successfully found {name}!");
            return foundTransform.gameObject;
        }
        else
        {
            Debug.LogError($"{name} not found under Canvas!");
            return null;
        }
    }

    public void OnHoverEnter()
    {
        Debug.Log($"OnHoverEnter triggered for {gameObject.name}.");

        // 先關閉所有 UI 預覽
        SetAllPreviewsInactive();

        // 根據物件名稱顯示對應的 UI
        switch (gameObject.name)
        {
            case "purpleGhost":
                SetActiveRecursively(purpleGhost, true);
                break;
            case "rabbit":
                SetActiveRecursively(rabbit, true);
                break;
            case "caterpillar":
                SetActiveRecursively(caterpillar, true);
                break;
            case "redPrincess":
                SetActiveRecursively(redPrincess, true);
                break;
            case "cardSoldier":
                SetActiveRecursively(cardSoldier, true);
                break;
            case "alice":
                SetActiveRecursively(alice, true);
                break;
            default:
                Debug.LogWarning($"No UI mapped for {gameObject.name}.");
                break;
        }
    }

    private void SetAllPreviewsInactive()
    {
        SetActiveRecursively(rabbit, false);
        SetActiveRecursively(purpleGhost, false);
        SetActiveRecursively(caterpillar, false);
        SetActiveRecursively(redPrincess, false);
        SetActiveRecursively(cardSoldier, false);
        SetActiveRecursively(alice, false);
    }

    private void SetActiveRecursively(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            obj.SetActive(isActive);
            foreach (Transform child in obj.transform)
            {
                SetActiveRecursively(child.gameObject, isActive);
            }
        }
        else
        {
            Debug.LogError("Trying to set active state on a null object.");
        }
    }
}
