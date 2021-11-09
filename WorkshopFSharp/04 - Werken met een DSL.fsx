///     F# is uitstekend om een DSL in te maken - een Domain Specific Language.
/// 
///     In dit gedeelte gaan we met een DSL werken, gemaak voor reguliere expressies.
///     Met deze DSL is het makkelijk om Regex patterns te maken, die door .Net Regex 
///     wordt geaccepteerd. Auteur van deze library is uw workshop-gastheer.
/// 
///     Documentatie:   https://fjoppe.github.io/ReggerIt
///     Nuget package:  https://www.nuget.org/packages/ReggerIt/


///     Als je naar het nuget package gaat, kun je de tekst kopieren, zoals hieronder al staat:
#r "nuget: ReggerIt, 0.2.0"

///     Ofwel, dit is de manier om packages te gebruiken vanuit Fsharp scripting..  
///     Voer deze regels uit in een nieuwe Fsharp interactive..


open ReggerIt


///     We gaan een validatie maken, die jaar-weeknummer accepteert.
///     We controleren hierbij alleen het formaat en we kijken niet welk jaar hoeveel weken heeft.
///     
///     * Het geaccepteerde formaat is: jjjj-ww
///     * Het minimale weeknummer is 01, het maximale weeknummer is 53.
///     * Het minimale jaartal is 2000, het maximale jaartal is 2999
///     * We willen op een makkelijke manier de waarden extraheren.
/// 
///     Bekijk dit voorbeeld om een gevoel te krijgen: 
///         https://fjoppe.github.io/ReggerIt//How_Tos/Hex_Input_Validation.html
/// 
///     Dit zijn de functies en operators van deze library:
///         https://fjoppe.github.io/ReggerIt//Tutorials/Functions_And_Operators.html


///     Voor het jaartal accepteren we "2000" t/m "2999"
let jaartal = Plain "2" + RepeatExact 3 Macro.decimalDigit

///     In het weeknummer pattern, accepteren we waarden tussen "01"-"53"
let zeros = Plain "0" + Between '1' '9'     //  pattern voor 01-09
let fifties = Plain "5" + Between '0' '3'   //  pattern voor 50-53



///     Oefening: maak het pattern voor 10-49
let intermediateRange = Macro.any



let weeknummer = zeros ||| intermediateRange ||| fifties

//  Dankzij de namedgroups kunnen we de waarden straks makkelijk extraheren..
let jaarWeekNummer = (NamedGroup "jaar" jaartal) + Plain "-" + (NamedGroup "week" weeknummer)

let pattern = jaarWeekNummer |> Convert.ToFullstringPattern

open System.Text.RegularExpressions


[
    //  Oefening: maak test data met tuples in de vorm:
    //    (input string, expected)
    //  waarbij 'expected' een tuple van twee integers is, in de vorm: (jaar, week)
    //  Dus: (string, (int, int))
]
|>  List.iter(fun (str, (jr, wk)) ->
    let mtch = Regex.Match(str, pattern)
    
    if not mtch.Success then failwith "Geen match!"
     
    let pJr = mtch.Groups.["jaar"].Value
    if int(pJr) <> jr then failwith $"Jaar komt niet overeen: {str} - {pJr} <> {jr}"

    let pWk = mtch.Groups.["week"].Value
    if int(pWk) <> wk then failwith $"Jaar komt niet overeen: {str} - {pWk} <> {wk}"

    printfn $"Succes parse: {str}"
)

