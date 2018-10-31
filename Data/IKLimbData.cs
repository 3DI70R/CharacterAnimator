using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public class IKLimbData
    {
        public float positionWeight;
        public float rotationWeight;
        public float hintWeight;

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 hintPosition;
            
        public void Reset()
        {
            positionWeight = 0f;
            rotationWeight = 0f;
            hintWeight = 0f;
            position = Vector3.zero;
            rotation = Quaternion.identity;
            hintPosition = Vector3.zero;
        }

        public void Mix(IKLimbData data, float weight)
        {
            positionWeight = Mathf.LerpUnclamped(positionWeight, data.positionWeight, weight);
            rotationWeight = Mathf.LerpUnclamped(rotationWeight, data.rotationWeight, weight);
            hintWeight = Mathf.LerpUnclamped(hintWeight, data.hintWeight, weight);
            position = Vector3.LerpUnclamped(position, data.position, weight);
            hintPosition = Vector3.LerpUnclamped(hintPosition, data.hintPosition, weight);
            rotation = Quaternion.SlerpUnclamped(rotation, data.rotation, weight);
        }

        public void Add(IKLimbData data, float weight)
        {
            positionWeight += data.positionWeight * weight;
            rotationWeight += data.rotationWeight * weight;
            hintWeight += data.hintWeight * weight;
            position += data.position * weight;
            hintPosition += data.hintPosition * weight;
            rotation *= Quaternion.SlerpUnclamped(Quaternion.identity, data.rotation, weight);
        }
    }
}