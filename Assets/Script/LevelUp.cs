﻿using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Assets.Script;



public class LevelUp : MonoBehaviour
{
    public GameObject Menu;

    public Text textRemainingSouls;

    public Slider sliderForce;
    public Slider sliderDef;
    public Slider sliderSpeed;

    public Button buttonForce;
    public Button buttonDef;
    public Button buttonSpeed;
    public Button exitButton;

    private int forceValue;
    private int defenseValue;
    private int hpValue;
    private int SpeedValue;
    private int soulsNumber;

    private bool bdModifier = false;

    int maxForce = 120;
    int maxDef = 60;
    int maxSpeed = 30;

    int pointForce = 20;
    int pointDef = 10;
    int pointSpeed = 5;

    // Use this for initialization
    void Start()
    {
        AccesBD bd = new AccesBD();

        try
        {
            IDataReader reader;
            reader = bd.select("select * from Stats where Stats.idStats = 1");

            while (reader.Read())
            {
                hpValue = reader.GetInt32(1);
                forceValue = reader.GetInt32(2);
                defenseValue = reader.GetInt32(3);
                SpeedValue = reader.GetInt32(4);
                soulsNumber = reader.GetInt32(5);
            }

            UpdateUI();

            buttonForce.onClick.AddListener(onForceClick);
            buttonDef.onClick.AddListener(onDefenseClick);
            buttonSpeed.onClick.AddListener(onSpeedClick);
            exitButton.onClick.AddListener(exitLevelMenu);
        }

        catch (SqliteException e)
        {
            bd.Close();
            Debug.Log(e);
        }
        finally
        {
            bd.Close();
            //Debug.Log("bd fermer");
        }
    }

    public void UpdateUI()
    {
        textRemainingSouls.text = soulsNumber.ToString();

        sliderForce.value = forceValue / pointForce;
        sliderDef.value = defenseValue / pointDef;
        sliderSpeed.value = SpeedValue / pointSpeed;
    }

    public void onForceClick()
    {
        if (forceValue < maxForce && soulsNumber > 0)
        {
            bdModifier = true;
            forceValue += pointForce;
            soulsNumber--;
            UpdateUI();
        }
    }

    private void onDefenseClick()
    {
        if (defenseValue < maxDef && soulsNumber > 0)
        {
            bdModifier = true;
            defenseValue += pointDef;
            hpValue += 250;
            soulsNumber--;
            UpdateUI();
        }
    }

    private void onSpeedClick()
    {
        if (SpeedValue < maxSpeed && soulsNumber > 0)
        {
            bdModifier = true;
            SpeedValue += pointSpeed;
            soulsNumber--;
            UpdateUI();
        }
    }

    // voir si le escape met a jour la bd IMPORTANT
    public void exitLevelMenu()
    {
        if (bdModifier)
        {
            AccesBD bd = new AccesBD();

            try
            {
                string query = "update Stats Set Force = " + forceValue + ", Defence = " + defenseValue + ", Vitesse = " + SpeedValue + ", nbAmes = " + soulsNumber + ", Point_de_vie = " + hpValue + " where Stats.idStats = 1";
                bd.insert(query);
                bd.Close();
            }
            catch (SqliteException e)
            {
                bd.Close();
                Debug.Log(e);
                bdModifier = false;
            }
        }
    }

    public void saveSools()
    {
        AccesBD bd = new AccesBD();

        try
        {
            string query = "update Stats Set nbAmes = " + soulsNumber + " where Stats.idStats = 1";
            bd.insert(query);
            bd.Close();
        }
        catch (SqliteException e)
        {
            bd.Close();
            Debug.Log(e);
            bdModifier = false;
        }
    }

    public void addSouls(int theSouls)
    {
        soulsNumber += theSouls;
    }
}
