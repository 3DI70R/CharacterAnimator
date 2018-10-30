using System;
using UnityEngine;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class AnimationIKNode : NodeBase, IAnimationIKNode
        {
            private static readonly StreamType[] ikOutputTypes =
            {
                StreamType.Animation
            };

            private class IKLook : IIKLook
            {
                public AnimationIKNode node;
                public IKLookJobData jobData;

                public IKLook(AnimationIKNode node)
                {
                    this.node = node;
                }

                public float EyesWeight
                {
                    get { return jobData.eyesWeight; }
                    set
                    {
                        jobData.eyesWeight = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public float HeadWeight
                {
                    get { return jobData.headWeight; }
                    set
                    {
                        jobData.headWeight = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public float BodyWeight
                {
                    get { return jobData.bodyWeight; }
                    set
                    {
                        jobData.bodyWeight = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public float WeightClamp
                {
                    get { return jobData.weightClamp; }
                    set
                    {
                        jobData.weightClamp = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }
                public Vector3 Position
                {
                    get { return jobData.position; }
                    set
                    {
                        jobData.position = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }
            }

            private class IKLimb : IIKLimb
            {
                public AnimationIKNode node;
                public IKLimbJobData jobData;

                public IKLimb(AnimationIKNode node)
                {
                    this.node = node;
                }

                public float PositionWeight
                {
                    get { return jobData.positionWeight; }
                    set
                    {
                        jobData.positionWeight = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public float RotationWeight
                {
                    get { return jobData.rotationWeight; }
                    set
                    {
                        jobData.rotationWeight = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }
                public float HintWeight 
                { 
                    get { return jobData.hintWeight; }
                    set
                    {
                        jobData.hintWeight = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public Vector3 Position
                {
                    get { return jobData.position; }
                    set
                    {
                        jobData.position = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public Quaternion Rotation
                {
                    get { return jobData.rotation; }
                    set
                    {
                        jobData.rotation = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }

                public Vector3 HintPosition
                {
                    get { return jobData.hintPosition; }
                    set
                    {
                        jobData.hintPosition = value;
                        node.UpdateAnimationJobData(); // TODO: TEMP
                    }
                }
            }
            
            [Serializable]
            private struct IKLimbJobData
            {
                public float positionWeight;
                public float rotationWeight;
                public float hintWeight;

                public Vector3 position;
                public Quaternion rotation;
                public Vector3 hintPosition;
            }

            [Serializable]
            private struct IKLookJobData
            {
                public float eyesWeight;
                public float headWeight;
                public float bodyWeight;
                public float weightClamp;

                public Vector3 position;
            }
            
            [Serializable]
            private struct IKAnimationJob : IAnimationJob
            {
                public IKLimbJobData leftHand;
                public IKLimbJobData rightHand;
                public IKLimbJobData leftFoot;
                public IKLimbJobData rightFoot;
                public IKLookJobData look;
        
                public void ProcessAnimation(AnimationStream stream)
                {
                    if (stream.isHumanStream)
                    {
                        var human = stream.AsHuman();

                        ApplyLimb(human, leftHand, AvatarIKGoal.LeftHand, AvatarIKHint.LeftElbow);
                        ApplyLimb(human, rightHand, AvatarIKGoal.RightHand, AvatarIKHint.RightElbow);
                        ApplyLimb(human, leftFoot, AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee);
                        ApplyLimb(human, rightFoot, AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee);
                        
                        human.SetLookAtPosition(look.position);
                        human.SetLookAtEyesWeight(look.eyesWeight);
                        human.SetLookAtHeadWeight(look.headWeight);
                        human.SetLookAtBodyWeight(look.bodyWeight);
                        human.SetLookAtClampWeight(look.weightClamp);
                        
                        human.SolveIK();
                    }
                }

                private void ApplyLimb(AnimationHumanStream human, IKLimbJobData l, AvatarIKGoal goal, AvatarIKHint hint)
                {
                    human.SetGoalWeightPosition(goal, l.positionWeight);
                    human.SetGoalWeightRotation(goal, l.rotationWeight);
                    human.SetHintWeightPosition(hint, l.hintWeight);
                            
                    if (l.positionWeight > 0)
                    {
                        human.SetGoalPosition(goal, l.position);
                    }

                    if (l.rotationWeight > 0)
                    {
                        human.SetGoalRotation(goal, l.rotation);
                    }

                    if (l.hintWeight > 0)
                    {
                        human.SetHintPosition(hint, l.hintPosition);
                    }
                }

                public void ProcessRootMotion(AnimationStream stream) { }
            }

            private AnimationScriptPlayable animationScriptPlayable;
            private readonly IKLimb leftHand;
            private readonly IKLimb rightHand;
            private readonly IKLimb leftFoot;
            private readonly IKLimb rightFoot;
            private readonly IKLook look;

            public override Playable Playable => animationScriptPlayable;
            public override StreamType[] OutputTypes => ikOutputTypes;

            public IIKLimb LeftHand => leftHand;
            public IIKLimb RightHand => rightHand;
            public IIKLimb LeftFoot => leftFoot;
            public IIKLimb RightFoot => rightFoot;
            public IIKLook Look => look;

            public AnimationIKNode(PlayableGraph graph)
            {
                leftHand = new IKLimb(this);
                rightHand = new IKLimb(this);
                leftFoot = new IKLimb(this);
                rightFoot = new IKLimb(this);
                look = new IKLook(this);
                
                animationScriptPlayable = AnimationScriptPlayable.Create(graph, new IKAnimationJob());
                animationScriptPlayable.SetProcessInputs(false);
            }

            private void UpdateAnimationJobData()
            {
                var job = animationScriptPlayable.GetJobData<IKAnimationJob>();

                job.leftHand = leftHand.jobData;
                job.rightHand = rightHand.jobData;
                job.leftFoot = leftFoot.jobData;
                job.rightFoot = rightFoot.jobData;
                job.look = look.jobData;
                
                animationScriptPlayable.SetJobData(job);
            }
        }
    }
}