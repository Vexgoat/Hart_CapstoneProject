using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    // Method called at the end of the attack animation
    public void AttackAtEndOfAnimation()
    {
        // Deal damage to tombstones
        Collider2D[] tombstoneDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, tomb);

        for (int i = 0; i < tombstoneDamage.Length; i++)
        {
            Tombstone tombstone = tombstoneDamage[i].GetComponent<Tombstone>();
            if (tombstone != null)
            {
                tombstone.TakeDamage(damage);
            }
        }

        // Deal damage to enemies
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
