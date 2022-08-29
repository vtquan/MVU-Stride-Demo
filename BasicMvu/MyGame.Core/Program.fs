namespace MyGame.Core

open Stride.Engine;

open Game

type MvuGame() =
    inherit Game()

    let mutable Model, Messages = Game.empty, []
        
    override this.BeginRun () = 
        let mainScene = this.Content.Load<Scene>("MainScene")
        let model, messages = init mainScene
        Model <- model
        Messages <- messages    
        
    override this.Update gameTime =
        base.Update(gameTime);
            
        Messages <- Messages @ mapAllEvent ()

        let model, messages = update Model Messages gameTime
        Model <- model
        Messages <- messages

        view Model gameTime |> ignore

    override this.Destroy () =
        base.Destroy()