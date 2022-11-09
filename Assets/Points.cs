using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    List<GameObject> pointObjects;
    public GameObject prefab;
    public int debug;
    Fluid fluid;
    int N = 100;
    int iter = 16;
    Vector3 mousePos = new Vector3(0,0,0);
    float density, xPos, yPos;
    float fadeAmount = 0.0f;
    float prevX, prevY, randX, randY, temp = 0f;
    GameObject Triangle;


    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = N/2;
        Camera.main.transform.position = new Vector3(0, -0.5f, -10);
        
        pointObjects = new List<GameObject>();

        //dt, diffusion (mixing), viscosity
        fluid = new Fluid(0.1f, 0.0000f, 0.00f, iter, N);
        //fluid.addDensity(N / 2, N / 2, 700, N);

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                temp = Mathf.PerlinNoise(i * 10f / (N + 1f), j * 10f / (N + 1f)) * 2 - 1;
                //Debug.Log(temp);
                //fluid.addDensity(i, j, 10*temp, N);

                pointObjects.Add(Instantiate(prefab, new Vector3(i-N / 2, j-N / 2, 0), Quaternion.identity));
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        //fluid.addDensity(N/2, N/2, 10000, N);
        xPos = Mathf.Lerp(-N, N, Mathf.InverseLerp(0, 1100, Input.mousePosition.x)) + N / 2;
        yPos = Mathf.Lerp(-N / 2, N / 2, Mathf.InverseLerp(0, 510, Input.mousePosition.y)) + N / 2;
        //Debug.Log(xPos);

        Color color = Color.white;

        if (Input.GetMouseButton(1))
        {
            fluid.addVelocity((int)yPos, (int)xPos, 10 * (yPos - prevY), 10 * (xPos - prevX), N);
        }

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                //fluid.addVelocity(i, j, 0f,-0.001f,  N);

                if (Input.GetMouseButton(0))
                {
                    fluid.addDensity((int)yPos, (int)xPos, 10.001f, N);
                    //fluid.addDensity(N - 1, N /2, 10.001f, N);
                    /*fluid.addDensity((int)yPos+1, (int)xPos, 0.1f, N);
                    fluid.addDensity((int)yPos-1, (int)xPos, 0.1f, N);
                    fluid.addDensity((int)yPos, (int)xPos+1, 0.1f, N);
                    fluid.addDensity((int)yPos, (int)xPos-1, 0.1f, N);*/
                }

                Triangle = pointObjects[j + i * N].transform.GetChild(0).gameObject;
                Triangle.transform.eulerAngles = new Vector3(0,0,fluid.getVelocity(j,i)*180f/3.14f);
                density = fluid.getDensity(j + i * N)/100f;
                //density = fluid.fadeDensity(j + i * N, fadeAmount);
                color = new Color(density, density, density);
                pointObjects[j + i * N].GetComponent<SpriteRenderer>().material.color = color;
            }
        }
        prevX = xPos;
        prevY = yPos;
        fluid.step();
    }
}