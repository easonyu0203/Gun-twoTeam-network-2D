using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieText : MonoBehaviour
{
    //public bool show = false;
    public float showTime = -1;
    public GameObject whenDie;

    private void Update()
    {
        if (showTime > 0)
        {
            turnOn();
            leftTime(showTime);
            showTime -= Time.deltaTime;
        }
        if(showTime <= 0)
        {
            TurnOff();
        }
    }

    void turnOn()
    {
        whenDie.SetActive(true);
    }

    void TurnOff()
    {
        whenDie.SetActive(false);
    }

    void leftTime(float time)
    {
        whenDie.GetComponentInChildren<Text>().text = "respawn in " + (int)time + " second...";
    }
}
