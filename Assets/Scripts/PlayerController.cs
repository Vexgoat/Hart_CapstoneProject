using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{ 
    public float speed;
    public Animator playerAnim;

    private Rigidbody2D body;
    private bool ontheground;

    PhotonView view; 
 
    private void Start()
    {
        view = GetComponent<PhotonView>();
        body = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

    }
 
    //This method controls player movement and calls the flip player method and it also calls the jumping method.
    //And no i do not know why there is a "error" for not cleaning the rpcs
    //I looked for hours upon hours but it doesnt effect the game so i left it
    private void Update()
    {
            if (view.IsMine){
            float horizontalTrigger = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalTrigger * speed, body.velocity.y);
    
            if (horizontalTrigger > 0.3f){
                view.RPC("FlipPlayer", RpcTarget.All, true);
                PhotonNetwork.CleanRpcBufferIfMine(view);
            }else if (horizontalTrigger < -0.01f)
                view.RPC("FlipPlayer", RpcTarget.AllBuffered, false);

            
        if (Input.GetKey(KeyCode.Space) && ontheground)
            Jumping2();

        playerAnim.SetBool("walk", horizontalTrigger != 0);
        playerAnim.SetBool("ontheground", ontheground);
            }
    }

    //This method will call the jump method
    private void Jumping(){

        if (view.IsMine){
        view.RPC("Jump", RpcTarget.All);
        }

    }

    //This method checks to see if they are on the ground by using the ground tag.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            ontheground = true;
    }

    //This method is what is used to flip the player by using checking if they are facing one way and if so makes that number a negative on the x-axis
    [PunRPC]
    public void FlipPlayer(bool flipRight){
        if (flipRight)
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    else
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    //This method allows the player to jump by using the speed variable, it also plays the jump animation for player1 only
    [PunRPC]
    public void Jumping2(){
        body.velocity = new Vector2(body.velocity.x, speed);
        playerAnim.SetTrigger("jump");
        ontheground = false;
    }
}