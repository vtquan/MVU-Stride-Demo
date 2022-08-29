namespace MyGame.Core

open Stride.Engine
open Stride.Engine.Events
open Stride.Games
open System.Linq

module Game =  
    type Model =
        {
            PlayerModel : Player.Model
        }

    type Msg =
        | PlayerMsg of Player.Msg

    let empty =
        { PlayerModel = Player.empty }
        
    let init (scene : Scene) : Model * Msg list =
        let playerModel, playerMsgs = Player.init scene
        let gameMsgs =
            [
                yield! List.map (PlayerMsg) playerMsgs
            ]
            |> List.distinct
        { empty with PlayerModel = playerModel }, gameMsgs

    let private mapEvent (eventReceiver : EventReceiver<'a>) (eventMap : 'a -> 'b list) (listMap : 'b list -> Msg list) =   
        let eventList = (Seq.empty).ToList()
        let numEvent = eventReceiver.TryReceiveAll(eventList)
        let events = Seq.toList eventList
        let messages =
            [
                for e in events do
                    yield! listMap (eventMap e)
            ]
        messages

    let mapAllEvent () : Msg list =
        let messages =
            [
                yield! mapEvent MyGame.Events.PlayerEventListener Player.map (List.map PlayerMsg)
            ] |> List.distinct
        messages

    let view (state : Model) (gameTime : GameTime) =
        let deltaTime = float32 gameTime.Elapsed.TotalSeconds

        Player.view state.PlayerModel deltaTime

    let update (state : Model) (cmds : Msg list) (gameTime : GameTime) =
        let deltaTime = float32 gameTime.Elapsed.TotalSeconds

        let updateFold ((state, msgs) : Model * Msg list) cmd  = 
            match cmd with
            | PlayerMsg(m) ->
                let (model,msg) = Player.update m state.PlayerModel deltaTime
                { state with PlayerModel = model }, msgs @ msg

        let newState, nextMessages = List.fold updateFold (state, []) cmds
        newState , List.distinct nextMessages