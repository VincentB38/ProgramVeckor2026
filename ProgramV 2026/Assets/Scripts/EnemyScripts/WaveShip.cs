using UnityEngine;

public class WaveShip : MonoBehaviour
{
    public float speed = 5f; 
    public float offscreenOffset = 1f;  // how far off-screen horizontally
    public float verticalOffset = 0.3f; // slightly above the top edge

    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        Camera cam = Camera.main;

        float screenTop = cam.transform.position.y + cam.orthographicSize;
        float screenLeft = cam.transform.position.x - cam.orthographicSize * cam.aspect;
        float screenRight = cam.transform.position.x + cam.orthographicSize * cam.aspect;

        // Y is always at the top of the screen + optional offset
        float yPos = screenTop + verticalOffset;

        // Randomly fly left-to-right or right-to-left
        bool leftToRight = Random.value > 0.5f;

        if (leftToRight)
        {
            startPos = new Vector3(screenLeft - offscreenOffset, yPos, 1f);
            endPos = new Vector3(screenRight + offscreenOffset, yPos, 1f);
            transform.eulerAngles = new Vector3(0f, 180f, 0f); // face right
        }
        else
        {
            startPos = new Vector3(screenRight + offscreenOffset, yPos, 1f);
            endPos = new Vector3(screenLeft - offscreenOffset, yPos, 1f);
            transform.eulerAngles = new Vector3(0f, 0f, 0f); // face left
        }

        transform.position = startPos;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPos) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
