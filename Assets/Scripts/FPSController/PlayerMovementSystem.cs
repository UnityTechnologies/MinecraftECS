 using UnityEngine;
 using Unity.Mathematics;
 using Unity.Entities;
 using Unity.Transforms;
 using Unity.Collections;
 using Unity.Physics;

//[UpdateBefore(typeof(LateSimulationSystemGroup))]
partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //float dt = SystemAPI.Time.DeltaTime;
        float3 inputs = default(float3);
        const float speed = 3f;
        Vector3 cameraPos = new Vector3(0,1,0);
        //Vector3 cameraFrd = new Vector3(0,0,0);
        float2 PlayerPos = new float2(0,0);
        Quaternion currentQuaternion;
        
        inputs = new float3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));

        //Get Camera GO Component & position/Rotation
        var cameraTransform = CameraLink.Instance.transform;
        //cameraFrd = cameraTransform.forward;
        
        //Sync Y Rotation
        var cameraRotation = cameraTransform.rotation.eulerAngles;
        currentQuaternion = Quaternion.Euler(0, cameraRotation.y, 0);
        
        Entities
            .WithAll<PlayerEntity>()
            .ForEach( ( ref TransformAspect transform, ref PhysicsVelocity vel) =>
            {
                

                //Sync camera position & Rotation to player position
                cameraPos = transform.Position + new float3(0,1.5f,0);
                transform.Rotation = currentQuaternion;

                //Movement
                if(inputs.z != 0)
                {
                    PlayerPos = transform.Forward.xz * inputs.z * speed;
                }
                if(inputs.x != 0)
                {
                    PlayerPos += transform.Right.xz * inputs.x * speed;
                }
                //Jump check
                if(inputs.y > 0)
                {
                    vel.Linear.y = transform.Up.y * speed;
                }

                //push to Velocity if inputs = 0 than Velocity = 0;
                vel.Linear.xz = PlayerPos;


        })
        .Run();
        //.WithBurst().ScheduleParallel();

        //camera Position
        cameraTransform.position = cameraPos;

        
    }
        
}
