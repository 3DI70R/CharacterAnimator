using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKLimbData
    {
        public struct JobData
        {
            public float positionWeight;
            public float rotationWeight;
            public float hintWeight;

            public Vector3 position;
            public Quaternion rotation;
            public Vector3 hintPosition;
        }

        public JobData jobData;
            
        public void Reset()
        {
            jobData.positionWeight = 0f;
            jobData.rotationWeight = 0f;
            jobData.hintWeight = 0f;
            jobData.position = Vector3.zero;
            jobData.rotation = Quaternion.identity;
            jobData.hintPosition = Vector3.zero;
        }

        public void Mix(IKLimbData data, float weight)
        {
            jobData.positionWeight = Mathf.LerpUnclamped(jobData.positionWeight, data.jobData.positionWeight, weight);
            jobData.rotationWeight = Mathf.LerpUnclamped(jobData.rotationWeight, data.jobData.rotationWeight, weight);
            jobData.hintWeight = Mathf.LerpUnclamped(jobData.hintWeight, data.jobData.hintWeight, weight);
            jobData.position = Vector3.LerpUnclamped(jobData.position, data.jobData.position, weight);
            jobData.hintPosition = Vector3.LerpUnclamped(jobData.hintPosition, data.jobData.hintPosition, weight);
            jobData.rotation = Quaternion.SlerpUnclamped(jobData.rotation, data.jobData.rotation, weight);
        }

        public void Add(IKLimbData data, float weight)
        {
            jobData.positionWeight += data.jobData.positionWeight * weight;
            jobData.rotationWeight += data.jobData.rotationWeight * weight;
            jobData.hintWeight += data.jobData.hintWeight * weight;
            jobData.position += data.jobData.position * weight;
            jobData.hintPosition += data.jobData.hintPosition * weight;
            jobData.rotation *= Quaternion.SlerpUnclamped(Quaternion.identity, data.jobData.rotation, weight);
        }
    }
}