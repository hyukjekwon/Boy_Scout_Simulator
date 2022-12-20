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
    public  AudioClip[] Growls;
    public  AudioClip[] Breaths;
    private Vector3 StartPos;
    private Vector3 newDest;
    private Vector3 candyPosition;
    public GameObject PieceOfCandy;
    public float speed = 15.0f;
    public float stalkingdistance = 100.0f;
    public float fieldofview = 45.0f;
    public float hearingdistance = 40.0f;
    public float attackdistance = 4.5f; 
    private float lastactiontime;
    private bool scaredAway;
    private float timer;
    private bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        scaredAway = false;
        StartPos = transform.position;
        dest = player.transform;
        anim = GetComponent<Animator>();
        navMeshA = GetComponent<NavMeshAgent>();
        speed = navMeshA.speed;
        lastactiontime = Time.time;
        timer=0;
        triggered = false;
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        
        /*
        General idea is that if the player is within stalking distance or there is an unobstructed line of sight then the wolf will walk towards at the player at 5.0f speed.
        If the wolf is within hearing distance of the wolf it will then run at 15.0f speed.
        Otherwise it moves in the general area and plays various idle animations.
        wolf can be scared away staying still
        */
        if (!(scaredAway)){
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
            if (Physics.Raycast(dest.position, -Vector3.up, out hit)) { //Line straight down
                newDest = hit.point;
            }
            else{
                newDest = player.transform.position;
            }
            bool DoesPathExist = navMeshA.CalculatePath(newDest, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete;
            //Debug.Log(DoesPathExist);
            //If player is in field of view, and it can directly see the player, or the distance is less than stalkingdistance 
            //and if a path exists to the player
            //and it has been 3 seconds since an idle animation started playing
            //or if wolf is within hearing distance of player
            //Player must have "Player" tag for this to work
            if ((((Vector3.Angle(dest.position - transform.position, transform.forward) <= fieldofview && Vector3.Distance(dest.position, transform.position) <= 150 && doesHitplayer) || Vector3.Distance(dest.position, transform.position) <= stalkingdistance) || Vector3.Distance(dest.position, transform.position) <= hearingdistance && Time.time-lastactiontime > 3) && !(anim.GetCurrentAnimatorStateInfo(0).IsName("howl") || anim.GetAnimatorTransitionInfo(0).IsName("breathes -> howl") || anim.GetAnimatorTransitionInfo(0).IsName("howl -> breathes")) && DoesPathExist){ 
                //If wolf is within hearing distance of player
                //Test to see if player yells
                if (PieceOfCandy.GetComponent<Candy>().candyCount > 0){
                    if(Input.GetKeyUp("q")){ 
                        scaredAway = true;
                    }
                }
                if (Vector3.Distance(dest.position, transform.position) <= hearingdistance) {
                    //If wolf is within attacking distance of player
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
                        navMeshA.SetDestination(newDest);
                    }
                }
                else{ //Only within seeing distance
                    anim.SetInteger ("walk", 1);
                    anim.SetInteger ("run", 0);
                    navMeshA.speed = 5.0f;
                    //Debug.Log("I see you");
                    //Debug.DrawLine (new Vector3(transform.position.x, transform.position.y+1, transform.position.z), hitplayer.point, Color.green);
                    navMeshA.isStopped = false;
                    navMeshA.SetDestination(newDest);
                }
            }
            else{
                //Add idle animations here
                anim.SetInteger ("eat", 0);
                anim.SetInteger ("walk", 0);
                anim.SetInteger ("run", 0);
                anim.SetInteger ("attack2", 0);
                navMeshA.isStopped = true;
                anim.SetInteger ("howl", 0);
                if(Time.time-lastactiontime > 10.0f){ //One random howl a minute
                    PlayHowl();
                }
            }
            anim.SetInteger ("eat", 0);
            triggered = false;
        }
        else{
            navMeshA.isStopped = false;
            anim.SetInteger ("walk", 0);
            anim.SetInteger ("attack2", 0);
            if(!(PieceOfCandy.GetComponent<Candy>().candyLanded || triggered)){
                NavMeshHit navhitpoint;
                if (NavMesh.SamplePosition(PieceOfCandy.transform.position, out navhitpoint, 50.0f, NavMesh.AllAreas)){
                    candyPosition = navhitpoint.position; 
                }
                else{
                    candyPosition = PieceOfCandy.transform.position;
                }
            }
            else{
                triggered = true;
            }
            navMeshA.SetDestination(candyPosition);
            navMeshA.CalculatePath(candyPosition, navMeshPath);
            //Debug.DrawLine (PieceOfCandy.transform.position, transform.position, Color.cyan);
            if (Vector3.Distance(candyPosition, transform.position) <= 3.5 || !(navMeshPath.status == NavMeshPathStatus.PathComplete || navMeshA.hasPath && navMeshPath.status == NavMeshPathStatus.PathPartial)){
                navMeshA.speed = 0.0f;
                anim.SetInteger ("walk", 0);
                anim.SetInteger ("run", 0);
                anim.SetInteger ("attack2", 0);
                anim.SetInteger ("howl", 0);
                anim.SetInteger ("eat", 1);
                timer+=Time.deltaTime;
                if(timer >= 300){
                    timer=0;
                    anim.SetInteger ("eat", 0);
                    anim.SetInteger ("walk", 0);
                    anim.SetInteger ("attack2", 0);
                    anim.SetInteger ("howl", 0);
                    anim.SetInteger ("run", 0);
                    scaredAway=false;
                }
            }
            else{
                anim.SetInteger ("run", 1);
                navMeshA.speed = 15.0f;

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
  