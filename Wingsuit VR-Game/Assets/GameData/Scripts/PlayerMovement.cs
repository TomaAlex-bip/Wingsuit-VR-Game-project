using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector2 minContraints;
    [SerializeField] private Vector2 maxContraints;

    Transform cam;

    private void Start()
    {
        cam = transform.Find("Main Camera");
    }

    private void Update()
    {
        // move the player forward
        transform.position += cam.transform.forward * movementSpeed * Time.deltaTime;


        // limit the movement space
        float x = Mathf.Clamp(transform.localPosition.x, minContraints.x, maxContraints.x);
        float y = Mathf.Clamp(transform.localPosition.y, minContraints.y, maxContraints.y);
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);

    }

}
