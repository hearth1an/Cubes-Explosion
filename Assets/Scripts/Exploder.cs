using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{    
    [SerializeField] private float _baseExplosionForce = 10f;
    [SerializeField] private float _baseExplosionRadius = 40f;    
    
    private float _upwardsModifier = 0.1f;    

    public void Explode(GameObject cube)
    {
        int forceMultiplier = 3;
        float cubeSize = cube.transform.localScale.magnitude;
        float force = _baseExplosionForce / cubeSize * forceMultiplier;
        float radius = _baseExplosionRadius / cubeSize * forceMultiplier;
        Vector3 center = cube.transform.position;

        Collider[] colliders = Physics.OverlapSphere(center, radius);

        foreach (Collider collider in colliders)
        {   
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                AddExplosion(rigidBody.gameObject, force, center, radius);               
            }
        }

        Destroy(cube);
    }

    public void ExplodeCreatedCubes(List<GameObject> cubes, Vector3 explosionCenter)
    {
        foreach (GameObject cube in cubes)
        {
            AddExplosion(cube, _baseExplosionForce, explosionCenter, _baseExplosionRadius);
        }        
    }

    private void AddExplosion(GameObject gameObject, float force, Vector3 center, float radius)
    {
        gameObject.GetComponent<Rigidbody>().AddExplosionForce(force, center, radius, _upwardsModifier, ForceMode.Impulse);
    }
}
