using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKLookData
    {
        public float eyesWeight;
        public float headWeight;
        public float bodyWeight;
        public float weightClamp;

        public Vector3 position;

        public void Reset()
        {
            eyesWeight = 0f;
            headWeight = 0f;
            bodyWeight = 0f;
            weightClamp = 0f;
            position = Vector3.zero;
        }

        public void Mix(IKLookData data, float weight)
        {
            eyesWeight = Mathf.LerpUnclamped(eyesWeight, data.eyesWeight, weight);
            headWeight = Mathf.LerpUnclamped(headWeight, data.headWeight, weight);
            bodyWeight = Mathf.LerpUnclamped(bodyWeight, data.bodyWeight, weight);
            weightClamp = Mathf.LerpUnclamped(weightClamp, data.weightClamp, weight);
            position = Vector3.LerpUnclamped(position, data.position, weight);
        }

        public void Add(IKLookData data, float weight)
        {
            eyesWeight += data.eyesWeight * weight;
            headWeight += data.headWeight * weight;
            bodyWeight += data.bodyWeight * weight;
            weightClamp += data.weightClamp * weight;
            position += data.position * weight;
        }
    }
}