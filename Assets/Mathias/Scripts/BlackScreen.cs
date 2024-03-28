using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] Image blackScreen;
    Color blackoutColor;
    bool blackout;
    // Start is called before the first frame update
    void Start()
    {
        blackoutColor = blackScreen.color;
    }
    // Update is called once per frame
    void Update()
    {
        if (blackScreen.color.a != blackoutColor.a)
        {
            blackScreen.color = Color.Lerp(blackScreen.color, blackoutColor, 10*Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.LogWarning("Switch "+blackout);
            blackout = !blackout;
            blackoutColor.a = blackout ? 0 : 1;

        }
    }
}
