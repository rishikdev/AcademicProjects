using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [SerializeField] private float distance = 50.0f;
    [SerializeField] private float angle = 80.0f;
    [SerializeField] private float height = 1.75f;
    [SerializeField] private Color meshColour = Color.red;
    [SerializeField] private int scanFrequency = 30;
    [SerializeField] private LayerMask layers;
    [SerializeField] private LayerMask occlusionLayers;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    private Mesh mesh;
    private Collider[] colliders = new Collider[50];
    private int count;
    private float scanInterval;
    private float scanTimer;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer = scanTimer - Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer = scanTimer + scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        objects.Clear();

        for(int i= 0; i < count; i = i + 1)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                objects.Add(obj);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        //if (direction.y < 0 || direction.y > height)
        //    return false;

        //direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
            return false;

        origin.y = origin.y + height;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
            return false;

        return true;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero - new Vector3(0, height, 0) * height;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance + bottomCenter * 1.5f;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance + bottomCenter * 1.5f;

        Vector3 topCenter = bottomCenter + new Vector3(0, 3 * height, 0) * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height + topCenter * 1.5f;
        Vector3 topRight = bottomRight + Vector3.up * height + topCenter * 1.5f;

        int vert = 0;

        // Left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // Right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for(int i = 0; i < segments; i = i + 1)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance + bottomCenter * 1.5f;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance + bottomCenter * 1.5f;

            topLeft = bottomLeft + Vector3.up * height + topCenter * 1.5f;
            topRight = bottomRight + Vector3.up * height + topCenter * 1.5f;

            // Far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle = currentAngle + deltaAngle;
        }

        for(int i = 0; i < numVertices; i = i + 1)
            triangles[i] = i;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(mesh)
        {
            Gizmos.color = meshColour;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);

        for(int i = 0; i < count; i = i + 1)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in objects)
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
    }
}
