using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    public Camera cam;
    public GameObject enemy;

    private float timer = 0f;  // Timer to keep track of time
    public float interval = 0.1f;
    private SpriteRenderer spriteRenderer;
    public bool isGrowingGeneration = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(RunEveryTenSeconds());

        // Calculate the bounds of the object
        // Bounds bounds = spriteRenderer.bounds;

        // // Get the left, right, top, and bottom coordinates
        // float left = bounds.min.x;
        // float right = bounds.max.x;
        // float top = bounds.max.y;
        // float bottom = bounds.min.y;
    }

    IEnumerator RunEveryTenSeconds()
    {
        while (isGrowingGeneration)
        {
            // Code to run every 10 seconds
            interval = interval * 0.96f;
            
            // Wait for 10 seconds
            yield return new WaitForSeconds(10f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            // Get the camera area
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            Vector3 camPosition = cam.transform.position;
            
            bool noEnemyGenerated = true; 
            while(noEnemyGenerated) {
                // Get random position in the enemy generation area
                float xPos = Random.Range(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.x);
                float yPos = Random.Range(spriteRenderer.bounds.min.y, spriteRenderer.bounds.max.y);

                // Check if it is within the camera area
                if (
                    ((camPosition.x - (camWidth / 2)) <= xPos) &
                    (xPos <= (camPosition.x + (camWidth / 2))) & 
                    ((camPosition.y - (camHeight / 2)) <= yPos) &
                    (yPos <= (camPosition.y + (camHeight / 2))) 
                    ) {
                    //print("not generated" + Time.deltaTime);
                    //noEnemyGenerated = true;
                } else {
                    GenerateEnemy(enemy, xPos, yPos);
                    //print("Enemy generated in (" + xPos + ", " + yPos + ")");
                    noEnemyGenerated = false;
                }
            }
            timer = 0f;
        }
    }

    public void GenerateEnemy(GameObject enemyMonster, float xPos, float yPos) {
        if (enemyMonster != null) {
            GameObject enemyInstance = Instantiate(enemyMonster, new Vector3(xPos, yPos, transform.position.z), Quaternion.identity);
            //print("enemy generated");
        }
    }
}
