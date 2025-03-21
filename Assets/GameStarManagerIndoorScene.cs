using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarManagerIndoorScene : MonoBehaviour
{
    public GameObject redPrincessStarImage;
    public GameObject cardSoldierStarImage;
    public GameObject aliceStarImage;

    public Sprite star1;
    public Sprite star3;
    public Sprite star5;

    public void UpdateNPCStars(Dictionary<string, bool> completedTasks)
    {
        Debug.Log("Updating NPC Stars based on completed tasks...");

        // Print out the entire completedTasks dictionary
        Debug.Log("Completed Tasks:");
        foreach (var task in completedTasks)
        {
            Debug.Log($"UpdateNPCStars completedTasks {task.Key}: {task.Value}");
        }



        int redPrincessStars = 5;
        int cardSoldierStars = 4;
        int aliceStars = 3;



        // 根據紅心皇后的完成情況來更新星級
        if (completedTasks.ContainsKey("CardSoldier") && completedTasks.ContainsKey("RedQueen"))
        {

            aliceStars = 5;
        }
        else if (completedTasks.ContainsKey("RedQueen"))
        {
            // 紅心皇后完成，但鋪克牌士兵尚未完成
            cardSoldierStars = 5;
            aliceStars = 4;
        }

        Debug.Log($"RedPrincessStars: {redPrincessStars}, CardSoldierStars: {cardSoldierStars}, AliceStars: {aliceStars}");


        UpdateStarImage(redPrincessStarImage, redPrincessStars);
        UpdateStarImage(cardSoldierStarImage, cardSoldierStars);
        UpdateStarImage(aliceStarImage, aliceStars);
    }

    // Update star image for a character
    void UpdateStarImage(GameObject starImageObj, int stars)
    {
        if (starImageObj != null)
        {
            Image starImageComponent = starImageObj.GetComponent<Image>();
            if (starImageComponent != null)
            {
                starImageComponent.sprite = stars == 5 ? star5 : stars == 4 ? star3 : star1;
            }
        }
    }

    // This method will be called when a task is marked as completed
    public void MarkTaskComplete(Dictionary<string, bool> completedTasks)
    {
        Debug.Log("GameStarManager Marking multiple tasks as complete...");
        UpdateNPCStars(completedTasks);
    }
}
