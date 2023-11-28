using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPunCallbacks
{
    public Transform[] patrolPoints;
    public float Speed = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private Animator enemyAnimator;
    private int patrolArray = 0;
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
            //Move the enemy to the patrol point
            Transform targetPoint = patrolPoints[patrolArray];

            if (transform.position.x < targetPoint.position.x)
            {
                enemySpriteRenderer.flipX = false; //Flip him to the right
            }
            else
            {
                enemySpriteRenderer.flipX = true; //Flip him to the left
            }

            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, Speed * Time.deltaTime);
                yield return null;
            }

            //Switch the patrol point to the next one
            patrolArray = (patrolArray + 1) % patrolPoints.Length;

            //Have him wait a second before patrolling to the other point
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        if (isAttacking)
            return;

        //Check if player2 is nearby and if so attack him
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
        
        yield return new WaitForSeconds(0.5f);

        //Deal damage to player2
        GameObject player = GameObject.FindGameObjectWithTag("Player2");
        if (player != null)
        {
            Player2Health player2Health = player.GetComponent<Player2Health>();
            if (player2Health != null)
            {
                player2Health.TakeDamage(attackDamage);
            }
        }

        //Coolodown before the enemy can attack again or else it is literally broken
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}
