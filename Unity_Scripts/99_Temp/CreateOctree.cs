using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOctree : MonoBehaviour
{

    public GameObject[] _worldObj;
    public int _nodeMinSize = 5;
    Octree _octree;
    void Start()
    {
        _octree = new Octree(_worldObj, _nodeMinSize);
    }
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            _octree._rootNode.Draw();
        }
    }
}
