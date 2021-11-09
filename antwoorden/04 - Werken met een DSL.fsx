#r "nuget: ReggerIt, 0.2.0"

open ReggerIt


///     Voor het jaartal accepteren we "2000" t/m "2999"
let jaartal = Plain "2" + RepeatExact 3 Macro.decimalDigit

///     In het weeknummer pattern, accepteren we waarden tussen "01"-"53"
let zeros = Plain "0" + Between '1' '9'     //  pattern voor 01-09
let fifties = Plain "5" + Between '0' '3'   //  pattern voor 50-53

///     ANTWOORD
///     Match tussen 10-49
let intermediateRange = Between '1' '4' + Macro.decimalDigit

let weeknummer = zeros ||| intermediateRange ||| fifties

let jaarWeekNummer = (NamedGroup "jaar" jaartal) + Plain "-" + (NamedGroup "week" weeknummer)

let pattern = jaarWeekNummer |> Convert.ToFullstringPattern

open System.Text.RegularExpressions

[
///     ANTWOORD    
    "2019-53", (2019,53)
    "2020-04", (2020,04)
    "2021-21", (2021,21)
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
