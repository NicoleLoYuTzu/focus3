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

        //foreach (Transform child in parentTransform)
        //{
        //    // 假設非 rabbit 的預設物件名為 "Sheep"
        //    if (child.name != "RABBIT1")
        //    {
        //        Destroy(child.gameObject);
        //        LogInfo($"Removed default object: {child.name}");
        //    }
        //}


        //UpdateImagesBasedOnCondition("phanto"); // 使用 phanto 作为条件
        //GenerateImages();
      
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

        if (condition == "PurpleGhostContent")
        {
            // 加入兔子和毛毛虫的图片
            Sprite rabbitSprite = Resources.Load<Sprite>("Rabbit/RABBIT1");
            Sprite caterpillarSprite = Resources.Load<Sprite>("Caterpillar/CATERPILLER");

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
        else if (condition == "RabbitContent")
        {
            Sprite caterpillarSprite = Resources.Load<Sprite>("Caterpillar/CATERPILLER");
        }

        GenerateImages();
    }

    void GenerateImages()
    {
        LogInfo($"Generating {imagesArray.Count} images.");

        for (int i = 0; i < imagesArray.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, parentTransform);
            LogInfo($"Instantiated new button for image: {imagesArray[i].name}");

            newButton.name = imagesArray[i].name;

            // 為兔子的 Button 設定星星和圖片
            if (imagesArray[i].name == "RABBIT1")
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
                newButton.GetComponent<Image>().color = Color.gray;
                LogInfo("Set button background color to gray for caterpillar.");

                Transform starTransform = newButton.transform.Find("stars");
                if (starTransform != null)
                {
                    starTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Caterpillar/bull");
                    LogInfo("Set star image for caterpillar.");
                }

                Transform caterpillarTransform = newButton.transform.Find("image");
                if (caterpillarTransform != null)
                {
                    caterpillarTransform.GetComponent<Image>().sprite = imagesArray[i];
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
