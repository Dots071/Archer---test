using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform attackPosition;

    Animator animator;
    NavMeshAgent navMesh;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        MoveToPlayer();
    }

    private void Update()
    {
        if (navMesh.remainingDistance <= 0.3f)
        {
            animator.SetBool("IsRunning", false);

        }
    }

    private void MoveToPlayer()
    {
        navMesh.SetDestination(attackPosition.position);
        animator.SetBool("IsRunning", true);
    }
}
