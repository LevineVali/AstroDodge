using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IAPManager : MonoBehaviour
{
    /// <summary>
    /// UIManager
    /// </summary>
    private UIManager uiManager;

    /// <summary>
    /// PlayerController
    /// </summary>
    private PlayerController pc;

    /// <summary>
    /// sound of button
    /// </summary>
    private Sound buttonsound;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        pc = FindObjectOfType<PlayerController>();

        AudioManager AM = FindObjectOfType<AudioManager>();

        buttonsound = Array.Find(AM.Sound, sound => sound.Name == "Button");
    }

    public void Purchase(int _value)
    {
        pc.Stats.Currency.data += _value;

        uiManager.DataAmount.text = pc.Stats.Currency.data.ToString();

        buttonsound.Source.Play();
    }
}
