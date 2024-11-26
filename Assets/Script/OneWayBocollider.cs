
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class OneWayBocollider : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;
    private new BoxCollider collider = null;

    private BoxCollider collisionCheckTrigger = null;

    public MeshRenderer meshRenderer;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = new Vector3(collider.size.x + 2.239903f, collider.size.y + 0.459365f, collider.size.z);
        collisionCheckTrigger.center = new Vector3(collider.center.x + -0.05989552f, collider.center.y + -0.09031925f, collider.center.z - 0.115641f);
        collisionCheckTrigger.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {

        other.TryGetComponent(out CharacterBase character);
        if (character == null) return;
        if (character.velocity.z < 0)
        {
            Physics.IgnoreCollision(collider, other, false);
        }
        else
        {
            Physics.IgnoreCollision(collider, other, true);
            meshRenderer.enabled = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Vector3 direction;
        if (localDirection)
        {
            direction = transform.TransformDirection(entryDirection.normalized);
        }
        else
        {
            direction = entryDirection;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, entryDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -entryDirection);
    }
}
