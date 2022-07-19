using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PlayerController : NetworkBehaviour
{
    float speed = 0;
    Animator animator;
    const float initial_speed = 30;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [SerializeField]
    public Bullet bulletPrefab = null;

    [Networked]
    public NetworkButtons ButtonsPrevious { get; set; }


    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            NetworkButtons buttons = data.buttons;
            var pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;
            bool isKeyDown = false;

            if (pressed.IsSet(InputButtons.Mouse0))
            {
                Bullet bullet = Runner.Spawn(
                bulletPrefab,
                transform.position,
                Quaternion.LookRotation(data.mousePosition - transform.position),
                Object.InputAuthority);
                bullet.direction = Vector3.Normalize(data.mousePosition - transform.position);
            }

            if (buttons.IsSet(InputButtons.D))
            {
                speed = initial_speed;
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                gameObject.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
                isKeyDown = true;
            }
            if (buttons.IsSet(InputButtons.A))
            {
                speed = initial_speed;
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                gameObject.transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
                isKeyDown = true;
            }
            if (buttons.IsSet(InputButtons.W))
            {
                speed = initial_speed;
                gameObject.transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
                isKeyDown = true;
            }
            if (buttons.IsSet(InputButtons.S))
            {
                speed = initial_speed;
                gameObject.transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
                isKeyDown = true;
            }
            if (!isKeyDown)
            {
                speed = 0;
            }
            if (speed != 0)
            {
                animator.SetBool("W", true);
            }
            else
            {
                animator.SetBool("W", false);
            }
        }
    }
}
