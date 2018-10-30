using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThreeDISevenZeroR.CharacterAnimator
{
    public partial class CharacterAnimator
    {
        private class SwitcherNode : CollectionNode<SwitcherNode.SwitcherChild>, ISwitcher
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

            public ISwitchable<IMixer> AddMixer()
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<ILayerMixer> AddLayerMixer()
            {
                throw new System.NotImplementedException();
            }

            public ISwitchable<ISwitcher> AddSwitcher()
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

            public ISwitchable<IAnimationControllerNode> AddIK()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}