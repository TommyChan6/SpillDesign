using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMonster : MonoBehaviour
{
    GameObject player;
    public Enemy enemyComponent;  // For stopping movement and change animation
    private float cooldownTimer;
    public float cooldownLengthForRangedAttack = 2f;
    private bool isInCooldown = false;
    private bool startChargingAttack = false;
    public GameObject energyBall;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(player.transform.position.x - transform.position.x), 2f) +  Mathf.Pow(Mathf.Abs(player.transform.position.y - transform.position.y), 2f));
        if (distanceToPlayer < 1f) {
            if (!isInCooldown) {
                startChargingAttack = true;
            }
        }
        if (startChargingAttack) {
            isInCooldown = true;
            cooldownTimer = 0f;
            startChargingAttack = false;
        }
        if(isInCooldown) {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldownLengthForRangedAttack) {
                cooldownTimer = 0f;
                isInCooldown = false;
                Attack();
            }
        }
    }

    public void Attack() {
        //print("rattack!!!!!");
        Instantiate(energyBall, transform.position, Quaternion.identity);
    }
}
