using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject buttons;
    public GameObject inputScene;
    public GameObject mValue;
    public GameObject nValue;
    public GameObject kValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play()
    {
        Transform[] buttonChild = buttons.GetComponentsInChildren<Transform>();
        GameObject playButton = buttonChild[1].gameObject;
        playButton.GetComponentsInChildren<Text>()[0].color = Color.black;

        buttons.SetActive(false);
        inputScene.SetActive(true);
        
    }

    public void HowToPlay()
    {
    }

    public void GoBack()
    {
        Transform[] inputSceneChild = inputScene.GetComponentsInChildren<Transform>();
        GameObject goBack = inputSceneChild[20].gameObject;
        goBack.GetComponentsInChildren<Text>()[0].color = Color.black;

        buttons.SetActive(true);
        inputScene.SetActive(false);
    }
    public void Exit()
    {
        SceneManager.LoadScene("Scenes/game_exit_confirmation");
    }

    public void StartGame()
    {
        
        GameBoard.numRows = Convert.ToInt32(mValue.GetComponent<TMP_Dropdown>().options[mValue.GetComponent<TMP_Dropdown>().value].text);
        GameBoard.numColumns = Convert.ToInt32(nValue.GetComponent<TMP_Dropdown>().options[nValue.GetComponent<TMP_Dropdown>().value].text);
        GameBoard.numPiecesToWin = Convert.ToInt32(kValue.GetComponent<TMP_Dropdown>().options[kValue.GetComponent<TMP_Dropdown>().value].text);        
        SceneManager.LoadScene("Scenes/game_board");
    }

}
