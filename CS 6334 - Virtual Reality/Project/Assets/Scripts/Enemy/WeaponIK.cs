using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class WeaponIK : MonoBehaviour
{
    [SerializeField] private Transform aimTransform;
    [Range(0, 1)]
    [SerializeField] private float weight = 1.0f;
    [SerializeField] private HumanBone[] humanBones;
    [SerializeField] private float angleLimit = 90.0f;
    [SerializeField] private float distanceLimit = 1.5f;

    private Transform[] boneTransforms;
    public Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();

        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i = i + 1)
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);

        if (targetAngle > angleLimit)
            blendOut = blendOut + (targetAngle - angleLimit) / 50.0f;

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
            blendOut = blendOut + distanceLimit - targetDistance;

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);

        return aimTransform.position + direction;
    }

    void LateUpdate()
    {
        if (targetTransform == null || aimTransform == null)
            return;

        Vector3 targetPosition = GetTargetPosition();

        for (int i = 0; i < 10; i = i + 1)
        {
            for (int j = 0; j < boneTransforms.Length; j = j + 1)
            {
                Transform bone = boneTransforms[j];
                float boneWeight = humanBones[j].weight * weight;
                AimAtTarget(bone, targetPosition, boneWeight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}
