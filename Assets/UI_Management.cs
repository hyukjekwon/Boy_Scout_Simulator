// Hyuk-Je Kwon, CS 576
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Management : MonoBehaviour
{
    private GameObject HomeScreen;
    private GameObject PauseEmpty;
    private GameObject VolumeEmpty;
    private GameObject ControlsEmpty;
    private Button KeyBindsBtn;
    private Button VolumeBtn;
    private Button RestartBtn;
    private Slider SfxSlider;
    private Slider MusicSlider;
    private Slider MasterSlider;
    private AudioSource Click;
    private AudioSource Scroll;
    private GameObject lastActive;
    private Vector3 PausePosition;
    private Vector3 SubPosition;
    private TextMeshProUGUI meatCounter;
    private float initSpeed;
    private float currSpeed;
    private float deccel;
    private bool gameStarted;
    private bool gamePaused;
    private int meatCount;

    void ToggleUIElement(GameObject elem) {
        if (elem.transform.localScale.Equals(Vector3.zero)) {
            if (lastActive) {
                lastActive.transform.localScale = !lastActive.tag.Equals("Menu") ? Vector3.zero : Vector3.one;
            }
            elem.transform.localScale = Vector3.one;
            lastActive = elem;
        } else {
            elem.transform.localScale = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Click = GameObject.Find("click").GetComponent<AudioSource>();
        Scroll = GameObject.Find("scroll").GetComponent<AudioSource>();

        meatCounter = GameObject.Find("MeatCounter").GetComponent<TextMeshProUGUI>();
        meatCount = 5;

        HomeScreen = GameObject.Find("HomeScreen");
        ToggleUIElement(HomeScreen);
        gameStarted = false;
        gamePaused = false;

        Button startBtn = GameObject.Find("Start").GetComponent<Button>();
        startBtn.onClick.AddListener(() => {
            Click.Play();
            HomeScreen.transform.localScale = Vector3.zero;
            PauseEmpty.transform.localScale = Vector3.zero;
            VolumeEmpty.transform.localScale = Vector3.zero;
            gameStarted = true;
        });

        PauseEmpty = GameObject.Find("PauseMenu");
        PauseEmpty.transform.localScale = Vector3.zero;
        PausePosition = PauseEmpty.transform.position;

        VolumeEmpty = GameObject.Find("VolumeEmpty");
        VolumeEmpty.transform.localScale = Vector3.zero;

        ControlsEmpty = GameObject.Find("ControlsEmpty");
        ControlsEmpty.transform.localScale = Vector3.zero;
        SubPosition = VolumeEmpty.transform.position;

        foreach (GameObject VolumeBtn in GameObject.FindGameObjectsWithTag("VolumeBtn")) {
            VolumeBtn.GetComponent<Button>().onClick.AddListener(() => {
                Click.Play();
                ToggleUIElement(VolumeEmpty);
            });
        }

        foreach (GameObject ControlsBtn in GameObject.FindGameObjectsWithTag("ControlsBtn")) {
            ControlsBtn.GetComponent<Button>().onClick.AddListener(() => {
                Click.Play();
                ToggleUIElement(ControlsEmpty);
            });
        }

        RestartBtn = GameObject.Find("Restart").GetComponent<Button>();
        RestartBtn.GetComponent<Button>().onClick.AddListener(() => {
            Click.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        SfxSlider = GameObject.Find("SfxSlider").GetComponent<Slider>();
        SfxSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying)
                Scroll.Play();
        });
        MusicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        MusicSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying)
                Scroll.Play();
        });
        MasterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        MasterSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying)
                Scroll.Play();
            AudioListener.volume = value;
        });
        initSpeed = 2400.0f;
        currSpeed = 0.0f;
        deccel = 12.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted) {
            if (PauseEmpty.transform.position.x < PausePosition.x) {            // Animate pause panel
                PauseEmpty.transform.position += new Vector3(currSpeed, 0, 0) * Time.deltaTime;
                currSpeed = Mathf.Max(currSpeed - deccel, 50.0f);
            } else if (Input.GetKeyDown(KeyCode.Escape)) {                      // Toggle pause panel
                if (PauseEmpty.transform.localScale.Equals(Vector3.zero)) {     // Pause
                    PauseEmpty.transform.position = PausePosition - new Vector3(450, 0, 0);
                    currSpeed = initSpeed;
                    gamePaused = true;
                } else {                                                        // Unpause
                    VolumeEmpty.transform.localScale = Vector3.zero;
                    ControlsEmpty.transform.localScale = Vector3.zero;
                    gamePaused = false;
                }
                ToggleUIElement(PauseEmpty);
            } else if (Input.GetKeyDown("q") && !gamePaused) {                  // If game unpaused and throw
                meatCount = Mathf.Max(0, meatCount - 1);
                meatCounter.text = meatCount.ToString();
            }
        }
    }
}
