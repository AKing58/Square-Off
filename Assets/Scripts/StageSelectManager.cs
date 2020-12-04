﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{

    public Image backgroundImg;

    private Dictionary<string, Sprite> spriteList = new Dictionary<string, Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        spriteList.Add("The Round", Resources.Load<Sprite>("GameObjects/UI/Images/TheRound"));
        spriteList.Add("The Flame", Resources.Load<Sprite>("GameObjects/UI/Images/TheFlame"));
        spriteList.Add("The Button", Resources.Load<Sprite>("GameObjects/UI/Images/TheButton"));
        spriteList.Add("Squareagone", Resources.Load<Sprite>("GameObjects/UI/Images/Squareagone"));
        
    } 

    //change the background of stage select manager
    public void changeBackground(string spriteName)
    {
        backgroundImg.sprite = spriteList[spriteName];
    }

    //loads the specified scene
    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
