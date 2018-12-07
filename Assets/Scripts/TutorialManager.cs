﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public GameObject athenaUI;
    public Text athenaDialogue;
    public bool dialogueTriggered = false;
    public GameObject hud;
    public Queue<string> sentences;
    public int curWave;
    public GameObject station;
    public bool skip = false;
    public bool wave1TutComplete = false;

    private int timesPressed = 0;

    // Use this for initialization
    void Start () {
        //Time.timeScale = 0;
        station = GameObject.Find("HoleStation(Clone)");
        hud = gameObject.GetComponent<LevelManager>().hud;
        athenaUI = hud.transform.Find("Athena Canvas").gameObject;
        sentences = hud.transform.Find("DialogueManager").gameObject.GetComponent<DialogueManager>().sentences;
        curWave = hud.GetComponent<GameUIPanels>().curWave;
    }
    
    // Update is called once per frame
    void Update () {
        curWave = hud.GetComponent<GameUIPanels>().curWave;
        sentences = hud.transform.Find("DialogueManager").gameObject.GetComponent<DialogueManager>().sentences;

        if (curWave == 1)
        {
            if (timesPressed == 0)
            {
                Time.timeScale = 0;
            }

            if (!wave1TutComplete && (Input.GetKeyDown("y") || Input.GetKey("t") || Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton2)) && !dialogueTriggered)
            {
                // We are in danger scientists! I am Athena, an AI security protocol that you created.
                // I've just detected an enemy intrusion and we must act quickly to defend the lab.
                FindObjectOfType<DialogueTrigger>().TriggerDialogue();
                dialogueTriggered = true;
                timesPressed++;
            }
            else if (!wave1TutComplete && (Input.GetKeyDown("y") || Input.GetKeyDown(KeyCode.JoystickButton1))) // && timesPressed == 1)
            {
                // I have activated the interface that displays your vital statistics.
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
                Debug.Log(sentences.Count);
                timesPressed++;
            }

            if (!wave1TutComplete && (Input.GetKey("t") || Input.GetKey(KeyCode.JoystickButton2)))
            {
                skip = true;

                while (sentences.Count > 2)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                }

                wave1TutComplete = true;
            }

            if (sentences.Count == 2 && (Input.GetKeyDown("y") || Input.GetKeyDown("t") || Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton2)))
            {
                Debug.Log(sentences.Count);
                athenaUI.SetActive(false);
                athenaUI.transform.Find("Skip").gameObject.SetActive(false);
                Time.timeScale = 1;
                wave1TutComplete = true;
            }
        }

        if (curWave == 2)
        {

            if (sentences.Count == 2 && (Input.GetKeyUp("y") || Input.GetKeyUp(KeyCode.JoystickButton1)))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
                athenaUI.SetActive(false);
                Time.timeScale = 1;
                station.GetComponent<StationStatus>().playucksound();
            }

        }

        if (curWave == 3)
        {

            if (Input.GetKeyDown("y") || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
                athenaUI.SetActive(false);
                Time.timeScale = 1;
                station.GetComponent<StationStatus>().playucksound();
            }

        }

    }
}
