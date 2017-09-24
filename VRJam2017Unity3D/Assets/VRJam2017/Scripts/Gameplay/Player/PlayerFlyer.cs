using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyer : MonoBehaviour
{
    // FLIGHT MODE
    public event FlightStateChangedHandler FlightStateChanged;
    public delegate void FlightStateChangedHandler(object sender, FlightStates newState);

    public enum FlightStates
    {
        GROUNDED,
        FLYING
    };
    private FlightStates flightState = FlightStates.GROUNDED;
    public FlightStates FlightState
    {
        get
        {
            return flightState;
        }
        set
        {
            if (flightState != value)
            {
                flightState = value;

                if (FlightStateChanged != null)
                {
                    FlightStateChanged(this, flightState);
                }
            }
        }
    }

    public void ToggleFlightMode()
    {
        if (flightState == FlightStates.FLYING)
        {
            flightState = FlightStates.GROUNDED;
        }
        else if (flightState == FlightStates.GROUNDED)
        {
            flightState = FlightStates.FLYING;
        }

        Debug.LogWarning("TOGGLE: " + FlightState.ToString());
    }
}
