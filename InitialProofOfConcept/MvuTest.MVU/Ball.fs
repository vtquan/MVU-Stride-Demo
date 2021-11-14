

namespace MvuTest.MVU

module Ball = 
    open Stride.Core.Collections
    open Stride.Core.Mathematics
    open Stride.Engine
    open Stride.Games;
    open Stride.Physics
    open System.Linq
    open Message

    type Model =
        { Velocity : Vector3; Direction : Vector3; Jumped : bool; Counter : int; Entities : TrackingCollection<Entity> }
        
    let empty () =
        { Velocity = new Vector3(0f, 0.f, 0f); Direction = new Vector3(0f, 0.f, 0f); Jumped = false; Counter = 0; Entities = new TrackingCollection<Entity>() }
        
    let init (sceneSystem : SceneSystem) =
        { Velocity = new Vector3(0f, 0f, 0f); Direction = new Vector3(0f, 0.f, 0f); Jumped = false; Counter = 0; Entities = sceneSystem.SceneInstance.RootScene.Entities }, Empty

    //let update msg model =
    //    match msg with
    //    | Freeze -> 
    //        model, BallMsg(Freeze)
            
    //    | Collision when model.Counter >= 5 -> 
    //        { model with Velocity = new Vector3(0f, 0f, 0f) }, BallMsg(Freeze)

    //    | Collision -> 
    //        { model with Counter = model.Counter + 1 }, Empty

    //    | MoveLeft when model.Velocity.X > -30f -> 
    //        { model with Velocity = model.Velocity + new Vector3(-0.5f, 0f, -0.5f) }, BallMsg(MoveLeft)

    //    | MoveLeft -> 
    //        { model with Velocity = model.Velocity + new Vector3(-0.5f, 0f, -0.5f) }, BallMsg(MoveRight) 
            
    //    | MoveRight when model.Velocity.X < 30f -> 
    //        { model with Velocity = model.Velocity + new Vector3(0.5f, 0f, 0.5f) }, BallMsg(MoveRight)
            
    //    | MoveRight -> 
    //        { model with Velocity = model.Velocity + new Vector3(0.5f, 0f, 0.5f) }, BallMsg(MoveLeft)
            
    //let update msg model =
    //    match msg with
    //    | Collision -> 
    //        { model with Counter = model.Counter + 1 }, Empty

    //    | MoveLeft when model.Velocity.X < -10f -> 
    //        model, Empty
            
    //    | MoveLeft -> 
    //        { model with Direction = model.Direction + new Vector3(-1f, 0f, 0f) }, BallMsg(NewVelocity)
                        
    //    | MoveRight when model.Velocity.X > 10f -> 
    //        model, Empty
                        
    //    | MoveRight -> 
    //        { model with Direction = model.Direction + new Vector3(1f, 0f, 0f) }, BallMsg(NewVelocity)
            
    //    | MoveUp when model.Velocity.Z < -10f -> 
    //        model, Empty
            
    //    | MoveUp -> 
    //        { model with Direction = model.Direction + new Vector3(0f, 0f, -1f) }, BallMsg(NewVelocity)
            
    //    | MoveDown when model.Velocity.Z > 10f -> 
    //        model, Empty
            
    //    | MoveDown -> 
    //        { model with Direction = model.Direction + new Vector3(0f, 0f, 1f) }, BallMsg(NewVelocity)
            
    //    | NewVelocity -> 
    //        { model with Velocity = ((model.Velocity*0.90f + model.Direction * 0.10f)); Direction = new Vector3(0f, 0f, 0f) }, Empty
            
    //    | NoMovement -> 
    //        { model with Direction = new Vector3(0f, 0f, 0f) }, BallMsg(NewVelocity)

    let private velocityLimit = 1f;
    let private speed = 30f;

    // Cap x and z movement speed
    let capHorizontalVelocity (velocity : Vector3) speedLimit =
        let horizontalVelocity = new Vector3(velocity.X, 0f, velocity.Z)
        match horizontalVelocity.Length() > speedLimit with
        | true -> 
            let cappedHorizontalVelocity = Vector3.Normalize(velocity)
            let cappedVelocity = new Vector3(cappedHorizontalVelocity.X, velocity.Y, cappedHorizontalVelocity.Z)
            cappedVelocity
        | false -> velocity
    
    let update msg model =
        match msg with
        | Collision -> 
            { model with Counter = model.Counter + 1 }, Empty

        | MoveLeft when model.Velocity.X < -10f -> 
            model, Empty
            
        | MoveLeft -> 
            { model with Direction = model.Direction + new Vector3(-1f, 0f, 0f) }, BallMsg(NewVelocity)
                        
        | MoveRight when model.Velocity.X > 10f -> 
            model, Empty
                        
        | MoveRight -> 
            { model with Direction = model.Direction + new Vector3(1f, 0f, 0f) }, BallMsg(NewVelocity)
            
        | MoveUp when model.Velocity.Z < -10f -> 
            model, Empty
            
        | MoveUp -> 
            { model with Direction = model.Direction + new Vector3(0f, 0f, -1f) }, BallMsg(NewVelocity)
            
        | MoveDown when model.Velocity.Z > 10f -> 
            model, Empty
            
        | MoveDown -> 
            { model with Direction = model.Direction + new Vector3(0f, 0f, 1f) }, BallMsg(NewVelocity)

        | Jump when model.Jumped = false -> 
            let w = (Vector3.Normalize(model.Direction))
            let nv = new Vector3(w.X, 3f, w.Z)
            { model with Velocity = new Vector3(model.Velocity.X, 1.3f, model.Velocity.Z); Jumped = true }, Empty
            
        | Jump -> 
            model, BallMsg(NewVelocity)
            
        | NewVelocity when model.Jumped = true -> 
            { model with Velocity = new Vector3(model.Velocity.X, model.Velocity.Y - 0.03f, model.Velocity.Z) + model.Direction * 0.02f; Direction = new Vector3(0f, 0f, 0f) }, Empty   // Reduce player control while in the air
            
        | NewVelocity -> 
            { model with Velocity = model.Velocity*0.90f + model.Direction * 0.10f; Direction = new Vector3(0f, 0f, 0f) }, Empty
            
        | Grounded when model.Jumped = true -> 
            { model with Jumped = false }, Empty
            
        | Grounded -> 
            model, Empty
            
        | NoMovement -> 
            { model with Direction = new Vector3(0f, 0f, 0f) }, BallMsg(NewVelocity)
            
    let view model =
        //model.Entities.Item(4).Transform.UseTRS <- false
        //model.Entities.Item(4).Transform.Position <- model.Position
        let characterEntity = model.Entities.First(fun x -> x.Name = "Sphere")
        let characterComponent = characterEntity.Get<CharacterComponent>()
        //characterComponent.SetVelocity(model.Velocity )
        characterComponent.SetVelocity( (capHorizontalVelocity model.Velocity velocityLimit)  * speed)