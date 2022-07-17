using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulledByBlackholes : MonoBehaviour
{
    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // get reference to game manager
        gameManager = GameObject.Find("GameManager");

        // add this object to the gravity object list, so can be pulled by blackholes
        gameManager.GetComponent<GameManager>().gravityObjects.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
