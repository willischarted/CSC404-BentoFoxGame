using UnityEngine;

public class basicMovement : MonoBehaviour
{

    public Rigidbody rb;

    public float sidewaysForce = 5f;
    public Animator monsterAnim;
    // Update is called once per frame
    void FixedUpdate()
    {


        if (Input.GetKey("s"))
        {
            rb.AddForce(sidewaysForce, 0, 0, ForceMode.VelocityChange);
        }
        if(Input.GetKey("d"))
        {
            rb.AddForce(0, 0, sidewaysForce , ForceMode.VelocityChange);
        }

        if(Input.GetKey("a"))
        {
            rb.AddForce(0, 0, -sidewaysForce, ForceMode.VelocityChange);
        }

        if (Input.GetKey("w"))
        {
            rb.AddForce(-sidewaysForce , 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("p"))
        {
            monsterAnim.SetTrigger("isStunned");
        }

    }
}
