using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKLookData
    {
        public struct JobData
        {
            public float eyesWeight;
            public float headWeight;
            public float bodyWeight;
            public float weightClamp;

            public Vector3 position;
        }

        public JobData jobData;

        public void Reset()
        {
            jobData.eyesWeight = 0f;
            jobData.headWeight = 0f;
            jobData.bodyWeight = 0f;
            jobData.weightClamp = 0f;
            jobData.position = Vector3.zero;
        }

        public void Mix(IKLookData data, float weight)
        {
            jobData.eyesWeight = Mathf.LerpUnclamped(jobData.eyesWeight, data.jobData.eyesWeight, weight);
            jobData.headWeight = Mathf.LerpUnclamped(jobData.headWeight, data.jobData.headWeight, weight);
            jobData.bodyWeight = Mathf.LerpUnclamped(jobData.bodyWeight, data.jobData.bodyWeight, weight);
            jobData.weightClamp = Mathf.LerpUnclamped(jobData.weightClamp, data.jobData.weightClamp, weight);
            jobData.position = Vector3.LerpUnclamped(jobData.position, data.jobData.position, weight);
        }

        public void Add(IKLookData data, float weight)
        {
            jobData.eyesWeight += data.jobData.eyesWeight * weight;
            jobData.headWeight += data.jobData.headWeight * weight;
            jobData.bodyWeight += data.jobData.bodyWeight * weight;
            jobData.weightClamp += data.jobData.weightClamp * weight;
            jobData.position += data.jobData.position * weight;
        }
    }
}