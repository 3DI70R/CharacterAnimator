using UnityEngine;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface IAnimationControllerNode : INode
    {
        RuntimeAnimatorController Controller { get; }

        ControllerValue<bool> GetBoolParam(string name);
        ControllerValue<int> GetIntParam(string name);
        ControllerValue<float> GetFloatParam(string name);
        ControllerValue<bool> GetTriggerValue(string name);

        // TODO: shitton of animator methods
    }
}