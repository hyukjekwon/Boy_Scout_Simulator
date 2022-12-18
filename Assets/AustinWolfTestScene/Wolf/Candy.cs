using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public GameObject Player;
    public GameObject candyclone;
    private Rigidbody body;
    public bool candyLanded;
    bool canthrownew;
    public int candyCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-600,4,800); //Place out of view
        canthrownew = true;
        candyLanded = false;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyUp("q") && candyCount > 0 && canthrownew){
            //var mousePos = Input.mousePosition;
            candyLanded = false;
            canthrownew = false;
            candyCount -=1;
            Vector3 forwards = Player.transform.forward;
            transform.rotation = Player.transform.rotation;
            transform.position = Player.transform.position+Player.transform.forward;
            body.velocity = forwards * 15.0f;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!(other.gameObject.CompareTag("Wolf") || other.gameObject.CompareTag("Player"))) //Wolf and Player must have these tags
        {
            candyLanded = true;
            Debug.Log(other.gameObject.name);
            body.velocity = new Vector3(0,0,0);
            canthrownew = true;
            GameObject clone = Instantiate(candyclone, transform.position, transform.rotation);
            transform.position = new Vector3(-600,4,800); //Place out of view
            body.velocity = new Vector3(0,0,0);
        }
    }
}
