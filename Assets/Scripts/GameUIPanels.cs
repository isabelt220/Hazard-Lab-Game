﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIPanels : MonoBehaviour {

    [SerializeField] private GameObject waveCompletePanel;
    [SerializeField] private GameObject gameCompletePanel;
    [SerializeField] private GameObject gameOverPanel;
    public GameObject station;
    public bool paneltrue;
    public GameObject gm;
    public GameObject athenaUI;
    public bool curWaveComplete;
    public bool finalWaveComplete;
    public bool playerLose;
    private bool waveSwitched = false;
    private bool pauseGameInProgress;
    public AudioSource clickSound;
    public AudioSource beesintro;
    public int curWave = 1;

    void Start()
    {
        station = GameObject.Find("HoleStation(Clone)");
        waveCompletePanel = gameObject.transform.Find("Wave Complete Pause").gameObject;
        gameCompletePanel = gameObject.transform.Find("Game Complete Menu").gameObject;
        gameOverPanel = gameObject.transform.Find("Game Over Menu").gameObject;
        athenaUI = gameObject.transform.Find("Athena Canvas").gameObject;
        waveCompletePanel.SetActive(false);
        gameCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        paneltrue = false;
        gm = GameObject.FindWithTag("GameManager");
        curWaveComplete = gm.GetComponent<GameConstants>().curWaveComplete;
        finalWaveComplete = gm.GetComponent<GameConstants>().completeLvl3;

        pauseGameInProgress = false;
    }

    void Update()
    {
        curWaveComplete = gm.GetComponent<GameConstants>().curWaveComplete;
        finalWaveComplete = gm.GetComponent<GameConstants>().completeLvl3;

        if (curWaveComplete)
        {
            if (!waveCompletePanel.activeInHierarchy && !pauseGameInProgress)
            {   
                pauseGameInProgress = true;
                Invoke("PauseGame", 3);
                if (Input.GetKey(KeyCode.JoystickButton0))
                {
                    clickSound.Play();
                }
            }
        }

        if (finalWaveComplete)
        {
            if (!gameCompletePanel.activeInHierarchy && !pauseGameInProgress)
            {
                pauseGameInProgress = true;
                Invoke("GameComplete", 3);
                if (Input.GetKey(KeyCode.JoystickButton0))
                {
                    clickSound.Play();
                }
            }
        }

        if (playerLose)
        {
            if (!gameOverPanel.activeInHierarchy)
            {
                GameOver();
                if (Input.GetKey(KeyCode.JoystickButton0))
                {
                    clickSound.Play();
                }
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0.0001f;
        if (!waveSwitched)
        {
            //Debug.Log(waveCompletePanel.transform.Find("buttons/Next Button").gameObject.ToString());
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(waveCompletePanel.transform.Find("buttons/Next Button").gameObject);
            //waveCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave1enemyKillCount.ToString();
            //waveCompletePanel.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave1comboFalling.ToString();
            //waveCompletePanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave1comboBoom.ToString();
            //waveCompletePanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave1comboShatter.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Guile Death/Player Death Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave1GuileDeath.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Bob Death/Player Death Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave1BobDeath.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Enemies Killed/Enemies Killed Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave1enemyKillCount.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Combos/Shrink+Hole Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave1comboFalling.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Combos/Oil+Electricity Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave1comboBoom.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Combos/Freeze+Shockwave Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave1comboShatter.ToString();
        }
        else if (waveSwitched)
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(waveCompletePanel.transform.Find("buttons/Next Button").gameObject);
            //waveCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave2enemyKillCount.ToString();
            //waveCompletePanel.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave2comboFalling.ToString();
            //waveCompletePanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave2comboBoom.ToString();
            //waveCompletePanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave2comboShatter.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Guile Death/Player Death Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave2GuileDeath.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Bob Death/Player Death Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave2BobDeath.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Enemies Killed/Enemies Killed Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave2enemyKillCount.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Combos/Shrink+Hole Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave2comboFalling.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Combos/Oil+Electricity Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave2comboBoom.ToString();
            waveCompletePanel.transform.Find("Panel/Content/Combos/Freeze+Shockwave Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave2comboShatter.ToString();
        }
            waveCompletePanel.SetActive(true);
        station.GetComponent<StationStatus>().stopsucksound();
        //Disable scripts that still work while timescale is set to 0
    }

    private void GameComplete()
    {
        Time.timeScale = 0.0001f;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(gameCompletePanel.transform.Find("buttons/Restart Button").gameObject);
        //gameCompletePanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave3enemyKillCount.ToString();
        //gameCompletePanel.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave3comboFalling.ToString();
        //gameCompletePanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave3comboBoom.ToString();
        //gameCompletePanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = GameManager.instance.GetComponent<GameConstants>().wave3comboShatter.ToString();
        waveCompletePanel.transform.Find("Panel/Content/Guile Death/Player Death Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave3GuileDeath.ToString();
        waveCompletePanel.transform.Find("Panel/Content/Bob Death/Player Death Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave3BobDeath.ToString();
        gameCompletePanel.transform.Find("Panel/Content/Enemies Killed/Enemies Killed Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave3enemyKillCount.ToString();
        gameCompletePanel.transform.Find("Panel/Content/Combos/Shrink+Hole Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave3comboFalling.ToString();
        gameCompletePanel.transform.Find("Panel/Content/Combos/Oil+Electricity Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave3comboBoom.ToString();
        gameCompletePanel.transform.Find("Panel/Content/Combos/Freeze+Shockwave Number").gameObject.GetComponent<Text>().text = gm.GetComponent<GameConstants>().wave3comboShatter.ToString();
        station.GetComponent<StationStatus>().stopsucksoundforreal();
        gameCompletePanel.SetActive(true);
    }

    public void NextWave()
    {
        gm.GetComponent<GameConstants>().curWaveComplete = false;
        Time.timeScale = 1;
        if(gm.GetComponent<GameConstants>().completeLvl2){
            beesintro.Play();
        }
        //station.GetComponent<StationStatus>().playucksound();
        waveCompletePanel.SetActive(false);
        paneltrue = false;
        if (!waveSwitched) { waveSwitched = true; }
        else if (waveSwitched) { waveSwitched = false; }

        //enable the scripts again

        pauseGameInProgress = false;
        //if (!gm.GetComponent<TutorialManager>().skip)
        //{
        //    Time.timeScale = 0;
        //    athenaUI.SetActive(true);
        //    //curWave++;
        //    Invoke("Whatever3", 1);
        //}

    }
    void Whatever3(){
        //athenaUI.SetActive(true);
        curWave++;
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        gm.GetComponent<GameConstants>().gameOver = false;

        if (gameCompletePanel.activeInHierarchy)
        {
            gameCompletePanel.SetActive(false);
        }

        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }

        pauseGameInProgress = false;
    }

    public void GameOver()
    {
        Time.timeScale = 0.0001f;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(gameOverPanel.transform.Find("buttons/Restart Button").gameObject);
        gameOverPanel.SetActive(true);
    }

    public void MainMenu()
    {
        gm.GetComponent<GameConstants>().gameOver = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
