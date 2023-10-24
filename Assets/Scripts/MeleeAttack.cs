using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeleeAttack : MonoBehaviourPunCallbacks
{
    private Animator playerAnim;

    public Transform attackPosition;
    public float attackRange;
    public LayerMask tomb;
    public int damage;

    private PhotonView view;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(view.IsMine){
        if (Input.GetMouseButtonDown(0))
        {
            playerAnim.SetTrigger("attack");
            Collider2D[] tombstoneDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, tomb);

            for (int i = 0; i < tombstoneDamage.Length; i++)
            {
                Tombstone tombstone = tombstoneDamage[i].GetComponent<Tombstone>();
                if (tombstone != null)
                {
                    tombstone.TakeDamage(damage);
                }
            }
        }
    }
}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
