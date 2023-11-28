using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Not much to really say here
public class MeleeAttack : MonoBehaviourPunCallbacks
{
    public Transform attackPosition;
    public float attackRange;
    public LayerMask tomb;
    public LayerMask enemy;
    public int damage;

    private Animator playerAnim;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        playerAnim = GetComponent<Animator>();
    }

    //when the player presses left click triggers the animation
    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerAnim.SetTrigger("attack");
            }
        }
    }
    
    //This is for the animation event so that he deals the damage at the end of the attack instead of right away
    //Comparing both the enemy and tombstone using the same logic(colliders)
    public void AttackAtEndOfAnimation()
    {
        // Deals damage to the tombstone object
        Collider2D[] tombstoneDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, tomb);

        for (int i = 0; i < tombstoneDamage.Length; i++)
        {
            Tombstone tombstone = tombstoneDamage[i].GetComponent<Tombstone>();
            if (tombstone != null)
            {
                tombstone.TakeDamage(damage);
            }
        }

        // Deal damage to enemy
        Collider2D[] enemyDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemy);

        for (int i = 0; i < enemyDamage.Length; i++)
        {
            EnemyHealth enemyHealth = enemyDamage[i].GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
