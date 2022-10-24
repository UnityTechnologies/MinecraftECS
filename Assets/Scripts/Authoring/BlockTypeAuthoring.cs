using Unity.Entities;

    // Authoring MonoBehaviours are regular GameObject components.
    // They constitute the inputs for the baking systems which generates ECS data.
    public class BlockTypeAuthoring : UnityEngine.MonoBehaviour
    {
        public UnityEngine.GameObject sixSidedPrefab;
        public UnityEngine.GameObject defaultPrefab;
        public UnityEngine.GameObject defaultAlphaPrefab;
        public UnityEngine.GameObject plantPrefab;
    }

    // Bakers convert authoring MonoBehaviours into entities and components.
    public class BlockTypeBaker : Baker<BlockTypeAuthoring>
    {
        public override void Bake(BlockTypeAuthoring authoring)
        //static void Bake(BlockTypeAuthoring authoring)
        {
            AddComponent(new BlockType
            {
                sixSidedPrefab = GetEntity(authoring.sixSidedPrefab),
                defaultPrefab = GetEntity(authoring.defaultPrefab),
                defaultAlphaPrefab = GetEntity(authoring.defaultAlphaPrefab),
                plantPrefab = GetEntity(authoring.plantPrefab)
            });
        }
    }