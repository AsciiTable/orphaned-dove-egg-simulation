using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComfortZone : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color alertColor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = alertColor;
        DoveBehavior.SetIsStartled(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
        DoveBehavior.SetIsStartled(false);
    }
}
