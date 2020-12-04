using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
    Sets the background of the scene when selecting a stage
 */
public class ButtonSelected : MonoBehaviour, ISelectHandler
{   
    
    public StageSelectManager ssm;
    public TMP_Text text;
    public void OnSelect(BaseEventData eventData)
    {
        ssm.changeBackground(text.text);
    }
}
