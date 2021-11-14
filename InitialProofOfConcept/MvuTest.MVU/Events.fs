

namespace MvuTest.MVU
module Events =
    open Stride.Engine
    open Stride.Engine.Events

    let MessageEventKey = new EventKey<string*Entity>()    
    let MessageEvent = new EventReceiver<string*Entity>(MessageEventKey, EventReceiverOptions.Buffered)
    
    let OtherEventKey = new EventKey<float>()        
    let OtherEvent = new EventReceiver<float>(OtherEventKey, EventReceiverOptions.Buffered)

    type EventReceiverType = 
        | MessageEventReceiver of EventReceiver<string*Entity>
        | OtherEventReceiver of EventReceiver<float>

    
    let AllEvents =
        [MessageEventReceiver(MessageEvent); OtherEventReceiver(OtherEvent)]