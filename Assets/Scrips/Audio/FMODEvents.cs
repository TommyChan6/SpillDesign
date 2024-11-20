using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps {get; private set;}
    [field: SerializeField] public EventReference playerFire {get; private set;}
    [field: Header("WeaponCollected SFX")]
    [field: SerializeField] public EventReference weaponCollect {get; private set;}
    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference enemyHit {get; private set;}
    [field: Header("BGM")]
    [field: SerializeField] public EventReference BGM {get; private set;}
    [field: SerializeField] public EventReference BGM2 {get; private set;}
    [field: SerializeField] public EventReference BGM3 {get; private set;}
    [field: SerializeField] public EventReference BGM4 {get; private set;}
    [field: SerializeField] public EventReference Outdoor {get; private set;}
    [field: SerializeField] public EventReference Ship {get; private set;}

    public static FMODEvents instance {get; private set;}

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
