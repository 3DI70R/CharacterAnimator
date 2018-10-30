namespace ThreeDISevenZeroR.CharacterAnimator
{
    public interface ICollectionNode : INode { }
    
    public interface IChild<out T> where T : INode
    {
        T Node { get; }
        void Remove();
    }
}