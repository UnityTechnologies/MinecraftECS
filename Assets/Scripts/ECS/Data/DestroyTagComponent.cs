using System;
using Unity.Entities;

//
[Serializable]
public struct DestroyTag : IComponentData { }
public class DestroyTagComponent : ComponentDataWrapper<DestroyTag> { }