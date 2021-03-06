﻿using System.Collections;
using System.Collections.Generic;
using BlueGame;
using UnityEngine;
using Audio;


namespace Core
{
    /*public class ObjectRef
    {
        public string tag { get; set; }
        public Vector3 lastLocation { get; set; }
        public float DropRateBonus { get; set; }

    }*/
    internal class Health : MonoBehaviour
    {
        [Range(0, 1)]
        public float dropChance = 0.3f;
        [HideInInspector]
        [SerializeField] public int healthPoints;
        [SerializeField] public int maxHealthPoints;
        //[HideInInspector] bool godMode;       
        //public delegate void SomeBodyDiedEventHandler(ObjectRef refe);
        //subscribe to this event to get notification on death of object
        //public event SomeBodyDiedEventHandler DieMOFO;       
        [SerializeField] public string diedVFXName;
        [SerializeField] public string diedSFXName;
        private bool died;
        private void Awake()
        {
            healthPoints = maxHealthPoints;
        }
        /*
         * it's more reliable to set in the inspector
        private void Awake()
        {
            healthpoints = 100;
            maxhealtpoints = 100;           
            godMode = false;
            died = false;
        }     
        I don't think we need godmode *yet*
        public void SetGodMode(bool stat)
        {
            if (gameObject.tag == "Player")
            {
                godMode = stat;
            }
        }
        */
        public void TakeDamage(int damage) //, GameObject caller = null) explain why we neeed caller?
        {
            if (!died)
            {
                healthPoints = (healthPoints - damage) > 0 ? (healthPoints - damage) : 0;
                if (healthPoints <= 0)
                {
                    died = true;
                    Died();
                }
                
            }
        }
        public void Died()
        {
            //last location can be used for drop location
            //I originally used this event for chest drops on a hack and slash game
            /* we can uncomment this later
            DieMOFO(new ObjectRef()
            {
                tag = gameObject.tag,
                DropRateBonus = 0,
                lastLocation = transform.position
            });
            */
            //disable movement
            //disable animations
            if (gameObject.CompareTag("Enemy"))
            {
                GameController.instance.enemyKilled++;
                if (Random.Range(0, 1f) > 1 - dropChance)
                {
                    PoolerManager.instance.Spawn(PowerUpList.instance.GetName(Random.Range(0, 2)), transform.position, Quaternion.identity);
                }
            }
            if (diedVFXName.Length != 0) PoolerManager.instance.Spawn(diedVFXName, transform.position, Quaternion.identity);
            if (diedSFXName.Length != 0) AudioManager.instance.Play(diedSFXName);
            gameObject.SetActive(false);
        }
        //heals the gameobject
        public void Heal(int extrahealth)
        {
            healthPoints = (healthPoints + extrahealth) < maxHealthPoints ? (healthPoints + extrahealth) : maxHealthPoints;
        }
        public bool HealthCheck(float percent)
        {
            var min = (maxHealthPoints - (maxHealthPoints * (percent + 3)) / 100);
            var max = (maxHealthPoints - (maxHealthPoints * (percent - 10)) / 100);
            if (healthPoints < max && healthPoints >= min)
            {
                return true;
            }
            return false;
        }
    }
}

