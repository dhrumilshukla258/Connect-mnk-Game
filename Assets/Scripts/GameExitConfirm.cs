using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameExitConfirm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        SceneManager.LoadScene("Scenes/main_menu");
    }
}
