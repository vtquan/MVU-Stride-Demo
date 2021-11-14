

namespace MvuTest.MVU




module Helper =
    open Events
    open Message
    open Stride.Engine
    open Stride.Engine.Events
    open System.Linq

    
    //let private ProcessEvent (messageContent : string * Entity) =
    //    match fst messageContent with
    //    | "Collision" ->  BallMsg(Collision)
    //    | _ -> Empty
        
    //let ProcessAllEvent (events : EventReceiver<'a> list) =
    //    let TryReceiveEvent (event : EventReceiver<string * Entity>) =
    //        let c = ("", new Entity())
    //        let b = event.TryReceive(ref c)
    //        match b with
    //        | true -> 
    //            ProcessEvent c
    //        | false -> 
    //            Empty
    //    let result  =
    //        seq {
    //            for i in events do
    //                match TryReceiveEvent i with
    //                | Empty -> ()
    //                | msg -> yield msg
    //        }

    //    List.ofSeq result

    let private TryReceiveAllEvent (eventReceiver : EventReceiver<'a>) =   
        let eventList = (Seq.empty).ToList()
        let m = eventReceiver.TryReceiveAll(eventList)
        let a = Seq.toList eventList
        m, Seq.toList eventList

    // Could be expand for larger project by creating function of each scene
    let private ProcessGameEvent ((message, entity) : string * Entity) : GameMessage = 
        match message with
        | "Collision" -> BallMsg(Collision)
        | "Left" -> BallMsg(MoveLeft)
        | "Right" -> BallMsg(MoveRight)
        | "Up" -> BallMsg(MoveUp)
        | "Down" -> BallMsg(MoveDown)
        | "Jump" -> BallMsg(Jump)
        | "Grounded" -> BallMsg(Grounded)
        | "NoMovement" -> BallMsg(NoMovement)
        | "Camera"| "Camera2"| "Camera3"| "Camera4" -> Empty    //for example all these message can be process by ProcessCameraEvent message
        | _ -> Empty

    let ProcessAllGameEvent (eventReceiver : EventReceiver<string * Entity>) : GameMessage list =
        let numEvent, events = TryReceiveAllEvent eventReceiver
        match numEvent with
        | 0 -> []
        | _ ->
            let msgSeq =
                seq {
                    for e in events do
                        match ProcessGameEvent e with
                        | Empty -> ()
                        | m -> yield m
                }
            List.ofSeq msgSeq

    // Could be expand for larger project by creating function of each scene
    let private ProcessOtherEvent (num : float) : GameMessage = 
        Empty
            
    let ProcessAllOtherEvent (eventReceiver : EventReceiver<float>) : GameMessage list=        
        let numEvent, events = TryReceiveAllEvent eventReceiver
        match numEvent with
        | 0 -> []
        | _ ->
            let msgSeq =
                seq {
                    for e in events do
                        match ProcessOtherEvent e with
                        | Empty -> ()
                        | m -> yield m
                }
            List.ofSeq msgSeq    
    
    let ProcessAllEvent () : GameMessage list=        
        let msgSeq =
            seq {
                yield! ProcessAllGameEvent MessageEvent
                yield! ProcessAllOtherEvent OtherEvent
            }
        List.ofSeq (Seq.filter(fun m -> match m with | Empty -> false | _ -> true ) (Seq.distinct msgSeq))
    
    //let ProcessAllEvent (evenReceivers : EventReceiverType list) : GameMessage list=        
    //    let msgSeq =
    //        seq {
    //            for e in evenReceivers do
    //                match e with
    //                | MessageEventReceiver(m) ->
    //                    yield! ProcessAllGameEvent m
    //                | OtherEventReceiver(m) ->
    //                    yield! ProcessAllOtherEvent m
    //        }
    //    List.ofSeq (Seq.filter(fun m -> match m with | Empty -> false | _ -> true ) (Seq.distinct msgSeq))

