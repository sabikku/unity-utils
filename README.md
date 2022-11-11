# unity-utils
Some personal utils I can share publicly.

## Easing.cs

Simple, popular easing functions (probably inspired by easings.net). The easing type works as an enum, so it's easy to serialise it and test fast in Unity's inspector.

## Enumerators.cs

Enumerators to fulfil common needs while iterating through collections: looping, picking random (but not previously picked) elements.

## CopyComponents.cs

Copying components from one GameObject to another through the use of reflection.

## RenderersBounds.cs

Calculating the sum of gameObject hierarchy tree renderers bounds.

## TimeSpanLogger.cs & GCChangesLogger.cs

Simple logging utilities to measure time and GC usage differences in Unity.
