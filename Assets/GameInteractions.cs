using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteractions : MonoBehaviour
{
    public int objectiveToFind = -1;

    public GameObject notes;

    public GameObject objective_intro;
    public GameObject objective0;
    public GameObject objective1;
    public GameObject objective2;
    public GameObject objective3;

    public GameObject objective4_1;
    public GameObject objective4_2;
    public GameObject objective4_3;
    public GameObject objective4_4;

    public GameObject objective5;

    private bool objective4_1Found = false;
    private bool objective4_2Found = false;
    private bool objective4_3Found = false;
    private bool objective4_4Found = false;

    public AudioSource startVL;
    public AudioSource finishObj3VL;
    public AudioSource finishAirportVL;
    public AudioSource winVL;

    public GameObject pressEtext;

    private Dictionary<int, string> notesText;

    void Start()
    {
        // Directions that get displayed in the notepad depending on the objective the player needs to find
        notesText = new Dictionary<int, string>();
        notesText.Add(-1, "Notes");
        notesText.Add(0, "1.) Start at the\nlamppost\n\n2.) 5° 29 paces\n\n3.) 15° 20 paces\n\n4.) 320° 49 paces\n\n5.) 350° 13 paces");
        notesText.Add(1, "1.) 260° 25 paces\n\n2.) 315° 70 paces\n\n3.) 15° 25 paces\n\n4.) 90° 40 paces\n\n5.) 120° 80 paces");
        notesText.Add(2, "1.) 345° 79 paces\n\n2.) 300° 25 paces");
        notesText.Add(3, "1.) 300° 65 paces");
        notesText.Add(4, "1.) 60° 62 paces\n2.) Find the radio\nparts in the broken\ndown plane and in\nthe 3 trucks in the\nhangars");
        notesText.Add(5, "1.) Start at the\ngate entrance\n2.) 360° 60 paces\n3.) 315° 80 paces\n4.) 270° 50 paces\n5.) 360° 120 pace\n6.) 90° 120 paces\n7.) 135° 90 paces\n8.) 45° 70 paces\n9.) 360° 20 paces");
        notesText.Add(6, "You win!");

        pressEtext.SetActive(false);

        notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
    }

    // Update is called once per frame
    void Update()
    {
        
        // If player is near an objective, he can press e to listen to the next radio commands, and then update the objective.
        pressEtext.SetActive(true);
        if (Vector3.Distance(transform.position, objective_intro.transform.position) < 2 && objectiveToFind == -1){
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 0;
                notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
                startVL.Play();
            }
        }
        else if (Vector3.Distance(transform.position, objective0.transform.position) < 2 && objectiveToFind == 0){
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 1;
                notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
            }
        }
        else if (Vector3.Distance(transform.position, objective1.transform.position) < 2 && objectiveToFind == 1){
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 2;
                notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
            }
        }
        else if (Vector3.Distance(transform.position, objective2.transform.position) < 2 && objectiveToFind == 2){
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 3;
                notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
            }
        }
        else if (Vector3.Distance(transform.position, objective3.transform.position) < 2 && objectiveToFind == 3){
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 4;
                notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
                finishObj3VL.Play();
            }
        }
        else if (Vector3.Distance(transform.position, objective4_1.transform.position) < 15 && objectiveToFind == 4 && objective4_1Found == false){
            if (Input.GetKeyDown(KeyCode.E)){
                objective4_1Found = true;
                notes.GetComponent<TMPro.TextMeshPro>().SetText("Crashed Plane " + (objective4_1Found ? "✓" : "") + "\n\nTruck 1 " + (objective4_2Found ? "✓" : "") + "\n\nTruck 2 " + (objective4_3Found ? "✓" : "") + "\n\nTruck 3 " + (objective4_4Found ? "✓" : ""));
            }
        }
        else if (Vector3.Distance(transform.position, objective4_2.transform.position) < 10 && objectiveToFind == 4 && objective4_2Found == false){
            if (Input.GetKeyDown(KeyCode.E)){
                objective4_2Found = true;
                notes.GetComponent<TMPro.TextMeshPro>().SetText("Crashed Plane " + (objective4_1Found ? "✓" : "") + "\n\nTruck 1 " + (objective4_2Found ? "✓" : "") + "\n\nTruck 2 " + (objective4_3Found ? "✓" : "") + "\n\nTruck 3 " + (objective4_4Found ? "✓" : ""));
            }
        }
        else if (Vector3.Distance(transform.position, objective4_3.transform.position) < 10 && objectiveToFind == 4 && objective4_3Found == false){
            if (Input.GetKeyDown(KeyCode.E)){
                objective4_3Found = true;
                notes.GetComponent<TMPro.TextMeshPro>().SetText("Crashed Plane " + (objective4_1Found ? "✓" : "") + "\n\nTruck 1 " + (objective4_2Found ? "✓" : "") + "\n\nTruck 2 " + (objective4_3Found ? "✓" : "") + "\n\nTruck 3 " + (objective4_4Found ? "✓" : ""));
            }
        }
        else if (Vector3.Distance(transform.position, objective4_4.transform.position) < 10 && objectiveToFind == 4 && objective4_4Found == false){
            if (Input.GetKeyDown(KeyCode.E)){
                objective4_4Found = true;
                notes.GetComponent<TMPro.TextMeshPro>().SetText("Crashed Plane " + (objective4_1Found ? "✓" : "") + "\n\nTruck 1 " + (objective4_2Found ? "✓" : "") + "\n\nTruck 2 " + (objective4_3Found ? "✓" : "") + "\n\nTruck 3 " + (objective4_4Found ? "✓" : ""));
            }
        }
        else if (Vector3.Distance(transform.position, objective5.transform.position) < 2 && objectiveToFind == 5){
            if (Input.GetKeyDown(KeyCode.E)){
                objectiveToFind = 6;
                notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
                winVL.Play();
            }
        }
        else{
            pressEtext.SetActive(false);
        }

        if(objective4_1Found == true && objective4_2Found == true && objective4_3Found == true && objective4_4Found == true && objectiveToFind < 5){
            finishAirportVL.Play();
            objectiveToFind = 5;
            notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
        }
    }
}
