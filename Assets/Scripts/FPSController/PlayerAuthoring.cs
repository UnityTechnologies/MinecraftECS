using Unity.Entities;

    // An empty component is called a "tag component".
    struct PlayerEntity : IComponentData
    {
    }

    // Authoring MonoBehaviours are regular GameObject components.
    // They constitute the inputs for the baking systems which generates ECS data.
    class PlayerAuthoring : UnityEngine.MonoBehaviour
    {
    }

    // Bakers convert authoring MonoBehaviours into entities and components.
    class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            AddComponent<PlayerEntity>();
        }
    }