using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveBehavior : MonoBehaviour
{
    private enum NestState { 
        Mom,
        Dad,
        Both,
        None
    }

    private Vector2 nestingLocation;    // The location of the nest
    private float timeBothAway;         // The amount of time both parents have been away from the nest for
    private NestState ns;               // Current nest state




    // Start is called before the first frame update
    void Start(){
        nestingLocation = DataManager.GetTreePosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchStates(NestState newState) { 
        
    }
}
