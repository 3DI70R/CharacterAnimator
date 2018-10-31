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
                private IKLookData jobData;

                public IKLook(IKLookData node)
                {
                    jobData = node;
                }

                public float EyesWeight
                {
                    get { return jobData.eyesWeight; }
                    set { jobData.eyesWeight = value; }
                }

                public float HeadWeight
                {
                    get { return jobData.headWeight; }
                    set { jobData.headWeight = value; }
                }

                public float BodyWeight
                {
                    get { return jobData.bodyWeight; }
                    set { jobData.bodyWeight = value; }
                }

                public float WeightClamp
                {
                    get { return jobData.weightClamp; }
                    set { jobData.weightClamp = value; }
                }
                public Vector3 Position
                {
                    get { return jobData.position; }
                    set { jobData.position = value; }
                }
            }

            private class IKLimb : IIKLimb
            {
                private IKLimbData jobData;

                public IKLimb(IKLimbData data)
                {
                    jobData = data;
                }

                public float PositionWeight
                {
                    get { return jobData.positionWeight; }
                    set { jobData.positionWeight = value; }
                }

                public float RotationWeight
                {
                    get { return jobData.rotationWeight; }
                    set {  jobData.rotationWeight = value; }
                }
                public float HintWeight 
                { 
                    get { return jobData.hintWeight; }
                    set { jobData.hintWeight = value; }
                }

                public Vector3 Position
                {
                    get { return jobData.position; }
                    set { jobData.position = value; }
                }

                public Quaternion Rotation
                {
                    get { return jobData.rotation; }
                    set { jobData.rotation = value; }
                }

                public Vector3 HintPosition
                {
                    get { return jobData.hintPosition; }
                    set { jobData.hintPosition = value; }
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