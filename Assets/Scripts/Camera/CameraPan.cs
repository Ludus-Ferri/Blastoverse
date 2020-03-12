using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CameraPan : CameraEffect
{
    public float magnitude;
    public float smoothing;

    Quaternion oldRot;

    private void Start()
    {
        oldRot = GameManager.Instance.playerController.transform.rotation;
    }

    public override void Perform()
    {
        Quaternion rot = GameManager.Instance.playerController.transform.rotation;

        Quaternion newRot = Quaternion.Lerp(oldRot, rot, smoothing * Time.deltaTime);

        Vector2 displacement = new Vector2(magnitude * Mathf.Cos(newRot.eulerAngles.z * Mathf.Deg2Rad), magnitude * Mathf.Sin(newRot.eulerAngles.z * Mathf.Deg2Rad));
        SetDisplacement(displacement);

        oldRot = newRot;
    }
}
