using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : PlayerControl {
    private Transform hero;
    private Gun gun;
    private float shootAngle = 30;

    
	// Use this for initialization
	void Start () {
        hero = GameObject.FindGameObjectWithTag("Player").transform;
        gun = GetComponentInChildren<Gun>();
	}

    float CalculaVelocidad(Transform target, float angle)
    {
        var dir = target.position - transform.position;
        var h = dir.y;
        dir.y=0;
        var dist = dir.magnitude;
        var a = angle * Mathf.Deg2Rad;
        dist += h / Mathf.Tan(a);

        return Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)) * Random.Range(1.2f, 1.8f);
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        if (hasTurn)
        {
            if (facingRight)
                Flip();
            if (tilt != shootAngle)
            {
                if (tilt < shootAngle)
                    RotaIzquierdaDown();
                else
                    RotaDerechaDown();
            }
            else
            {
                gun.Fire(CalculaVelocidad(hero, shootAngle));
                RotaDerechaUp();
                RotaIzquierdaUp();

            }
        }
        else
        {
            shootAngle = Random.Range(25, 45);
        }
        
	}
}
