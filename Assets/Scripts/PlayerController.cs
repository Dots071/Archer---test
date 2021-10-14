using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A static class that is in charge of the player/bow movement and state.
 */

public class PlayerController : Singleton<PlayerController>
{
    public Events.EventBowState OnBowStateChanged;

    public enum BowState {
        READY,
        PULLED,
        RELEASED,
        RELOADING
    }

    private BowState currentBowState = BowState.READY;

    public BowState CurrentBowState
    {
        get { return currentBowState; }
        private set { currentBowState = value; }
    }

    private List<GameObject> arrowsQuiver;
    [SerializeField] int arrowsAmount = 5;
    [SerializeField] GameObject arrowPrefab;

    [SerializeField] Transform arrowTransform;

    
    private bool isBowLoaded = false;

   //  private Transform initialArrowTransform;


    private void Start()
    {
        InitQuiver();

        StartCoroutine("LoadBow");

    }


    private void Update()
    {
        if (isBowLoaded)
        {
            if (Input.GetMouseButtonDown(0) && currentBowState == BowState.READY)
            {
                    UpdateBowState(BowState.PULLED);
            }
            else if (Input.GetMouseButtonUp(0) && currentBowState == BowState.PULLED)
            {
                    UpdateBowState(BowState.RELEASED);
            }
        }

    }

    public void UpdateBowState (BowState state)
    {
        BowState previousBowState = currentBowState;
        currentBowState = state;

        switch (currentBowState)
        {
            case BowState.READY:
                break;

            case BowState.PULLED:
                break;

            case BowState.RELEASED:
                break;

            case BowState.RELOADING:
                isBowLoaded = false;
                StartCoroutine("LoadBow");
                break;

            default:
                break;
        }

        OnBowStateChanged.Invoke(currentBowState, previousBowState);
    }


    // Creates an arrow pool at the arrowTransform position.
    private void InitQuiver()
    {
        arrowsQuiver = new List<GameObject>();
        GameObject tempArrow;
        for (int i = 0; i < arrowsAmount; i++)
        {
            tempArrow = Instantiate(arrowPrefab, arrowTransform.position, Quaternion.identity);

            tempArrow.SetActive(false);
            arrowsQuiver.Add(tempArrow);
        }
    }

    // Returns the active arrow from the arrow pool.
    private GameObject GetArrowFromQuiver()
    {
        for (int i = 0; i < arrowsAmount; i++)
        {
            if (!arrowsQuiver[i].activeInHierarchy)
            {
                return arrowsQuiver[i];
            }            
        }

        return null;
    }

    // Loads an arrow from the quiver and updates the bow state.
    private IEnumerator LoadBow()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject loadedArrow;
        loadedArrow = GetArrowFromQuiver();

        if (loadedArrow != null)
        {
            loadedArrow.transform.parent = arrowTransform.transform;
            loadedArrow.transform.position = arrowTransform.position;
            loadedArrow.transform.rotation = arrowTransform.rotation;
            loadedArrow.SetActive(true);
        }
        //GameObject newArrow = Instantiate(arrowPrefab, initialArrowTransform);

        isBowLoaded = true;
        UpdateBowState(BowState.READY);
    }

}
