using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [Networked]
    private TickTimer life { get; set; }

    [SerializeField]
    private float bulletSpeed = 5f;

    [SerializeField]
    public Vector3 direction;

    public override void Spawned()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(0,0,0));
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += bulletSpeed * direction * Runner.DeltaTime;
        }

    }
}
