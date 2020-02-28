using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaryController : MonoBehaviour
{
    public float boundaryPadding;

    BoxCollider2D collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();

        Vector2 screenSizeInUnits = GameManager.Instance.mainCamera.ScreenToWorldPoint(Vector2.one);
        collider.size = new Vector2(Mathf.Abs(screenSizeInUnits.x * 2), Mathf.Abs(screenSizeInUnits.y * 2)) + Vector2.one * boundaryPadding;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"{collision.gameObject.name} has left the screen!");
    }
}
