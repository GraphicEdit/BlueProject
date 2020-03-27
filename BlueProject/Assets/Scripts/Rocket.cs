﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 1;
    [SerializeField]
    private Rigidbody2D rb;
	public float destroyDelay = 3f;
    private bool stillRunning = false; //to check if the coroutine is stil running

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (stillRunning) StopCoroutine("Deactivate");
        StartCoroutine(Deactivate(destroyDelay)); // Deactivate this bullet after the specified seconds
        rb.velocity = Vector2.zero;
    }	
   
    void Update()
    {
        rb.AddForce(speed * Time.fixedDeltaTime * Vector2.right);
    }

    IEnumerator Deactivate(float delay)
    {
        stillRunning = true;
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        stillRunning = false;
        yield return null;
    }
}
