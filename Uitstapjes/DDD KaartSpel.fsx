//  Hier wordt een DDD voorbeeld gegeven adhv een kaartspel (Texas Holdem)

//  De beste manier om hier naar te kijken tijdens de workshop is als volgt:
//  1.  Bekijk de code onder "Definieer het domein"
//  2.  Sla de code onder "hulp functies" over - deze kun je later bekijken
//  3.  Voer alle code uit tot helemaal onderaan sectie "Kaartspel Texas Holdem"
//  4.  Ga vanaf sectie "Kaartspel Texas Holdem" weer stap-voor-stap te werk

//  Overigens wordt hier 'Texas Holdem' geprogrammeerd in ca 300 regels,
//  kan dat in jouw programmeertaal, gegeven dat je de reguliere code styleguide volgt? 

//  -------------------------------------------------------------------------------------
//  Definieer het domein
//  -------------------------------------------------------------------------------------

//  Een speelkaart bestaat uit een waarde en een kleur
type KaartWaarde =
    |   Twee  = 0   //  je kunt alleen een numerieke waarde toekennen
    |   Drie  = 1   //  in de type definitie
    |   Vier  = 2   //  dit converteert de Discriminated Union naar een enum
    |   Vijf  = 3   //  enum's kunnen geen data bevatten
    |   Zes   = 4
    |   Zeven = 5
    |   Acht  = 6
    |   Negen = 7
    |   Tien  = 8
    |   Boer  = 9
    |   Dame  = 10
    |   Heer  = 11
    |   Aas   = 12


//  Een kaart bestaat uit één vd kleuren en één vd toegestane waarden. 
type Kaart =
    |   Harten   of KaartWaarde
    |   Ruiten   of KaartWaarde
    |   Schoppen of KaartWaarde
    |   Klavers  of KaartWaarde

//  Andere modellen waren natuurlijk ook mogelijk:
//  type Kaart = 
//      | Twee of KaartKleur    // in dit model hebben de kaarten geen volgorde nummer
//      ....                    // die info is nodig om te kijken of een combinatie een straight is
//                              // dus dat moet op een andere manier aan worden toegevoegd
//  
//  type Kaart = {              // dit model kan sommige functies hieronder korter maken
//      Kleur : KaartKleur      // en andere weer langer. 
//      Waarde: KaartWaarde     // uiteindelijk is dit alleen maar een extra container rondom het type 
//  }                           // dat we in dit script gebruiken.


//  Een combinatie bestaat atijd uit 5 kaarten
//  De twee van een speler en drie van de kaarten die op tafel liggen.
type Combinatie = private {
    Kaarten' : Kaart list
}
with
    static member Create l =
        if l |> List.length = 5 then { Kaarten' = l }
        else failwith "Een hand moet exact 5 kaarten hebben"

    //  Dit is de property "Kaarten" met alleen een getter
    //  Alle velden in dit record zijn private, zodat je deze alleen
    //  kunt aanmaken via de static method "Create", waarin
    //  een data-consistentie check wordt uitgevoerd.
    //  NB. de naam van het record-veld eindigt op single-quote, 
    //  De method wordt hier gedefinieerd met "this",
    //  maar je mag zelf weten wat je dat noemt, x.Kaarten mag ook.
    member this.Kaarten with get() = this.Kaarten'


let kaartSpel =
    //  Het kaartspel wordt gemaakt vanuit een list met alle kaartwaarden.
    //  Iedere waarde wordt gemapped naar de vier kleur-kaarten met die waarde
    //  Bijv. Aas -> [ Harten Aas; Ruiten Aas; Schoppen Aas; Klaver Aas]
    //  Dus een list van kaartwaardes wordt een list van een list met vier kaarten
    //  Op het laatst wordt de lists platgeslagen met List.collect.
    //  Met List.collect kun je een list-van-lists platslaan naar een list met enkel de elementen
    //  Dus [[1;2];[3;4]] |> List.collect id resulteert in [1;2;3;4]

    KaartWaarde.GetValues()
    |>  List.ofArray
    |>  List.map(fun krt -> [Harten krt; Ruiten krt; Schoppen krt; Klavers krt])
    |>  List.collect id //  slaat een list van lists plat


// Om het tempo erin te houden kun je het bestuderen van
// de code hieronder overslaan, tijdens de workshop
// Ga verder naar sectie "Kaartspel Texas Holdem" helemaal onderaan.

//  ---------------------------------------------------------------------------------------------------
//  Hulp Functies
//  ---------------------------------------------------------------------------------------------------

let maakKaartLijstString kl =
    kl
    |>  List.map(fun i -> i.ToString())
    |>  List.toArray
    |>  fun v -> System.String.Join(", ", v)


///  Geef de waarde van een kaart terug
let kaartWaarde krt =
    match krt with
    |   Harten   k -> k
    |   Ruiten   k -> k
    |   Schoppen k -> k
    |   Klavers  k -> k

/// Geef de kleur van een kaart terug.
/// Althans, vertaal de kleur naar een unieke id die makkelijk te vergelijken is met euqality.
let kaartKleur krt =
    match krt with
    |   Harten   k -> 1 
    |   Ruiten   k -> 2
    |   Schoppen k -> 3
    |   Klavers  k -> 4

/// groepeer de input naar kaartwaarde, harten-twee en ruiten-twee komen in dezelfde groep
let groepeerOpKaartWaarde (h:Combinatie) =
    h.Kaarten
    |>  List.groupBy kaartWaarde
    |>  List.map(fun (kw, kl) -> kl)

/// heeft de input lijst een bepaalde lengte?
let hasListWithLength n l =
    l |> List.exists(fun sl -> sl |> List.length = n)

/// heeft gegeven combinatie n dezelfde kaartwaardes?
let ``has n of a Kind`` n = groepeerOpKaartWaarde >> hasListWithLength n
// de >> operator zorgt voor het laatste functie-argument van ``has n of a Kind``
// deze parameter is onzichtbaar in de code, maar wordt ingevuld in groepeerOpKaartWaarde


//  Functies die bepalen of de gegeven vijf kaarten voldoen aan een Poker-combinatie
//  ---------------------------------------------------------------------------------

let isStraight (h:Combinatie) = 
    h.Kaarten
    |>  List.map kaartWaarde
    |>  List.sort
    |>  List.pairwise // zie: https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html#pairwise
    |>  List.map(fun (p,s) -> int(s) - int(p)) // vertaal naar de afstand tussen twee opvolgende kaarten
                                               // de afstand tussen een twee en een drie is dus 1
    |>  List.distinct //  lijst met alle afstanden indunnen
    |>  (=) [1] // alle twee opvolgende kaarten hebben een afstand van 1 tot elkaar

let isFlush (h:Combinatie) =
    h.Kaarten
    |>  List.map kaartKleur
    |>  List.distinct
    |>  List.length
    |>  fun ln -> ln = 1

let isFullHouse (h:Combinatie) = ``has n of a Kind`` 2 h && ``has n of a Kind`` 3 h

let isFourOfAKind = ``has n of a Kind`` 4

let isThreeOfAKind = ``has n of a Kind`` 3

let isTwoPair (h:Combinatie) =
    groepeerOpKaartWaarde h 
    |>  List.filter(fun l -> l.Length = 2) // filter alle paren
    |>  List.length 
    |> (=) 2    // een operator is een functie die ook met partial function application werkt...

let isOnePair = ``has n of a Kind`` 2

let isStraightFlush (h:Combinatie) = isStraight h && isFlush h

let isRoyalFlush (h:Combinatie) =
    isStraightFlush h &&
    h.Kaarten |> List.map kaartWaarde |> List.contains KaartWaarde.Aas

let getHighCard (h:Combinatie) =
    h.Kaarten
    |>  List.sortByDescending(kaartWaarde)
    |>  List.head

//  De poker score per combinatie - we doen dit makkelijk in een tupel
let pokerScoreRanking = [
    (1, isRoyalFlush, "Royal Flush")
    (2, isStraightFlush, "Straight Flush")
    (3, isFourOfAKind, "Four of a Kind")
    (4, isFullHouse, "Full House")
    (5, isFlush, "Flush")
    (6, isStraight, "Straight")
    (7, isThreeOfAKind, "Three of a Kind")
    (8, isTwoPair, "Two Pair")
    (9, isOnePair, "One Pair")
]

//  Deal functies - dealen naar spelers en de tafel
//  ---------------------------------------------------------------------------------

let schudKaarten deck =
    let nieuwAantal = 
        let rnd = System.Random()   //  ``rnd```` is een closure, "nieuwAantal" wordt hiermee een stateful function
        fun() -> rnd.Next(5)

    //  zie ook: https://www.wikihow.com/Riffle-and-Bridge-Shuffle
    //  Bij RiffleShuffle maak je van twee halve decks een hele.
    //  Maar in dit algoritme maken we van een heel deck juist twee halve, 
    //  die we op het eind weer samenvoegen. Vandaar de naam "Reversed"
    let rec reversedRiffleShuffle actief passief aantal kaartenResterend =
        match kaartenResterend with
        |   []                        -> actief @ passief // samenvoegen (@ = List.concat)
        |   h :: rest when aantal > 0 -> reversedRiffleShuffle (h::actief) passief (aantal-1) rest
        |   _                         -> reversedRiffleShuffle passief actief (nieuwAantal()) kaartenResterend
    
    let startShuffle = reversedRiffleShuffle [] [] (nieuwAantal()) 
    
    [0..5]  
    |>  List.fold(fun s _ -> startShuffle s) deck

let schudKaartspel() = schudKaarten kaartSpel

let ``Deal 2 kaarten naar iedere Player`` players deck =
    let rec deal dealt cardDeck toDeal =
        match (toDeal, cardDeck) with
        |   ([], _)  -> (cardDeck, dealt)
        |   (player :: others, card :: rest)  -> deal ((card::player)::dealt) rest others 
        |   (_, []) -> failwith "Geen kaarten meer, teveel spelers"
    [1 .. players]
    |>  List.map(fun _ -> [])
    |>  deal [] deck
    ||>  deal [] 


let ``Deal een kaart naar de tafel`` (h :: deck) = h, deck

let ``Deal N kaarten naar de tafel`` n deck =
   [1 .. n] 
   |>   List.fold(fun (t,r) _ -> 
        let (card, restDeck) = ``Deal een kaart naar de tafel`` r
        card :: t, restDeck
    ) ([], deck)
    |>  fst


//  Functies om de winnaar te bepalen
//  ---------------------------------------------------------------------------------

let ``Bepaal alle mogelijke combinaties`` playerHand tableCards =
    let rec distribute e = function
      | [] -> [[e]]
      | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]

    let rec permute = function
      | [] -> [[]]
      | e::xs -> List.collect (distribute e) (permute xs)

    permute tableCards
    |>  List.map(fun l -> l |> List.take 3)
    |>  List.distinct
    |>  List.map(List.append playerHand >> Combinatie.Create)


let zoekHoogstePokerScore (h:Combinatie) =
    pokerScoreRanking 
    |> List.tryPick(fun (r, isFnc, comboName) -> 
        if isFnc h then Some (r,h, comboName) else None
    )


let zoekHoogsteCombinatiesVanAlleSpelers playerHands tafel =
    playerHands
    |>  List.map(fun (kaarten, naam) -> ``Bepaal alle mogelijke combinaties`` kaarten tafel, naam)
    |>  List.map(fun (playerCombinations, naam) -> 
            let highestCombo =
                playerCombinations
                |>  List.fold(fun fnd hand -> 
                    zoekHoogstePokerScore hand
                    |>  function
                        |   Some r -> r :: fnd
                        |   None   -> fnd
                ) []
                |>  List.sortBy(fun (r, _, _) -> r)
                |>  List.tryHead
            highestCombo, naam
        )
    |>  List.choose(fun (r,n) -> r |> Option.map(fun rs -> rs, n)) 
    |>  List.map(fun ((r, h, cn), pn) -> {| Rank = r;  Hand = h; CombinatieNaam = cn; PlayerNaam = pn |}) // anoniem record!


let bepaalWinnaar hands tafelKaarten =
    let hoogsteCombos = zoekHoogsteCombinatiesVanAlleSpelers hands tafelKaarten
    
    let mkResult p k c = {| PlayerNaam = p; ``Kaart(en)`` = k; Combinatie = c |}

    hoogsteCombos
    |>  List.sortBy(fun r -> r.Rank)
    |>  List.tryHead
    |>  function
        |   Some comboWinnaar -> mkResult comboWinnaar.PlayerNaam comboWinnaar.Hand.Kaarten comboWinnaar.CombinatieNaam
        |   None -> 
            hands
            |>  List.map(fun (kl, pl) -> (kl |> List.maxBy(kaartWaarde)), kl, pl)
            |>  List.groupBy(fun (highCard, _, _) -> kaartWaarde highCard)
            |>  List.sortByDescending(fun (highCardValue, _) -> highCardValue)
            |>  List.head
            |>  fun (_, lst) ->
                match lst with
                |   [(winCard, hand, playerName)] -> mkResult playerName [winCard] "High Card"
                |   _ ->  mkResult "Niemand" [] "Gelijkspel"


//  ---------------------------------------------------------------------------------------------------
//  Kaartspel Texas Holdem
//  ---------------------------------------------------------------------------------------------------

let players = ["Frank"; "Lucas"]
let (playerHands, dealtDeck) =
    let (restDeck, cardHands) = ``Deal 2 kaarten naar iedere Player`` (players.Length) (schudKaartspel())
    (players |> List.zip cardHands), restDeck // zip combineert de spelernaam met de kaarten in zijn hand

let tafelKaarten = ``Deal N kaarten naar de tafel`` 5 dealtDeck

let winnaar =  bepaalWinnaar playerHands tafelKaarten

printfn $"De winnaar is {winnaar.PlayerNaam}, met combinatie {winnaar.Combinatie}"
printfn $"De winnende kaarten: {maakKaartLijstString winnaar.``Kaart(en)``}"

//  om individuele values uit te lezen:
winnaar
playerHands
tafelKaarten

//  je kunt het hele spel opnieuw uitvoeren voor een andere winnaar, dus vanaf "let players ="
