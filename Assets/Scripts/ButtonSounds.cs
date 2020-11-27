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
        SoundManager.PlayOneShot(SoundManager.SFX.UISelect);
    }

    public void OnClick() {
        SoundManager.PlayOneShot(SoundManager.SFX.UIConfirm);
    }

    public void OnCancel() {
        SoundManager.PlayOneShot(SoundManager.SFX.UICancel);
    }

}
