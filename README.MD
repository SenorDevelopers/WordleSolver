## Wordle Solver 


![image](https://user-images.githubusercontent.com/43640455/204058097-cae2ffbf-0739-40c7-9fb1-bb53ec6fe9de.png)

### Team Members: 
- Achim-Panescu Silvian  https://github.com/SilvianAchim (Grupa 152)
- Apostol Ilie-Daniel https://github.com/apostoldaniel854 (Grupa 152)
- Dinu Matei-Alexandru https://github.com/MatthewAlgo  (Grupa 152)
- Scarlat Marius-Stefan https://github.com/marius004 (Grupa 152)

## Project Summary:
The average of guesses is ```3.986```

In ```solutions.txt``` you can find the gusses for each word in the database

## Technologies Used:
For the development of this project we have used .NET CORE for the API, and python for the player
## Installation Guide:

### API:
```
.NET Desktop Development(Visual Studio Extension)
ASP.NET and web development(Visual Studio Extension)
```

### Database:
```
To skip the calculation process you can restore the database from Database.bak file
```

### Client
```
pip3 install customtkinter
pip3 install requests
```

## Run the Project

### API:
```
1) open web api project folder
2) dotnet run web-api.csproj

have the web api running:
dotnet run WebApi.csproj

```

### Client:
```
cd player

# run in cli mode:
python3 main.py --cli

# run in gui mode:
python3 main.py --gui
```

### Bibliography:
- [Solving Wordle using information theory](https://www.youtube.com/watch?v=v68zYyaEmEA) (3Blue1Brown)
- [Solving Wordle using information theory](https://www.youtube.com/watch?v=fRed0Xmc2Wg&t=2s&ab_channel=3Blue1Brown) (3Blue1Brown)
- [Arhitectura Sistemelor de Calcul - Curs 0x02](https://cs.unibuc.ro/~crusu/asc/Arhitectura%20Sistemelor%20de%20Calcul%20(ASC)%20-%20Curs%200x02.pdf) (Cristian Rusu)
- [Information Theory applied to wordle](https://towardsdatascience.com/information-theory-applied-to-wordle-b63b34a6538e) (towardsdatascience.com)
- [Make http-client program python](https://linuxhint.com/make-http-client-program-python/) (linuxhint.com)
- [Multiprocessing Shared Memory](https://docs.python.org/3/library/multiprocessing.shared_memory.html) (docs.python.org)
