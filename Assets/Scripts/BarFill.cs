using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFill : MonoBehaviour
{
    public float Amount;
    public float MaxAmount;

    public void UpdateImage(float amount)
    {
        Amount = amount;
        gameObject.transform.localScale = new Vector3(Amount / MaxAmount, 1f, Amount / MaxAmount);
    }
}
