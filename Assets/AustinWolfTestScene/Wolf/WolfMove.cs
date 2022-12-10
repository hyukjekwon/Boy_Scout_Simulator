using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfMove : MonoBehaviour
{
    public GameObject player;
    private Transform dest;
    private NavMeshAgent navMeshA;
    private NavMeshPath navMeshPath;
    private Vector3 groundpos;
    private RaycastHit hit;
    private RaycastHit hitplayer;
    private Animator anim;
    private AudioSource source;
    public  AudioClip[] Howls;
    public float speed = 15.0f;
    public float lineofsightdistance = 100.0f;
    public float fieldofview = 45.0f;
    public float hearingdistance = 30.0f;
    public float attackdistance = 3.0f; 
    private float lastactiontime;
    // Start is called before the first frame update
    void Start()
    {
        dest = player.transform;
        anim = GetComponent<Animator>();
        navMeshA = GetComponent<NavMeshAgent>();
        speed = navMeshA.speed;
        lastactiontime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   
        /*
        General idea is that if the player is within viewing distance an there is an unobstructed line of sight then the wolf will walk towards at the player at 10.0f speed.
        If the wolf is within hearing distance of the wolf it will then run at 15.0f speed.
        Otherwise it moves in the general area and plays various idle animations.
        */

        bool doesHitplayer;
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y+1, transform.position.z), new Vector3(dest.position.x, dest.position.y-0.5f, dest.position.z) - transform.position, out hitplayer)){ //Direct line of sight raycast from wolf to middle of player
            if(hitplayer.collider.gameObject.CompareTag("Player")){
                doesHitplayer = true;
            }
            else{
                doesHitplayer = false;
            }
        }
        else{
            doesHitplayer = false;
        }
        navMeshPath = new NavMeshPath();
        //bool DoesPathExist = navMeshA.CalculatePath(dest.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
        bool DoesPathExist = true;

        //If player is in field of view, and it can directly see the player, and the distance is less than lineofsightdistance 
        //and if a path exists to the player
        //and it has been 3 seconds since an idle animation started playing
        //or if wolf is within hearing distance of player
        //Player must have "Player" tag for this to work
        if ((Vector3.Angle(dest.position - transform.position, transform.forward) <= fieldofview && Vector3.Distance(dest.position, transform.position) <= lineofsightdistance && doesHitplayer) || Vector3.Distance(dest.position, transform.position) <= hearingdistance && Time.time-lastactiontime > 3 && DoesPathExist){ 
            //If wolf is within hearing distance of player
            if (Vector3.Distance(dest.position, transform.position) <= hearingdistance) {
                 //If wolf is within attacking distance of player
                if(Vector3.Distance(dest.position, transform.position) <= attackdistance){
                    Debug.Log("Caught!");
                    anim.SetInteger ("attack2", 1);
                    anim.SetInteger ("run", 0);
                    anim.SetInteger ("walk", 0);
                    navMeshA.speed = 0.0f;
                    navMeshA.isStopped = true;
                }
                else{ //Within hearing distance
                    anim.SetInteger ("run", 1);
                    anim.SetInteger ("attack2", 0);
                    navMeshA.speed = 15.0f;
                    Debug.Log("I hear you");
                    navMeshA.isStopped = false;
                    if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                        Debug.DrawLine (dest.position, hit.point, Color.cyan);
                        navMeshA.SetDestination(hit.point); //Set destination to the ground location of player
                    }
                    else{
                        navMeshA.SetDestination(dest.transform.position); //If there is no ground beneath the player then set the destination to the players position
                    }
                }
            }
            else{ //Only within seeing distance
                anim.SetInteger ("walk", 1);
                anim.SetInteger ("run", 0);
                navMeshA.speed = 10.0f;
                Debug.Log("I see you");
                Debug.DrawLine (new Vector3(transform.position.x, transform.position.y+1, transform.position.z), hitplayer.point, Color.green);
                navMeshA.isStopped = false;
                if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                        Debug.DrawLine (dest.position, hit.point, Color.cyan);
                        navMeshA.SetDestination(hit.point); //Set destination to the ground location of player
                } else {
                    navMeshA.SetDestination(dest.transform.position); //If there is no ground beneath the player then set the destination to the players position
                }
            }
            //lastactiontime = Time.time; //Only want idle animations to play when the wolf has been in an idle state for awhile
        }
        else{
            //Add idle animations here
            anim.SetInteger ("walk", 0);
            anim.SetInteger ("run", 0);
            anim.SetInteger ("attack2", 0);
            navMeshA.isStopped = true;
            anim.SetInteger ("howl", 0);
            if(Time.time-lastactiontime > 10.0f){ //One random howl a minute
                PlayHowl();
            }
        }
    }
    void PlayHowl(){
        lastactiontime = Time.time;
        AudioClip NextHowl = Howls[Random.Range(0, Howls.Length)];
        AudioSource.PlayClipAtPoint(NextHowl, transform.position);
        anim.SetInteger ("howl", 1);
    }
}
  