using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateDetailUpperRecycleImage : MonoBehaviour
{
    public GameObject buttonPrefab; // 预制的 Button 模板，包含两个 Image
    public Transform parentTransform; // 用来放置生成的 Button 的父物件（ImageContainer）
    private List<Sprite> imagesArray = new List<Sprite>(); // 使用 List 可以灵活添加或删除图片

    public Sprite purpleGhostSprite;
    public Sprite rabbitSprite;
    public Sprite caterpillarSprite;
    public Sprite redprincess2dSprite;
    public Sprite cardSoldierSprite;
    public Sprite aliceSprite;

    public Sprite fiveStars;
    public Sprite threeStars;
    public Sprite oneStar;

    void Start()
    {
        LogInfo("Start called. Updating images based on condition.");
    }

    public void ClearExistingButtons()
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
            LogInfo($"Removed existing button: {child.name}");
        }
    }

    public void MultipleHint(string condition)
    {
        imagesArray.Clear();
        LogInfo($"Updating images based on condition: {condition}");

        if (condition == "PurpleGhost")
        {
            imagesArray.Add(purpleGhostSprite);
            imagesArray.Add(rabbitSprite);
            imagesArray.Add(caterpillarSprite);
        }
        else if (condition == "Rabbit")
        {
            imagesArray.Add(rabbitSprite);
            imagesArray.Add(caterpillarSprite);
        }
        else if (condition == "Caterpillar")
        {
            imagesArray.Add(caterpillarSprite);
        }
        else if (condition == "RedPrincess")
        {
            imagesArray.Add(redprincess2dSprite);
            imagesArray.Add(cardSoldierSprite);
            imagesArray.Add(aliceSprite);
        }
        else if (condition == "CardSoldier")
        {
            imagesArray.Add(cardSoldierSprite);
            imagesArray.Add(aliceSprite);
        }
        else if (condition == "Alice")
        {
            imagesArray.Add(aliceSprite);
        }

        GenerateImages(condition);
    }

    public void SingleHint(string condition)
    {
        imagesArray.Clear();
        LogInfo($"Updating images based on condition: {condition}");

        if (condition == "PurpleGhost")
        {
            imagesArray.Add(purpleGhostSprite);
        }
        else if (condition == "Rabbit")
        {
            imagesArray.Add(rabbitSprite);
        }
        else if (condition == "Caterpillar")
        {
            imagesArray.Add(caterpillarSprite);
        }
        else if (condition == "RedPrincess")
        {
            imagesArray.Add(redprincess2dSprite);
        }
        else if (condition == "CardSoldier")
        {
            imagesArray.Add(cardSoldierSprite);
        }
        else if (condition == "Alice")
        {
            imagesArray.Add(aliceSprite);
        }

        GenerateImages(condition);
    }






    void GenerateImages(string condition)
    {
        LogInfo($"Generating {imagesArray.Count} images.");

        for (int i = 0; i < imagesArray.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, parentTransform);
            LogInfo($"Instantiated new button for image: {imagesArray[i].name}");

            newButton.name = imagesArray[i].name;

            Transform starTransform = newButton.transform.Find("stars");
            Transform imageTransform = newButton.transform.Find("image");

            if (starTransform != null)
            {
                if (condition == "PurpleGhost")
                {
                    starTransform.GetComponent<Image>().sprite = imagesArray[i] == purpleGhostSprite ? fiveStars :
                                                                   imagesArray[i] == rabbitSprite ? threeStars :
                                                                   imagesArray[i] == caterpillarSprite ? oneStar : null;
                }
                else if (condition == "Rabbit")
                {
                    starTransform.GetComponent<Image>().sprite = imagesArray[i] == rabbitSprite ? fiveStars :
                                                                   imagesArray[i] == caterpillarSprite ? threeStars : null;
                }
                else if (condition == "Caterpillar")
                {
                    starTransform.GetComponent<Image>().sprite = fiveStars;
                }
                else if (condition == "RedPrincess")
                {
                    starTransform.GetComponent<Image>().sprite = imagesArray[i] == redprincess2dSprite ? fiveStars :
                                                                   imagesArray[i] == cardSoldierSprite ? threeStars :
                                                                   imagesArray[i] == aliceSprite ? oneStar : null;
                }
                else if (condition == "CardSoldier")
                {
                    starTransform.GetComponent<Image>().sprite = imagesArray[i] == cardSoldierSprite ? fiveStars :
                                                                   imagesArray[i] == aliceSprite ? threeStars : null;
                }
                else if (condition == "Alice")
                {
                    starTransform.GetComponent<Image>().sprite = fiveStars;
                }
            }

            if (imageTransform != null)
            {
                imageTransform.GetComponent<Image>().sprite = imagesArray[i];
            }
        }
        LogInfo("Image generation completed.");
    }

    private void LogInfo(string message)
    {
        Debug.Log($"ImageSpawner LogInfo: {message}");
    }

    private void LogWarning(string message)
    {
        Debug.LogWarning($"ImageSpawner LogWarning: {message}");
    }
}
