namespace MyGame.Core

open Stride.Engine;

open Game

type MvuGame() =
    inherit Game()

    let mutable State, Messages = Game.empty,[]
        
    override this.BeginRun () = 
        let mainScene = this.Content.Load<Scene>("MainScene")
        let state, messages = init mainScene
        State <- state
        Messages <- messages    
        
    override this.Update gameTime =
        base.Update(gameTime);
            
        Messages <- Messages @ mapAllEvent ()

        let newState, newMessages = update State Messages gameTime
        State <- newState
        Messages <- newMessages

        view State gameTime |> ignore

    override this.Destroy () =
        base.Destroy()