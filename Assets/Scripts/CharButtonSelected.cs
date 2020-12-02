using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CharButtonSelected : MonoBehaviour, ISelectHandler
{
    public PlayerSetupMenuController psmc;
    public TMP_Text charNameText;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(charNameText.text);
        psmc.ChangeCharacterStats(charNameText.text);
    }
}
