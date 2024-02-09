using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject chaseTarget;
    
    public bool isOnPatrol;
    public bool isOnChase;

    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float chaseRotateSpeed = 1f;
    [SerializeField] private float stopOffset = 1f;
    
    private bool _isChaseTargetActive;
    
    // Start is called before the first frame update
    void Start()
    {
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
        if (!isOnChase) { return; }
        if (Vector3.Distance(chaseTarget.transform.position, transform.position) <= stopOffset) { return; }
        
        var lookAtPosition =
            Quaternion.LookRotation(chaseTarget.transform.position - transform.position);
        
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, lookAtPosition, chaseRotateSpeed * Time.deltaTime);
        
        transform.Translate(0, 0, chaseSpeed * Time.deltaTime);
    }
}
