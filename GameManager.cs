using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public double gameTime;
    public double gravitationalConstant = 1;//6.674e-11; 

    public List<GameObject> gravityObjects;

    int gravityObjectIndex = 0;
    int gravityObjectsCheckedPerFrame = 8;

    // Start is called before the first frame update
    void Start()
    {
        gameTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        // clean up gravityObject list
        for (int i = 0; i < gravityObjectsCheckedPerFrame; i++)
        {
            if (gravityObjectIndex >= gravityObjects.Count)
                gravityObjectIndex = 0;

            if (gravityObjects[gravityObjectIndex] == null)
                gravityObjects.RemoveAt(gravityObjectIndex);

            gravityObjectIndex += 1;
        }

    }

    // fixed update
    private void FixedUpdate()
    {
        
    }
}
