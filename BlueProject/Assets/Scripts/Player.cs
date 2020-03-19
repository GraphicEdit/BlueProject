﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 12f;
    Rigidbody2D rb;

    private Vector2 screenTop;
    private Vector2 screenBottom;

    private float shipHeight;
    private float shipWidth;


	private float move;
	private float shipY;
	private float shipX;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        this.screenBottom = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        this.screenTop = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        this.shipHeight = transform.localScale.y / 2;
        this.shipWidth = transform.localScale.x / 2;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GameController.instance.isDead == false) {
            if (Input.GetButton("Vertical")) {
                move = Input.GetAxis("Vertical") * Time.fixedDeltaTime * this.speed;
                shipY = transform.position.y;

                if (shipY > this.screenTop.y - this.shipHeight && move > 0) {
                    move = 0;
                }

                if (shipY < this.screenBottom.y + this.shipHeight && move < 0) {
                    move = 0;
                }

                rb.AddForce(move * Vector2.up);
                //transform.Translate(move * Vector2.up);
                //Debug.Log(shipY + " " + this.screenTop.y + " " + this.screenBottom.y);
            }

            if (Input.GetButton("Horizontal")) {
                move = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * this.speed;
                shipX = transform.position.x;

                if (shipX > this.screenTop.x - this.shipWidth && move > 0) {
                    move = 0;
                }

                if (shipX < this.screenBottom.x + this.shipWidth && move < 0) {
                    move = 0;
                }

                rb.AddForce(move * Vector2.right);
                //transform.Translate(move * Vector2.right);
                //Debug.Log(shipY + " " + this.screenTop.y + " " + this.screenBottom.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        //Debug.Log(coll.gameObject.tag);
        if (coll.gameObject.tag == "Enemy") {
            GameController.instance.PlayerCrash();
            this.GetComponent<Animator>().CrossFade("Explosion",0);
            //this.gameObject.SetActive(false);
        }
    }
    
}
