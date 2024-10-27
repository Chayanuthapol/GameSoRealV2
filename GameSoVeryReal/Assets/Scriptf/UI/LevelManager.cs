using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : PauseMenu
{
    public string sceneName;
    public string sceneName2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);   
        Time.timeScale = 1.0f;
        GameisPaused = false;
    }
    public void ChangeScene2()
    {
        SceneManager.LoadScene(sceneName2);   
        Time.timeScale = 1.0f;
        GameisPaused = false;
    }
    
}
