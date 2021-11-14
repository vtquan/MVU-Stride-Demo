


namespace MvuTest.MVU

open Stride.Engine.Events
open System.Collections

module Game =
    open System;
    open System.Linq
    open Stride.Core.Mathematics
    open Stride.Core.Diagnostics
    open Stride.Engine;
    open Stride.Games;
    open Stride.Profiling
    open Elmish
    open Events
    open Message
    open Counter
    open Ball
    
    type CurrentScene =
        | Counter
        | RollingBall
    
    type GameModel =
        {
            id : int
            CounterModel : Counter.Model
            BallModel : Ball.Model
            CurrentScene : CurrentScene
        }

    type GameCompositionRoot() =
        inherit Game()

        let init () =
            { id = 0; CounterModel = Counter.init (); BallModel = Ball.empty (); CurrentScene = RollingBall}, []

        let view state =
            match state.CurrentScene with
            | RollingBall -> Ball.view state.BallModel
            | _ -> ()
            
        let mutable State, Messages = init ()
        let mutable NextMessages : GameMessage list = []
        
        override this.BeginRun () =      
            let (newModel,message) = Ball.init (this.SceneSystem)
            State <- { State with BallModel = newModel }
            Messages <- Messages @ [message]
            ()
        
        member this.SendMessage (msg : GameMessage) =
            NextMessages <- NextMessages @ [msg]
            
        member this.SendCollisionMessage () =
            NextMessages <- NextMessages @ [BallMsg(Collision)]

        member private this.GameUpdate (cmds : GameMessage list) (state : GameModel) (gameTime : GameTime) =
            let GameUpdateFold ((state, msgs) : GameModel * GameMessage list) cmd  = 
                match cmd with
                | CounterMsg(m) -> 
                    let newModel, newMsg = Counter.update m state.CounterModel
                    { state with CounterModel = newModel }, msgs @ [CounterMsg(newMsg)]
                | BallMsg(m) -> 
                    let newModel, newMsg = Ball.update m state.BallModel
                    { state with BallModel = newModel }, msgs @ [newMsg]
                | Start -> state, msgs
                | Stop -> state, msgs
                | Empty -> state, msgs

            let newState, nextMessages = List.fold GameUpdateFold (State, []) cmds
            State <- newState
            NextMessages <- NextMessages @ (List.filter(fun m -> match m with | Empty -> false | _ -> true ) (List.distinct nextMessages))

            //let GameUpdateIter cmd = 
            //    let update cmd = 
            //        match cmd with
            //        | CounterMsg(m) -> 
            //            let newModel, newMsg = Counter.update m state.CounterModel
            //            { state with CounterModel = newModel }, CounterMsg(newMsg)
            //        | BallMsg(m) -> 
            //            let newModel, newMsg = Ball.update m state.BallModel
            //            { state with BallModel = newModel }, newMsg
            //        | Start -> state, cmd
            //        | Stop -> state, cmd
            //    let (s,c) = update cmd
            //    State <- s
            //    match c with
            //    | Empty -> ()
            //    | _ -> NextMessages <- NextMessages @ [c]

            //List.iter GameUpdateIter cmds

        
        override this.Update gametime =
            base.Update(gametime);
            
            Messages <- Messages @ Helper.ProcessAllEvent ()
            this.GameUpdate Messages State gametime
            view State

            this.DebugTextSystem.Print(sprintf "%i\n\n%A\n\n%A\n\n%A" State.BallModel.Counter Messages State.BallModel.Velocity State.BallModel.Direction, new Int2(50,50))

            Messages <- NextMessages
            NextMessages <- []


        override this.Destroy () =
            ()