using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private GameObject Player;
    private GameInteractions gameIntScript;
    public static int gameprogress;
    
    void Awake(){
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("First Person Controller");
        Player.transform.position = transform.position; //Sets player position to location of spawnblock 
        gameIntScript = Player.GetComponent<GameInteractions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIntScript.objectiveToFind >= gameprogress){
            gameprogress = gameIntScript.objectiveToFind;
        }
    }
}
