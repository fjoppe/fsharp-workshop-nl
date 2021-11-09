(*
    Pipe Operator : |>
    ==============================================================================

    Het Fsharp logo is gebaseerd op deze pipe operator:
    https://nl.wikipedia.org/wiki/F%E2%99%AF#/media/Bestand:Fsharp_logo.png

    Links van de operator staat een waarde, rechts een functie. 
    De operator vult de waarde in, in de functie.

    Dus dit:        x |> f() 
    Voert dit uit:  f(x)

    De kracht hiervan is leesbaarheid. Je kunt nu code maken die van links naar rechts, 
    van boven naar onder gelezen kan worden, net als natuurlijke tekst. 
    
    Hiermee kun je code leesbaar maken voor non-programmeurs (wel met analytisch vermogen).
    Je kunt bijvoorbeeld een Domain Specific Language (DSL) maken, die een deel 
    van het business domain beslaat. Dat maakt het makkelijk om samen met een business
    analyst een feature te bespreken of uit te werken.


    Leesbaarheid wordt hier gedemonstreerd met een mini-DSL, een rekenmachine met vier reken-functies.

    Deze code staat los van de hoofdworkshop en kan in een eigen Fsharp Interactive window worden uitgevoerd.
*)


let add b a = a + b
let min b a = a - b
let mul b a = a * b
let div b a = a / b


//  De rekensom: 8 + 4 - 9 * 7 
//  NB. geen haakjes, maar volgorde van invoer en steeds op de = drukken, net als op een echte rekenmachine: 
//  8 + 4 = -9 = *7 = 
//  --> 21

//  Lastig leesbare code:
mul 7 (min 9 (add 8 4))


//  Door de pipe operator kun je code van boven naar beneden lezen:
4
|>  add 8
|>  min 9
|>  mul 7


//  De pipe operator is generiek en wordt vaak gebruikt om functie-aanroepen aan elkaar te lijmen.
//  Dit idee is nog verder uitgekristaliseerd in het populaire Railway Oriented Programming: https://fsharpforfunandprofit.com/rop/

