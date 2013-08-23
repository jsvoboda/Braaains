using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Extensions
{
    public static Vector3 nullYAxis(this Vector3 original){
        return new Vector3(original.x, 0, original.z);
    }
}

