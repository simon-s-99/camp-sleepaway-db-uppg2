Databaser i ett Sammanhang, inlämning nr. 2
Gruppuppgift av: 
  Adam Kumlin | Samuel Lööf | Simon Sörqvist

⛺👩‍👧‍👦🏕️

Link to this projects board on Trello: https://trello.com/b/zJkVoxCf/camp-sleepaway-databaser-gruppuppgift-adam-k-samuel-l-simon-s

About halfway through the project we decided to make use of the "Issues" function in github (instead of Trello) which turned out to be a more handy type of workflow for us.

ATTENTION: To run this program you must run command "Update-Database" in Package Manager Console before attempting to run program. 

==================================================

Assignment:
Skapa en EF Core applikation (Enkel menybaserad console app) för att hålla reda på Camp
Sleepaways gäster, släktingar och personal och byggnader.

Databasen skall innehålla minst följande tabeller och deras lämpliga relationer och constraints.
Namnge entiteter och tabeller med lämpliga plural och singularnamn
Databasen skall ge en ögonblicksbild av situationen 
• Camper – Lägerdeltagare
• NextOfKin – Släktingar till Campers, endast dessa får besöka campers
• Councelor – Lägerledare
• Cabin – Stuga

En deltagare sover endast i en stuga men en stuga kan ha många deltagare, dock max 4 samt en
Councelor. En stuga får inte fyllas med campers om den inte har en councelor. Councelors får bytas
ut. 

En Counselor ansvarar för en stuga och endast en.

NextOfKin måste höra till en camper. En camper får ha valfritt antal NextOfKin, dvs så många som man vill ange. Det är frivilligt för er som
programmerare att tillåta en NextOfKin som har flera campers eller ej.

Councelors får vara NextOfKin men då har de en egen rad i NextOfKin tabellen, personer kan ha flera
roller men då ligger de i respektive tabell som motsvarar rollen. Vi hanterar inte detta i denna
applikation.

G kriterier
1. Skapa en EF code first applikation där alla ovanstående entiteter hanteras och där ni
utnyttjar annotations för att tex requiredfält och nycklar.
2. Välj lämpliga fält och datatyper samt grundläggande constraints för alla entiteter
3. Fyll på applikationen med seed-data:
a. Minst 18 campers
b. 3 counselors
c. minst 4 campers skall ha next of kin
d. 3 cabins,
4. Koppla campers till cabins, ge cabins counselors
5. Visa att man kan CRUDa ovanstående – enkel consoleapp
6. Skapa en rapportfunktion där man kan söka efter campers baserat på stuga eller counselor,
om det visar sig att en stuga saknar councelors så skall rapporten varna för det - med
användarinterface
7. Skapa en rapportfunktion som kan visa upp campers med eventuella next of kins 
sorterat på cabins - med användarinterface
8. Med användarinterface avses en enkel console app. Ni behöver inte implementera
avancerad felhantering av användarinput.
