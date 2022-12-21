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
    private AudioSource Whoosh;
    private AudioSource stepAudio;
    private AudioSource runningAudio;
    private GameObject lastActive;
    private Vector3 PausePosition;
    private Vector3 SubPosition;
    private TextMeshProUGUI meatCounter;
    private List<AudioSource> sources;

    private GameObject fpsController;
    private FirstPersonMovement fpsScript;
    private Jump jumpScript;
    private Crouch crouchScript;
    private GameInteractions interactionsScript;
    private FirstPersonLook fpsLookScript;
    private Zoom zoomScript;

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

    void setSfxVolume(float value) {
        foreach(AudioSource source in sources) {
            source.volume = value;
        }
    }

    void Pause() {
        fpsScript.enabled = false;
        jumpScript.enabled = false;
        crouchScript.enabled = false;
        interactionsScript.enabled = false;
        fpsLookScript.enabled = false;
        zoomScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        stepAudio.volume = 0;
        runningAudio.volume = 0;
    }

    void Unpause() {
        fpsScript.enabled = true;
        jumpScript.enabled = true;
        crouchScript.enabled = true;
        interactionsScript.enabled = true;
        fpsLookScript.enabled = true;
        zoomScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        stepAudio.volume = SfxSlider.value;
        runningAudio.volume = SfxSlider.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        fpsController = GameObject.Find("First Person Controller");
        fpsScript = fpsController.GetComponent<FirstPersonMovement>();
        jumpScript = fpsController.GetComponent<Jump>();
        crouchScript = fpsController.GetComponent<Crouch>();
        interactionsScript = fpsController.GetComponent<GameInteractions>();

        Click = GameObject.Find("click").GetComponent<AudioSource>();
        Scroll = GameObject.Find("scroll").GetComponent<AudioSource>();
        GameObject.Find("Step Audio").GetComponent<AudioSource>().volume = 0;
        sources = new List<AudioSource>();

        foreach (AudioSource source in GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[]) {
            sources.Add(source);
        }
        foreach (GameObject wolfObj in GameObject.FindGameObjectsWithTag("Wolf")) {
            sources.Add(wolfObj.GetComponent<AudioSource>());
        }
        foreach (GameObject sfxObj in GameObject.FindGameObjectsWithTag("SFX")) {
            sources.Add(sfxObj.GetComponent<AudioSource>());
        }

        GameObject FirstPersonCamera = GameObject.Find("First Person Camera");
        fpsLookScript = FirstPersonCamera.GetComponent<FirstPersonLook>();
        zoomScript = FirstPersonCamera.GetComponent<Zoom>();

        stepAudio = GameObject.Find("Step Audio").GetComponent<AudioSource>();
        runningAudio = GameObject.Find("Running Audio").GetComponent<AudioSource>();
        Pause();

        meatCounter = GameObject.Find("MeatCounter").GetComponent<TextMeshProUGUI>();
        meatCount = 10;
        meatCounter.text = meatCount.ToString();
        Whoosh = GameObject.Find("MeatCounter").GetComponent<AudioSource>();

        HomeScreen = GameObject.Find("HomeScreen");
        HomeScreen.transform.localScale = Vector3.one;
        gameStarted = false;
        gamePaused = false;

        Button startBtn = GameObject.Find("Start").GetComponent<Button>();
        startBtn.onClick.AddListener(() => {
            Click.Play();
            HomeScreen.transform.localScale = Vector3.zero;
            PauseEmpty.transform.localScale = Vector3.zero;
            VolumeEmpty.transform.localScale = Vector3.zero;
            ControlsEmpty.transform.localScale = Vector3.zero;
            gameStarted = true;
            Unpause();
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
            if (!Scroll.isPlaying) {
                Scroll.Play();
            }
            setSfxVolume(value);
        });
        MusicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        MusicSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying) {
                Scroll.Play();
            }
        });
        MasterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
        MasterSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying) {
                Scroll.Play();
            }
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
                PauseEmpty.transform.position += new Vector3(currSpeed, 0, 0) * Time.unscaledDeltaTime;
                currSpeed = Mathf.Max(currSpeed - deccel, 50.0f);
            } else if (Input.GetKeyDown(KeyCode.Escape)) {                      // Toggle pause panel
                if (PauseEmpty.transform.localScale.Equals(Vector3.zero)) {     // Pause
                    PauseEmpty.transform.position = PausePosition - new Vector3(450, 0, 0);
                    currSpeed = initSpeed;
                    gamePaused = true;
                    Pause();
                } else {                                                        // Unpause
                    VolumeEmpty.transform.localScale = Vector3.zero;
                    ControlsEmpty.transform.localScale = Vector3.zero;
                    gamePaused = false;
                    Unpause();
                }
                ToggleUIElement(PauseEmpty);
            }
        }
    }
}
