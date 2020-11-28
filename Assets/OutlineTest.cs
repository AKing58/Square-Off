using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineTest : MonoBehaviour
{
    public Color colorStart;
    public Color colorEnd;
    //private float lerpControl = 0;
    private float alphaRate = 0.5f;
    private float widthControl = 0;
    private float widthRate = 1.5f;
    public float alphaControl = 0;
    //private Color[] colorArray;
    //private int colorEndIndex;

    private void Start()
    {
        //colorArray = new Color[8];

        //colorEnd = new Color(1, 0, 0, 0);
        //colorEndIndex = 1;


        //colorArray[0] = Color.red; // red
        //colorArray[1] = new Color(1.0f, 0.37f, 0, 1.0f); // orange
        //colorArray[2] = Color.yellow; // yellow
        //colorArray[3] = Color.green; // green
        //colorArray[4] = Color.cyan; // cyan
        //colorArray[5] = Color.blue; // blue
        //colorArray[6] = new Color(0.5f, 0.1f, 0.9f); //purple
        //colorArray[7] = new Color(0.9f, 0.3f, 0.85f); //pink
        //colorStart = colorArray[1];
        //colorEnd = colorArray[colorEndIndex];

        colorStart = Color.red;
        colorEnd = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    }
    // Update is called once per frame
    void Update()
    {
        alphaControl += Time.deltaTime * alphaRate;
        widthControl += Time.deltaTime * widthRate;

        //gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_FirstOutlineColor", Color.Lerp(colorStart, colorEnd, lerpControl));
        Color nextColor = Color.Lerp(colorStart, colorEnd, alphaControl);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_FirstOutlineColor", nextColor);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", Mathf.Lerp(0, 1.0f, widthControl));
        //gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_FirstOutlineWidth", 1.0f);


        // gameObject.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", new Color(1.0f, 0.5f, 0.5f, 1.0f));

        if (widthControl >= 2.0)
        {
            widthControl = 0;
            alphaControl = 0;
        }
        //if (lerpControl >= 1.0) {
        //    lerpControl = 0;
        //    colorStart = gameObject.GetComponent<Renderer>().sharedMaterial.GetColor("_FirstOutlineColor");

        //    if (colorEndIndex == 7)
        //    {
        //        colorEndIndex = 0;
        //    }
        //    else {
        //        colorEndIndex++;
        //    }
        //    colorEnd = colorArray[colorEndIndex];
        //}

    }
}
