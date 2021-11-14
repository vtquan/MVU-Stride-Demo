namespace MyGame.MvuGame

open Stride.Core.Mathematics
open Stride.Engine;
open Stride.Games;
open System
open System.Linq
open Stride.Engine.Events

module Game =
    type GameModel =
        {
            Velocity : Vector3
            Sphere : Entity
        }

    type GameMsg =
        | Up
        | Down
        | Left
        | Right
        | Stop
        
    let empty () =
        { Velocity = Vector3.Zero; Sphere = new Entity () }, []
        
    let init (scene : Scene) =
        let sphere = scene.Entities.FirstOrDefault(fun x -> x.Name = "Sphere")     
        { Velocity = Vector3.Zero; Sphere = sphere }, []
    
    let view model (gameTime : GameTime) =
        model.Sphere.Transform.Position <- model.Sphere.Transform.Position + model.Velocity * (float32 gameTime.Elapsed.TotalSeconds)
    
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

    let private ProcessGameEvent ((message, entity) : string * Entity) : GameMsg list = 
        match message with
        | "Left" -> [Left]
        | "Right" -> [Right]
        | "Up" -> [Up]
        | "Down" -> [Down]
        | "Stop" -> [Stop]
        | _ -> []
        
    let private TryReceiveAllEvent (eventReceiver : EventReceiver<'a>) =   
        let events = (Seq.empty).ToList()
        let numEvent = eventReceiver.TryReceiveAll(events)
        numEvent, events

    let ProcessAllGameEvent (eventReceiver : EventReceiver<string * Entity>) : GameMsg seq =
        let numEvent, events = TryReceiveAllEvent eventReceiver
        match numEvent with
        | 0 -> Seq.empty
        | _ ->
            let msgSeq =
                seq {
                    for e in events do
                        yield! ProcessGameEvent e
                }
            msgSeq
    
    let ProcessAllEvent () : GameMsg list=        
        let msgSeq =
            seq {
                yield! ProcessAllGameEvent MyGame.Events.gameListener  //Can be expanded with additional yield! process listener
            }

        List.ofSeq (Seq.distinct msgSeq)

    type MvuGame() =
        inherit Game()

        let mutable State, Messages = empty ()
        let mutable NextMessages : GameMsg list = []
        
        override this.BeginRun () = 
            let mainScene = this.Content.Load<Scene>("MainScene")
            let state, messages = init mainScene
            State <- state
            Messages <- messages

        member private this.GameUpdate (cmds : GameMsg list) (state : GameModel) (gameTime : GameTime) =
            let GameUpdateFold ((state, msgs) : GameModel * GameMsg list) cmd  = 
                let (model,messages) = update cmd state (float32 gameTime.Elapsed.TotalSeconds)
                model, msgs @ messages

            let newState, nextMessages = List.fold GameUpdateFold (State, []) cmds
            State <- newState
            NextMessages <- NextMessages @ (List.distinct nextMessages)

        
        override this.Update gameTime =
            base.Update(gameTime);
            
            Messages <- Messages @ ProcessAllEvent ()

            this.GameUpdate Messages State gameTime
            view State gameTime |> ignore

            Messages <- NextMessages
            NextMessages <- []

        override this.Destroy () =
            base.Destroy()