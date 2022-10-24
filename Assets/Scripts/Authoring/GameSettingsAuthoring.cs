using System;
using Unity.Entities;
using UnityEngine;

    class GameSettingsAuthoring : MonoBehaviour
    {
        public int chunkSize;
    }

    class GameSettingsBaker : Baker<GameSettingsAuthoring>
    {
        public override void Bake(GameSettingsAuthoring authoring)
        {
            AddComponent(new GameSettings
            {
                chunkSize = authoring.chunkSize
            });
        }
    }