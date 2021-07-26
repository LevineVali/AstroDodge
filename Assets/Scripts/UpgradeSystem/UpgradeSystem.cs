using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpgradeSystem : MonoBehaviour
{
    /// <summary>
    /// Playerstats
    /// </summary>
    private PlayerStats Stats;

    /// <summary>
    /// gamemanager
    /// </summary>
    public GameManager Manager;

    /// <summary>
    /// Base Health of the player
    /// </summary>
    public float BaseHealth = 100;

    /// <summary>
    /// Base weight of the player
    /// </summary>
    public float BaseWeight = 100;

    /// <summary>
    /// Base amount of fuel of the player
    /// </summary>
    public float BaseFuel = 150;

    /// <summary>
    /// Base speed of the player
    /// </summary>
    public float BaseSpeed = 10;

    /// <summary>
    /// Base amount of possible upgrades
    /// </summary>
    public int BaseAmount = 10;

    /// <summary>
    /// Base Slide speed
    /// </summary>
    public float BaseSlide = 1;

    /// <summary>
    /// Base Acceleration
    /// </summary>
    public float BaseAcceleration = 10;

    /// <summary>
    /// Bonusmultiplayier for offline
    /// </summary>
    public float BaseOfflineBonus = 1;


    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Header("Strength of Upgrades in %")]
    [Range(1, 100)]
    public int HullUpgradePercent = 1;

    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Range(1, 100)]
    public int FuelUpgradePercent = 1;

    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Range(1, 100)]
    public int JetUpgradePercent = 1;

    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Range(1, 100)]
    public int WeightUpgradePercent = 1;

    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Range(1, 100)]
    public int SlideUpgradePercent = 1;

    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Range(1, 100)]
    public int AccelerationUpgradePercent = 1;

    /// <summary>
    /// Strenght of the updates
    /// </summary>
    [Range(1, 100)]
    public int OfflineUpgradePercent = 1;

    [Header("Maximum amount of Upgrades")]
    public UpgradeLimit[] Limits;

    /// <summary>
    /// Sound of button
    /// </summary>
    private Sound buttonsound;

    private void Start()
    {
        Stats = FindObjectOfType<PlayerController>().Stats;
        Manager = FindObjectOfType<GameManager>();

        AudioManager AM = FindObjectOfType<AudioManager>();

        buttonsound = Array.Find(AM.Sound, sound => sound.Name == "Purchase");
    }

    public void Upgrade(int _type)
    {
        // increase Amount of current Upgrade
        Stats.Upgrades[_type].amount += 1;

        // check Upgradetype and Recalculate right value of the right Type
        switch (Stats.Upgrades[_type].type)
        {
            case UpgradeType.HULL:
                Stats.maxHealth += BaseHealth * (HullUpgradePercent / 100f);
                Stats.maxSpeedWOD += BaseSpeed * (JetUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            case UpgradeType.FUEL:
                Stats.maxFuel += BaseFuel * (FuelUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            case UpgradeType.JET:
                Stats.maxSpeed += BaseSpeed * (JetUpgradePercent / 100f);
                Stats.maxHealth += BaseHealth * (HullUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            case UpgradeType.WEIGHT:
                Stats.weight += BaseWeight * (WeightUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            case UpgradeType.SLIDE:
                Stats.slide += BaseSlide * (SlideUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            case UpgradeType.ACCELERATION:
                Stats.acceleration += BaseAcceleration * (AccelerationUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            case UpgradeType.OFFLINE:
                Stats.OfflineBonus += BaseOfflineBonus * (OfflineUpgradePercent / 100f);
                Stats.Currency.data -= Stats.Upgrades[_type].cost;
                break;

            default:
                break;
        }

        Stats.Upgrades[_type].cost = Manager.GetUpgradeCost(_type);

        // save playerstats
        SaveSystem.SavePlayerStats(Stats);

        buttonsound.Source.Play();

        // update UI
        Manager.uiManager.DataAmount.text = Stats.Currency.data.ToString();
    }

    public void Research(int _type)
    {
        // increase Amount of current Research
        Stats.Researches[_type].amount += 1;

        // check ResearchType an increase Maxamount of the correct UpgradeType
        switch (Stats.Researches[_type].type)
        {
            case ResearchType.HULL:
                Stats.Upgrades[(int)UpgradeType.HULL].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            case ResearchType.FUEL:
                Stats.Upgrades[(int)UpgradeType.FUEL].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            case ResearchType.JET:
                Stats.Upgrades[(int)UpgradeType.JET].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            case ResearchType.WEIGHT:
                Stats.Upgrades[(int)UpgradeType.WEIGHT].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            case ResearchType.SLIDE:
                Stats.Upgrades[(int)UpgradeType.SLIDE].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            case ResearchType.ACCELERATION:
                Stats.Upgrades[(int)UpgradeType.ACCELERATION].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            case ResearchType.OFFLINE:
                Stats.Upgrades[(int)UpgradeType.OFFLINE].maxAmount += BaseAmount;
                Stats.Currency.satellite -= Stats.Researches[_type].cost;
                break;

            default:
                break;
        }

        Stats.Researches[_type].cost = Manager.GetResearchCost(_type);

        // save playerstats
        SaveSystem.SavePlayerStats(Stats);

        buttonsound.Source.Play();

        // update UI
        Manager.uiManager.SatelliteAmount.text = Stats.Currency.satellite.ToString();
    }

    public void ResetPlayerStats(PlayerStats _stats, GameManager _manager)
    {
        _stats.maxFuel = BaseFuel;
        _stats.maxHealth = BaseHealth;
        _stats.maxSpeed = BaseSpeed;
        _stats.maxSpeedWOD = BaseSpeed;
        _stats.weight = BaseWeight * 2;
        _stats.slide = BaseSlide * 10;
        _stats.OfflineBonus = 1;
        _stats.highScore = 0;
        _stats.time = DateTime.UtcNow;
        
        for (int i = 0; i < _stats.Upgrades.Length; i++)
        {
            _stats.Upgrades[i].amount = 0;
            _stats.Upgrades[i].maxAmount = 10;
            _stats.Upgrades[i].cost = _manager.GetUpgradeCost(i);
        }

        for (int i = 0; i < _stats.Researches.Length; i++)
        {
            _stats.Researches[i].amount = 1;
            _stats.Researches[i].cost = _manager.GetResearchCost(i);
        }
    }

    [Serializable]
    public struct UpgradeLimit
    {
        public UpgradeType Type;
        public int Amount;
    }
}
