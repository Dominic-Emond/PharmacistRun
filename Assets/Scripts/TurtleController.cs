using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    public float Speed;
    public float MaxSpeed;
    float directionY;
    float directionX;

    // Start is called before the first frame update
    void Start()
    {
        //Gives Random direction
        float radian = Random.Range(0, 2 * Mathf.PI);
        directionX = Mathf.Cos(radian);
        directionY = Mathf.Sin(radian);

        ChangeRotation();

        //Debug.Log("Angle: " + radian + ", Direction X: " + directionX + ", Direction Y: " + directionY);
    }

    // Update is called once per frame
    void Update()
    {
        MovementType1();
    }

    void MovementType1()
    {
        //Change it to make it jump of a wall
        Vector2 position = transform.position;

        position.x = position.x + Time.deltaTime * Speed * directionX;
        position.y = position.y + Time.deltaTime * Speed * directionY;

        transform.position = position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.tag);
        if (other.tag == "Wall")
        {
            Vector2 position = transform.position;
            var wallPosition = other.ClosestPoint(position);
            /*
            //if (!Mathf.Approximately(wallPosition.y, position.y) && !Mathf.Approximately(wallPosition.x, position.x))
            Debug.Log("Position Y: " + position.y + ", Position Wall Y: "  + wallPosition.y + ", Position X: " + position.x + ", Position Wall X: " + wallPosition.x);
            Debug.Log("Position Y: " + System.Math.Round(position.y, 2) + ", Position Wall Y: " + System.Math.Round(wallPosition.y, 2) + ", Position X: " + System.Math.Round(wallPosition.x, 2) + ", Position Wall X: " + System.Math.Round(position.x, 2));
            */
            if (System.Math.Round(wallPosition.y, 3) != System.Math.Round(position.y, 3))
            {
                directionY = 0 - directionY;
            }

            if (System.Math.Round(wallPosition.x, 3) != System.Math.Round(position.x, 3))
            {
                directionX = 0 - directionX;
            }

            ChangeRotation();
        }
    }

    void ChangeRotation()
    {
        float angle = Mathf.Atan2(directionY, directionX) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }

    public void ChangeSpeed(int amount)
    {
        if ((Speed + amount) <= MaxSpeed && (Speed + amount) >= 0)
        {
            Speed += amount;
        }
    }
}
