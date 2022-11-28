using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree : MonoBehaviour
{
    public OctreeNode _rootNode;

    public Octree(GameObject[] worldObjects, float minNodeSize)
    {
        Bounds bounds = new Bounds();
        foreach(GameObject go in worldObjects)
        {
            bounds.Encapsulate(go.GetComponent<Collider>().bounds);
        }
        float maxSize = Mathf.Max(new float[] { bounds.size.x, bounds.size.y, bounds.size.z });
        Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 0.5f;
        bounds.SetMinMax(bounds.center - sizeVector, bounds.center + sizeVector);
        _rootNode = new OctreeNode(bounds, minNodeSize);
        AddObjencts(worldObjects);
    }

    public void AddObjencts(GameObject[] woldObjects)
    {
        foreach(GameObject go in woldObjects)
        {
            _rootNode.AddObject(go);
        }
    }
}
