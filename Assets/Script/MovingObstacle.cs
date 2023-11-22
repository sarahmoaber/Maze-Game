using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed;
    Vector3 tragetpos;

    public GameObject way;

    public Transform[] waypoint;

    int pointindex;
    int pointcount;
    int dirction;

    private void Awake()
    {
        waypoint = new Transform[way.transform.childCount];

        for (int i = 0; i < way.gameObject.transform.childCount; i++)
        {
            waypoint[i] = way.transform.GetChild(i).gameObject.transform;
        }

        

    }

    private void Start()
    {
        pointcount = waypoint.Length;
        pointindex = 1;
        tragetpos = waypoint[pointindex].transform.position;
    }

    private void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, tragetpos, step);

        if(transform.position == tragetpos)
        {
            Nextpoint();
        }
    }

    void Nextpoint() {
        if(pointindex == pointcount-1) {
            dirction = -1;
        }

        if (pointindex == 0) { 
            dirction = 1;
        }

        pointindex += dirction;
        tragetpos = waypoint[pointindex].transform.position;
    }
}
