using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteractions : MonoBehaviour
{
    public int objectiveToFind = 0;

    public GameObject notes;

    public GameObject objective0;
    public GameObject objective1;
    public GameObject objective2;
    public GameObject objective3;

    public GameObject pressEtext;

    private Dictionary<int, string> notesText;

    void Start()
    {
        // Directions that get displayed in the notepad depending on the objective the player needs to find
        notesText = new Dictionary<int, string>();
        notesText.Add(0, "1.) Start at the\nlamppost\n\n2.) 5° 29 paces\n\n3.) 15° 20 paces\n\n4.) 320° 49 paces\n\n5.) 350° 13 paces");
        notesText.Add(1, "1.) 260° 25 paces\n\n2.) 315° 70 paces\n\n3.) 15° 25 paces\n\n4.) 90° 40 paces\n\n5.) 120° 80 paces");
        notesText.Add(2, "1.) 345° 79 paces\n\n2.) 300° 25 paces");
        notesText.Add(3, "1.) 300° 65 paces");

        pressEtext.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If player is near an objective, he can press e to listen to the next radio commands, and then update the objective.
        if (Vector3.Distance(transform.position, objective0.transform.position) < 2 && objectiveToFind == 0){
            pressEtext.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 1;
            }
        }
        else if (Vector3.Distance(transform.position, objective1.transform.position) < 2 && objectiveToFind == 1){
            pressEtext.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 2;
            }
        }
        else if (Vector3.Distance(transform.position, objective2.transform.position) < 2 && objectiveToFind == 2){
            pressEtext.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 3;
            }
        }
        else if (Vector3.Distance(transform.position, objective3.transform.position) < 2 && objectiveToFind == 3){
            pressEtext.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 4;
            }
        }
        else{
            pressEtext.SetActive(false);
        }

        notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
    }
}
