﻿using UnityEngine;
using System.Collections;

public class StationStatus : MonoBehaviour
{

    public bool activated = false;
    public bool waiting = false;
    public bool prepared = true;
    public float flashDuration = 0.075f;
    public float maxIntensity = 5.0f;
    Light myLight;
    Coroutine flashLight;
    GameObject gm;
    public ParticleSystem ParticleEffect;

    public float duraiton = 4f;
    private bool used;

    private void Start()
    {
        myLight = GetComponentInChildren<Light>();
        ParticleEffect = GetComponentInChildren<ParticleSystem>();

        gm = GameObject.FindGameObjectWithTag("GameManager");
        if (gm.GetComponent<GameConstants>().completeLvl1 == false &&
            (gameObject.name.Contains("ElectricityStation") ||
             gameObject.name.Contains("FreezeStation"))) {

            prepared = false;
        }

        // Set Arrow angle
        used = false;
        Vector3 pos = new Vector3(0f, 300f, -350f);
        transform.GetChild(0).GetChild(0).LookAt(pos);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!waiting && activated){
            StartCoroutine(waitForTermination());
            waiting = true;

            flashLight = StartCoroutine(flashNow());
            //Debug.Log("Waiting on Termination!!!");

            used = true;
        }

        if (!prepared || used)
        {
            disableArrow();

        }
        else {
            enableArrow();
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!prepared)
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            // Enable player control UI
            GameObject player = other.transform.root.gameObject;

            GameObject ui = player.transform.Find("ControlUI").gameObject;

            ui.GetComponent<Canvas>().enabled = true;

            // Enable station UI
            GameObject stationUI = gameObject.transform.Find("StationCanvas").gameObject;

            stationUI.GetComponent<Canvas>().enabled = true;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (!prepared) {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            // Disable player control UI
            GameObject player = other.transform.root.gameObject;

            GameObject ui = player.transform.Find("ControlUI").gameObject;

            ui.GetComponent<Canvas>().enabled = false;

            // Disable station UI
            GameObject stationUI = gameObject.transform.Find("StationCanvas").gameObject;

            stationUI.GetComponent<Canvas>().enabled = false;
        }
    }

    private IEnumerator waitForTermination()
    {
        yield return new WaitForSeconds(duraiton);
        activated = false;
        waiting = false;
        ParticleEffect.Stop();

        StopCoroutine(flashLight);
        myLight.intensity = maxIntensity;
    }

    private IEnumerator flashNow()
    {
        float waitTime = flashDuration  / (2 * maxIntensity);

        while(true){
            while (myLight.intensity < maxIntensity)
            {
                myLight.intensity += Time.deltaTime / waitTime;        // Increase intensity
                yield return null;
            }
            while (myLight.intensity > 0)
            {
                myLight.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
                yield return null;
            }
        }
    }

    public void disableArrow()
    {
        var parts = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer part in parts)
        {
            part.enabled = false;
        }
    }

    public void enableArrow()
    {
        var parts = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer part in parts)
        {
            part.enabled = true;
        }
    }
}
