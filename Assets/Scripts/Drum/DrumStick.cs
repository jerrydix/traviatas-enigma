using UnityEngine;

public class DrumStick : MonoBehaviour
{
    private bool pressed;
    private bool isMoving;
   
    private Transform originalPosition;

    [SerializeField] private Transform drumStickPressedPosition;
    
    private float positionTurnSpeed;
    private float rotationTurnSpeed;
    
    private float rot = 5.5f;

    private void Start()
    {
        pressed = false;
        isMoving = false;
        originalPosition = new GameObject().transform;
        originalPosition.position = transform.position;
        originalPosition.rotation = transform.rotation;
    }
    
    public void BeatStick(float positionTurnSpeed, float rotationTurnSpeed)
    {
        this.positionTurnSpeed = positionTurnSpeed;
        this.rotationTurnSpeed = rotationTurnSpeed;
        pressed = true;
        isMoving = true;
    }

    private void Update()
    {
      
        if (isMoving && pressed && Vector3.Distance(transform.position, drumStickPressedPosition.position) < 0.005f)
        {
            pressed = false;
        }
      
        if (isMoving && pressed)
        {
            transform.position = Vector3.Lerp(transform.position, drumStickPressedPosition.position, positionTurnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles), drumStickPressedPosition.rotation, rotationTurnSpeed * Time.deltaTime);
        }
        else if (isMoving && !pressed)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition.position, positionTurnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles), originalPosition.rotation, rotationTurnSpeed * Time.deltaTime);
        }
    }
}