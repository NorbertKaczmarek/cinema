## Database Tables

**Seats** 
| Parametr  | Typ            | Opis             |
| :-------- | :------------- | :--------------- |
| `Id`      | `char(36)`     | Id siedzenia     |
| `Row`     | `varchar(1)`   | Litera rządu     |
| `Number`  | `int`          | Numer miejsca    |

**Categories** 
| Parametr  | Typ            | Opis             |
| :-------- | :------------- | :--------------- |
| `Id`      | `char(36)`     | Id kategorii     |
| `Name`    | `varchar(100)` | Nazwa kategorii  |

**Movies** 
| Parametr       | Typ              | Opis                |
| :------------- | :--------------- | :------------------ |
| `Id`           | `char(36)`       | Id filmu            |
| `Title`        | `varchar(100)`   | Nazwa filmu         |
| `Duration`     | `time(6)`        | Czas trwania filmu  | 
| `PosterUrl`    | `varchar(100)`   | Url plakatu         |
| `Director`     | `varchar(100)`   | Reżyser             |
| `Cast`         | `varchar(100)`   | Obsada              |
| `Description`  | `varchar(100)`   | Opis filmu          |
| `Rating`       | `double`         | Ocena filmu (0-10)  |
| `CategoryId`   | `char(36)`       | Id kategorii        |

**Screenings** 
| Parametr        | Typ            | Opis                             |
| :-------------- | :------------- | :------------------------------- |
| `Id`            | `char(36)`     | Id seansu                        |
| `MovieId`       | `char(36)`     | Id filmu                         |
| `StartDateTime` | `datetime(6)`  | Data rozpoczęcia                 |
| `EndDateTime`   | `datetime(6)`  | Data zakończenia                 |

**Orders** 
| Parametr       | Typ             | Opis                             |
| :------------- | :-------------- | :------------------------------- |
| `Id`           | `char(36)`      | Id rezerwacji                    |
| `Email`        | `varchar(100)`  | Email klienta                    |
| `PhoneNumber`  | `varchar(100)`  | Numer telefonu klienta           |
| `Status`       | `varchar(100)`  | Status                           |
| `ScreeningId`  | `char(36)`      | Id seansu                        |

**OrderSeat** 
| Parametr       | Typ             | Opis                                 |
| :------------- | :-------------- | :----------------------------------- |
| `OrderId`      | `char(36)`      | Id rezerwacji                        |
| `SeatId`       | `char(36)`      | Id siedzenia                         |

**Users**
| Parametr       | Typ             | Opis                                 |
| :------------- | :-------------- | :----------------------------------- |
| `Id`           | `char(36)`      | Id użytkownika                       |
| `IsAdmin`      | `Boolean`       | Czy użytkownik jest administratorem  |
| `Email`        | `varchar(100)`  | Email użytkownika                    |
| `FirstName`    | `varchar(100)`  | Imię użytkownika                     |
| `LastName`     | `varchar(100)`  | Nazwisko użytkownika                 |
| `PasswordHash` | `varchar(100)`  | Zabezpieczone hasło użytkownika      |
