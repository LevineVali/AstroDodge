using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerStats
{
    public bool saved;

    public float maxFuel = 100;
    public float weight;
    public float maxHealth = 100;
    public float maxSpeed = 10;
    public float slide = 1;
    public float acceleration = 1;

    public float currentSpeed;
    public float fuel;
    public float health;

    public double currentScore;
    public double highScore;

    public Currency Currency;
    public Upgrade[] Upgrades = new Upgrade[(int)UpgradeType.MAX];
    public Research[] Researches = new Research[(int)ResearchType.MAX];
    public List<Skin> skinIDs = new List<Skin>(1);
    public int activeSkin;
    public int activeParticle;

    public int cometCount;
    public int asteroidCount;
    public int cloudCount;
    public int birdCount;
    public int cookedbirdCount;
    public int duckCount;
    public int ufoCount;
    public int prestige;
    public bool isPrestigeable;

    public float maxSpeedWOD;

    public DateTime time;

    public float OfflineBonus;

    // Update 1

    //Tracking information
    public List<Track> tracks = new List<Track>();

    // collectable count informations
    public int burgerCount;
    public int sandwichCount;
    public int presentCount;
    public int teddyCount;
    public int fireworksCount;
    public int electricCount;
    public int sodaCount;
    public int waterCount;

    // Update Number to check for older save-version
    public float UpdateNr;

    public PlayerStats(PlayerStats stats)
    {
        saved = stats.saved;
        maxFuel = stats.maxFuel;
        weight = stats.weight;
        maxHealth = stats.maxHealth;
        maxSpeed = stats.maxSpeed;
        maxSpeedWOD = stats.maxSpeedWOD;
        slide = stats.slide;
        acceleration = stats.acceleration;
        currentSpeed = stats.currentSpeed;
        fuel = stats.fuel;
        health = stats.health;
        currentScore = stats.currentScore;
        highScore = stats.highScore;
        Currency = stats.Currency;
        Upgrades = stats.Upgrades;
        Researches = stats.Researches;
        skinIDs = stats.skinIDs;
        activeSkin = stats.activeSkin;
        activeParticle = stats.activeParticle;
        cometCount = stats.cometCount;
        asteroidCount = stats.asteroidCount;
        cloudCount = stats.cloudCount;
        birdCount = stats.birdCount;
        cookedbirdCount = stats.cookedbirdCount;
        duckCount = stats.duckCount;
        ufoCount = stats.ufoCount;
        prestige = stats.prestige;
        isPrestigeable = stats.isPrestigeable;
        OfflineBonus = stats.OfflineBonus;
        time = stats.time;

        // if Update 1 isnt saved by older savefiles
        if (stats.UpdateNr <= 0)
        {
            // tracking informations
            if (tracks == null) 
                tracks = new List<Track>();
            tracks.Add(Track.UFO);
            tracks.Add(Track.DUCK);

            // Collectable counts
            burgerCount = 0;
            sandwichCount = 0;
            presentCount = 0;
            teddyCount = 0;
            fireworksCount = 0;
            electricCount = 0;
            sodaCount = 0;
            waterCount = 0;

            // correct skinIDs
            for (int i = 0; i < skinIDs.Count; i++)
            {
                switch (skinIDs[i].id)
                {
                    case 7:
                        Skin skin = new Skin()
                        {
                            id = 8,
                            color = skinIDs[i].color
                        };
                        skinIDs[i] = skin;
                        break;
                    case 8:
                        skin = new Skin()
                        {
                            id = 9,
                            color = skinIDs[i].color
                        };
                        skinIDs[i] = skin;
                        break;
                    case 9:
                        skin = new Skin()
                        {
                            id = 10,
                            color = skinIDs[i].color
                        };
                        skinIDs[i] = skin;
                        break;
                    case 10:
                        skin = new Skin()
                        {
                            id = 11,
                            color = skinIDs[i].color
                        };
                        skinIDs[i] = skin;
                        break;
                    case 11:
                        skin = new Skin()
                        {
                            id = 12,
                            color = skinIDs[i].color
                        };
                        skinIDs[i] = skin;
                        break;
                };
            }

            // set Updateversion
            UpdateNr = 1.1f;
        }
        // otherwise the savefile has already Update 1
        else
        {
            // tracking informations
            tracks = stats.tracks;

            // Collectable counts
            burgerCount = stats.burgerCount;
            sandwichCount = stats.sandwichCount;
            presentCount = stats.presentCount;
            teddyCount = stats.teddyCount;
            fireworksCount = stats.fireworksCount;
            electricCount = stats.electricCount;
            sodaCount = stats.sodaCount;
            waterCount = stats.waterCount;

            UpdateNr = stats.UpdateNr;
        }
    }
}

[System.Serializable]
public struct Currency
{
    public float data;
    public float satellite;
}

[System.Serializable]
public struct Upgrade
{
    public UpgradeType type;
    public int amount;
    public int maxAmount;
    public int cost;
}

[System.Serializable]
public struct Research
{
    public ResearchType type;
    public int amount;
    public int cost;
}

[System.Serializable]
public struct Skin
{
    public int id;
    public SkinColor color;
}

[System.Serializable]
public struct SkinColor
{
    public int grayIndex;

    public float r;
    public float g;
    public float b;
    public float a;

    public float true_r;
    public float true_g;
    public float true_b;
    public float true_a;
}

[System.Serializable]
public enum Track
{
    UFO,
    DUCK,
    BURGER,
    SANDWICH,
    PRESENT,
    TEDDY,
    FIREWORK,
    WATER,
    SODA,
    MAX
}