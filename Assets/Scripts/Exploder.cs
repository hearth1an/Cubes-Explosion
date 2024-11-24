using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{    
    [SerializeField] private float _baseExplosionForce = 10f;
    [SerializeField] private float _baseExplosionRadius = 40f;    
    
    private float _upwardsModifier = 0.1f;    

    public void Explode(GameObject cube)
    {
        float cubeSize = cube.transform.localScale.magnitude;
        float explosionForce = Mathf.Max(_baseExplosionForce / cubeSize, 5f);
        float explosionRadius = Mathf.Max(_baseExplosionRadius / cubeSize, 10f);

        ApplyExplosionForce(cube.transform.position, explosionForce, explosionRadius);

        Destroy(cube);
    }

    public void ExplodeCreatedCubes(List<GameObject> cubes, Vector3 explosionCenter)
    {
        ApplyExplosionForce(explosionCenter, _baseExplosionForce, _baseExplosionRadius);
    }

    private void ApplyExplosionForce(List<GameObject> cubes, Vector3 center, float force, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius);


        foreach (Collider collider in colliders)
        {
            collider.GetComponent<Rigidbody>().AddExplosionForce(force, center, radius, _upwardsModifier, ForceMode.Impulse);
        }
    }
}
