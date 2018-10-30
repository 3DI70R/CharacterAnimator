using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface IAnimationClipNode : INode
    {
        AnimationClip Clip { get; }
        bool FootIk { get; set; }
        bool PlayableIk { get; set; }
    }
}