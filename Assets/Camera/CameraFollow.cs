using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Transform rightBorder;

    public Transform leftBorder;

    public SpriteRenderer backgroundSprite;

    float minX, maxX;

    // Start is called before the first frame update
    void Start()
    {
        minX = leftBorder.position.x;
        maxX = rightBorder.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        // align x-axis of camera with the player
        Vector3 cameraPos = transform.position;
        cameraPos.x = player.position.x;

        // stop camera at the background border
        Camera c = GetComponent<Camera>();

        float cameraHalfWidth = c.orthographicSize * ((float)Screen.width / Screen.height);
        if ((cameraPos.x - cameraHalfWidth) > minX 
            && (cameraPos.x + cameraHalfWidth) < maxX)
        {
            Debug.Log("Old camera pos: " + transform.position);
            transform.position = cameraPos;
            Debug.Log("New camera pos: " + transform.position);
        }
    }
}
