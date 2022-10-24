using Unity.Burst;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Mathematics;
    //using Unity.Rendering;
    using Unity.Transforms;
    using Unity.Jobs;
    //using UnityEngine.UI;

    [BurstCompile]
    partial struct BlockGenerator : IJobEntity
    {
        //public EntityCommandBuffer ECB; //IEntityJob
        public EntityCommandBuffer.ParallelWriter ECB; //IJobEntity + Parallel
        public int2 mapSize2D;

        public Entity blockPrefab;

        //int blockPrefab;
        //int matNumber;
        float m_mat;
        int ranDice;
        bool spawnFlag;
        
        //Execute with BlockType Aspect
        [BurstCompile]
        void Execute([ChunkIndexInQuery] int index, in BlockTypeAspect BlockType)
        {

            //defaultPrefab = BlockType.defaultPrefab;
            //float3 posTemp = new float3(0,0,0);

            //spawn 2d map
            for (int y = 0; y < mapSize2D.x * mapSize2D.y; y++)
            {
                //每一個x,y座標都有往下15個方塊
                for(int i=0; i<15; i++)
                {
                    spawnFlag = true;
                    bool b_rotate = false;
                    //處理地表上的磚塊
                    if(i==0)
                    {
                        spawnFlag = false;

                        //如果高度是0, 代表這是地表上，亂數隨機產生(沒方塊、草、花、樹木、雲)  
                        var Random = Unity.Mathematics.Random.CreateFromIndex((uint)y);
                        ranDice = Random.NextInt(1,201);
    
                        if (ranDice <= 20)
                        {
                            //草 88-95
                            blockPrefab = BlockType.plantPrefab;
                            //matNumber = 3;
                            m_mat = Random.NextInt(88,95);
                            spawnFlag = true;
                            b_rotate = true;
                        }
                        if (ranDice == 198)
                        {
                            //雲
                            blockPrefab = BlockType.defaultPrefab;
                            //matNumber = 1;
                            m_mat = 66;
                            TreeNCloudGenerator(y,0,BlockType);
                        }
                        if (ranDice == 200)
                        {
                            //花
                            blockPrefab = BlockType.plantPrefab;
                            //matNumber = 3;
                            m_mat = Random.NextInt(12,13);
                            spawnFlag = true;
                            b_rotate = true;
                        }
                        if (ranDice == 199)
                        {
                            //樹
                            TreeNCloudGenerator(y,1,BlockType);
                        }
                    }
                    //處理草皮
                    else if(i == 1)
                    {
                            blockPrefab = BlockType.sixSidedPrefab;
                            m_mat = 1;
                            //matNumber = 0;
                    }
                    //地底方塊(i > 1)
                    else
                    {
                        blockPrefab = BlockType.defaultPrefab;
                        //matNumber = 1;

                        //switch materials that underground
                        switch (i)
                        {
                            //如果高度1,代表這是地表層，塞入地表方塊
                            //case 0:
                            //    m_mat = 0;

                                //break;
                                //如果高度為2,3,4塞入泥土方塊
                            case 2:
                            case 3:
                            case 4:
                                //Dirt
                                m_mat = 2f;
                                break;
                                //如果高度為5,6塞入石頭方塊
                            case 5:
                            case 6:
                                //stone block
                                m_mat = 1f;
                                break;
                                //如果高度是7,8塞入鵝軟石方塊
                            case 7:
                            case 8:
                                m_mat = 16f;
                                break;
                                //number 32 block
                            default:
                            m_mat = 32f;
                                break;
                        }
                    }
                    float3 posTemp = ComputeTransform(y, i);

                    if(posTemp.y > -16 && spawnFlag)
                    {
                        BlockSpawner(index, posTemp, b_rotate);
                    }
                }               
            }
        }
        
        [BurstCompile]
        public float3 ComputeTransform(int index, int i)
        {
            int y = index / mapSize2D.x;
            int x = index % mapSize2D.y;
            float2 posTemp2 = new float2(x - (float)mapSize2D.y * 0.5f, y - (float)mapSize2D.x * 0.5f);
            //Prelin noise for hight value
            int yValue = (int)(noise.cnoise(posTemp2/10) * 4) - 5 - i;
            float3 pos = new float3(x - (float)mapSize2D.y * 0.5f, yValue, y - (float)mapSize2D.x * 0.5f);
            
            return pos;
            //return float4x4.Translate(pos);
        }

        //樹木雲產生器
        [BurstCompile]
        public void TreeNCloudGenerator(int index,int plantType, in BlockTypeAspect BlockType)
        {
            //location
            float3 posTemp = new float3(0,0,0);
            float3 blockPos = ComputeTransform(index, 0);

            //cloud
            if(plantType == 0)
            {

                //從該座標往上Y加15,隨機產生4-7大小的雲塊，產生的Entity指定模型材質
                var Random = Unity.Mathematics.Random.CreateFromIndex((uint)index);
                int ranBlock = Random.NextInt(4,7);
                
                for (int i = 0; i < ranBlock; i++)
                {
                    for (int j = 0; j < ranBlock; j++)
                    {
                        posTemp = new float3((int)blockPos.x+i, (int)blockPos.y + 15, (int)blockPos.z+j);
                        BlockSpawner(index, posTemp, false);
                    }
                }
            }
            else if(plantType == 1)
            {
                int3 blockTemp = new int3((int3)blockPos);
                //樹木，把當前座標xpos,ypos,zpos作為樹根，往上長其他樹幹和樹葉
                for (int i = 0; i < 7; i++)
                {
                    //高度到頂要放樹葉
                    if (i == 6)
                    {
                        //leaves
                        blockPrefab = BlockType.defaultAlphaPrefab;
                        //matNumber = 2;
                        m_mat = 53f;
                    }
                    else
                    {
                        //wood
                        blockPrefab = BlockType.sixSidedPrefab;
                        //matNumber = 0;
                        m_mat = 0.67f;
                    }

                    posTemp = new float3((int)blockPos.x, (int)blockPos.y + i, (int)blockPos.z);
                    BlockSpawner(index, posTemp, false);

                    //如果高度在3-6之間要額外種樹葉
                    if(i >= 3 && i <= 6)
                    {
                        for (int j = blockTemp.x - 1; j <= blockTemp.x + 1; j++)
                        {
                            for (int k = blockTemp.z - 1; k <= blockTemp.z + 1; k++)
                            {
                                if (k != blockTemp.z || j != blockTemp.x)
                                {
                                    //leaves
                                    blockPrefab = BlockType.defaultAlphaPrefab;
                                    //matNumber = 2;
                                    m_mat = 52f;

                                    posTemp = new float3((int)j, blockPos.y+(int)i, (int)k);
                                    BlockSpawner(index, posTemp, false);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        [BurstCompile]
        public void BlockSpawner(int index, float3 blockPos, bool m_rotate)
        {
            //defaultPrefab = BlockType.defaultPrefab;

            float3 r;
            //rotate 45c for plants
            if(m_rotate)
            {
                r = new float3(0, 45, 0);
            }
            else
            {
                r = new float3(0,0,0);
            }
            
            quaternion R;
            //float3 s = new float3(1, 1, 1);
            R = quaternion.EulerXYZ(r);

            var e = ECB.Instantiate(index, blockPrefab);

            //ECB.SetComponent(index, e, MaterialMeshInfo.FromRenderMeshArrayIndices(matNumber, meshNumber));
            //ECB.SetComponent(index, e, new LocalToWorld {Value = float4x4.Translate(blockPos)});
            //ECB.SetComponent(index, e, new LocalToWorld {Value = float4x4.TRS(blockPos,R,s)});
            ECB.SetComponent(index, e, new Translation {Value = blockPos});
            ECB.SetComponent(index, e, new Rotation {Value = R});
            
            //設定Shader Graph對應材質編號
            ECB.SetComponent(index, e, new BlockID {blockID = m_mat});
                    
        }
    }

    [BurstCompile]
    public partial struct BlockSpawningSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state){}
        [BurstCompile]
        public void OnDestroy(ref SystemState state){}
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<GameSettings>();
            int2 mapSize2D = new int2(config.chunkSize*10,config.chunkSize*10);
            
            //宣告陣列和commandBuffer
            //方塊數量等於chunk x chunk x 1500
            int totalamount = (config.chunkSize * config.chunkSize) * 15;

            var blocks = CollectionHelper.CreateNativeArray<Entity>(totalamount, Allocator.Temp);
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            //把參數傳到Job System
            var blocksJob = new BlockGenerator
            {
                //ECB = ecb,
                ECB = ecb.AsParallelWriter(),
                mapSize2D = mapSize2D
            };

            // Schedule execution in a single thread, and do not block main thread.
            //blocksJob.Schedule(); //IJobEntity
            blocksJob.ScheduleParallel(); //IJobEntity + Parallel

        state.Enabled = false;
        }
    }
