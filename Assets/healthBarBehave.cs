using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarBehave : MonoBehaviour
{
    [SerializeField]
    PlayerController playcon;
    [SerializeField]
    Slider slider;

    // Update is called once per frame
    void Update()
    {
        if (playcon.faceRight) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);
    }

    public void setHealthBar(float _value)
    {
        slider.value = _value;
    }
}
