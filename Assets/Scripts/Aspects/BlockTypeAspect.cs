using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Rendering;

    // Instead of directly accessing the BlockType component, we are creating an aspect.
    // Aspects allows you to provide a customized API for accessing your components.
    readonly partial struct BlockTypeAspect : IAspect
    {
        // This reference provides read only access to the BlockType component.
        // Trying to use ValueRW (instead of ValueRO) on a read-only reference is an error.
        readonly RefRO<BlockType> m_BlockType;

        // Note the use of ValueRO in the following properties.
        public Entity sixSidedPrefab => m_BlockType.ValueRO.sixSidedPrefab;
        public Entity defaultPrefab => m_BlockType.ValueRO.defaultPrefab;
        public Entity defaultAlphaPrefab => m_BlockType.ValueRO.defaultAlphaPrefab;
        public Entity plantPrefab => m_BlockType.ValueRO.plantPrefab;
    }

    readonly partial struct GameSettingsAspect : IAspect
    {
        // This reference provides read only access to the BlockType component.
        // Trying to use ValueRW (instead of ValueRO) on a read-only reference is an error.
        readonly RefRO<GameSettings> m_GameSettings;

        // Note the use of ValueRO in the following properties.
        public int chunkSize => m_GameSettings.ValueRO.chunkSize;
    }