using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotateSpeed;
    public float dualTouchRotateSpeed;

    public float inertia;

    private float rotInput;
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (touchPos.x < GameManager.Instance.screenWidth / 2) rotInput = rotateSpeed;
            else rotInput = -rotateSpeed;
        }
        else if (touchCount == 2)
        {
            Vector2[] touches = new Vector2[2];
            for (int i = 0; i < 2; i++)
                touches[i] = Input.GetTouch(i).position;

            if (touches[0].x > touches[1].x)
            {
                Vector2 tmp = touches[0];
                touches[0] = touches[1];
                touches[1] = tmp;
            }

            if (touches[0].x < GameManager.Instance.screenWidth / 2 && touches[1].x >= GameManager.Instance.screenWidth / 2)
            {
                float deltaY = (touches[0].y - touches[1].y) / GameManager.Instance.screenHeight;

                rotInput = -(deltaY * deltaY * dualTouchRotateSpeed) * Mathf.Sign(deltaY);
            }
            else
            {
                if (touches[0].x < GameManager.Instance.screenWidth / 2) rotInput = rotateSpeed;
                else rotInput = -rotateSpeed;
            }

        }
        else
        {
            rotInput = 0;
        }

        Move();
    }

    void Move()
    {
        if (rotInput == 0)
            rotation = Mathf.Lerp(rotation, 0, inertia * Time.deltaTime);
        else 
            rotation = Mathf.Lerp(rotation, rotInput, 5 * inertia * Time.deltaTime);

        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
    }
}
