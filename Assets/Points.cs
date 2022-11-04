using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    List<GameObject> pointObjects;
    public GameObject prefab;
    Fluid fluid;
    int N = 100;
    int iter = 0;
    Vector3 mousePos = new Vector3(0,0,0);
    float density, xPos, yPos;
    float fadeAmount = 0f;
    float prevX, prevY, randX, randY, temp = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = N/2;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        
        pointObjects = new List<GameObject>();

        //dt, diffusion, viscosity
        fluid = new Fluid(0.1f, 0.000000001f, 0.0001f, iter, N);
        //fluid.addDensity(N / 2, N / 2, 700, N);

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                temp = Mathf.PerlinNoise(i * 10f / (N + 1f), j * 10f / (N + 1f)) * 2 - 1;
                Debug.Log(temp);
                fluid.addDensity(i, j, 100*temp, N);

                pointObjects.Add(Instantiate(prefab, new Vector3(i-N / 2, j-N / 2, 0), Quaternion.identity));
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        xPos = Mathf.Lerp(-N, N, Mathf.InverseLerp(0, 1100, Input.mousePosition.x)) + N / 2;
        yPos = Mathf.Lerp(-N / 2, N / 2, Mathf.InverseLerp(0, 510, Input.mousePosition.y)) + N / 2;
        //Debug.Log(xPos);

        fluid.addVelocity((int)yPos, (int)xPos, yPos-prevY, xPos-prevX,  N);
        fluid.step();
        Color color = Color.white;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                randX = Mathf.PerlinNoise((float)i * 10 / ((float)N + 1f), (float)j * 10 * Time.time / ((float)N + 1f)) * 2 - 1;
                randY = Mathf.PerlinNoise((float)i * 10 * Time.time / ((float)N + 1f), (float)j * 10 / ((float)N + 1f)) * 2 - 1;
                //Debug.Log(Mathf.PerlinNoise((float)i * Time.time / ((float)N + 1f), (float)j * Time.time / ((float)N + 1f)) * 2 - 1);
                //fluid.addVelocity(i, j, randX, randY, N);
                density = fluid.getDensity(j + i * N);
                density = fluid.fadeDensity(i, fadeAmount)/100;
                color = new Color(density, density, density);
                pointObjects[j + i * N].GetComponent<SpriteRenderer>().material.color = color;
            }
        }
        prevX = xPos;
        prevY = yPos;
    }
}
/*
        Debug.Log((mousePos.x - 1100 / 2) - (Input.mousePosition.y - 510 / 2));
        fluid.addDensity((int)Input.mousePosition.y - 510 / 2,(int)Input.mousePosition.x - 1100 / 2,  1000, N);
        fluid.addVelocity( (int)Input.mousePosition.y - 510 / 2,(int)Input.mousePosition.x - 1100 / 2, (mousePos.x - 1100/2) - (Input.mousePosition.y - 510 / 2), (Input.mousePosition.x - 1100 / 2) -(mousePos.x - 1100 / 2), N);*/