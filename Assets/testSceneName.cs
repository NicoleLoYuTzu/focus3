using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class testSceneName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("testSceneName Current Scene: " + SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
