using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public Button yourButton;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Text[] myText = GetComponentsInChildren<Text>();
        myText[0].color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Text[] myText = GetComponentsInChildren<Text>();
        myText[0].color = Color.black; 
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
