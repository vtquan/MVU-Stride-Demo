namespace MvuTest.MVU

module Counter =
    open Elmish
    open Message

    type Model =
        { x : int }
        
    let init () =
        { x = 0 }

    let update msg model =
        match msg with
        | Increment when model.x < 3 -> 
            { model with x = model.x + 1 }, Increment
            
        | Increment -> 
            { model with x = model.x + 1 }, Decrement
            
        | Decrement when model.x > 0 -> 
            { model with x = model.x - 1 }, Decrement
            
        | Decrement -> 
            { model with x = model.x - 1 }, Increment