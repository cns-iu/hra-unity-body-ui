using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesFinder : MonoBehaviour
{
    private List<Transform> _leafArray = new List<Transform>();
    
    void Start()
    {
        FindLeaves(this.transform, _leafArray);
    }

    /// <summary>
    /// Utility function that finds all the leaf children of the organs
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="leafArray"></param>
    /// <returns></returns>
    public static List<Transform> FindLeaves(Transform parent, List<Transform> leafArray)
    {
        if (parent.childCount == 0)
        {
            //if there are no children just add the parent
            leafArray.Add(parent);
        }
        else
        {
            //else loop through the children and recursively call find leaves and add it to the leaf array
            foreach (Transform child in parent)
            {
                FindLeaves(child, leafArray);
                leafArray.Add(parent);
            }
        }

        return leafArray;
    }
}