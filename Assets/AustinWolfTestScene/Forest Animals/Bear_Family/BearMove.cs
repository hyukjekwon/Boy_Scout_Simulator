using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearMove : MonoBehaviour
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
    public  AudioClip[] Growls;
    public  AudioClip[] Breaths;
    private Vector3 StartPos;
    private Vector3 LastPos;
    private Vector3 runAwayPos;
    public float speed = 15.0f;
    public float stalkingdistance = 100.0f;
    public float fieldofview = 45.0f;
    public float hearingdistance = 40.0f;
    public float attackdistance = 4.5f; 
    private float lastactiontime;
    private bool scaredAway;
    private float timer;
    private Vector3 newDest;
    // Start is called before the first frame update
    void Start()
    {
        scaredAway = false;
        StartPos = transform.position;
        runAwayPos = StartPos;
        dest = player.transform;
        anim = GetComponent<Animator>();
        navMeshA = GetComponent<NavMeshAgent>();
        speed = navMeshA.speed;
        lastactiontime = Time.time;
        timer = 0;
        LastPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        /*
        General idea is that if the player is within stalking distance or there is an unobstructed line of sight then the bear will walk towards at the player at 5.0f speed.
        If the bear is within hearing distance of the bear it will then run at 15.0f speed.
        Otherwise it moves in the general area and plays various idle animations.
        Bear can be scared away staying still
        */
        if (!(scaredAway)){
            if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                newDest = hit.point;
            }
            else{
                newDest = player.transform.position;
            }
            bool doesHitplayer;
            if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y+1, transform.position.z), new Vector3(dest.position.x, dest.position.y-0.5f, dest.position.z) - transform.position, out hitplayer)){ //Direct line of sight raycast from bear to middle of player
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
            bool DoesPathExist = navMeshA.CalculatePath(newDest, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
            //Debug.Log(DoesPathExist);
            //If player is in field of view, and it can directly see the player, or the distance is less than stalkingdistance 
            //and if a path exists to the player
            //and it has been 3 seconds since an idle animation started playing
            //or if bear is within hearing distance of player
            //Player must have "Player" tag for this to work
            if ((((Vector3.Angle(dest.position - transform.position, transform.forward) <= fieldofview && Vector3.Distance(dest.position, transform.position) <= 150 && doesHitplayer) || Vector3.Distance(dest.position, transform.position) <= stalkingdistance) || Vector3.Distance(dest.position, transform.position) <= hearingdistance && Time.time-lastactiontime > 3) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("howl") || anim.GetAnimatorTransitionInfo(0).IsName("breathes -> howl") || anim.GetAnimatorTransitionInfo(0).IsName("howl -> breathes")) && DoesPathExist){ 
                //If bear is within hearing distance of player
                if (Vector3.Distance(dest.position, transform.position) <= hearingdistance) {
                    //If bear is within attacking distance of player
                    if(Vector3.Distance(dest.position, transform.position) <= attackdistance){
                        //Debug.Log("Caught!");
                        anim.SetInteger ("attack2", 1);
                        anim.SetInteger ("run", 0);
                        anim.SetInteger ("walk", 0);
                        navMeshA.speed = 0.0f;
                        navMeshA.isStopped = true;
                        if(Time.time-lastactiontime > Random.Range(3.0f, 5.0f)){ 
                            PlayGrowl();
                        }
                    }
                    else{ //Within hearing distance
                        anim.SetInteger ("run", 1);
                        anim.SetInteger ("attack2", 0);
                        navMeshA.speed = 15.0f;
                        //Debug.Log("I hear you");
                        navMeshA.isStopped = false;
                        if(Time.time-lastactiontime > 1.0f){
                            PlayBreath();
                        }
                        if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                            //Debug.DrawLine (dest.position, hit.point, Color.cyan);
                            navMeshA.SetDestination(hit.point); //Set destination to the ground location of player
                        }
                        else{
                            navMeshA.SetDestination(dest.transform.position); //If there is no ground beneath the player then set the destination to the players position
                        }

                        //Test to see if player moved
                        if (Vector3.Distance(LastPos, player.transform.position) <= 0.01f){
                            timer+=Time.deltaTime;
                        }
                        else{
                            timer = 0;
                        }
                        if(timer >= 1f){ //If player stay still for 1 seconds then run away
                            scaredAway = true;
                            runAwayPos = transform.position - transform.forward * 100f;
                        }
                        Debug.Log(timer);
                        LastPos = player.transform.position;
                    }
                }
                else{ //Only within seeing distance
                    //Test to see if player moved
                    if (Vector3.Distance(LastPos, player.transform.position) <= 0.01f){
                        timer+=Time.deltaTime;
                    }
                    else{
                        timer = 0;
                    }
                    if(timer >= 1f){ //If player stay still for 1 seconds then run away
                        scaredAway = true;
                        runAwayPos = transform.position - transform.forward * 40f;
                    }
                    LastPos = player.transform.position;
                    Debug.Log(timer);
                    anim.SetInteger ("walk", 1);
                    anim.SetInteger ("run", 0);
                    navMeshA.speed = 5.0f;
                    //Debug.Log("I see you");
                    //Debug.DrawLine (new Vector3(transform.position.x, transform.position.y+1, transform.position.z), hitplayer.point, Color.green);
                    navMeshA.isStopped = false;
                    if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                            //Debug.DrawLine (dest.position, hit.point, Color.cyan);
                            navMeshA.SetDestination(hit.point); //Set destination to the ground location of player
                    } else {
                        navMeshA.SetDestination(dest.transform.position); //If there is no ground beneath the player then set the destination to the players position
                    }
                }
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
        else{
            navMeshA.SetDestination(runAwayPos);
            anim.SetInteger ("run", 0);
            anim.SetInteger ("attack2", 0);
            anim.SetInteger ("howl", 0);
            anim.SetInteger ("walk", 1);
            navMeshA.speed = 5.0f;
            //Debug.DrawLine (runAwayPos, transform.position, Color.cyan);
            if (Vector3.Distance(new Vector3(runAwayPos.x, transform.position.y, runAwayPos.z), transform.position) <= 5){
                scaredAway = false;
                navMeshA.speed = 0.0f;
                anim.SetInteger ("walk", 0);
                anim.SetInteger ("run", 0);
                anim.SetInteger ("attack2", 0);
                anim.SetInteger ("howl", 0);
            }
        }
    }
    void PlayHowl(){
        lastactiontime = Time.time;
        if (Howls.Length > 0){
            AudioClip NextHowl = Howls[Random.Range(0, Howls.Length)];
            AudioSource.PlayClipAtPoint(NextHowl, transform.position);
        }
        anim.SetInteger ("howl", 1);
    }
    void PlayGrowl(){
        lastactiontime = Time.time;
        if (Growls.Length > 0){
            AudioClip NextGrowl = Growls[Random.Range(0, Growls.Length)];
            AudioSource.PlayClipAtPoint(NextGrowl, transform.position);
        }
    }
    void PlayBreath(){
        lastactiontime = Time.time;
        if (Breaths.Length > 0){
            AudioClip NextBreath = Breaths[Random.Range(0, Breaths.Length)];
            AudioSource.PlayClipAtPoint(NextBreath, transform.position);
        }
    }
}
  