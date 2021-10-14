using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class is in charge of arrow's functionality & movement


public class ArrowController : MonoBehaviour
{

    public enum ArrowState
    {
        NOTACTIVE,
        LOADED,
        PULLED,
        FLYING,
        HIT
    }

    ArrowState currentArrowState;

    Rigidbody rb;

    bool isArrowActive;     

    float pullingTime;
    float shootForce;

    [SerializeField] float startingShootForce;
    [SerializeField] float pullingForce;
    [SerializeField] float minShootForce;
    [SerializeField] float maxShootForce;
    [SerializeField] float shootTorque;     


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        PlayerController.Instance.OnBowStateChanged.AddListener(HandleBowStateChanged);
        rb.useGravity = false;

    }

    private void OnEnable()
    {
        currentArrowState = ArrowState.LOADED;
        isArrowActive = true;
        shootForce = startingShootForce;
    }


    // This function gets notified from the PlayerController about the state of the bow and changes the arrow settings accordignly.

    private void HandleBowStateChanged(PlayerController.BowState currentBowState, PlayerController.BowState previousBowState)
    {
        if (isArrowActive)
        {
           // Debug.Log("current state: " + currentBowState + "  previous state: " + previousBowState);

            switch (currentBowState)
            {
                case PlayerController.BowState.RELEASED:
                    if (currentArrowState == ArrowState.PULLED)
                    {
                        if(shootForce > minShootForce)
                        {
                            currentArrowState = ArrowState.FLYING;
                            gameObject.transform.SetParent(null);
                            PlayerController.Instance.UpdateBowState(PlayerController.BowState.RELOADING);

                            rb.useGravity = true;
                        } else
                        {
                            shootForce = startingShootForce;
                            currentArrowState = ArrowState.LOADED;
                            PlayerController.Instance.UpdateBowState(PlayerController.BowState.READY);
                        }

                    }

                    break;

                case PlayerController.BowState.PULLED:
                    if (currentArrowState == ArrowState.LOADED)
                    {
                        currentArrowState = ArrowState.PULLED;
                        pullingTime = 0;
                    }
                    break;
            }
        }
        
    }

    void FixedUpdate()
    {
        if (isArrowActive)
        {
            //Debug.Log("Shoot Force: " + shootForce);

            if (currentArrowState == ArrowState.PULLED)
                shootForce += pullingTime + Time.deltaTime * pullingForce;

            if (currentArrowState == ArrowState.FLYING)
            {
                if (transform.position.y < -1)
                {
                    Debug.Log("Arrow didn't hit anything!");
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.useGravity = false;
                    DestroyArrow();
                    return;
                }
                //rb.AddRelativeForce(transform.right * shootForce);
                rb.velocity = transform.forward * shootForce * Time.deltaTime;
                Debug.Log("Velocity: " + rb.velocity);
               // rb.AddTorque(-Vector3.right * shootTorque);
                transform.rotation = Quaternion.LookRotation(rb.velocity - Vector3.up * shootTorque);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentArrowState == ArrowState.FLYING)
        {
            currentArrowState = ArrowState.HIT;
            // Debug.Log("Collided with: " + collision.gameObject.name);
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            StartCoroutine("DestroyArrowSequence");
        }
    }
    //  Resets the arrow before returning it to its pool.


    private IEnumerator DestroyArrowSequence()
    {
        yield return new WaitForSeconds(1);
        DestroyArrow();
    }
    private void DestroyArrow()
    {
        currentArrowState = ArrowState.NOTACTIVE;
        isArrowActive = false;
        rb.constraints = RigidbodyConstraints.None;
        gameObject.SetActive(false);
    }

}
