# Submarine Game
## Feladat
Készítsünk programot a következő játékra.
A játékban egy tengeralattjárót kell irányítanunk a képernyőn (balra, jobbra, fel, illetve le), amely
felett ellenséges hajók köröznek, és folyamatosan aknákat dobnak a tengerbe. Az aknáknak három
típusa van (könnyű, közepes, nehéz), amely meghatározza, hogy milyen gyorsan süllyednek a vízben
(minél nehezebb, annál gyorsabban).
Az aknákat véletlenszerűen dobják a tengerbe, ám mivel a hajóskapitányok egyre türelmetlenebbek,
egyre gyorsabban kerül egyre több akna a vízbe. A játékos célja az, hogy minél tovább elkerülje az
aknákat. A játék addig tart, ameddig a tengeralattjárót el nem találta egy akna.
A program biztosítson lehetőséget új játék kezdésére, valamint játék szüneteltetésére (ekkor nem
telik az idő, és nem mozog semmi a játékban). Ismerje fel, ha vége a játéknak, és jelenítse meg,
mennyi volt a játékidő. Ezen felül szüneteltetés alatt legyen lehetőség a játék elmentésére, valamint
betöltésére.
![Game](https://github.com/ozoli99/SubmarineGame-WF/blob/main/SubmarineGameWF.png)
## A feladat elemzése
- A játékban egy játékos vesz részt, valamint egy nehézségi szint van, ugyanakkor az aknák
három típus valamelyikét képviselik: könnyű, közepes, nehéz.
- A program indításkor automatikusan új játékot kezd, és hat darab véletlenszerűen generált
nehézségű aknák indít a pálya aljáról induló tengeralattjáróra.
- A feladatot egyablakos asztali alkalmazásként Windows Forms grafikus felülettel valósítjuk
meg.
- Az ablakban elhelyezünk egy menüt egyetlen menüponttal: File (New game, Load game…,
Save game…, Exit). Az ablak alján pedig a státuszsor kap helyet, amely az eltelt időt, valamint
a már felrobbant aknák számát jelzi.
- Mind a tengeralattjárót, mind az aknákat képszerűen jelenítjük meg, és a tengeralattjáró
billentyűzeten történő gombnyomásra (ez történhet a nyilakkal, vagy az AWSD betűkkel)
változtatja a pozícióját a pályán belül. Ezen felül a Space lenyomására tudjuk szüneteltetni a
játékot, ekkor egy dialógusablak felugrik, valamint az idő és a játék menete megáll. Az ablak
lezárása után a játék folytatódik.
- A játék automatikusan feldob egy dialógusablakot, amikor vége a játéknak, ezen megjelenik
az eltelt idő, valamint a felrobbant aknák száma. Elfogadása után új játék kezdődik.
- Szintén dialógusablakkal végezzük el a mentést, illetve a betöltést, a fájlneveket a
felhasználó adja meg. Valamint a játék menüsorból való bezárása esetén is egy dialógusablak
bizonyosodik meg a szándékunkról.
![Use case diagram](https://github.com/ozoli99/SubmarineGame-WF/blob/main/Submarine%20Game%20Use%20Case%20Diagram.jpeg)
## Tervezés
### Programszerkezet:
- A programot háromrétegű architektúrában valósítjuk meg. A megjelenítés a **View**,
a modell a **Model**, míg a perzisztencia a **Persistence** névtérben helyezkedik el.
Továbbá a rétegeket külön projektként adjuk hozzá az újrafelhasználhatóság
érdekében.
![Package diagram](https://github.com/ozoli99/SubmarineGame-WF/blob/main/Package%20diagram.jpeg)
### Perzisztencia:
- Az adatkezelés feladata a tengeralattjáró, valamint az aknák helyzetével,
tulajdonságaival kapcsolatos információk tárolása, valamint a betöltés/mentés
biztosítása
- A **Shape** osztály valósítja meg a tengeralattjáró, valamint az aknák típusát, tárolja a
koordinátákat (**X**, **Y**), méreteket (**Width**, **Height**), az aknák esetén a súlyt (**Weight**),
valamint, hogy aknáról vagy tengeralattjáróról van-e szó (**ShapeType.Submarine**,
**ShapeType.Mine**).
- A hosszú távú adattárolás lehetőségeit az **IPersistence** interfész adja meg, amely
lehetőséget ad a játékállás (vagyis a tengeralattjáró, valamint aknák
tulajdonságainak) betöltésére (**Load**), valamint mentésére (**Save**).
- Az interfészt szöveges fájl alapú adatkezelésre a **TextFilePersistence** osztály
valósítja meg. A fájlkezelés során fellépő hibákat a **DataException** kivétel jelzi.
- Program az adatokat szöveges fájlként tudja eltárolni, melyek az **smg** kiterjesztést
kapják. Ezeket az adatokat a programban bármikor be lehet tölteni, illetve ki lehet
menteni az aktuális állást.
- A fájl első sora megadja a játékidőt, valamint a már felrobbant aknák számát. A
második sora a tengeralattjáró adatait, a többi sora pedig az aknák adatait,
szóközökkel elválasztva. Az első soron kívül mindegyik sor 6 számot tartalmaz (típus,
x, y, szélesség, magasság, súly).
### Modell:
- A modell lényegi részét a **SubmarineGameModel** osztály valósítja meg, amely
beállítja a megfelelő koordinátákat (ellenőrzés után), valamint a többi paramétert,
mint például a játékidő (**gameTime**), felrobbant aknák száma
(**_destroyedMineCount**). A típus lehetőséget ad új játék kezdésére (**NewGame**),
valamint lépésre a tengeralattjáróval (**Submarine_MoveUp**,
**Submarine_MoveDown**, **Submarine_MoveLeft**, **Submarine_MoveRight**), valamint
beállítja az aknák koordinátáit (**MoveMines**), és generálja őket
(**GenerateStartingMines**, **AddMine**). Ezen túl a játékot is ellenőrzi minden
koordinátaváltozás után (**CheckGame**).
- A tengeralattjáró koordinátaváltozásáról a **SubmarineMoved** esemény, egy akna
pályaelhagyásáról a **MineDestroyed** esemény, míg a játék végéről a **GameOver**
esemény, a játék szüneteltetéséről a **TimePaused** esemény tájékoztat. Az
események argumentuma (**SubmarineEventArgs**) tárolja a játékidőt, a már
felrobban aknák számát, valamint a mozgás megfelelő irányát.
- A modell példányosításkor megkapja az adatkezelés felületét, amelynek
segítségével lehetőséget ad betöltésre (**LoadGame**) és mentésre (**SaveGame**). Ezt a
**_persistence** adattagban tárolja.
### Nézet:
- A nézetet a **GameForm** osztály biztosítja, amely tárolja a modell egy példányát
(**_model**).
- A tengeralattjárót egy inicializálás során létrehozott, az aknákat pedig dinamikusan
létrehozott képek (**PictureBox**) reprezentálják. Az aknák esetén ezeket egy listában
tároljuk. A felületen létrehozzuk a megfelelő menüpontot (**File**), illetve a státuszsort,
valamint dialógusablakokat, és a hozzájuk tartozó eseménykezelőket. A kezdeti
állapot előállítása a konstruktor és a **NewGame** metódus segítségével történik.
- Három különböző időzítő tartozik a nézethez. A **_timer** felelős az aknák
mozgatásáért, a **_gameTimer** felel a játék közben eltelt idő mutatásáért, valamint a
**_minesTimer** felelős az aknák generálásáért. Ezek indításáért a **StartTimers**,
leállításáért a **StopTimers** metódusok felelnek.
### A program teljes statikus szerkezete:
![Class diagram](https://github.com/ozoli99/SubmarineGame-WF/blob/main/Class%20diagram.jpeg)
