using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
