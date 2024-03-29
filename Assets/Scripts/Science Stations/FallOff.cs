﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOff : MonoBehaviour {

    public float fallSpeed = 20.0f;
    public float fallThreshold = 26.0f;
    public float destroyDelay = 5.0f;
    public float rotateRate = 270f;
    public float expandDuration = 0.75f;
    private GameObject station;
    private Renderer myRend;
    private bool opened;
    private float scaleX;
    private float scaleY;
    private float scaleZ;
    public ParticleSystem pullingEffect;
    public AudioSource dropsound;
    public AudioSource actualDropSound;
    public bool play;
    public GameObject hole1,hole2,hole3,hole4;
    GameObject gm;

    private void Start()
    {
        station = GameObject.Find("HoleStation(Clone)");
        myRend = GetComponent<Renderer>();
        myRend.enabled = false;
        opened = false;
        play = true;
        hole1 = GameObject.Find("Portal");
        //hole1 = GameObject.Find("HoleTile_1");
        //hole2 = GameObject.Find("HoleTile_2");
        //hole3 = GameObject.Find("HoleTile_3");
        //hole4 = GameObject.Find("HoleTile_4");
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        scaleZ = transform.localScale.z;
        gm = GameObject.FindWithTag("GameManager");
    }

    private void OnTriggerStay(Collider other)
    {
        if (station.GetComponent<StationStatus>().activated)
        {
            GameObject hitTarget = other.transform.root.gameObject;

            if (hitTarget.transform.localScale.x <= fallThreshold)
            {
                actualDropSound.Play();
                // Freeze movement by setting parameters
                if (hitTarget.name == "P1(Clone)")
                {
                    hitTarget.GetComponent<P1Status>().Fall();
                    StartCoroutine(StartFallingPlayer(hitTarget, 2.0f));

                    // Drop item after short delay
                }
                else if (hitTarget.name == "P2(Clone)")
                {
                    hitTarget.GetComponent<P2Status>().Fall();
                    StartCoroutine(StartFallingPlayer(hitTarget, 2.0f));

                    // Drop item after short delay
                }
                else if (hitTarget.tag == "Monster" && hitTarget.GetComponent<EnemyStatus>().willDie == false)
                {
                    // Enemy Fall
                    hitTarget.GetComponent<EnemyStatus>().Fall();
                    Destroy(hitTarget, destroyDelay);
                    gm.GetComponent<GameConstants>().enemyKillCount += 1;
                    gm.GetComponent<GameConstants>().comboFalling += 1;
                    gm.GetComponent<LevelManager>().myStats.GetComponent<StatsCounter>().incShrinkHoleKill();

                    // Start falling animation
                    StartCoroutine(StartFalling(hitTarget));
                }
                else{
                    return;
                }

                // Fall
                //dropsound.Play();
                //Vector3 tarDest = transform.position + new Vector3(0f, -100f, 0f);
                //Vector3 direction = tarDest - hitTarget.transform.position;
                //direction.Normalize();
                //hitTarget.transform.Translate(direction * fallSpeed * Time.deltaTime);

                // Destroy
                //Destroy(hitTarget, destroyDelay);
                // update the enemy death count
                //GameManager.instance.GetComponent<GameConstants>().enemyKillCount += 1;

            }
            if (hitTarget.transform.localScale.x > fallThreshold)
            {
                //play sound effect for  monster being too big to fall into hole(right now will do it for all monsters)
                if (hitTarget.tag == "Monster"){
                    if ((!dropsound.isPlaying) && (play))
                    {
                        dropsound.PlayOneShot(dropsound.clip);
                        hole1.GetComponent<FallOff>().play = false;
                        //hole2.GetComponent<FallOff>().play = false;
                        //hole3.GetComponent<FallOff>().play = false;
                        //hole4.GetComponent<FallOff>().play = false;
                        play = false;
                        Invoke("Whatever", 5);
                    }
                }

            }

        }

    }
    void Whatever()
    {
        play = true;
        hole1.GetComponent<FallOff>().play = true;
        //hole2.GetComponent<FallOff>().play = true;
        //hole3.GetComponent<FallOff>().play = true;
        //hole4.GetComponent<FallOff>().play = true;
    }

    private IEnumerator StartFalling (GameObject target) {
        // Disable colliders
        Collider[] allCollider = target.GetComponentsInChildren<Collider>();
        foreach (Collider col in allCollider)
        {
            col.enabled = false;
        }

        //dropsound.Play();
        Vector3 tarDest = transform.position + new Vector3(0f, -75f, 0f);
        Vector3 direction = tarDest - target.transform.position;
        direction.Normalize();

        while (target != null) {
            target.transform.Translate(2f * direction * fallSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator StartFallingPlayer(GameObject target, float duration)
    {
        // P1/P2
        if (target.name.Contains("P1")) {
            target.GetComponent<p1_movement>().speed = 0f;
        }else {
            target.GetComponent<p2_movement>().speed = 0f;
        }

        // Disable colliders
        Collider[] allCollider = target.GetComponentsInChildren<Collider>();
        foreach (Collider col in allCollider)
        {
            col.enabled = false;
        }

        //dropsound.Play();
        Vector3 tarDest = transform.position + new Vector3(0f, -75f, 0f);
        Vector3 direction = tarDest - target.transform.position;
        direction.Normalize();

        float i = 0;
        while (target != null && i < duration)
        {
            target.transform.Translate(2f * direction * fallSpeed * Time.deltaTime);
            i += Time.deltaTime;
            yield return null;
        }
        foreach (Collider col in allCollider)
        {
            col.enabled = true;
        }

    }

    private void FixedUpdate()
    {
        //IEnumerator holeAnimation = holeOpening();
        //if (station.GetComponent<StationStatus>().activated && !opened) {
        //    myRend.enabled = true;
        //    opened = true;
        //    StartCoroutine(holeAnimation);
        //}
        //else if (!station.GetComponent<StationStatus>().activated && opened) {
        //    myRend.enabled = false;
        //    opened = false;
        //    StopCoroutine(holeAnimation);
        //}
        if (station.GetComponent<StationStatus>().activated && !opened) {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            opened = true;
            pullingEffect.Play();
        }
        else if (!station.GetComponent<StationStatus>().activated && opened) {
            transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            opened = false;
            pullingEffect.Stop();
        }

        if (opened) {
            Attract();
        }
    }

    IEnumerator holeOpening()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        float curRotation = 0f;

        // Reach original size
        float i = 0f;
        while (i < expandDuration) {
            transform.localScale = transform.localScale + new Vector3(scaleX * Time.deltaTime / expandDuration,
                                                                      scaleY * Time.deltaTime / expandDuration,
                                                                      scaleZ * Time.deltaTime / expandDuration);
            curRotation += rotateRate * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, curRotation, 0));
            i += Time.deltaTime;
            yield return null;
        }
        // Keep rotating
        while (true) {
            curRotation += rotateRate * Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, curRotation, 0));
            yield return null;
        }
    }

    public void Attract() {
        //float pullForce = 5000f;
        //Non-escapable distance = 200
        //Monster scale: 30 -> 15
        //Player scale: 50 -> 25
        float maxPullForce = 5000f;
        float distFactor = 3000f * 200f;
        float sizeFactor = 2000f * 15f;
        float pullRadius = 200f;

        foreach (Collider body in Physics.OverlapSphere(transform.position, pullRadius)) {
            if (body.gameObject.tag == "Player" || body.gameObject.tag == "Monster")
            {
                // calculate direction from target to portal
                Vector3 forceDirection = new Vector3(transform.position.x - body.transform.position.x,
                                                     0f,
                                                     transform.position.z - body.transform.position.z);

                float distance = Vector3.Distance(transform.position, body.transform.position);
                if (distance < 1f) {
                    return;
                }

                float pullForce = distFactor / distance + sizeFactor / body.transform.localScale.x;
                if (pullForce > maxPullForce) {
                    pullForce = maxPullForce;
                }

                // apply force on target towards portal
                body.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
            }
        }
    }
}
