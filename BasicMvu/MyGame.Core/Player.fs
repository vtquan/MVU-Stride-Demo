namespace MyGame.Core

open Stride.Core.Mathematics
open Stride.Engine;
open System.Linq

module Player =  
    type Model =
        {
            Velocity : Vector3
            Sphere : Entity
        }

    type Msg =
        | Up
        | Down
        | Left
        | Right
        | Stop

    let map message : Msg list = 
        match message with
        | "Left" -> [Left]
        | "Right" -> [Right]
        | "Up" -> [Up]
        | "Down" -> [Down]
        | "Stop" -> [Stop]
        | _ -> []
        
    let empty =
        { Velocity = Vector3.Zero; Sphere = new Entity () }
        
    let init (scene : Scene) : Model * Msg list =
        let sphere = scene.Entities.FirstOrDefault(fun x -> x.Name = "Sphere")     
        { empty with Velocity = Vector3.Zero; Sphere = sphere }, []
    
    let view model (deltaTime : float32) =
        model.Sphere.Transform.Position <- model.Sphere.Transform.Position + model.Velocity * deltaTime
    
    let update msg model (deltaTime : float32) =
        match msg with        
        | Left -> 
            { model with Velocity = model.Velocity - Vector3.UnitX }, []                        
        | Right -> 
            { model with Velocity = model.Velocity + Vector3.UnitX }, []            
        | Up -> 
            { model with Velocity = model.Velocity - Vector3.UnitZ }, []            
        | Down -> 
            { model with Velocity = model.Velocity + Vector3.UnitZ }, []            
        | Stop -> 
            { model with Velocity = Vector3.Zero }, []