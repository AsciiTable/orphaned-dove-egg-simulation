using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPathfinding : MonoBehaviour
{
    [SerializeField] private List<int> goOutHour;
    [SerializeField] private List<Vector3> checkpoints;
    [SerializeField] private float walkingSpeed = 5.0f;

    private CapsuleCollider2D col;
    private bool walking = false;
    private int currentCheckpoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        if (col == null) {
            Debug.Log("Your human needs a collider!");
            this.gameObject.SetActive(false);
            return;
        }
        col.enabled = false;
    }

    private void OnEnable()
    {
        SceneHandler.OnTick += CheckForTime;
    }

    private void OnDisable()
    {
        SceneHandler.OnTick -= CheckForTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (walking) {
            gameObject.transform.position += new Vector3((checkpoints[currentCheckpoint].x - gameObject.transform.position.x), 
                (checkpoints[currentCheckpoint].y - gameObject.transform.position.y), 0.0f) * Time.deltaTime * walkingSpeed;
        }   
    }

    private void CheckForTime() {
        if (!walking) {
            foreach (int h in goOutHour)
            {
                if (SceneHandler.GetCurrentHour() == h)
                {
                    SpawnPerson();
                }
            }
        }
    }

    private void SpawnPerson() {
        walking = true;
        col.enabled = true;
        gameObject.transform.position = checkpoints[currentCheckpoint];
        currentCheckpoint++;
        //SceneHandler.OnTick += WalkOnTick;
    }

    private void DespawnPerson() {
        currentCheckpoint = 0;
        walking = false;
        col.enabled = false;
        gameObject.transform.position = checkpoints[currentCheckpoint];
        //SceneHandler.OnTick -= WalkOnTick;
    }

    private void WalkOnTick()
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(checkpoints[currentCheckpoint]);
        currentCheckpoint++;
        if (currentCheckpoint >= checkpoints.Count)
        {
            DespawnPerson();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            //Debug.Log(currentCheckpoint);
            currentCheckpoint++;
        }
        else if (collision.gameObject.CompareTag("End") && currentCheckpoint > 1) {
            currentCheckpoint++;
            if (currentCheckpoint >= checkpoints.Count)
            {
                DespawnPerson();
            }
        }
    }
}
