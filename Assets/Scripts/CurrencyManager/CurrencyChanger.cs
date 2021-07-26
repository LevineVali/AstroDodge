using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CurrencyChanger : MonoBehaviour
{
    /// <summary>
    /// UI Manager
    /// </summary>
    private UIManager uiManager;

    /// <summary>
    /// Stats of the player
    /// </summary>
    private PlayerController pc;

    /// <summary>
    /// Values of Datas for switching currency
    /// </summary>
    public int[] DataValues;

    /// <summary>
    /// Values of satellite for switching
    /// </summary>
    public int[] SatellitValues;

    /// <summary>
    /// List of all Texts on Switchbuttons
    /// </summary>
    public Text[] SwitchTexts;

    /// <summary>
    /// All Currency Change Buttons
    /// </summary>
    public Button[] CCButtons;

    /// <summary>
    /// sound of button
    /// </summary>
    private Sound buttonsound;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        pc = FindObjectOfType<PlayerController>();

        for (int i = 0; i < SwitchTexts.Length; i++)
        {
            SwitchTexts[i].text = SatellitValues[i].ToString() + " <~ " + DataValues[i].ToString();
        }

        AudioManager AM = FindObjectOfType<AudioManager>();

        buttonsound = Array.Find(AM.Sound, sound => sound.Name == "Button");
    }

    private void Update()
    {
        if (!pc.Run)
        {
            // go through all Changeoptions
            for (int i = 0; i < CCButtons.Length; i++)
            {
                // if the player have enought Data for current Changeoption
                if (pc.Stats.Currency.data >= DataValues[i])
                {
                    // enable changeoption
                    CCButtons[i].interactable = true;
                }
                // otherwise
                else
                {
                    // deactivate changeoption
                    CCButtons[i].interactable = false;
                }
            }
        }
    }

    public void ChangeCurrency(int _index)
    {
        pc.Stats.Currency.data -= DataValues[_index];
        pc.Stats.Currency.satellite += SatellitValues[_index];

        uiManager.DataAmount.text = pc.Stats.Currency.data.ToString();
        uiManager.SatelliteAmount.text = pc.Stats.Currency.satellite.ToString();

        SaveSystem.SavePlayerStats(pc.Stats);

        buttonsound.Source.Play();
    }
}
