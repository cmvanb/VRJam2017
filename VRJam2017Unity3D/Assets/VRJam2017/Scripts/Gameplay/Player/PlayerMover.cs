using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerMover : MonoBehaviour
{
    public VRTK_DashTeleport DashTeleport;

    // DASH
    public void Dash(DestinationMarkerEventArgs dashArgs)
    {
        Debug.LogWarning("DASH to " + dashArgs.destinationPosition);

        DashTeleport.Teleport(dashArgs);
    }
}
