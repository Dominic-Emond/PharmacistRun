using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }
    public Sprite[] sprites;

    void Awake()
    {
        Debug.Log("test");
        instance = this;
    }

    // Update is called once per frame
    public void SetValue(int health)
    {
        var image = gameObject.GetComponent<Image>();
        if (health == 3)
        {
            image.sprite = sprites[0];
        }
        else if (health == 2)
        {
            image.sprite = sprites[1];
        }
        else if (health == 1)
        {
            image.sprite = sprites[2];
        }
        else
        {
            image.sprite = sprites[3];
        }
    }
}
