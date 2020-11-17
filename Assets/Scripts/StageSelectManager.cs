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
        spriteList.Add("The Grid", Resources.Load<Sprite>("Images/TheGrid"));
        spriteList.Add("The Non-Grid", Resources.Load<Sprite>("Images/spiritfarer"));
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
