using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 4f;
    public float timeToDisappear;
    public Vector2 direction;
    Rigidbody2D rb;
    //Quaternion initialQuaternion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindWithTag("Player");
        direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        direction.Normalize();
        // Find direction the bullet should fly toward
        // Vector2 mousePos = Input.mousePosition;
        // Camera cam = Camera.main;
        // Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        // //Vector3 screenPos = gameObject.transform.parent.GetComponentInParent<WeaponPosition>().screenPos;
        // //Vector3 screenPos = new Vector3(0.1f, 0.1f, 0f);
        // direction = new Vector2(mousePos.x - screenPos.x, mousePos.y - screenPos.y);
        // direction.Normalize();

        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            // Deal damage
            print("hit enemy");
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
