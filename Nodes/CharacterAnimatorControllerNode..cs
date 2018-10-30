using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class AnimationControllerNode : NodeBase, IAnimationControllerNode
        {
            private static readonly StreamType[] animatorControllerOutputs =
            {
                StreamType.Animation
            };

            private RuntimeAnimatorController animatorController;
            private AnimatorControllerPlayable controllerPlayable;

            public override Playable Playable
            {
                get { return controllerPlayable; }
            }

            public override StreamType[] OutputTypes
            {
                get { return animatorControllerOutputs; }
            }

            public RuntimeAnimatorController Controller
            {
                get { return animatorController; }
            }

            public ControllerValue<bool> GetBoolParam(string name)
            {
                return ControllerValue<bool>
                    .CreateBool(controllerPlayable, Animator.StringToHash(name));
            }

            public ControllerValue<int> GetIntParam(string name)
            {
                return ControllerValue<bool>
                    .CreateInt(controllerPlayable, Animator.StringToHash(name));
            }

            public ControllerValue<float> GetFloatParam(string name)
            {
                return ControllerValue<float>
                    .CreateFloat(controllerPlayable, Animator.StringToHash(name));
            }

            public ControllerValue<bool> GetTriggerValue(string name)
            {
                return ControllerValue<bool>
                    .CreateBool(controllerPlayable, Animator.StringToHash(name));
            }

            public AnimationControllerNode(PlayableGraph graph, RuntimeAnimatorController controller)
            {
                animatorController = controller;
                controllerPlayable = AnimatorControllerPlayable.Create(graph, controller);
            }
        }
    }
}