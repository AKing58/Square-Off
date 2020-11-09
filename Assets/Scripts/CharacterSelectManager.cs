using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public Text joinText;
    // Start is called before the first frame update

    public void showJoinText() {
        if (PlayerConfigurationManager.Instance.GetPlayerConfigs().Count == 0)
        {
            joinText.enabled = true;

        }
        else
        {
            joinText.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
