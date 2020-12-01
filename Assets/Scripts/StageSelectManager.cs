using System.Collections;
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
        //backgroundImg = gameObject.GetComponent<Image>();
        //spriteList.Add("The Grid", Resources.Load<Sprite>("Images/TheGrid"));
        spriteList.Add("The Round", Resources.Load<Sprite>("GameObjects/UI/Images/TheRound"));
        spriteList.Add("The Flame", Resources.Load<Sprite>("GameObjects/UI/Images/TheFlame"));
        spriteList.Add("The Button", Resources.Load<Sprite>("GameObjects/UI/Images/TheButton"));
        spriteList.Add("Squareagon", Resources.Load<Sprite>("GameObjects/UI/Images/Squareagon"));
        
    } 

    public void changeBackground(string spriteName)
    {
        backgroundImg.sprite = spriteList[spriteName];
    
    }

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
