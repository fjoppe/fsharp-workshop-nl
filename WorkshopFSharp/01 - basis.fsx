//  Dit is commentaar

/// Dit is ook commentaar, maar verschijnt in intellisense


/// Files in FSharp:
///     .fs  - wordt gecompileerd, wordt in deze workshop niet gebruikt
///     .fsx - script voor de F# Interactive (net als deze file)


/// Hoe voer je een stukje script uit:
///     *   Selecteer de tekst die je wilt uitvoeren
///     *   Druk op Alt+Enter
///     *   Bekijk de output, deze verschijnt in het interactieve window, meestal in een tab onderaan.
///
/// Het interactieve window laat eerst het stukje script zien wat je uitvoert, daarna het resultaat.
/// 
/// Het interactieve window kan opnieuw gestart worden door:
/// * in VS Code: het weg te gooien met prullebak icoon en daarna weer een stukje
///     FSharp uit te voeren.
/// * in Visual Studio: rechtermuis in het interactieve window en dan Restart


/// We gaan beginnen..
let hoverTest = 0 // hover met de muis over de naam "hoverTest" en alles hierboven 
                  // achter triple-slash verschijnt in de intellisense


//  Sommige definities in dit script worden later hergebruikt. 
//  Als je de F# Interactive window restart, moet alles in dit script opnieuw uitvoeren,
//  tot het punt waar je verder wilt gaan. Je kunt grote stukken script selecteren en dan uitvoeren.


//  Maar om de kennis eigen te maken, kun je het beste alles hieronder stap-voor-stap uitvoeren
//  in je eigen tempo. Learning by example.


//  =====================================================================================================
//  1. Onze eerste values, voer uit zoals hierboven beschreven
//  =====================================================================================================
let helloCodeCafe = "Hello Code Cafe"

//  je mag spaties en andere tekens gebruiken in een label via tick-tick
let ``hello code cafe`` = "Hello code cafe met een leesbaar label"

//  Een value is strong typed, en het type wordt via "type inference" bepaald tijdens compileren.
//  Die automatische type-detectie kan heel handig zijn, als je het datatype wilt wijzigen
//  van een gegeven die door een hele flow wordt doorgegeven.
//  Type inference werkt ook voor de parameters en return van een functie.


//  Opdracht
//  -----------------------------------------------------------------------------------------------------
//  Dit zijn alle Fsharp primitive types: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/basic-types
//  Maak een value van het type float, en eentje van het type char



//  =====================================================================================================
//  2. Tuples zijn een simpele anonieme manier om data te groeperen. 
//  =====================================================================================================

//  Voer deze sectie regel-voor-regel uit

let tup1 = (1, 2) 

//  minimum cardinaliteit van een tuple is 2, komma gescheiden, haakjes zijn niet altijd nodig
let tup2 = 1, "een string" 

open System //  include een standaard library voor DateTime type
let (v1, v2, v3) = DateTime.Now, 32.89752, "taart!"   // deconstructie naar v1, v2 en v3

printf "%O" v1  //  fsharp string formatting, met functie "sprintf" kun je strings maken. 
                //  Voor meer info: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/plaintext-formatting#format-specifiers-for-printf
printfn $"{v2}"  //  dotnet string formatting 


//  =====================================================================================================
//  3a. Onze eerste functies
//  =====================================================================================================
// Dezelfde syntax als een value, maar met spatie-gescheiden parameters, voer regel-voor-regel uit:
let isGroterDan x y = x > y

// Test de functie
isGroterDan 2 3

isGroterDan 8.0 6.0 // Kijk! Het is een generieke functie!

isGroterDan "b" "a"

isGroterDan DateTime.Now (DateTime.Now.AddDays -3.0)


//  In plaats van accolades als in Java/C# gebruik je inspringen om blokken aan te geven
//  Je kunt ook op ieder moment een lokale value of lokale functie beginnen
//  Let op: selecteer de hele functie (het hele blok) en voer dan uit
let fibonacci n =
    let start = (0,1)           //  lokale value (tuple)
    let rec loop (n1,n2) i =    //  lokale recursieve functie, "rec" betekent recursief
                                //  Let op, (n1,n2) is een tuple deconstruct
                                //  in de functie definitie.
        if i < n then loop (n1+n2,n1) (i+1)
        else n1

    loop start 0    //  Geen "return" statement, de laatste expressie in een functie 
                    //  is automatisch de return waarde. In dit geval, de uitkomst
                    //  van de functie-aanroep.

fibonacci 5

fibonacci 6


//  if-then-else is een functie die iets teruggeeft
let ``5 punten als er "hello" in voorkomt`` =
    if helloCodeCafe.ToLower().Contains "hello" then 5
    else 0

//  het is echt een waarde geworden, kijk maar:
``5 punten als er "hello" in voorkomt``


//  Opdracht
//  -----------------------------------------------------------------------------------------------------
//  Maak een functie die uit een 3-tuple het derde element teruggeeft
//  Dus: geefDerde ("a", "b", "c") geeft "c" terug. Tip: gebruik tuple-deconstructie 



//  Type inference bepaalt dat divInt met integers werkt
let divInt a b = a / b

//  Maar met annotaties kun je andere types afdwingen 
let divFloat (a:float) (b:float) = a / b

divInt 12 3

divFloat 6.9 4.2
 
//  waarom is "divInt" niet generiek en "isGroterDan" van hierboven wel?
//  omdat de > operator op alle datatypes werkt en de / operator niet.

//  Type Inference maakt je functie zo generiek mogelijk, en als dat niet lukt de best guess. 
//  Onderstaande functie heeft de signature int-> int-> int, wat betekent, twee int parameters erin 
//  en een int als return. Het laatst type in die notatie is dus je return.
//  Overigens, wat in Java en C# "void" wordt genoemd, heet in F# "unit"

//  Decomment hieronder eens de regel die de functie "add" aanroept met twee strings
//  kijk daarna naar de signature van "add".
//  De signature staat rechts achter de functie, of je kunt met je muis over de functienaam hoveren.
let add b a = a + b
//  add "hello" "world"

//  het enige moment dat je geïntereseerd bent het type van een functie-parameter, is het moment dat 
//  je een aanroep schrijft. Daarna - het compileert toch?
//  het fijne van type inference is dat je makkelijk een type kunt wijzigen, en dat sijpelt automatisch
//  door je code heen...
//  Let op, fsharp is en blijft STRONG typed... maar valt je er alleen mee lastig op relevante momenten


//  inline functies zijn nóg generieker dan gewone functies
//  eigenlijk is dit een veredelde macro-substitutie zoals in C/C++ bestaat.
let inline divInline a b = a / b
divInline 20 4
divInline 60.0 12.0



//  =====================================================================================================
//  3b. Lambda Functions
//  =====================================================================================================

//  Een lamdba begint met `fun`, dan parameters, dan ->, dan de definitie
fun a -> a + 2  
//  and it's gone! er is geen manier om deze functie aan te roepen...


//  lambda toekennen aan een value, converteer die value tot een functie
let addLambda = fun a b -> a + b

addLambda 6 4


//  Meestal geef je een lambda mee aan een andere functie, die een functie als parameter verwacht.
let ``3 keer Hole in the Middle pattern`` f =
    printfn "begin"
    
    f 0
    f 1
    f 2
     
    printfn "einde"

``3 keer Hole in the Middle pattern`` (fun i -> printfn $"Dit was mijn input: {i}") 



//  =====================================================================================================
//  3c. Partial Functions
//  =====================================================================================================

let divIntOrig a b = a / b

//  Je mag een functie met minder parameters aanroepen dan gedefinieerd - partial function application.
divIntOrig 20    //  dit levert een functie op met de resterende parameters
//  and it's gone!


//  Je maakt een nieuwe functie als je "divIntOrig 20" toekent aan een value. 
//  "divIntOrig 20" wil zeggen dat de aanroep naar divIntOrig gebeurt met een vaste waarde 20 voor parameter "a" 
//  maar de functie wordt pas uitgevoerd als parameter "b" ook wordt meegegeven.
let div20 = divIntOrig 20   

div20 2
div20 4 
//  UITSTAPJE:
//  de kracht van partial function application/currying wordt hier nog niet echt duidelijk.
//  als je die kracht wél wilt zien, zie $/Uitstapjes/PartialFunctionApplication.fsx 



//  =====================================================================================================
//  4. Lijsten 
//  =====================================================================================================

//  FSharp kent out-of-the-box `List`, `Seq` (sequences) en `Array` om homogene lijstjes te maken.
//  Nu kijken we alleen naar - `List` om simpel te beginnen.

//  lege lijstjes
let empty1 = []
let empty2 = List.empty


//  Voorgeprogrameerde lijstjes
let getallen1 = [2 ; 4; 6; 8; 10] // puntkomma gescheiden

let getallen2 = [   //  newline gescheiden - voer uit door het hele stuk te selecteren
    1
    3
    5
]


let getallen3 = [20 .. 30] // gegenereerd, je kunt ook met stapjes van 2, of van hoog naar laag.


//  plak een element aan de voorkant van de lijst
let getallen4 = 10 :: getallen3     //  :: is de "Cons" operator, ook beschikbaar als functie via List.Cons


///  Geef het eerste element van een lijst terug
let eersteElement lst =
    match lst with
    |   []      ->  None    //  waarde van een option bij missende informatie
        //  Deconstructie:
        //  hd krijgt het eerste element uit lst
        //  met _ (underscore) geven we aan: vergeet dit, geldt hier dus voor de lijst-tail
    |   hd :: _ ->  Some hd //  waarde van een option bij aanwezige informatie

eersteElement ["yes me!!"]

eersteElement ["yes me!!"; "not me.."]

eersteElement getallen1

eersteElement ([]:int List)     //  lege lijst, maar type annotatie is hier verplicht
                                //  omdat [] uit zichzelf geen type heeft
eersteElement List.empty<int>   //  lege lijst van type int


//  =====================================================================================================
//  5.  Pipe operator |> wat links staat wordt rechts ingevuld
//  =====================================================================================================

//  de volgende twee regels doen hetzelfde
5 |> fibonacci
fibonacci 5

//  De pipe operator staat centraal in Fsharp. 
//  Zie je niet de Wow-factor? Probeer ook $/uitstapjes/PipeOperator.fsx 


//  =====================================================================================================
//  6. List module - allerlei functies die op lijstjes werken
//  =====================================================================================================

//  Deze list module is standaard bij FSharp inbegrepen


//  List.iter - doe iets per element
[0 .. 10]
|>  List.iter(fun i ->  //  lamdba wordt per lijst-element aangeroepen
    printfn $"fibonacci {i} = {fibonacci i}"
)

//  List.map - voor ieder element in de lijst, converteer naar andere waarde, type.. 
[0 .. 10] |> List.map(fun i -> fibonacci i)
getallen1 |> List.map fibonacci // hetzelfde als hierboven, maar korter

[10; 5; 3] |> List.map(fun i -> sprintf $"fibonacci {i} = {fibonacci i}") // map naar ander type


//  List.filter - filter elementen in de lijst
//      ieder element waarbij de filter-functie true teruggeeft, komt in de output.

let isEven i = i % 2 = 0 // functie om mee te spelen

//  eerst mappen, dan filteren, dan conversie naar string
[10; 5; 3]
|>  List.map(fun i -> i, fibonacci i)       // map naar tuple
|>  List.filter(fun (_ ,f) -> isEven f)     // filter op fibonacci waarde
|>  List.map(fun (i, f) -> sprintf $"fibonacci {i} = {f}")


//  eerst filteren, dan mappen, dan conversie naar string
[10; 5; 3]
|>  List.filter isEven                      //  filter op input-waarde
|>  List.map(fun i -> i, fibonacci i)       //  map naar tuple
|>  List.map(fun (i, f) -> sprintf $"fibonacci {i} = {f}")

//  Hoewel er hierboven verschillende List functies worden aangeroepen, 
//  optimaliseert de compiler deze zoveel mogelijk tot één loopje over alle elementen..


//  List.fold wordt gebruikt om adhv een lijst een state op te bouwen.
//  In onderstaand genereren we random getallen en vervolgens scheiden 
//  we ze naar twee lijstjes, even en oneven

let rnd = System.Random(29)


let genereerRandom1 =
    [0 .. 30]
    |>  List.map(fun _ -> rnd.Next(20))   

let genereerRandom2 =
    //  je kunt ook ter plekken een random lijst genereren tussen twee [] haken..
    [ for _ in [0 .. 30] do rnd.Next(20) ]

let even, oneven =
    //  dit gebruiken we als input lijst..
    genereerRandom1  

    //  de lambda die we meegeven aan List.fold krijgt
    //  twee parameters mee: currentState, currentItem
    //  die state bouwen we op met ieder element uit de input-lijst 
    //  die we binnen krijgen via currentItem (hier gebruiken we de value-naam `rn` van random-nummer)
    //  In dit geval is die state een tupel met daarin twee lijstjes.
    //  De lambda geeft een nieuwe state terug, die gebruikt wordt voor het volgende element.
    //  List.fold geeft de laatste state terug als alle elementen zijn verwerkt
    |>  List.fold(fun (ev, ov) rn ->
        if rn % 2 = 0 then 
            (rn :: ev, ov)  //  voeg rn toe aan het ev-lijstje
        else 
            (ev, rn :: ov)  //  voeg rn toe aan het ov-lijstje
    ) ([],[])   //  Dit: ([],[]) is de initïele state

even
oneven


//  Loopjes maak je meestal met List-functies. 
//  Maar indien dat niet kan, bijvoorbeeld omdat de input-list groeit tijdens verwerking
//  dan worden recursieve functies gebruikt om te loopen. Dat is overigens een kwestie van wennen,
//  maar als je daaraan begint, verdiep je dan ook even in "Tail Recursion Optimization" 
//  om stackoverflow's te voorkomen..
