using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{ 
    public float speed;
    public Animator playerAnim;

    private Rigidbody2D body;
    private bool ontheground;

    PhotonView view; 
 
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
    }
 
    private void Update()
    {
            if (view.IsMine){
        
            //Player Movement
            float horizontalTrigger = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalTrigger * speed, body.velocity.y);
    
            // Flipping the player when moving.
            if (horizontalTrigger > 0.3f)
                view.RPC("FlipPlayer", RpcTarget.All, true);
            else if (horizontalTrigger < -0.01f)
                view.RPC("FlipPlayer", RpcTarget.All, false);
        

            //Player Jump
        if (Input.GetKey(KeyCode.Space) && ontheground)
            Jumping2();

        playerAnim.SetBool("walk", horizontalTrigger != 0);
        playerAnim.SetBool("ontheground", ontheground);
            }
    }

    private void Jumping(){

        if (view.IsMine){
        view.RPC("Jump", RpcTarget.All);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            ontheground = true;
    }

    [PunRPC]
    public void FlipPlayer(bool flipRight){
        if (flipRight)
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    else
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    [PunRPC]

    public void Jumping2(){
        body.velocity = new Vector2(body.velocity.x, speed);
        playerAnim.SetTrigger("jump");
        ontheground = false;
    }
}