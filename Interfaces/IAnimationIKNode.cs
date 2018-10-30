using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface IIKLimb
    {
        float PositionWeight { get; set; }
        float RotationWeight { get; set; }
        float HintWeight { get; set; }
        
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        Vector3 HintPosition { get; set; }
    }

    public interface IIKLook
    {
        float EyesWeight { get; set; }
        float HeadWeight { get; set; }
        float BodyWeight { get; set; }
        float WeightClamp { get; set; }
        
        Vector3 Position { get; set; }
    }
    
    public interface IAnimationIKNode : INode
    {
        IIKLimb LeftHand { get; }
        IIKLimb RightHand { get; }
        IIKLimb LeftFoot { get; }
        IIKLimb RightFoot { get; }
        
        IIKLook Look { get; }
    }
}