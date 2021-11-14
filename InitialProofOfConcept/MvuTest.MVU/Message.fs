namespace MvuTest.MVU

module Message =    
    type CounterMsg = 
        | Increment
        | Decrement
    
    type BallMsg = 
        | MoveLeft
        | MoveRight
        | MoveUp
        | MoveDown
        | NoMovement
        | NewVelocity
        | Jump
        | Grounded
        | Collision

    type GameMessage =
        | CounterMsg of CounterMsg
        | BallMsg of BallMsg
        | Start
        | Stop 
        | Empty // Considered None but that clashed with Option.None
