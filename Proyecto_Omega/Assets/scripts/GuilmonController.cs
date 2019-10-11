using UnityEngine;

public class GuilmonController : MonoBehaviour
{

    public float movementSpeed = 5.0f;
    public float HorizontalMove;
    public float VerticalMove;
    public CharacterController Player;
    public void Start()
    {
        Player = GetComponent<CharacterController>();
    }
    void Update()
    {
        // get inputs
        HorizontalMove = Input.GetAxis("Horizontal");
        VerticalMove = Input.GetAxis("Vertical");

        Vector3 moveVectorX = transform.forward * VerticalMove;
        Vector3 moveVectorY = transform.right * HorizontalMove;
        Vector3 moveVector = (moveVectorX + moveVectorY).normalized * movementSpeed * Time.deltaTime;

        Player.Move(moveVector);

    }
    private void FixedUpdate()
    {

    }


}
