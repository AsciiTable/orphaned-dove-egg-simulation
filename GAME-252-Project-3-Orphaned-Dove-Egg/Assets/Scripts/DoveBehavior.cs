using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveBehavior : MonoBehaviour
{
    private enum NestState { 
        Mother,
        Father,
        Both,
        None,
        Null
    }

    private Vector2 nestingLocation;        // The location of the nest
    private float timeBothAway = -1.0f;             // The amount of time both parents have been away from the nest for
    private NestState ns = NestState.Null;  // Current nest state
    private int stressLevel = 0;

    [SerializeField] private List<GameObject> stateSprites;

    [SerializeField] private bool motherAtDay = true;
    [SerializeField] private int numOfHuntsPerTrade = 3;
    //[SerializeField] private float probabilityBothGone = 0.2f;
    [SerializeField] private float probabilityReturn = 0.65f;

    private static bool isStartled = false;

    // Start is called before the first frame update
    void Start(){
        nestingLocation = DataManager.GetTreePosition();
        gameObject.transform.position = new Vector3(nestingLocation.x, nestingLocation.y, gameObject.transform.position.z);
        if(motherAtDay)
            SwitchNestStates(NestState.Mother);
        else
            SwitchNestStates(NestState.Father);
        timeBothAway = Time.time;
    }

    private void OnEnable()
    {
        SceneHandler.OnSunrise += DoveSunrise;
        SceneHandler.OnSunset += DoveSunset;
    }

    private void OnDisable()
    {
        SceneHandler.OnSunrise -= DoveSunrise;
        SceneHandler.OnSunset -= DoveSunset;
    }

    // Update is called once per frame
    void Update(){
        if (isStartled) {
            stressLevel++;
            isStartled = false;
            DataManager.ChangeProbability(0.005f);
        }
    }

    private void SwitchNestStates(NestState newNestState) {
        if (ns == newNestState)
            return;

        for (int i = 0; i < stateSprites.Count; i++) {
            if (stateSprites[i].activeSelf)
                stateSprites[i].SetActive(false);
        }

        switch (newNestState) {
            case NestState.Mother:
                stateSprites[1].SetActive(true);
                timeBothAway = -1.0f;
                SceneHandler.OnTick += RollForReturn;
                break;
            case NestState.Father:
                stateSprites[0].SetActive(true);
                SceneHandler.OnTick += RollForReturn;
                timeBothAway = -1.0f;
                break;
            case NestState.Both:
                stateSprites[2].SetActive(true);
                SceneHandler.OnTick -= RollForReturn;
                timeBothAway = -1.0f;
                break;
            case NestState.None:
                SceneHandler.OnTick -= RollForReturn;
                timeBothAway = Time.time;
                break;
            default:
                break;
        }
        ns = newNestState;
    }

    public float GetTotalTimeAway() {
        if (timeBothAway != 1.0f)
            return Time.time - timeBothAway;
        return timeBothAway;
    }

    public static void SetIsStartled(bool startled) {
        isStartled = startled;
    }

    public void DoveSunrise() {
        if (motherAtDay)
        {
            SwitchNestStates(NestState.Mother);
        }
        else {
            SwitchNestStates(NestState.Father);
        }
    }

    public void DoveSunset() {
        if (!motherAtDay)
        {
            SwitchNestStates(NestState.Mother);
        }
        else
        {
            SwitchNestStates(NestState.Father);
        }
    }

    public void RollForReturn() {
        float roll = Random.value;
        Debug.Log("Rolling for return: " + roll);
        if (roll <= probabilityReturn)
            SwitchNestStates(NestState.Both);
    }
}
