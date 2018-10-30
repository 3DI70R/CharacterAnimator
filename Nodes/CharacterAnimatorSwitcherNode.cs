using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class SwitcherNode : CollectionNode<SwitcherNode.SwitcherChild>, ISwitcherNode
        {
            public class SwitcherChild : CollectionChild
            {
                public SwitcherChild(CollectionNode<SwitcherChild> parent, NodeBase node) : base(parent, node)
                {
                    
                }
            }

            public SwitcherNode(PlayableGraph graph, GameObject owner) : base(graph, owner)
            {
            }

            protected override SwitcherChild CreateContainer(NodeBase node)
            {
                return new SwitcherChild(this, node);
            }

            public ISwitchable<IMixerNode> AddMixer()
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<ILayerMixerNode> AddLayerMixer()
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<ISwitcherNode> AddSwitcher()
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<IAnimationClipNode> AddAnimationClip(AnimationClip c)
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<IAnimationControllerNode> AddController(RuntimeAnimatorController controller)
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<ITimelineNode> AddTimelineTrack(IEnumerable<TrackAsset> tracks)
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<IAnimationIKNode> AddIK()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}