//  Deze source behandelt structuren, zoals loopjes, missende data en samengestelde data types. 
//  Dit script staat helemaal los van de andere scripts




//  =====================================================================================================
//  1. Option Type
//  =====================================================================================================

//  Een gecontroleerde manier om aan te geven of een waarde wel/niet aanwezig is
//  Dit is ter vervanging van "null"..

let a = Some 3  //  Met Some geef je aan dat de waarde aanwezig is
let b = None    //  Met None geef je aan dat een waarde afwezig is

let printInfo inVal =
    match inVal with
    |   Some value    ->    printfn $"De waarde is: {value}"
    |   None          ->    printfn "Had geen waarde"

printInfo a
printInfo b

"string waarde"
|>  Some
|>  printInfo


printInfo None

a = Some 3  //  Je mag twee waarden vergelijken


//  Het option pattern geeft vele voordelen:
//  *   Alle waarden in data-types, of functie-returns zijn *altijd* mandatory, altijd aanwezig
//      >>  nooit meer overbodige null-checks;
//  *   Option is een wrapper om aan te geven dat een waarde misschien aanwezig is
//      >>  je weet altijd waar je aan toe bent, een optioneel type is ook mandatory - altijd aanwezig;
//  *   Zoals je in de functie `printInfo` hierboven ziet, je moet een option-type eerst uitpakken voordat 
//      je de eventuele waarde kunt gebruiken. Nooit meer null-pointer exceptions!

//  NB: Optionele waarden zijn altijd complexer dan mandatory waarden - je hebt altijd twee scenario's.. 
//      Mandatory als standaard is eenvoud als standaard.



//  =====================================================================================================
//  2. Record Type
//  =====================================================================================================

//  Een record is een DTO, kan niet overerft worden, en kent "structural equality"

type Persoon1 = {
    Naam        :   string
    Adres       :   string
    Email       :   string option   //  je kunt aan het type zien of een veld verplicht of optioneel is
    Telefoon    :   string list     //  dezelfde list als eerder behandeld 
}

let p1 = { Naam = "Frank"; Adres = "MyStreet 54"; Email = Some "fjoppe2@conclusion.nl"; Telefoon = []}

let p2 = { Naam = "Frank"; Adres = "MyStreet 54"; Email = Some "fjoppe2@conclusion.nl"; Telefoon = []}


p1 = p2 //  Structural Equality: property-gewijze vergelijking

// maak kopie van p1 maar geef property Address een andere waarde
let p3 = { p1 with Adres = "NewAddress" } 

//  voeg telefoon nummer toe
let p4 = { p3 with Telefoon = "061234567" :: p3.Telefoon }  //   herinner je de :: operator nog uit deel 1?


//  Met accolade en pipe kun je anonieme records maken {||}
let anon1 = {| Message = "Hello"; Destination = "World" |}
let anon2 = {| Message = "Hello"; Destination = "World" |}

anon1 = anon2   //   equality in type en inhoud


//  =====================================================================================================
//  3. Discriminated Union
//  =====================================================================================================

//  Discriminated union in één van de meest krachtige mechanismes in Fsharp
//  Het ziet eruit als een enum waarin je waarden kunt opslaan
//  Je kunt het ook zien als een interface met daaronder direct implementerende classes..

type ContactWijze =
    |   Telefoon    of string
    |   Email       of string
    |   Brief       //  gebruik hiervoor adres, die je op een andere plek al hebt

type  Persoon2 = {
    Naam        :   string
    Adres       :   string
    ContactWijze:   ContactWijze    /// DDD - wijze van contact op drie manieren vastgelegd in het datatype
}


let pers1 = { Naam = "Frank"; Adres = "FrankAdres"; ContactWijze = Email "fjoppe2@conclusion.nl"  }

let pers2 = { Naam = "Lucas"; Adres = "LucasAdres"; ContactWijze = Brief }


let printContactWijze ps2 =
    match ps2.ContactWijze with
    //  de kracht van match .. with: deconstruct de discriminated union en extraheer de waarde
    |   Telefoon s  ->  printfn $"Telefoon: {s}"
    |   Email    s  ->  printfn $"Email: {s}"
    |   Brief       ->  printfn $"Brief: {ps2.Adres}"

printContactWijze pers1

printContactWijze pers2


// Interessante gedachte:
//  We hebben list, option, record en discriminated unions. 
//  Vanuit de DDD gedachte kun je hiermee data-modellen maken die illegale representaties uitsluiten.
//  "Make illegal state unrepresentable": https://fsharpforfunandprofit.com/posts/designing-with-types-making-illegal-states-unrepresentable/
//  
//  Met weinig effort kun je een data-model maken die de grove lijnen van het Business Domein modelleren.
//  Een hele klasse aan errors zijn hierdoor niet meer van toepassing.
//  
//  UITSTAPJE:
//      Het is heel makkelijk om een kaartspel te modelleren.
//      Zie hier: $/uitstapjes/DDD KaartSpel.fsx

