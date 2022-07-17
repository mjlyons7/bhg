using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravityMass = 10e30f;
    double gravFactor;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // get reference to gameManager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // get gravity force
        gravFactor = gameManager.gravitationalConstant * gravityMass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Physics engine update
    private void FixedUpdate()
    {
        int gravityListSize;
        float gravForce;
        Vector2 distance;
        GameObject gravGameObject;

        // apply gravity acceleration to objects effected by gravity
        gravityListSize = gameManager.gravityObjects.Count;
        for (int i = 0; i < gravityListSize; i++)
        {
            gravGameObject = gameManager.gravityObjects[i];

            // skip null
            if (gravGameObject == null)
            {
                continue;
            }

            // calculate force vector direction
            distance = gameObject.transform.position - gravGameObject.transform.position;

            // apply force
            gravForce = (float)(gravFactor * gravGameObject.GetComponent<Rigidbody2D>().mass / Mathf.Pow(distance.magnitude, 2));
            gravGameObject.GetComponent<Rigidbody2D>().AddForce(gravForce * distance.normalized);
                
        }
    }
}
