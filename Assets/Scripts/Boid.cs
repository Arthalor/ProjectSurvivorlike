using Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helper.VectorFunctions;

public class Boid : MonoBehaviour
{
    public Vector2 currentVelocity { get; private set; }

    private Rigidbody2D rb;

    [SerializeField] private Transform target = default;
    [Space]
    [Header("Vision")]
    [SerializeField] private float maxProtectedRange = default;
    [SerializeField] private float maxVisionRange = default;
    [SerializeField] private float visionAngle = default;
    [Space]
    [SerializeField] private float minSpeed = default;
    [SerializeField] private float maxSpeed = default;
    [Space]
    [SerializeField] private Vector2 screenMarginsUpperBound = default;
    [SerializeField] private Vector2 screenMarginsLowerBound = default;
    [SerializeField] private float turnFactor = default;
    [Space]
    [Header("BoidsVariables")]
    [SerializeField] private float separationFactor = default;
    [SerializeField] private float alignmentFactor = default;
    [SerializeField] private float cohesionFactor = default;
    [SerializeField] private float huntingFactor = default;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentVelocity = Random.insideUnitCircle * maxSpeed;
        rb.velocity = currentVelocity;
    }

    private void FixedUpdate()
    {
        //apply velocity
        rb.velocity = CalculateVelocity();
    }

    private Vector2 CalculateVelocity()
    {
        List<Boid> boidsInProtectedRange = GetBoidsInProtectedRange();
        List<Boid> boidsInVisionRange = GetBoidsInVisionRange();

        //adding all velocity of all rules
        Vector2 velocity = currentVelocity + Separation(boidsInProtectedRange)
                                           + Alignment(boidsInVisionRange)
                                           + Cohesion(boidsInVisionRange)
                                           + Hunting(target)
                                           + EdgeAvoidance();

        float speed = velocity.magnitude;
        if (speed > maxSpeed) velocity = velocity.normalized * maxSpeed;
        if (speed < minSpeed) velocity = velocity.normalized * minSpeed;

        currentVelocity = velocity;
        return velocity;
    }

    #region GetBoids
    private List<Boid> GetBoidsInProtectedRange()
    {
        Collider2D[] collidersInProtectedRange = Physics2D.OverlapCircleAll(transform.position, maxProtectedRange);
        List<Boid> boidsInProtectedRange = new List<Boid>();
        foreach (var collider in collidersInProtectedRange)
        {
            collider.TryGetComponent(out Boid boid);
            boidsInProtectedRange.Add(boid);
        }

        return boidsInProtectedRange;
    }

    private List<Boid> GetBoidsInVisionRange()
    {
        Collider2D[] collidersInVisionRange = Physics2D.OverlapCircleAll(transform.position, maxVisionRange);
        List<Boid> boidsInVisionRange = new List<Boid>();

        foreach (var collider in collidersInVisionRange)
        {
            if (Vector2.Angle(currentVelocity, VectorFromAtoB(transform.position, collider.transform.position)) <= visionAngle)
            {
                collider.TryGetComponent(out Boid boid);
                boidsInVisionRange.Add(boid);
            }
        }
        return boidsInVisionRange;
    }
    #endregion

    #region Separation(Rule 1)
    //Separation: boids move away from other boids that are too close
    //called CollisionAvoidance in the Original Paper

    private Vector2 Separation(List<Boid> boidsInProtectedRange)
    {
        Vector2 velocityUpdate = Vector2.zero;

        foreach (var otherBoid in boidsInProtectedRange)
        {
            velocityUpdate += (Vector2)transform.position - (Vector2)otherBoid.transform.position;
        }
        return velocityUpdate * separationFactor;
    }
    #endregion

    #region Alignment(Rule 2)
    //Alignment: boids attempt to match the velocities of their neighbors
    //called Velocity Matching in the original paper

    private Vector2 Alignment(List<Boid> boidsInVisionRange)
    {
        if (boidsInVisionRange.Count <= 0) 
            return Vector2.zero;

        Vector2 averageVelocity = Vector2.zero;

        foreach (var otherBoid in boidsInVisionRange) 
        {
            averageVelocity += otherBoid.currentVelocity;
        }
        averageVelocity /= boidsInVisionRange.Count;
        return (averageVelocity - currentVelocity) * alignmentFactor;
    }
    #endregion

    #region Cohesion(Rule 3)
    //Cohesion: boids move toward the center of mass of their neighbors
    //called Flock Centering in the original paper

    Vector2 Cohesion(List<Boid> boidsInVisionRange)
    {
        if (boidsInVisionRange.Count <= 0)
            return Vector2.zero;

        Vector2 averagePosition = Vector2.zero;

        foreach (var otherBoid in boidsInVisionRange) 
        {
            averagePosition += (Vector2)otherBoid.transform.position;
        }
        averagePosition /= boidsInVisionRange.Count;
        return (averagePosition - (Vector2)transform.position) * cohesionFactor;
    }
    #endregion

    #region EdgeAvoidance
    private Vector2 EdgeAvoidance()
    {
        Vector2 velocityUpdate = Vector2.zero;
        if (transform.position.x < screenMarginsLowerBound.x)
            velocityUpdate.x += turnFactor;
        else if (transform.position.x > screenMarginsUpperBound.x)
            velocityUpdate.x -= turnFactor;
        if (transform.position.y > screenMarginsUpperBound.y)
            velocityUpdate.y -= turnFactor;
        else if (transform.position.y < screenMarginsLowerBound.y)
            velocityUpdate.y += turnFactor;
        return velocityUpdate;
    }
    #endregion

    #region Hunting
    //Hunting: boids move toward a target

    Vector2 Hunting(Transform target)
    {
        Vector2 velocityUpdate = Vector2.zero;
        velocityUpdate += (Vector2)target.transform.position - (Vector2)transform.position;
        return velocityUpdate * huntingFactor;
    }
    #endregion

}