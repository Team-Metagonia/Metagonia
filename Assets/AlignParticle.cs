using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignParticle : MonoBehaviour
{
    public Transform _point1; // Assign in the Inspector
    public Transform _point2; // Assign in the Inspector
    public ParticleSystem _particleSystem; // Assign in the Inspector

    void Start()
    {
        AlignParticles(_point1.position, _point2.position);
    }

    void AlignParticles(Vector3 start, Vector3 end)
    {
        // Calculate direction and distance
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        // Set the position of the particle system to the midpoint
        Vector3 midpoint = (start + end) / 2f;
        _particleSystem.transform.position = start;

        // Set the rotation of the particle system to face the direction
        _particleSystem.transform.rotation = Quaternion.LookRotation(direction);

        // Adjust the length of the particle system
        var shape = _particleSystem.shape;
        shape.scale = new Vector3(shape.scale.x, shape.scale.y, distance);
    }

    private void Update()
    {
        AlignParticles(_point1.position, _point2.position);
    }
}
