using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class mnkValues : MonoBehaviour
{
    

    // Start is called before the first frame update
    public void Start()
    {
        TMP_Dropdown dp = this.GetComponent<TMP_Dropdown>();
        List<TMP_Dropdown> mnkDropdown = new List<TMP_Dropdown>();

        if ( dp == null)
        { 
            Debug.Log("No TMP_Dropdown Component");
            return;
        }

        mnkDropdown.Add(dp);
        dp.options.Clear();

        int i = 4;
        if (this.name == "kValue") { i = 3; }

        for ( ; i<=20; ++i)
        { 
            dp.options.Add(new TMP_Dropdown.OptionData() { text = i.ToString() });
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

}
