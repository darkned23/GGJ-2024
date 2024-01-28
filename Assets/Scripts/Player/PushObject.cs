using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float pushForce = 2.0f;
    private float targetMass;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        targetMass = body.mass;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);

        body.velocity = pushDir * pushForce / targetMass;
    }
}
