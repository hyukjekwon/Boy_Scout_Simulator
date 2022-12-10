using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfControl : MonoBehaviour
{   
    public float speed;
    public float walkspeed = 15.0f;
    private float runspeed;
    public float ForwardAcceleration = 0.75f;
    public float gravity = 20.0f;
    //public float BackwardsAcceleration = 0.25f;
    public float turnSpeed = 2.0f;
    private Vector3 newdirection = Vector3.zero;

    public GameObject fps_player_obj;
    public float radius_of_search_for_player = 30.0f;
    private Animator anim;
    private CharacterController controller;
   

    void Start () {
        speed = 0;
        runspeed = walkspeed*1.5f;
        controller = GetComponent <CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update (){
        //if(controller.isGrounded){
            if (Vector3.Distance(fps_player_obj.transform.position, transform.position) <= radius_of_search_for_player) {
                Vector3 playerPos = new Vector3(fps_player_obj.transform.position.x, 0, fps_player_obj.transform.position.z);
                Vector3 wolfPos = new Vector3(transform.position.x, 0, transform.position.z); 
                Vector3 newDir = (playerPos - wolfPos) / Vector3.Magnitude(playerPos - wolfPos);
                newdirection = new Vector3(newDir.x, newdirection.y ,newDir.z);
                anim.SetInteger ("walk", 1);
                if (Input.GetKey(KeyCode.LeftShift)){ //If in line of sight later
                    anim.SetInteger ("run", 1);
                    speed += ForwardAcceleration;
                    speed = Mathf.Clamp(speed, 0.0f, walkspeed); //set to runspeed later
                }
                else{
                    anim.SetInteger ("run", 0);
                    speed += ForwardAcceleration;
                    speed = Mathf.Clamp(speed, 0.0f, walkspeed);
                }
                //speed = 0;
            }else{
                anim.SetInteger ("walk", 0);
                anim.SetInteger ("run", 0);
                speed = 0;
            }

            Debug.DrawRay(transform.position, newdirection, Color.red); //Rotate to face player
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, newdirection, turnSpeed * Time.deltaTime, 0.0f);
            Debug.DrawRay(transform.position, rotateDirection, Color.blue);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        //}

        // if(controller.isGrounded){
        //     if (Input.GetKey ("space")) {
        //         newdirection.y += gravity;
        //     }
        // }
        controller.Move(newdirection * speed * Time.deltaTime);
        //newdirection.y -= gravity * Time.deltaTime;
    }
    
}

