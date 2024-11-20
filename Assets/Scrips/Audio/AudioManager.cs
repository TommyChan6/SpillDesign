using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;



public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    public static AudioManager instance {get; private set;}
    private EventInstance backgroundMusic;
    public bool hasBGM1 = false;
    public bool hasBGM2 = false;
    public bool hasBGM3 = false;
    public bool hasBGM4 = false;
    public bool hasOutdoor = false;
    public bool hasShip = false;

    public void Start() {
        instance = this;
        if (instance != null)
        {
            Debug.Log("Instance is ready to use!");
            if (hasBGM1) {
                instance.PlayBGM(FMODEvents.instance.BGM);
            }
            if (hasBGM2) {
                instance.PlayBGM(FMODEvents.instance.BGM2);
            }
            if (hasBGM3) {
                instance.PlayBGM(FMODEvents.instance.BGM3);
            }
            if (hasBGM4) {
                instance.PlayBGM(FMODEvents.instance.BGM4);
            }
            if (hasOutdoor) {
                instance.PlayBGM(FMODEvents.instance.Outdoor);
            }
            if (hasShip) {
                instance.PlayBGM(FMODEvents.instance.Ship);
            }
        }
    }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        // if (hasBGM1) {
        //     instance.PlayBGM(FMODEvents.instance.BGM);
        // }
        // if (hasBGM2) {
        //     instance.PlayBGM(FMODEvents.instance.BGM2);
        // }
        // if (hasBGM3) {
        //     instance.PlayBGM(FMODEvents.instance.BGM3);
        // }
        // if (hasBGM4) {
        //     instance.PlayBGM(FMODEvents.instance.BGM4);
        // }
        // if (hasOutdoor) {
        //     instance.PlayBGM(FMODEvents.instance.Outdoor);
        // }
        // if (hasShip) {
        //     instance.PlayBGM(FMODEvents.instance.Ship);
        // }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos) {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayBGM(EventReference sound) {
        backgroundMusic = instance.CreateInstance(sound);
        backgroundMusic.start();
        backgroundMusic.release();
    }

    public EventInstance CreateInstance(EventReference eventReference) {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp() {
        foreach (EventInstance eventInstance in eventInstances) {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy() {
        CleanUp();
    }
}
