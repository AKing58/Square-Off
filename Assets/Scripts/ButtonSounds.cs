using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
    Plays SFX when selecting, confirming or canceling from a button
 */
[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour, ISelectHandler
{
    
    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.PlayOneShotUI(SoundManager.SFX.UISelect);
    }

    public void OnClick() {
        SoundManager.PlayOneShotUI(SoundManager.SFX.UIConfirm);
    }

    public void OnCancel() {
        SoundManager.PlayOneShotUI(SoundManager.SFX.UICancel);
    }

}
