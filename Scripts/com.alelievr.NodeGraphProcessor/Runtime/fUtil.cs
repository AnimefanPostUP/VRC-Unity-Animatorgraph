using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fUtil : MonoBehaviour
{
    //Write Error on Null
    public static bool nullError<T>(T obj, string message)
    {
        if (obj == null)
        {
            Debug.LogError(message);
            return true;
        }
        return false;
    }
}
