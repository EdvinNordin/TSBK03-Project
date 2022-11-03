using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    List<GameObject> pointObjects;
    public GameObject prefab;
    Fluid fluid;
    int N = 100;
    int dir = 0;

    // Start is called before the first frame update
    void Start()
    {
        pointObjects = new List<GameObject>();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                pointObjects.Add(Instantiate(prefab, new Vector3(i,j,0), Quaternion.identity));
                
            }
        }
        fluid = new Fluid(0.01f, 0.0f, 0.00001f);
        

    }
    // Update is called once per frame
    void Update()
    {
        fluid.addDensity(32, 32, 100, N);
        dir++;
        fluid.addVelocity(32, 32, dir, N-dir, N);
        fluid.step();
        Color color = Color.white;
        for (int i = 0; i < N * N; i++)
        {
            float density = fluid.renderD(i);
            color = new Color(density, density, density);
            pointObjects[i].GetComponent<SpriteRenderer>().material.color = color;
        }
    }
}
