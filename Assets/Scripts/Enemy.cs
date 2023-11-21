using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPunCallbacks
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private Animator enemyAnimator;
    private int currentPatrolIndex = 0;
    private bool isAttacking = false;
    private SpriteRenderer enemySpriteRenderer;

    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
        enemyAnimator = GetComponent<Animator>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            // Move towards the current patrol point
            Transform targetPoint = patrolPoints[currentPatrolIndex];

            if (transform.position.x < targetPoint.position.x)
            {
                enemySpriteRenderer.flipX = false; // Face right
            }
            else
            {
                enemySpriteRenderer.flipX = true; // Face left
            }

            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);
                yield return null;
            }

            // Change the patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

            // Wait for a moment before moving to the next patrol point
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        if (isAttacking)
            return;

        // Check if player is nearby and initiate attack
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player2"))
            {
                StartCoroutine(Attack());
                break;
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        enemyAnimator.SetTrigger("Attack");

        // Play attack animation, wait for the animation duration
        yield return new WaitForSeconds(0.5f);

        // Deal damage to the player
        GameObject player = GameObject.FindGameObjectWithTag("Player2");
        if (player != null)
        {
            Player2Health player2Health = player.GetComponent<Player2Health>();
            if (player2Health != null)
            {
                player2Health.TakeDamage(attackDamage);
            }
        }

        // Cooldown before the next attack
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}
