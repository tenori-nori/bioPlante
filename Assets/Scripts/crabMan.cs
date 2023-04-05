using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabMan : MonoBehaviour
{
    public float maxPatrolDistance = 4f;
    public float stepDistance = 0.1f;
    public float stepsPerSecond = 5f;

    float startingPointX;
    void Start()
    {
        startingPointX = transform.position.x;
    }

    void Update()
    {
        if(Mathf.Abs(transform.position.x - startingPointX) < maxPatrolDistance)
        {
            transform.position += new Vector3(stepDistance * stepsPerSecond * Time.deltaTime, 0, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX ^= true;
            startingPointX = transform.position.x;
            stepDistance = -stepDistance;
        }
        Cut();
    }

    bool Cut()
    {
        int collideLayer = LayerMask.GetMask("Player");
        var collidingBranchSegments = new List<Collider2D>();
        var filter = new ContactFilter2D();
        filter.SetLayerMask(collideLayer);
        var collisions = Physics2D.OverlapCollider(GetComponent<CircleCollider2D>(), filter, collidingBranchSegments);
        if(collisions > 0)
        {
            if(collidingBranchSegments[0].gameObject.TryGetComponent<FixedJoint2D>(out FixedJoint2D jointToCut))
            {
                jointToCut.enabled = false;
                return true;
            }
        }
        return false;
    }
}
