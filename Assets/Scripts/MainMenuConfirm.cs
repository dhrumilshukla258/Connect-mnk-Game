using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuConfirm : MonoBehaviour
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
        SceneManager.LoadScene("Scenes/main_menu");
    }

    public void No()
    {
        SceneManager.LoadScene("Scenes/pause_menu");
    }
}
