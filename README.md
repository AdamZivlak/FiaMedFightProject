# README #

This README details how to set up and run 'Fia med Fight'

## What is this repository for? ###

* Repo för fia med knuff. För grupp 3 på Mölk YH program: Mjukvaruutvecklare
* Version pre-Alpha

### Projektbeskrivning ###

I en värld med massiva spelvärldar, ständig variation och komplexa regler, kommer vi allt längre ifrån självklarheten i de klassiska spelen: Backgammon, Fia-med-knuff, Fyra-i-rad, Othello, Kalaha, och många fler. För spelentusiaster av den gamla skolan saknas dock en brygga över till den moderna brädspelsvärlden, och till den digitala spelvärlden. Det saknas en plats där enkelhet och spelvariation möts, en plats där fysiska brädspelare kan doppa tårna i digitala spel och mötas av något bekant, och en plats där gamla favoriter kan utökas med moderna regler och spelvariationer, där gamla och unga, spelkonservatister och spelmodernister kan glädjas tillsammans. Den platsen vill vi skapa.

Det gör vi med FiaMedFight - det klassiska brädspelet med en RPG-twist. 

Grunden i spelet är detsamma: 
2-4 lag slår tärningar för att ta sina pjäser från sin startgrop, runt en spelplan och in i mål, och om de hamnar på samma ruta blir det “knuff”. 

Hur det skiljer sig från klassikern: 
1. En knuff är inte så enkel som du kanske minns. Istället för att försvarslöst kastas tillbaka till din startgrop får du likt ett RPG en chans att försvara dig: båda spelarna slår tärningar och applicerar eventuella krafter, och resultatet avgör vem som förlorar striden och blir bortknuffad. 
2. Spelplanen är digital och spelas på datorn med skärm, tangentbord och mus. I första ledet spelas allt lokalt på samma maskin, men det ska därefter utvecklas med möjligheten till onlinespel.
3. För att öka utmaningen ytterligare kan spelplanen genereras med hinder, fällor och genvägar.
4. Spelarna ska kunna ha olika krafter, som ger dem fördelar och nackdelar under spelets gång. Dessa kan vara temporära (användas en gång), eller konstanta.
5. Spelarna ska kunna spela i par med upp till 8 spelare per lag.
6. Spelet ska använda mer än en tärning, för att kunna avgöra vissa specialhändelser.

## How do I get set up? ##

The project is currently in development and does not include any builds. To run it, you have to clone the repo, open the project in Visual Studio and build it from there.

### Prerequisites ###
* [Microsoft Visual Studio](https://visualstudio.microsoft.com)
* [Universal Windows Platform development for Visual Studio (with Windows 10 SDK (10.0.19041.0)](https://visualstudio.microsoft.com/vs/features/universal-windows-platform/)

## Loading the project ##

* Launch Visual Studio
* Press 'File -> Open -> Project/Solution...' in the top menu bar
* Navigate to and choose the '.\FiaMedFight\FiaMedFight.csproj' file and press the 'Open' button
* The project should be loaded into your Visual Studio process.

## Building and running a debug version of the application ##

* After loading the project into Visual Studio, press 'F5' to build and launch the app in Debug mode. To launch without debugging, press 'Ctrl + F5' instead.

## Notes ##

The provided program is internal and, therefore, is meant to be used only within its assembly. For further information or specific usage cases, refer to the inline comments and documentation.

Authors: Sebastian Senic, Adam Zivlak, Minna de Verdier, Mustafa Salahuddin
Date: 2024-03-15