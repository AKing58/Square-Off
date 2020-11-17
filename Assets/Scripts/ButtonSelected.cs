using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour, ISelectHandler
{   
    
    public StageSelectManager ssm;
    public TMP_Text text;
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(text.text);
        ssm.changeBackground(text.text);
    }
}
