    using Unity.Entities;

    // An empty component is called a "tag component".
    public struct BlockType : IComponentData
    {
        public Entity sixSidedPrefab;
        public Entity defaultPrefab;
        public Entity defaultAlphaPrefab;
        public Entity plantPrefab;
    }