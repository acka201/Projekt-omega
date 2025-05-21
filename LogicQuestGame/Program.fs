// Logic Quest Game
open System
open System.IO
open System.Text.Json
open System.Threading  // Szükséges az animációhoz (Thread.Sleep)

// Globális véletlenszám-gener.
let rnd = Random()

// Segédfüggvények graf. elemekhez

// Színek beállítása
let setColor color =
    Console.ForegroundColor <- color

let resetColor () =
    Console.ResetColor()

// üzenet után 3 pont, 500 ms késleltetéssel
let animateAction message =
    printf "%s" message
    for _ in 1 .. 3 do
         printf "."
         Thread.Sleep(500)
    printfn ""

// ASCII grafika
let displayAsciiGraphic graphicType =
    match graphicType with
    | "sword" ->
         printfn "   /|"
         printfn "  ||====>"
         printfn "   \\|"
    | "monster" ->
         printfn "   (◣_◢)"
         printfn "   --|--"
         printfn "    / \\"
    | "castle" ->
         printfn "    | |  /|"
         printfn "   _|_|_/ |"
         printfn "  |       |"
         printfn "  | CASTLE|"
         printfn "  |_______|"
    | "map" ->
         printfn "##########"
         printfn "#        #"
         printfn "#   P    #"
         printfn "#        #"
         printfn "##########"
    | _ -> ()

// ---------- Játékos profil ----------
type PlayerState = {
    Name: string
    HP: int
    Attack: int
    Defense: int
    Inventory: string list
    Gold: int
    XP: int
    Level: int
    Language: string  // "hu" vagy "en" választható
}

let newPlayerProfile name lang = {
    Name = name
    HP = 100
    Attack = 20
    Defense = 15
    Inventory = []
    Gold = 50
    XP = 0
    Level = 1
    Language = lang
}

let saveProfile (player: PlayerState) =
    let filename = "playerProfile.json"
    let options = JsonSerializerOptions(WriteIndented = true)
    let json = JsonSerializer.Serialize(player, options)
    File.WriteAllText(filename, json)
    if player.Language = "hu" then 
        printfn "A profilod el lett mentve: %s" filename
    else
        printfn "Profile saved to %s" filename

let tryLoadProfile () : PlayerState option =
    let filename = "playerProfile.json"
    if File.Exists(filename) then 
       try 
         let json = File.ReadAllText(filename)
         let player = JsonSerializer.Deserialize<PlayerState>(json)
         Some player
       with ex ->
         setColor ConsoleColor.Red
         printfn "Hiba a profil betöltésekor: %s" ex.Message
         resetColor()
         None
    else
       setColor ConsoleColor.Red
       printfn "Nem található mentett profil!"
       resetColor()
       None

// ---------- Szint adatainak betöltése ----------
let loadLevelData (level: int) (lang: string) : string list =
    let fileName = 
        if lang = "hu" then sprintf "level%d_hu.txt" level 
        else sprintf "level%d_en.txt" level
    if File.Exists(fileName) then
         File.ReadAllLines(fileName) |> Array.toList
    else
         []

/// Amikor a játékos 100 XP fölött van, szintet lép (max. 10. szintig)
let checkLevelUp (player: PlayerState) =
    if player.XP >= 100 && player.Level < 10 then
         let newLevel = player.Level + 1
         let xpOver = player.XP - 100
         if player.Language = "hu" then
             setColor ConsoleColor.Yellow
             printfn "\nGratulálunk, %s! Elérted a %d. szintet!" player.Name newLevel
         else
             setColor ConsoleColor.Yellow
             printfn "\nCongratulations, %s! You've reached level %d!" player.Name newLevel
         resetColor()
         // A szinten lévő újdonságok
         let newTasks = loadLevelData newLevel player.Language
         if newTasks.IsEmpty then
             if player.Language = "hu" then
                 printfn "Ehhez a szinthez nem tartozik új feladat vagy tárgy."
             else
                 printfn "No new quests or items for this level."
         else
             if player.Language = "hu" then
                 setColor ConsoleColor.Green
                 printfn "Új feladatok és tárgyak ezen a szinten:"
             else
                 setColor ConsoleColor.Green
                 printfn "New quests and items at this level:"
             newTasks |> List.iter (printfn "- %s")
             resetColor()
         { player with Level = newLevel; XP = xpOver }
    else
         player

// ---------- Matematek ----------
let mathPuzzleLang (lang: string) =
    let x = rnd.Next(1, 50)
    let y = rnd.Next(1, 50)
    let correctAnswer = x + y
    if lang = "hu" then
        setColor ConsoleColor.Cyan
        printfn "\nMatematikai fejtörő:"
        printfn "Mennyi %d + %d ?" x y
        resetColor()
        printf "Válasz: "
    else
        setColor ConsoleColor.Cyan
        printfn "\nMath Puzzle:"
        printfn "What is %d + %d ?" x y
        resetColor()
        printf "Answer: "
    let answerString = Console.ReadLine()
    match Int32.TryParse(answerString) with
    | (true, userAnswer) when userAnswer = correctAnswer ->
         if lang = "hu" then printfn "Helyes válasz!" else printfn "Correct answer!"
         true
    | _ ->
         if lang = "hu" then printfn "Rossz válasz!" else printfn "Wrong answer!"
         false

let rec solveMathPuzzle (lang: string) =
    if mathPuzzleLang lang then () else solveMathPuzzle lang

// ---------- Harc ----------
type Enemy = {
    Name: string
    HP: int
    Attack: int
    Defense: int
}

let combatEncounter (player: PlayerState) =
    let enemyNames = [ "Goblin"; "Orc"; "Skeleton" ]
    let enemy = {
        Name = enemyNames.[rnd.Next(enemyNames.Length)]
        HP = rnd.Next(50, 100)
        Attack = rnd.Next(15, 25)
        Defense = rnd.Next(5, 15)
    }
    if player.Language = "hu" then
        setColor ConsoleColor.Magenta
        printfn "\nEgy %s ellenséggel találkozol!" enemy.Name
        printfn "Ellenség statisztikái: HP=%d, Támadás=%d, Védekezés=%d" enemy.HP enemy.Attack enemy.Defense
        resetColor()
    else
        setColor ConsoleColor.Magenta
        printfn "\nYou encounter a %s enemy!" enemy.Name
        printfn "Enemy stats: HP=%d, Attack=%d, Defense=%d" enemy.HP enemy.Attack enemy.Defense
        resetColor()
    // ASCI szörny.
    displayAsciiGraphic "monster"
    // Harc start
    animateAction (if player.Language = "hu" then "Harc kezdete" else "Battle starting")
    
    let rec combatLoop enemyHP playerHP =
        if enemyHP <= 0 then 
            if player.Language = "hu" then
                setColor ConsoleColor.Green
                printfn "\nLegyőzted az ellenséget!"
            else
                setColor ConsoleColor.Green
                printfn "\nYou defeated the enemy!"
            resetColor()
            playerHP
        elif playerHP <= 0 then 
            if player.Language = "hu" then
                setColor ConsoleColor.Red
                printfn "\nMeghaltál a harcban!"
            else
                setColor ConsoleColor.Red
                printfn "\nYou have died in combat!"
            resetColor()
            0
        else
            if player.Language = "hu" then
                printfn "\nVálassz egy akciót:"
                printfn "1. Normál támadás"
                printfn "2. Speciális támadás (1.5x sebzés, de 50%% esély a sikertelenségre)"
                printfn "3. Védekezés (csökkenti az ellenség következő támadását)"
                printf "Választás: "
            else
                printfn "\nChoose an action:"
                printfn "1. Normal attack"
                printfn "2. Special attack (1.5x damage, 50%% chance to fail)"
                printfn "3. Defend (reduces enemy's next attack)"
                printf "Choice: "
            match Console.ReadLine() with
            | "1" ->
                let dmg = max 0 (player.Attack - (enemy.Defense / 2))
                if player.Language = "hu" then
                    printfn "\nNormál támadás! %d sebzést okozol." dmg
                else
                    printfn "\nNormal attack! You deal %d damage." dmg
                let newEnemyHP = enemyHP - dmg
                let enemyDmg = max 0 (enemy.Attack - player.Defense)
                if player.Language = "hu" then
                    printfn "Az ellenség %d sebzést okoz vissza." enemyDmg
                else
                    printfn "The enemy strikes back for %d damage." enemyDmg
                combatLoop newEnemyHP (playerHP - enemyDmg)
            | "2" ->
                if rnd.NextDouble() > 0.5 then
                    let dmg = max 0 ((int (float player.Attack * 1.5)) - enemy.Defense)
                    if player.Language = "hu" then
                        printfn "\nSpeciális támadás sikeres! %d sebzést okozol." dmg
                    else
                        printfn "\nSpecial attack successful! You deal %d damage." dmg
                    let newEnemyHP = enemyHP - dmg
                    let enemyDmg = max 0 (enemy.Attack - player.Defense)
                    if player.Language = "hu" then
                        printfn "Az ellenség %d sebzést okoz vissza." enemyDmg
                    else
                        printfn "The enemy strikes back for %d damage." enemyDmg
                    combatLoop newEnemyHP (playerHP - enemyDmg)
                else
                    if player.Language = "hu" then
                        printfn "\nSpeciális támadás sikertelen!"
                    else
                        printfn "\nSpecial attack failed!"
                    let enemyDmg = (max 0 (enemy.Attack - player.Defense)) + 5
                    if player.Language = "hu" then
                        printfn "Az ellenség egy extra támadással %d sebzést okoz." enemyDmg
                    else
                        printfn "The enemy deals an extra %d damage." enemyDmg
                    combatLoop enemyHP (playerHP - enemyDmg)
            | "3" ->
                if player.Language = "hu" then
                    printfn "\nVédekezel, így az ellenség kevesebb sebzést okoz."
                else
                    printfn "\nYou defend, reducing incoming damage."
                let enemyDmg = max 0 ((enemy.Attack - player.Defense) / 2)
                if player.Language = "hu" then
                    printfn "Az ellenség %d sebzést okoz." enemyDmg
                else
                    printfn "The enemy deals %d damage." enemyDmg
                combatLoop enemyHP (playerHP - enemyDmg)
            | _ ->
                if player.Language = "hu" then
                    printfn "\nÉrvénytelen választás, próbáld újra!"
                else
                    printfn "\nInvalid choice, try again!"
                combatLoop enemyHP playerHP
    let updatedPlayerHP = combatLoop enemy.HP player.HP
    if updatedPlayerHP > 0 then
        let gainedGold = rnd.Next(10, 30)
        if player.Language = "hu" then
            printfn "\nGyőzelem! %d aranyat nyersz." gainedGold
        else
            printfn "\nYou win! You receive %d gold." gainedGold
        // Minden harc után +25 XP
        let updatedPlayer = { player with HP = updatedPlayerHP; Gold = player.Gold + gainedGold; XP = player.XP + 25 }
        checkLevelUp updatedPlayer
    else
        { player with HP = 0 }

 // ---------- Történetmesélés ----------
let storyNarrative (player: PlayerState) =
    if player.Language = "hu" then
        setColor ConsoleColor.Blue
        printfn "\nEgy út elágazik előtted..."
        resetColor()
        printfn "1. Menj a misztikus erdőbe."
        printfn "2. Fedezd fel az elhagyatott kastélyt."
        printfn "3. Maradj a városban a pihenésért."
        printf "Választás: "
    else
        setColor ConsoleColor.Blue
        printfn "\nA path divides in front of you..."
        resetColor()
        printfn "1. Enter the mystical forest."
        printfn "2. Explore the abandoned castle."
        printfn "3. Stay in town to rest."
        printf "Choice: "
    match Console.ReadLine() with
    | "1" ->
        if player.Language = "hu" then
            printfn "\nAz erdő mélyén felfedezel egy különleges kristályt!"
        else
            printfn "\nDeep in the forest, you discover a mysterious crystal!"
        // Térkép ASCII grafika
        displayAsciiGraphic "map"
        let updatedInventory = "Misztikus Kristály" :: player.Inventory
        { player with Inventory = updatedInventory }
    | "2" ->
        if player.Language = "hu" then
            printfn "\nA kastélyban csapdák és kincsek várnak. Egy csapda aktiválódik, de elkerülöd."
        else
            printfn "\nIn the castle, traps and treasures await. You narrowly avoid a trap."
        // Kastély ASCII grafika
        displayAsciiGraphic "castle"
        let damage = rnd.Next(5, 15)
        if player.Language = "hu" then
            printfn "Sérülést szenvedsz: %d sebzés." damage
        else
            printfn "You take %d damage." damage
        let gainedGold = rnd.Next(20, 50)
        if player.Language = "hu" then
            printfn "Viszont találsz %d aranyat." gainedGold
        else
            printfn "But you find %d gold." gainedGold
        { player with HP = player.HP - damage; Gold = player.Gold + gainedGold }
    | "3" ->
        if player.Language = "hu" then
            printfn "\nA városban pihensz, és regenerálódik az életerőd."
        else
            printfn "\nYou rest in town, regenerating your health."
        // egyszerű text térkép
        displayAsciiGraphic "map"
        let healedHP = min 100 (player.HP + 30)
        if player.Language = "hu" then
            printfn "HP regenerálódik: %d -> %d" player.HP healedHP
        else
            printfn "Your HP increases: %d -> %d" player.HP healedHP
        { player with HP = healedHP }
    | _ ->
        if player.Language = "hu" then
            printfn "\nÉrvénytelen választás. A sors bizonytalan marad."
        else
            printfn "\nInvalid choice. Your fate remains uncertain."
        player

// ---------- Fő játékmenet ----------
let rec gameLoop (player: PlayerState) =
    if player.Language = "hu" then
        setColor ConsoleColor.Yellow
        printfn "\n-- A kaland folytatódik, %s! --" player.Name
        resetColor()
        printfn "Statisztikák: HP=%d, Támadás=%d, Védekezés=%d, Arany: %d, XP=%d, Szint=%d" 
                player.HP player.Attack player.Defense player.Gold player.XP player.Level
        printfn "Készletek: %A" player.Inventory
        printfn "\nVálassz egy akciót:"
        printfn "1. Logikai fejtörő"
        printfn "2. Küzdelem"
        printfn "3. Történetmesélés"
        printfn "q. Kilépés"
        printf "Parancs: "
    else
        setColor ConsoleColor.Yellow
        printfn "\n-- The adventure continues, %s! --" player.Name
        resetColor()
        printfn "Stats: HP=%d, Attack=%d, Defense=%d, Gold: %d, XP=%d, Level=%d" 
                player.HP player.Attack player.Defense player.Gold player.XP player.Level
        printfn "Inventory: %A" player.Inventory
        printfn "\nChoose an action:"
        printfn "1. Math Puzzle"
        printfn "2. Combat"
        printfn "3. Story"
        printfn "q. Quit"
        printf "Choice: "
    match Console.ReadLine() with
    | "1" ->
        solveMathPuzzle player.Language
        gameLoop player
    | "2" ->
        let updatedPlayer = combatEncounter player
        if updatedPlayer.HP <= 0 then
            updatedPlayer
        else
            gameLoop updatedPlayer
    | "3" ->
        let updatedPlayer = storyNarrative player
        gameLoop updatedPlayer
    | "q" | "Q" ->
        player
    | _ ->
        gameLoop player

// ---------- Nyelvválasztás ----------
let selectLanguage () =
    printfn "Válassz nyelvet / Choose language:"
    printfn "1. Magyar"
    printfn "2. English"
    printf "Választás / Choice: "
    match Console.ReadLine() with
    | "1" -> "hu"
    | "2" -> "en"
    | _ ->
        printfn "Érvénytelen választás, alapértelmezetten Magyar lesz."
        "hu"

// ---------- Főmenü ----------
let rec mainMenu () =
    let lang = selectLanguage ()
    if lang = "hu" then
        printfn "\n----- Logic Quest -----"
        printfn "1. Új játék (új profil)"
        printfn "2. Profil betöltése"
        printfn "3. Kilépés"
        printf "Választás: "
    else
        printfn "\n----- Logic Quest -----"
        printfn "1. New Game (create profile)"
        printfn "2. Load Profile"
        printfn "3. Exit"
        printf "Choice: "
    match Console.ReadLine() with
    | "1" ->
        if lang = "hu" then
            printf "Add meg a karaktered nevét: "
        else
            printf "Enter your character name: "
        let name = Console.ReadLine()
        let player = newPlayerProfile name lang
        if lang = "hu" then
            printfn "\nKaland kezdődik, %s! Nyomj entert a folytatáshoz..." player.Name
        else
            printfn "\nThe adventure begins, %s! Press Enter to continue..." player.Name
        Console.ReadLine() |> ignore
        let finalPlayer = gameLoop player
        if lang = "hu" then
            printfn "\nA kaland véget ért."
            printfn "Profil mentése? (i/n): "
        else
            printfn "\nThe adventure has ended."
            printfn "Save profile? (y/n): "
        if Console.ReadLine().Trim().ToLower() = (if lang = "hu" then "i" else "y") then
            saveProfile finalPlayer
        mainMenu ()
    | "2" ->
        match tryLoadProfile () with
        | Some player ->
            if lang = "hu" then
                printfn "\nProfil betöltve: %s" player.Name
            else
                printfn "\nProfile loaded: %s" player.Name
            let finalPlayer = gameLoop player
            if lang = "hu" then
                printfn "\nA kaland véget ért."
                printfn "Profil mentése? (i/n): "
            else
                printfn "\nThe adventure has ended."
                printfn "Save profile? (y/n): "
            if Console.ReadLine().Trim().ToLower() = (if lang = "hu" then "i" else "y") then
                saveProfile finalPlayer
            mainMenu ()
        | None ->
            mainMenu ()
    | "3" ->
        if lang = "hu" then
            printfn "\nKilépsz a játékból. Viszlát!"
        else
            printfn "\nExiting game. Goodbye!"
    | _ ->
        mainMenu ()

[<EntryPoint>]
let main argv =
    mainMenu ()
    0
