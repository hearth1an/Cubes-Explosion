using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private int _minCubes = 2;
    [SerializeField] private int _maxCubes = 6;

    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _explosionRadius = 2f;

    public void SpawnCubes(Transform sourceCube)
    {
        int childCount = Random.Range(_minCubes, _maxCubes + 1);
        List<GameObject> newCubes = new List<GameObject>();

        for (int i = 0; i < childCount; i++)
        {    
            newCubes.Add(CreateCube(sourceCube));
        }

        ApplyExplosionForce(newCubes, sourceCube.position);
                
        Destroy(sourceCube.gameObject);
    }

    private void ApplyExplosionForce(List<GameObject> cubes, Vector3 explosionCenter)
    {
        float upwardsModifier = 0.5f;

        foreach (var cube in cubes)
        {
            cube.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, explosionCenter, _explosionRadius, upwardsModifier, ForceMode.Impulse);            
        }
    }

    private GameObject CreateCube(Transform sourceCube)
    {
        int scaleReduceValue = 2;

        GameObject newCube = Instantiate(_cubePrefab, sourceCube.position, Random.rotation);

        newCube.transform.localScale = sourceCube.localScale / scaleReduceValue;

        Renderer renderer = newCube.GetComponent<Renderer>();
        renderer.material.color = new Color(Random.value, Random.value, Random.value);

        newCube.GetComponent<Rigidbody>().useGravity = true;

        return newCube;
    }
}
