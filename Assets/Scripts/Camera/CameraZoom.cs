using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraZoom : CameraEffect
{
    public float localZoom;

    public override void Perform()
    {
        SetZoom(localZoom);
    }
}