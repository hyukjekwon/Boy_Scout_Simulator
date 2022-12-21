using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MamaBearMove : MonoBehaviour
{
    public GameObject player;
    public GameObject cub;
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
    private float protectdistance = 20;
    public float speed = 15.0f;
    public float stalkingdistance = 100.0f;
    public float fieldofview = 45.0f;
    public float hearingdistance = 40.0f;
    public float attackdistance = 4.5f; 
    private float lastactiontime;
    public float wanderRadius;
    private float gameovertimer;
    private Vector3 newDest;
    private GameObject UImanager;
    private GameObject GameOver;
    private bool isgameover;
    private GameObject characterSpotted;
    private bool beingspotted;
    // Start is called before the first frame update
    void Start()
    {
        GameOver = GameObject.Find("GameOver");
        dest = player.transform;
        anim = GetComponent<Animator>();
        navMeshA = GetComponent<NavMeshAgent>();
        UImanager = GameObject.Find("EventSystem");
        speed = navMeshA.speed;
        lastactiontime = Time.time;
        source = gameObject.GetComponent<AudioSource>();
        isgameover = false;
        characterSpotted = GameObject.Find("AnimalLockOnText");
        beingspotted = false;
    }

    // Update is called once per frame
    void Update()
    {   if (isgameover){
            anim.SetInteger ("attack2", 1);
            anim.SetInteger ("run", 0);
            anim.SetInteger ("walk", 0);
            navMeshA.speed = 0.0f;
            if(Time.time-lastactiontime > Random.Range(3.0f, 5.0f)){ 
                PlayGrowl();
            }
            if(Time.time - gameovertimer >= 1 && Time.time - gameovertimer < 5){
                GameOver.transform.localScale = Vector3.one;
            }
            if(Time.time - gameovertimer >= 5){
                UImanager.GetComponent<UI_Management>().Pause();
            }
        }
        else {
            if (Vector3.Distance(player.transform.position, cub.transform.position) <= protectdistance && !(anim.GetCurrentAnimatorStateInfo(0).IsName("howl") || anim.GetAnimatorTransitionInfo(0).IsName("breathes -> howl") || anim.GetAnimatorTransitionInfo(0).IsName("howl -> breathes"))){
                /*
                General idea is that if the player is within stalking distance or there is an unobstructed line of sight then the bear will walk towards at the player at 5.0f speed.
                If the bear is within hearing distance of the bear it will then run at 15.0f speed.
                Otherwise it moves in the general area and plays various idle animations.
                */
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
                if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                    newDest = hit.point;
                }
                else{
                    newDest = player.transform.position;
                }
                bool DoesPathExist = navMeshA.CalculatePath(newDest, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
                //If player is in field of view, and it can directly see the player, or the distance is less than stalkingdistance 
                //and if a path exists to the player
                //and it has been 3 seconds since an idle animation started playing
                //or if bear is within hearing distance of player
                //Player must have "Player" tag for this to work
                if ((((Vector3.Angle(dest.position - transform.position, transform.forward) <= fieldofview && doesHitplayer) || Vector3.Distance(dest.position, transform.position) <= stalkingdistance) || Vector3.Distance(dest.position, transform.position) <= hearingdistance && Time.time-lastactiontime > 3) && DoesPathExist){ 
                    //If bear is within hearing distance of player
                    characterSpotted.SetActive(true);
                    characterSpotted.transform.localScale = Vector3.one;
                    beingspotted = true;
                    if (Vector3.Distance(dest.position, transform.position) <= hearingdistance) {
                        //If bear is within attacking distance of player
                        if(Vector3.Distance(dest.position, transform.position) <= attackdistance){
                            //Debug.Log("Caught!");
                            anim.SetInteger ("attack2", 1);
                            anim.SetInteger ("run", 0);
                            anim.SetInteger ("walk", 0);
                            navMeshA.speed = 0.0f;
                            navMeshA.isStopped = true;
                            if(Time.time-lastactiontime > Random.Range(2.0f, 3.0f)){ 
                                PlayGrowl();
                            }
                            //Game over
                            UImanager.GetComponent<UI_Management>().PausePlayer();
                            isgameover = true;
                        }
                        else{ //Within hearing distance
                            gameovertimer = Time.time;
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
                        }
                    }
                    else{ //Only within seeing distance
                        gameovertimer = Time.time;
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
                    if (beingspotted){
                        characterSpotted.SetActive(false);
                        beingspotted = false;
                    }
                    navMeshA.speed = 0.0f;
                    anim.SetInteger ("run", 0);
                    anim.SetInteger ("attack2", 0);
                    anim.SetInteger ("howl", 0);
                    anim.SetInteger ("walk", 0);
                }
            }
            else{
                if (beingspotted){
                    characterSpotted.SetActive(false);
                    beingspotted = false;
                }
                navMeshA.isStopped = false;
                //Stays near cub
                anim.SetInteger ("run", 0);
                anim.SetInteger ("attack2", 0);
                anim.SetInteger ("howl", 0);
                if (Vector3.Distance(transform.position, cub.transform.position) >= wanderRadius && !(anim.GetCurrentAnimatorStateInfo(0).IsName("howl") || anim.GetAnimatorTransitionInfo(0).IsName("breathes -> howl") || anim.GetAnimatorTransitionInfo(0).IsName("howl -> breathes"))) {
                    navMeshA.speed = 4.0f;
                    anim.SetInteger ("walk", 1);
                    anim.SetInteger ("howl", 0);
                    navMeshA.SetDestination(cub.transform.position);
                    //Debug.DrawLine (cub.transform.position, transform.position, Color.cyan);
                }
                else{
                    if (Vector3.Distance(transform.position, cub.transform.position) <= 15.0f){
                        navMeshA.speed = 0.0f;
                        anim.SetInteger ("walk", 0);
                        anim.SetInteger ("run", 0);
                        anim.SetInteger ("attack2", 0);
                        anim.SetInteger ("howl", 0);
                        if(Time.time-lastactiontime > 10.0f){
                            PlayHowl();
                        }
                    }

                }
            }
        }
    }
    void PlayHowl(){
        lastactiontime = Time.time;
        if (Howls.Length > 0){
            AudioClip NextHowl = Howls[Random.Range(0, Howls.Length)];
            AudioSource.PlayClipAtPoint(NextHowl, transform.position, source.volume);
        }
        anim.SetInteger ("howl", 1);
    }
    void PlayGrowl(){
        lastactiontime = Time.time;
        if (Growls.Length > 0){
            AudioClip NextGrowl = Growls[Random.Range(0, Growls.Length)];
            AudioSource.PlayClipAtPoint(NextGrowl, transform.position, source.volume);
        }
    }
    void PlayBreath(){
        lastactiontime = Time.time;
        if (Breaths.Length > 0){
            AudioClip NextBreath = Breaths[Random.Range(0, Breaths.Length)];
            AudioSource.PlayClipAtPoint(NextBreath, transform.position, source.volume);
        }
    }
}
  