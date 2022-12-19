using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubMove : MonoBehaviour
{
    public float wanderRadius;
    public float moveInterval;
    private Transform target;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 StartPos;
    private Vector3 newPos;
    private float timer;
 
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent> ();
        anim = GetComponent<Animator>();
        agent.speed = 1.0f;
        timer = moveInterval;
        anim.SetInteger ("walk", 1);
        StartPos = transform.position;
    }
 
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if (timer >= moveInterval) {
            agent.speed = 1.0f;
            anim.SetInteger ("walk", 1);
            NavMeshHit navHit;
            Vector3 RandPoint = (Random.insideUnitSphere*Mathf.Sin(Random.Range(20.0f,30f) * Mathf.Deg2Rad)) * 30.0f + transform.position;
            NavMesh.SamplePosition(RandPoint, out navHit, 30, NavMesh.AllAreas);
            newPos = navHit.position;
            if (Vector3.Distance(StartPos, newPos) >= wanderRadius){
                newPos = StartPos;
            }
            agent.SetDestination(newPos);
            timer = 0;
        }
        else{
            if (Vector3.Distance(transform.position, newPos) <= 1.0f){
                agent.speed = 0.0f;
                anim.SetInteger ("walk", 0);
            }

        }
        //Debug.DrawLine (new Vector3(transform.position.x, transform.position.y+1, transform.position.z), new Vector3(newPos.x, transform.position.y+1, newPos.z), Color.red);
    }
}
  