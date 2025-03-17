using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateDetailUpperRecycleImage : MonoBehaviour
{
    public GameObject buttonPrefab; // 预制的 Button 模板，包含两个 Image
    public Transform parentTransform; // 用来放置生成的 Button 的父物件（ImageContainer）
    private List<Sprite> imagesArray = new List<Sprite>(); // 使用 List 可以灵活添加或删除图片

    void Start()
    {
        LogInfo("Start called. Updating images based on condition.");
      
    }

    // 清除已經存在的按鈕物件
    public void ClearExistingButtons()
    {
        foreach (Transform child in parentTransform)
        {
            // 這裡刪除父物件下的所有子物件
            Destroy(child.gameObject);
            LogInfo($"Removed existing button: {child.name}");
        }
    }


    public void UpdateImagesBasedOnCondition(string condition)
    {

        imagesArray.Clear();
        LogInfo($"Updating images based on condition: {condition}");

        if (condition == "PurpleGhost")
        {
            // 加入兔子和毛毛虫的图片
            Sprite purpleGhostSprite = Resources.Load<Sprite>("PurpleGhost/purpleGhost");
            Sprite rabbitSprite = Resources.Load<Sprite>("Rabbit/Rabbit1");
            Sprite caterpillarSprite = Resources.Load<Sprite>("Caterpillar/CATERPILLER");

            imagesArray.Add(purpleGhostSprite); // RABBIT1 图片

            if (rabbitSprite != null)
            {
                imagesArray.Add(rabbitSprite); // RABBIT1 图片
                LogInfo("Added rabbit sprite.");
            }
            else
            {
                LogWarning("Rabbit sprite not found!");
            }

            if (caterpillarSprite != null)
            {
                imagesArray.Add(caterpillarSprite); // 毛毛虫图片
                LogInfo("Added caterpillar sprite.");
            }
            else
            {
                LogWarning("Caterpillar sprite not found!");
            }
        }
        else if (condition == "Rabbit")
        {
            Sprite rabbitSprite = Resources.Load<Sprite>("Rabbit/Rabbit1");
            imagesArray.Add(rabbitSprite); // RABBIT1 图片
            Sprite caterpillarSprite = Resources.Load<Sprite>("Caterpillar/CATERPILLER");
            imagesArray.Add(caterpillarSprite); // 毛毛虫图片
        }

        else if (condition == "Caterpillar")
        {
            Sprite caterpillarSprite = Resources.Load<Sprite>("Caterpillar/CATERPILLER");
            imagesArray.Add(caterpillarSprite); // 毛毛虫图片


        }

        else if (condition == "RedPrincess")
        {
            Sprite redprincess2dSprite = Resources.Load<Sprite>("RedPrincess/redprincess2d");
            imagesArray.Add(redprincess2dSprite); // 毛毛虫图片
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
            if (imagesArray[i].name == "purpleGhost") {
                Transform starTransform = newButton.transform.Find("stars");
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("PurpleGhost/stars");
                    LogInfo("Set star image for rabbit.");
                }

                Transform rabbitTransform = newButton.transform.Find("image");
                if (rabbitTransform != null)
                {
                    rabbitTransform.GetComponent<Image>().sprite = imagesArray[i];
                    LogInfo("Set rabbit image.");
                }
            }

            // 為兔子的 Button 設定星星和圖片
            if (imagesArray[i].name == "Rabbit1")
            {
                Transform starTransform = newButton.transform.Find("stars");
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Rabbit/stars");
                    LogInfo("Set star image for rabbit.");
                }

                Transform rabbitTransform = newButton.transform.Find("image");
                if (rabbitTransform != null)
                {
                    rabbitTransform.GetComponent<Image>().sprite = imagesArray[i];
                    LogInfo("Set rabbit image.");
                }
            }

            // 為毛毛蟲的 Button 設定背景、星星和圖片
            if (imagesArray[i].name == "CATERPILLER")
            {

                if (condition == "PurpleGhost")
                {
                    newButton.GetComponent<Image>().color = Color.gray;
                }
               
                LogInfo("Set button background color to gray for caterpillar.");

                Transform starTransform = newButton.transform.Find("stars");
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Caterpillar/stars");
                    LogInfo("Set star image for caterpillar.");
                }

                Transform caterpillarTransform = newButton.transform.Find("image");
                if (caterpillarTransform != null)
                {
                    caterpillarTransform.GetComponent<Image>().sprite = imagesArray[i];
                    LogInfo("Set caterpillar image.");
                }
            }

            if (imagesArray[i].name == "redprincess2d")
            {
                LogInfo("Set button background color to gray for caterpillar.");

                Transform starTransform = newButton.transform.Find("stars");
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Caterpillar/stars");
                    LogInfo("Set star image for caterpillar.");
                }

                Transform redprincessTransform = newButton.transform.Find("image");
                if (redprincessTransform != null)
                {
                    redprincessTransform.GetComponent<Image>().sprite = imagesArray[i];
                    LogInfo("Set caterpillar image.");
                }
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
