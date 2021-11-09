(*
    Partial Functon Application
    ========================================================================================

    Dit is een demonstratie van de kracht van partial function application. 

    We gaan hier ook zien, dat Fsharp foutloos vertaald kan worden naar Html/Javascript, 
    maar dat is bijvangst...


    We maken een grid, en als de muis over een cel heengaat, dan veranderen we de tekst.

    
    Partial Function Application wordt gedemonsteerd met de functie 'myOnMouseEnter'.

    'myOnMouseEnter' krijgt vier parameters binnen: x, y, element, event.
    Het OnMouseEnter event geeft twee parameters door: element, event.


    Tijdens het opbouwen van het grid, definiëren we het OnMouseEnter event per cel 
    met 'myOnMouseEnter' en geven we direct x, y als parameters mee. 
    Daarna blijft er een functie over met de twee resterende parameters: element, event. 
    Die resterende functie is compatibel met het OnMouseEnter event!

    Voer deze code als volgt uit:
    *   open deze link (Chrome): https://try.websharper.com/snippet/loic.denuziere/0000Gn
    *   maak op de webpagina links het tekstveld onder "Source" leeg;
    *   copy/paste de inhoud van deze file naar het veld onder "Source";
    *   klik de groene button "Run" rechtsboven.

    Test de code door met de muis over het grid te bewegen...

    Bekijk deze source, enkele comments geven meer toelichting.

    ========================================================================================

    Meer lezen over partial function application: 
        https://fsharpforfunandprofit.com/posts/partial-application/

    Meer achtergronden:
        https://fsharpforfunandprofit.com/posts/currying/
*)


namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Notation

[<JavaScript>]
module PartialFunctionApplication =
    /// Applicatie State, een 2 dimensionale array
    let gridState = Var.Create (Array2D.init 6 6 (fun i j -> false))
  


    /// On Mouse Enter wijzigt de applicatie state
    let myOnMouseEnter x y element event =
        gridState.Update(fun gv ->
            gv.[x , y] <- true  
            gv
            )


    /// de display, afhankelijk vd state
    let displayText x y v =
        if v then sprintf "%d,%d" x y
        else "."

    /// teken het grid
    let render (gv : View<bool[,]>) =
        div [ Attr.Class "game" ] [
            div [ Attr.Class "game-board" ] [
                for i in [0 .. 5] do
                    div [Attr.Class "board-row"] [
                        for j in [0 .. 5] do 
                            button [ 
                                //  Hier passen we partial function application toe op 'myOnMouseEnter'
                                on.mouseEnter (myOnMouseEnter i j)
                                Attr.Class "square"
                                ] [
                                    text (displayText i j gv.V.[i,j])
                            ]
                    ]
            ]
        ]
    
    
    /// Een kleine toeliching op de tool...
    /// Websharper vertaald Fsharp naar Html/Javascript.
    /// 
    /// Met het type "Var" (gridState in dit voorbeeld) definieer je client side application state.
    /// Je kunt 'Views' baseren op zo'n Var en adhv die Views kun je dingen op het scherm tekenen.
    /// Wanneer de state van een Var wijzigt, worden alle Views die erop gebaseerd zijn 
    /// opnieuw getekend adhv de nieuwe state..
    render gridState.View
    |> Doc.RunById "main"


    //  In de FSharp wereld zijn er twee grote FSharp-naar-Javascript vertalers (transpilators), WebSharper en Fable.
    //  Die vertaling is overigens foutloos, FSharp heeft namelijk een reflection-voor-code feature (Quotations),
    //  de gecompileerde code wordt uitgelezen en vervolgens vertaald...
    //  Fable wordt bijvoorbeeld gebruikt om VS Code plugins te schrijven, in FSharp.
    //  Inmiddels is er ook al iemand bezig met een Fsharp naar Python Transpiler....
