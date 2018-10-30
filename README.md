# CharacterAnimator
Animator specifically designed for character animation.

AnimatorController in Unity is nice for small objects, but player usually have tons of animations, and every animation should be configured in single AnimatorController.
That leads to very bloated god-controller which contain everything in one place, that cannot be modified at runtime.
It is also not suitable for configuring unique animations for specific environments/items, since most correct way for such situations, is to have character animation bound to used object, which you cannot do with AnimatorController.

This animator designed in such way, that user can create blending tree that suits his needs.

Features:
- Uses PlayableGraph and allows for blending between different states
- Ability to add/remove animation nodes in runtime
- Ability to use AnimatorController/Timeline as animation node

Still in concept phase, do not use (even if you can compile it).
