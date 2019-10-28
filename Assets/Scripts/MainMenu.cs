using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
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
        GameObject buttons = this.transform.parent.gameObject;

        //Buttons
        buttons.SetActive(false);
        
        //Convert Color of PlayButton text to Black
        this.GetComponentsInChildren<Text>()[0].color = Color.black;


        //MainMenuCanvas
        GameObject mainMenu = buttons.transform.parent.gameObject;

        //mnkValue Button
        mainMenu.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void HowToPlay()
    {
    }

    public void GoBack()
    {
        GameObject inputScene = this.transform.parent.gameObject;

        //InputScene
        inputScene.SetActive(false);

        //Convert Color of BackButton text to Black
        this.GetComponentsInChildren<Text>()[0].color = Color.black;

        //MainMenuCanvas
        GameObject mainMenu = inputScene.transform.parent.gameObject;

        //Buttons (Consist of Play,How to Play and Exit)
        mainMenu.transform.GetChild(2).gameObject.SetActive(true);



    }
    public void Exit()
    {
        SceneManager.LoadScene("Scenes/game_exit_confirmation");
    }

    public void StartGame()
    {
        for (int i = 0; i < 3; ++i)
        {
            int mnkIndex = mnkValues.mnkDropdown[i].value;
            List<TMP_Dropdown.OptionData> mnkOptions = mnkValues.mnkDropdown[0].options;
            string value = mnkOptions[mnkIndex].text;
      
            if (mnkValues.mnkDropdown[i].name == "mValue") { GameBoard.numRows = Convert.ToInt32(value); }
            if (mnkValues.mnkDropdown[i].name == "nValue") { GameBoard.numColumns = Convert.ToInt32(value); }
            if (mnkValues.mnkDropdown[i].name == "kValue") { GameBoard.numPiecesToWin = Convert.ToInt32(value); }
        }

        SceneManager.LoadScene("Scenes/game_board");
    }

}
