
//  Opdracht
//  -----------------------------------------------------------------------------------------------------
//  Dit zijn alle Fsharp primitive types: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/basic-types
//  Maak een value van het type float, en eentje van het type char


let floatValue = 12.34

let charValue = '@'



//  Opdracht
//  -----------------------------------------------------------------------------------------------------
//  Maak een functie die uit een 3-tuple het derde element teruggeeft
//  Dus: geefDerde ("a", "b", "c") geeft "c" terug. Tip: gebruik tuple-deconstructie 

let geefDerde (a,b,c) = c

geefDerde (1,2,3)

//  overigens kan dit ook met de discard operator underscore _

let geefDerde2 (_,_,c) = c

geefDerde2 (1,2,3)

geefDerde2 (1, System.DateTime.Now, "derde")


