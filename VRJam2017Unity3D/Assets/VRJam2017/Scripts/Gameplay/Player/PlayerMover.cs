using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerMover : MonoBehaviour
{
    public VRTK_DashTeleport DashTeleport;
    public float GroundTeleportDistance = 8f;
    public float GroundTeleportDuration = 0.2f;

    public bool IsTeleporting { get; private set; }

    [HideInInspector]
    public VRTK_BezierPointerRenderer PointerRenderer;

    public void Dash(DestinationMarkerEventArgs dashArgs, float duration)
    {
        // Debug.LogWarning("DASH to " + dashArgs.destinationPosition);

        IsTeleporting = true;

        DashTeleport.Teleported += (object sender, DestinationMarkerEventArgs args) => {
            IsTeleporting = false;
        };

        DashTeleport.normalLerpTime = duration;
        DashTeleport.Teleport(dashArgs);
    }

    public void SetTeleportDistance(float d)
    {
        PointerRenderer.maximumLength = new Vector2(d, Mathf.Infinity);
    }
}
