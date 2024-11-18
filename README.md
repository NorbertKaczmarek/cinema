# System do zarządzania kinem studyjnym

### main
[cinema.nkaczmarek.pl](https://cinema.nkaczmarek.pl)\
[admin panel](https://cinema.nkaczmarek.pl/admin)\
[swagger](https://cinema.nkaczmarek.pl/swagger)

### develop
[develop.cinema.nkaczmarek.pl](https://develop.cinema.nkaczmarek.pl)\
[admin panel](https://develop.cinema.nkaczmarek.pl/admin)\
[swagger](https://develop.cinema.nkaczmarek.pl/swagger)

## Database diagram

```mermaid
erDiagram
    ORDER       }o--||  SCREENING   : "includes"
    SCREENING   }o--||  MOVIE       : "is for"
    MOVIE       }o--||  CATEGORY    : "contains"
    
    ORDER       ||--|{  ORDERSEAT   : "includes"
    ORDERSEAT   }|--||  SEAT        : "assigned to"

    ORDER {
        char(36)        Id              PK  "Id rezerwacji"
        varchar(100)    Email               "Email klienta"
        varchar(100)    PhoneNumber         "Numer telefonu klienta"
        varchar(100)    Status              "Status rezerwacji"
        char(36)        ScreeningId     FK  "Id seansu"
    }

    SCREENING {
        char(36)        Id              PK  "Id rezerwacji"
        datetime(6)     StartDateTime       "Data rozpoczęcia"
        datetime(6)     EndDateTime         "Data zakończenia"
        char(36)        MovieId         FK  "Id filmu"
    }
    
    MOVIE {
        char(36)        Id              PK  "Id filmu"
        varchar(100)    Title               "Nazwa filmu"
        int             DurationMinutes     "Czas trwania filmu"
        varchar(100)    PosterUrl           "Url plakatu"
        varchar(100)    Director            "Reżyser"
        varchar(100)    Cast                "Obsada"
        varchar(1000)   Description         "Opis filmu"
        datetime(6)     Rating              "Ocena filmu (0-10)"
        char(36)        CategoryId      FK  "Id kategorii"
    }
    
    CATEGORY {
        char(36)        Id              PK  "Id kategorii"
        varchar(100)    Name                "Nazwa kategorii"
    }
    
    SEAT {
        char(36)        Id              PK  "Id siedzenia"
        varchar(1)      Row                 "Litera rządu"
        int             Number              "Numer miejsca"
    }
    
    ORDERSEAT {
        char(36)        OrderId         PK,FK  	"Id rezerwacji"
        char(36)        SeatId          PK,FK  	"Id siedzenia"
    }
    
    USER {
        char(36)        Id              PK  	"Id użytkownika"
        tinyint(1)      IsAdmin             	"Czy użytkownik jest administratorem"
        varchar(100)    Email               	"Email użytkownika"
        varchar(100)    FirstName           	"Imię użytkownika"
        varchar(100)    LastName            	"Nazwisko użytkownika"
        varchar(100)    Salt		        	"Salt użytkownika"
        varchar(100)    SaltedHashedPassword	"Zabezpieczone hasło użytkownika"
    }
```
