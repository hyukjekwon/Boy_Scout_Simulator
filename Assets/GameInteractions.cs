using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteractions : MonoBehaviour
{
    public int objectiveToFind = 0;

    public GameObject notes;

    private Dictionary<int, string> notesText;

    void Start()
    {
        notesText = new Dictionary<int, string>();
        notesText.Add(0, "1.) Start at the\nlamppost\n\n2.) 5° 29 paces\n\n3.) 15° 20 paces\n\n4.) 320° 49 paces\n\n5.) 350° 13 paces");
        notesText.Add(1, "1.) 260° 25 paces\n\n2.) 315° 70 paces\n\n3.) 15° 25 paces\n\n4.) 90° 40 paces\n\n5.) 120° 80 paces");
        notesText.Add(2, "1.) 345° 79 paces\n\n2.) 300° 25 paces");
        notesText.Add(3, "1.) 300° 65 paces");
    }

    // Update is called once per frame
    void Update()
    {
        notes.GetComponent<TMPro.TextMeshPro>().SetText(notesText[objectiveToFind]);
    }
}
