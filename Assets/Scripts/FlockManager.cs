using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class FlockManager : MonoBehaviour
{
    [SerializeField]
    private Flock _fishPrefab;
    private Flock[] _allFish;
    [SerializeField]
    private int _numFish = 20;
    [SerializeField]
    private float _minSpeed;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private Vector3 _swimLimits = new Vector3(5, 5, 5);
    private Vector3 _goalPos;
    [SerializeField]
    private float _neighbourDistance;
    [SerializeField]
    private float _rotationSpeed;

    public ReadOnlyCollection<Flock> AllFishes => new ReadOnlyCollection<Flock>(_allFish);
    public float MinSpeed => _minSpeed;
    public float MaxSpeed => _maxSpeed;
    public Vector3 SwimLimits => _swimLimits;
    public Vector3 GoalPosition => _goalPos;
    public Vector3 Position => transform.position;
    public float NeighbourDistance => _neighbourDistance;
    public float RotationSpeed => _rotationSpeed;

    internal void Start()
    {
        _allFish = new Flock[_numFish];
        for (int i = 0; i < _numFish; i++)
        {
            Vector3 pos = new Vector3(
                                        Random.Range(-_swimLimits.x, _swimLimits.x),
                                        Random.Range(-_swimLimits.y, _swimLimits.y),
                                        Random.Range(-_swimLimits.z, _swimLimits.z)
                                        );
            _allFish[i] = Instantiate<Flock>(_fishPrefab, pos, Quaternion.identity);
            _allFish[i].SetMyManager(this);
        }
        _goalPos = transform.position;
    }

    internal void Update()
    {
        if (Random.Range(0, 100) < 10)
            _goalPos = transform.position + new Vector3(
                Random.Range(-_swimLimits.x, _swimLimits.x),
                Random.Range(-_swimLimits.y, _swimLimits.y),
                Random.Range(-_swimLimits.z, _swimLimits.z)
            );
    }
}
