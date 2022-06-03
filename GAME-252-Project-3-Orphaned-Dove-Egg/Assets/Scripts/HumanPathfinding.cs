using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPathfinding : MonoBehaviour
{
    [SerializeField] private List<int> goOutHour;
    [SerializeField] private List<Vector2> checkpoints;

    private CapsuleCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        if (col == null) {
            Debug.Log("Your human needs a collider!");
            this.gameObject.SetActive(false);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
