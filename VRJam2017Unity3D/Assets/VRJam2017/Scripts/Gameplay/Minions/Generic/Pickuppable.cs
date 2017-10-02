using UnityEngine;
using System.Collections;
using AltSrc.UnityCommon.Math;
using DG.Tweening;

public class Pickuppable : MonoBehaviour
{
    [SerializeField]
    protected float pickupDuration = 0.25f;

    [SerializeField]
    protected float pickupXAngleOffset = -28;

    [SerializeField]
    protected Vector3 pickupPositionOffset = new Vector3(0f, 0.05f, 0f);

    private Transform owner = null;
    private bool isPickedUp = false;

    public void Pickup(Transform newOwner)
    {
        // Positioning logic.
        owner = newOwner;
        DoPickupAnimation();

        // Flail animation.
        gameObject.SendMessage("Flail", SendMessageOptions.DontRequireReceiver);

        // Capture logic.
        gameObject.SendMessage("Capture", SendMessageOptions.DontRequireReceiver);
    }

    public void Drop()
    {
        // Positioning logic.
        owner = null;
        isPickedUp = false;

        // Idle animation.
        gameObject.SendMessage("StopMoving", SendMessageOptions.DontRequireReceiver);

        // Release logic.
        gameObject.SendMessage("Release", SendMessageOptions.DontRequireReceiver);
    }

    public void Update()
    {
        // When fully picked up.
        if (owner != null && isPickedUp)
        {
            // Calculate position offset and lerp towards it.
            Vector3 targetPosition = CalculateTargetPosition();
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPosition, 
                100f * Time.deltaTime);

            // Lerp x angle to offset (due to animation glitch).
            // float xAngle = Mathf.LerpAngle(transform.eulerAngles.x, transform.eulerAngles.x + pickupAngleOffset, 10f * Time.deltaTime);
            // transform.eulerAngles = new Vector3(xAngle, transform.eulerAngles.y, transform.eulerAngles.z);

            float targetYAngle = CalculateTargetYAngle();
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetYAngle, transform.eulerAngles.z);
        }
    }

    private float animValue = 0f;

    private void DoPickupAnimation()
    {
        Vector3 targetPosition = CalculateTargetPosition();
        transform
            .DOMove(targetPosition, pickupDuration)
            .OnComplete(() => 
            { 
                isPickedUp = true; 
                animValue = 0f;
            });

        Vector3 targetEulerAngles = new Vector3(pickupXAngleOffset, CalculateTargetYAngle(), transform.eulerAngles.z);
        transform.DORotate(targetEulerAngles, pickupDuration);
    }

    private Vector3 CalculateTargetPosition()
    {
        return owner.position 
            + transform.forward.MultiplyByScalar(-0.15f)
            + transform.up.MultiplyByScalar(0.08f)
            + pickupPositionOffset;
    }

    private float CalculateTargetYAngle()
    {
        // Save current euler angles, snapshot look at angle towards player head.
        Vector3 originalEulerAngles = transform.eulerAngles;
        transform.LookAt(GameManager.Instance.Headset);
        Vector3 lookAtEulerAngles = transform.eulerAngles;
        transform.eulerAngles = originalEulerAngles;

        // Lerp y angle to look at player.
        return Mathf.LerpAngle(transform.eulerAngles.y, lookAtEulerAngles.y, 10f * Time.deltaTime);
    }
}
