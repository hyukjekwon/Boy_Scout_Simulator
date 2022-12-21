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
    //Checkpoint buttons
    private Button TryAgainBtnCheckpoint0;
    private Button TryAgainBtnCheckpoint1;
    private Button TryAgainBtnCheckpoint2;
    private Button TryAgainBtnCheckpoint3;
    private Button TryAgainBtnCheckpoint4;
    ///////////////////////////
    private GameObject spawnblock;
    private GameObject gameover;
    private Slider SfxSlider;
    private Slider DialogueSlider;
    private Slider MasterSlider;
    private AudioSource Click;
    private AudioSource Scroll;
    private AudioSource Whoosh;
    private AudioSource Fail;
    private AudioSource stepAudio;
    private AudioSource runningAudio;
    private GameObject lastActive;
    private Vector3 PausePosition;
    private Vector3 SubPosition;
    private TextMeshProUGUI meatCounter;
    private List<AudioSource> sources;
    private List<AudioSource> dialogueSources;

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
    private int GameProgression;
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

    void setDialogueVolume(float value) {
        foreach(AudioSource source in dialogueSources) {
            source.volume = value;

        }
    }

    public void PausePlayer() {
        fpsScript.enabled = false;
        jumpScript.enabled = false;
        crouchScript.enabled = false;
        interactionsScript.enabled = false;
        fpsLookScript.enabled = false;
        zoomScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Pause() {
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
        fpsController.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void Unpause() {
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
        GameProgression = CheckpointManager.gameprogress;
        spawnblock = GameObject.Find("SpawnBlock");
        gameover = GameObject.Find("GameOver"); 
        fpsController = GameObject.Find("First Person Controller");
        fpsScript = fpsController.GetComponent<FirstPersonMovement>();
        jumpScript = fpsController.GetComponent<Jump>();
        crouchScript = fpsController.GetComponent<Crouch>();
        interactionsScript = fpsController.GetComponent<GameInteractions>();
        fpsController.transform.position = spawnblock.transform.position;
        Click = GameObject.Find("click").GetComponent<AudioSource>();
        Scroll = GameObject.Find("scroll").GetComponent<AudioSource>();
        Fail = GameObject.Find("GameOver").GetComponent<AudioSource>();
        GameObject.Find("Step Audio").GetComponent<AudioSource>().volume = 0;
        sources = new List<AudioSource>();

        dialogueSources = new List<AudioSource>();
        foreach (AudioSource source in GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[]) {
            sources.Add(source);
        }
        foreach (GameObject wolfObj in GameObject.FindGameObjectsWithTag("Wolf")) {
            sources.Add(wolfObj.GetComponent<AudioSource>());
        }
        foreach (GameObject sfxObj in GameObject.FindGameObjectsWithTag("SFX")) {
            sources.Add(sfxObj.GetComponent<AudioSource>());
        }
        foreach (GameObject diaObj in GameObject.FindGameObjectsWithTag("Dialogue")) {
            dialogueSources.Add(diaObj.GetComponent<AudioSource>());
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
            GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        });

        // THESE ARE THE BUTTONS ON THE GAME OVER SCREEN
        //Spawnpos: spawnblock.transform.position = new Vector3(437.79f, 11.72f, 357.79f);
        //Checkpoint1: spawnblock.transform.position = new Vector3(561f, 41.4f, 934.736f);
        TryAgainBtnCheckpoint0 = GameObject.Find("TryAgainBtn0").GetComponent<Button>();
        TryAgainBtnCheckpoint0.onClick.AddListener(() => {
            Click.Play();
            spawnblock.transform.position = new Vector3(437.79f, 11.72f, 357.79f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        TryAgainBtnCheckpoint1 = GameObject.Find("TryAgainBtn1").GetComponent<Button>();
        TryAgainBtnCheckpoint1.onClick.AddListener(() => {
            Click.Play();
            spawnblock.transform.position = new Vector3(561f, 41.4f, 934.736f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        
        TryAgainBtnCheckpoint2 = GameObject.Find("TryAgainBtn2").GetComponent<Button>();
        TryAgainBtnCheckpoint2.onClick.AddListener(() => {
            Click.Play();
            spawnblock.transform.position = new Vector3(188.5f, 21.72f, 1089.64f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        
        TryAgainBtnCheckpoint3 = GameObject.Find("TryAgainBtn3").GetComponent<Button>();
        TryAgainBtnCheckpoint3.onClick.AddListener(() => {
            Click.Play();
            spawnblock.transform.position = new Vector3(415.6f, 7.4f, 1608.3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        TryAgainBtnCheckpoint4 = GameObject.Find("TryAgainBtn4").GetComponent<Button>();
        TryAgainBtnCheckpoint4.onClick.AddListener(() => {
            Click.Play();
            spawnblock.transform.position = new Vector3(805.28f, 3.22f, 1804.7f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        ////////////////////////////////////////////////////////////////////////////////////////

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
            spawnblock.transform.position = new Vector3(437.79f, 11.72f, 357.79f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        SfxSlider = GameObject.Find("SfxSlider").GetComponent<Slider>();
        SfxSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying) {
                Scroll.Play();
            }
            setSfxVolume(value);
        });
        DialogueSlider = GameObject.Find("DialogueSlider").GetComponent<Slider>();
        DialogueSlider.onValueChanged.AddListener((float value) => {
            if (!Scroll.isPlaying) {
                Scroll.Play();
            }
            setDialogueVolume(value);
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
        //Checkpoint stuff /////////////////////////////////////////////////////////////////////////
        GameProgression = CheckpointManager.gameprogress;
        if (!(GameProgression >= 1)){
            TryAgainBtnCheckpoint1.gameObject.SetActive(false);
        }
        else{
            TryAgainBtnCheckpoint1.gameObject.SetActive(true);
        }
        if (!(GameProgression >= 2)){
            TryAgainBtnCheckpoint2.gameObject.SetActive(false);
        }
        else{
            TryAgainBtnCheckpoint2.gameObject.SetActive(true);
        }
        if (!(GameProgression >= 3)){
            TryAgainBtnCheckpoint3.gameObject.SetActive(false);
        }
        else{
            TryAgainBtnCheckpoint3.gameObject.SetActive(true);
        }
        if (!(GameProgression >= 4)){
            TryAgainBtnCheckpoint4.gameObject.SetActive(false);
        }
        else{
            TryAgainBtnCheckpoint4.gameObject.SetActive(true);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////
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
