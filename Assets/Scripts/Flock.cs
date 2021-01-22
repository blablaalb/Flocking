using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class Flock : MonoBehaviour
{
    private bool _turning = false;
    private FlockManager _flockManager;
    private float _speed;

    public Vector3 Position => transform.position;

    internal void Start()
    {
        _speed = Random.Range(_flockManager.MinSpeed, _flockManager.MaxSpeed);
    }

    internal void Update()
    {
        Bounds bounds = new Bounds(_flockManager.Position, _flockManager.SwimLimits * 2);

        Vector3 direction = Vector3.zero;
        RaycastHit raycastHit;

        if (!bounds.Contains(transform.position))
        {
            _turning = true;
            direction = _flockManager.Position - transform.position;

        }
        else if (Physics.Raycast(transform.position, transform.forward * 50, out raycastHit))
        {
            _turning = true;
            direction = Vector3.Reflect(transform.forward, raycastHit.normal);
        }
        else
        {
            _turning = false;
        }

        if (_turning)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                _flockManager.RotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                _speed = Random.Range(_flockManager.MinSpeed, _flockManager.MaxSpeed);
            }
            if (Random.Range(0, 100) < 20)
            {
                ApplyRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * _speed);
    }

    public void SetMyManager(FlockManager manager)
    {
        _flockManager = manager;
    }

    private void ApplyRules()
    {
        ICollection<Flock> flocks = _flockManager.AllFishes;
        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float ditanceTowardNeighbourFlock;
        int groupSize = 0;

        foreach (Flock flock in flocks)
        {
            if (flock != this)
            {
                ditanceTowardNeighbourFlock = Vector3.Distance(flock.Position, Position);
                if (ditanceTowardNeighbourFlock <= _flockManager.NeighbourDistance)
                {
                    vcenter += flock.Position;
                    groupSize++;

                    if (ditanceTowardNeighbourFlock < 1.0f)
                    {
                        vavoid = vavoid + (Position - flock.transform.position);
                    }

                    gSpeed = gSpeed + flock._speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcenter = vcenter / groupSize + (_flockManager.GoalPosition - Position);
            _speed = gSpeed / groupSize;

            Vector3 direction = (vcenter + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    _flockManager.RotationSpeed * Time.deltaTime
                    );
        }

    }
}
