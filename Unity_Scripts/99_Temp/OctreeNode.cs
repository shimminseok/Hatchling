using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode
{
    Bounds _nodeBounds;
    float _minSize;
    Bounds[] _childBounds;
    OctreeNode[] _children = null;
    public OctreeNode(Bounds b, float minNodeSize)
    {
        _nodeBounds = b;
        _minSize = minNodeSize;

        float quarter =_nodeBounds.size.y / 4f;
        float childLength = _nodeBounds.size.y / 2;
        Vector3 childSize = new Vector3(childLength, childLength, childLength);
        _childBounds = new Bounds[8];
        _childBounds[0] = new Bounds(_nodeBounds.center + new Vector3(-quarter, quarter, -quarter),childSize);
        _childBounds[1] = new Bounds(_nodeBounds.center + new Vector3(quarter, quarter, -quarter),childSize);
        _childBounds[2] = new Bounds(_nodeBounds.center + new Vector3(-quarter, quarter, quarter),childSize);
        _childBounds[3] = new Bounds(_nodeBounds.center + new Vector3(quarter, quarter, -quarter),childSize);
        _childBounds[4] = new Bounds(_nodeBounds.center + new Vector3(-quarter, -quarter, -quarter),childSize);
        _childBounds[5] = new Bounds(_nodeBounds.center + new Vector3(quarter, -quarter, -quarter),childSize);
        _childBounds[6] = new Bounds(_nodeBounds.center + new Vector3(-quarter, -quarter, quarter),childSize);
        _childBounds[7] = new Bounds(_nodeBounds.center + new Vector3(quarter, -quarter, quarter),childSize);

    }
    public void AddObject(GameObject go)
    {
        DivideAndAdd(go);
    }
    public void DivideAndAdd(GameObject go)
    {
        if(_nodeBounds.size.y <= _minSize)
        {
            return;
        }
        if(_children == null)
        {
            _children = new OctreeNode[8];
        }

        bool dividing = false;
        for(int i = 0; i<8;i++)
        {
            if(_children[i] == null)
            {
                _children[i] = new OctreeNode(_childBounds[i], _minSize);
            }
            if(_childBounds[i].Intersects(go.GetComponent<Collider>().bounds))
            {
                dividing = true;
                _children[i].DivideAndAdd(go);
            }
        }
        if (dividing == false)
        {
            _children = null;
        }
    }

    public void Draw()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(_nodeBounds.center, _nodeBounds.size);
        if (_children != null)
        {
            for(int i = 0; i<8;i++)
            {
                if (_children[i] != null)
                {
                    _children[i].Draw();
                }
            }
        }
    }
}
