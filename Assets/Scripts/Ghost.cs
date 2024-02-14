using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ghost : MonoBehaviour
{
    public GameObject chaseTarget;
    
    public bool isOnPatrol;
    public bool isOnChase;

    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float chaseRotateSpeed = 1f;
    [SerializeField] private float startChaseOffset = 3f;
    [SerializeField] private float stopOffset = 1f;

    [SerializeField] private Animator animator;

    private float speedBefore;
    private bool _isChaseTargetActive;
    
    // Start is called before the first frame update
    void Start()
    {
        speedBefore = chaseSpeed;
        CheckActive();
        isOnChase = false;
        isOnPatrol = true;
    }

    void CheckActive()
    {
        _isChaseTargetActive = chaseTarget != null;
        
        Debug.Log(_isChaseTargetActive? $"CHASE TARGET IS {chaseTarget.name}" : "NO CHASE TARGET");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        isOnPatrol = !isOnChase;
        if (!_isChaseTargetActive)
        {
            return;
        }
        GhostChase();
    }

    void GhostChase()
    {

        if (Vector3.Distance(chaseTarget.transform.position, transform.position) <= startChaseOffset)
        {
            isOnPatrol = false;
            isOnChase = true;
        }
        else
        {
            isOnPatrol = true;
            isOnChase = false;
        }

        if (isOnPatrol) return;
        if (Vector3.Distance(chaseTarget.transform.position, transform.position) <= stopOffset )
        {
            animator.SetBool("NearTarget",true);
            chaseSpeed = 0;
        }
            
        if (Vector3.Distance(chaseTarget.transform.position, transform.position) >= stopOffset)
        {
            animator.SetBool("NearTarget",false);
            chaseSpeed = speedBefore;
        }
            
        var lookAtPosition =
            Quaternion.LookRotation(chaseTarget.transform.position - transform.position);
                    
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, lookAtPosition, chaseRotateSpeed * Time.deltaTime);
                    
        transform.Translate(0, 0, chaseSpeed * Time.deltaTime);

    }
    
}
