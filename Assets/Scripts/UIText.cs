using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public static UIText instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void ChangeText(int level)
    {
        var text = GetComponent<Text>();
        text.text = "Level " + level.ToString();
    }
}
