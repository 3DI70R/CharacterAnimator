using UnityEngine;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class AnimationIKNode : NodeBase, IAnimationIKNode
        {
            private static readonly StreamType[] ikOutputTypes =
            {
                StreamType.IK
            };

            private class IKLook : IIKLook
            {
                private IKLookData data;

                public IKLook(IKLookData node)
                {
                    data = node;
                }

                public float EyesWeight
                {
                    get { return data.jobData.eyesWeight; }
                    set { data.jobData.eyesWeight = value; }
                }

                public float HeadWeight
                {
                    get { return data.jobData.headWeight; }
                    set { data.jobData.headWeight = value; }
                }

                public float BodyWeight
                {
                    get { return data.jobData.bodyWeight; }
                    set { data.jobData.bodyWeight = value; }
                }

                public float WeightClamp
                {
                    get { return data.jobData.weightClamp; }
                    set { data.jobData.weightClamp = value; }
                }
                public Vector3 Position
                {
                    get { return data.jobData.position; }
                    set { data.jobData.position = value; }
                }
            }

            private class IKLimb : IIKLimb
            {
                private IKLimbData data;

                public IKLimb(IKLimbData data)
                {
                    this.data = data;
                }

                public float PositionWeight
                {
                    get { return data.jobData.positionWeight; }
                    set { data.jobData.positionWeight = value; }
                }

                public float RotationWeight
                {
                    get { return data.jobData.rotationWeight; }
                    set {  data.jobData.rotationWeight = value; }
                }
                public float HintWeight 
                { 
                    get { return data.jobData.hintWeight; }
                    set { data.jobData.hintWeight = value; }
                }

                public Vector3 Position
                {
                    get { return data.jobData.position; }
                    set { data.jobData.position = value; }
                }

                public Quaternion Rotation
                {
                    get { return data.jobData.rotation; }
                    set { data.jobData.rotation = value; }
                }

                public Vector3 HintPosition
                {
                    get { return data.jobData.hintPosition; }
                    set { data.jobData.hintPosition = value; }
                }
            }

            private IKAnimationBehavior ikBehavior;
            private ScriptPlayable<IKAnimationBehavior> ikPlayable;
            private readonly IKLimb leftHand;
            private readonly IKLimb rightHand;
            private readonly IKLimb leftFoot;
            private readonly IKLimb rightFoot;
            private readonly IKLook look;

            public override Playable Playable => ikPlayable;
            public override StreamType[] OutputTypes => ikOutputTypes;

            public IIKLimb LeftHand => leftHand;
            public IIKLimb RightHand => rightHand;
            public IIKLimb LeftFoot => leftFoot;
            public IIKLimb RightFoot => rightFoot;
            public IIKLook Look => look;

            public AnimationIKNode(PlayableGraph graph)
            {
                ikPlayable = ScriptPlayable<IKAnimationBehavior>.Create(graph);
                ikBehavior = ikPlayable.GetBehaviour();
                
                leftHand = new IKLimb(ikBehavior.leftHand);
                rightHand = new IKLimb(ikBehavior.rightHand);
                leftFoot = new IKLimb(ikBehavior.leftFoot);
                rightFoot = new IKLimb(ikBehavior.rightFoot);
                look = new IKLook(ikBehavior.look);
            }
        }
    }
}