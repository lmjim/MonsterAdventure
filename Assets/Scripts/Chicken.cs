using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{

    public float movementSpeed = .25f;
    private int walk_direction;
    private int count;
    public int reset = 60;

    Animator m_Animator;
    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        count = 0;
        walk_direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ++count;
        if (count == reset)
        {
            count = 0;
            ++walk_direction;
        }
        // This gives a little walk->eat->walk loop, though eventually i need to have
        // The chicken walk randomly, not fall of the edge, and maybe eat real good
        // At least have something though
        if (walk_direction % 2 == 0)
        {
            // Set to 0 because the chicken is falling over itself?
            // It seems like the rigibbody and collider are bumping or something idk?
            // Also might be that I need to connect movement more to animator
            Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
            m_Rigidbody.AddForce(movement * movementSpeed);
            m_Animator.SetBool("Eat", false);
            m_Animator.SetBool("Walk", true);
            //print("walk");
        }
        else
        {
            //Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
            //m_Rigidbody.AddForce(movement * movementSpeed);
            m_Animator.SetBool("Walk", false);
            m_Animator.SetBool("Eat", true);
            //print("eat");
        }
    }
}
