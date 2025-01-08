# Dokumentacja Aplikacji Kina Studyjnego

## Wprowadzenie
Aplikacja kina studyjnego wspiera zarówno pracowników, jak i klientów. Zapewnia funkcjonalności takie jak zarządzanie filmami, kategoriami, seansami, pracownikami oraz rezerwacjami w panelu administracyjnym, a także przeglądanie harmonogramu, szczegółów filmów oraz rezerwację biletów z miejscami przez klientów.

## Główne funkcje aplikacji

### Moduł administracyjny
Umożliwia:

- Zarządzanie filmami: Dodawanie, edycja i usuwanie filmów.
- Zarządzanie kategoriami: Tworzenie i edytowanie kategorii filmowych.
- Zarządzanie seansami: Tworzenie harmonogramów i przypisywanie filmów do terminów.
- Obsługę zamówień: Zarządzanie rezerwacjami, zmiana statusów.
- Zarządzanie użytkownikami: Tworzenie i aktualizacja kont pracowników.

### Moduł kliencki
Umożliwia:

- Przeglądanie filmów: Szczegóły filmów, takie jak opis, obsada, reżyseria, plakaty i oceny.
- Harmonogram seansów: Lista seansów z możliwością rezerwacji miejsc.
- Rezerwację miejsc: Wybór seansu i miejsc w sali.
- Zarządzanie rezerwacjami: Śledzenie i aktualizacja rezerwacji.

## Architektura aplikacji
- Frontend:
    - React, TypeScript, Vite
    - Tailwind CSS, Tanstack 
- Backend:
    - ASP.NET Core
    - Entity Framework Core, MySQL

## Przyszłe plany
- Integracja płatności online (Stripe, PayU).
- System powiadomień SMS/Email.
- Algorytm rekomendacji filmów.
- Aplikacja mobilna.

## Demo

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
        varchar(4)      FourDigitCode       "Kod rezerwacji"
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
        varchar(100)    TrailerUrl          "Url zwiastunu"
        varchar(100)    BackgroundUrl       "Url tła"
        varchar(100)    Director            "Reżyser"
        varchar(100)    Cast                "Obsada"
        varchar(1000)   Description         "Opis filmu"
        double          Rating              "Ocena filmu (0-10)"
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

## Zrzuty ekranu

![Screenshot 1](images/main_page_1.png)
*Zrzut ekranu nr 1: Główna strona kina - przedstawia karuzelę z ostatnio dodanymi filamami oraz repertuarem na najbliższe 7 dni.*

![Screenshot 2](images/main_page_2.png)
*Zrzut ekranu nr 2: Strona konkrentego filmu - przedstawia kategorię, opinię, opis, czas trwania, 5 najbliższych seansów oraz zwiastun.*

![Screenshot 3](images/main_page_3.png)
*Zrzut ekranu nr 3: Strona wyboru miejsc - zielone miejsca są wolne, czerwone zajęte, pomarańczowe wybrane przez klienta.*

![Screenshot 4](images/main_page_4.png)
*Zrzut ekranu nr 4: Strona zamawiania filmu - klient potwierdza zamówienie i podaje swoje dane.*

![Screenshot 5](images/email_ticket.png)
*Zrzut ekranu nr 5: Email, który natychmiatowo przychodzi na maila, widzimy dane dotyczące seansu, naszych miejsc oraz kod identyfikujący nasz bilet.*

![Screenshot 6](images/admin_login.png)
*Zrzut ekranu nr 6: Strona logowania dla pracowników kina - zalogować się mogą jedynie pracownicy, konta tworzy administrator.*

![Screenshot 7](images/admin_page_filmy.png)
*Zrzut ekranu nr 7: Strona z filmami - umożliwia podgląd, edycję, dodanie oraz usunięcie.*

![Screenshot 8](images/admin_page_film_joker.png)
*Zrzut ekranu nr 8: Strona do podglądu i edycji konkretnego filmu.*

![Screenshot 9](images/admin_page_kategorie.png)
*Zrzut ekranu nr 9: Strona z kategoriami filmów - umożliwia podgląd, edycję, dodanie oraz usunięcie.*

![Screenshot 10](images/admin_page_seanse.png)
*Zrzut ekranu nr 10: Strona z seansami - umożliwia podgląd, edycję, dodanie oraz usunięcie.*

![Screenshot 11](images/admin_page_pracownicy.png)
*Zrzut ekranu nr 11: Strona z pracownikami - umożliwia dodanie, usunięcie oraz reset hasła.*

![Screenshot 12](images/admin_page_nowy_pracownik.png)
*Zrzut ekranu nr 12: Strona dodawania nowego pracownika - automatycznie wygenerowane hasło zostanie wysłane na maila pracownika.*

![Screenshot 13](images/admin_page_zamowienia.png)
*Zrzut ekranu nr 13: Strona z zamówieniami - umożliwia wyszukiwanie po email oraz kodzie, opłatę zamówienia oraz anulowanie.*

![Screenshot 14](images/admin_page_zmiana_hasla.png)
*Zrzut ekranu nr 14: Strona podglądu użytkownika - umożliwia samodzielną edycję hasła.*

![Screenshot 15](images/email_nowy_pracownik_haslo.png)
*Zrzut ekranu nr 15: Email, który przychodzi do pracownika po założeniu konta lub odgórnym resecie hasła.*

## Zrzuty ekranu endpointów

![Screenshot 16](images/swagger_admin.png)
*Zrzut ekranu nr 16: Zrzut ekranu endpointów pracownika.*

![Screenshot 17](images/swagger_user.png)
*Zrzut ekranu nr 17: Zrzut ekranu endpointów klienta.*