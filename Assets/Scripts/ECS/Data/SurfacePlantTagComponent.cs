using System;
using Unity.Entities;

//
[Serializable]
public struct SurfacePlantTag : IComponentData { }
public class SurfacePlantTagComponent : ComponentDataWrapper<SurfacePlantTag> { }