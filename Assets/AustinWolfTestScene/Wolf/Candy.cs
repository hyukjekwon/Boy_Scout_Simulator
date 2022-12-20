using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Candy : MonoBehaviour
{
    public GameObject Player;
    public GameObject candyclone;
    private Rigidbody body;
    private TextMeshProUGUI meatCount;
    private AudioSource Whoosh;
    public bool candyLanded;
    bool canthrownew;
    public int candyCount = 10;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-600,4,800); //Place out of view
        canthrownew = true;
        candyLanded = false;
        body = GetComponent<Rigidbody>();
        timer = 0;
        GameObject MeatCounter = GameObject.Find("MeatCounter");
        meatCount = MeatCounter.GetComponent<TextMeshProUGUI>();
        Whoosh = MeatCounter.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyUp("q") && candyCount > 0 && canthrownew){
            //var mousePos = Input.mousePosition;
            timer = 0;
            candyLanded = false;
            canthrownew = false;
            candyCount -=1;
            meatCount.text = candyCount.ToString();
            Vector3 forwards = Player.transform.forward;
            transform.rotation = Player.transform.rotation;
            transform.position = Player.transform.position+Player.transform.forward;
            body.velocity = forwards * 15.0f;
            Whoosh.Play();
        }
        else{
            timer += Time.deltaTime;
        }
        if(timer >= 3){ //Can throw new piece at LEAST every 3 seconds 
            canthrownew = true;
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
