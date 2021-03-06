﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using JetBrains.Annotations;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float shieldDuration;
    public GameObject shield;

    private Health playerHealth;
    public float speed = 12f;
    private Rigidbody2D rb;

    [HideInInspector]
    public int[] ammo = new int[2];
    //0 for lasers
    //1 for rockets
    //to be added later;

    //private GameController controllerCommunicator;
    public GameObject controller;

    private Vector2 screenTop;
    private Vector2 screenBottom;

    private float shipHeight;
    private float shipWidth;


    private float move;
    private float shipY;
    private float shipX;

    private bool shieldActive=false;

    private void Awake()
    {
        instance = this;
    }
    public void Heal(int amount)
    {
        playerHealth.Heal(amount);
        DisplayStats.UpdateHealth();
    }
    public void AddAmmo(int type, int amount)
    {
        ammo[type] += amount;
        DisplayStats.UpdateRocketAmmo(ammo[1]);
    }
    public int GetCurrentHP()
    {
        return playerHealth.healthPoints;
    }
    // Use this for initialization
    void Start()
    {
        playerHealth = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        screenTop = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        shipHeight = transform.localScale.y / 2;
        shipWidth = transform.localScale.x / 2;
        //controllerCommunicator = controller.GetComponent<GameController>();
        DisplayStats.UpdateHealth();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.instance.isDead == false)
        {
            if (Input.GetButton("Vertical"))
            {
                move = Input.GetAxis("Vertical") * Time.fixedDeltaTime * this.speed;
                shipY = transform.position.y;

                if (shipY > this.screenTop.y - this.shipHeight && move > 0)
                {
                    move = 0;
                    rb.velocity = (rb.velocity.y > 0) ? new Vector2(rb.velocity.x, 0) : rb.velocity;
                }

                if (shipY < this.screenBottom.y + this.shipHeight && move < 0)
                {
                    move = 0;
                    rb.velocity = (rb.velocity.y < 0) ? new Vector2(rb.velocity.x, 0) : rb.velocity;
                }

                rb.AddForce(move * Vector2.up);
                //transform.Translate(move * Vector2.up);
                //Debug.Log(shipY + " " + this.screenTop.y + " " + this.screenBottom.y);
            }

            if (Input.GetButton("Horizontal"))
            {
                move = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * this.speed;
                shipX = transform.position.x;

                if (shipX > this.screenTop.x - this.shipWidth && move > 0)
                {
                    move = 0;
                    rb.velocity = (rb.velocity.x > 0) ? new Vector2(0, rb.velocity.y) : rb.velocity;
                }

                if (shipX < this.screenBottom.x + this.shipWidth && move < 0)
                {
                    move = 0;
                    rb.velocity = (rb.velocity.x < 0) ? new Vector2(0, rb.velocity.y) : rb.velocity;
                }

                rb.AddForce(move * Vector2.right);
                //transform.Translate(move * Vector2.right);
                //Debug.Log(shipY + " " + this.screenTop.y + " " + this.screenBottom.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //Debug.Log(coll.gameObject.tag);
        if (coll.gameObject.tag == "Enemy" && !shieldActive)
        {
            playerHealth.TakeDamage(1);
            DisplayStats.UpdateHealth();
            if (playerHealth.healthPoints > 0)
            {
                StartCoroutine(SetShield(shieldDuration));
            }
            if (playerHealth.healthPoints <= 0)
            {
                //controllerCommunicator.isDead = true;
                //controllerCommunicator.PlayerCrash();
                GameController.instance.isDead = true;
                playerHealth.Died();
            }

        }
    }
    IEnumerator SetShield(float duration)
    {
        shield.SetActive(true);
        shieldActive = true;
        yield return new WaitForSeconds(duration);
        shieldActive = false;
        shield.SetActive(false);
        yield return null;
    }
}
