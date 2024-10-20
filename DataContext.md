## Obiekty

**Seat** 
| Parametr  | Typ    | Opis           |
| :-------- | :----- | :------------- |
| `Id`      | `Guid` | Id siedzenia   |
| `Row`     | `char` | Litera rządu   |
| `Number`  | `int`  | Numer miejsca  |

**Category** 
| Parametr  | Typ      | Opis             |
| :-------- | :------- | :--------------- |
| `Id`      | `Guid`   | Id kategorii     |
| `Name`    | `string` | Nazwa kategorii  |

**Movie** 
| Parametr       | Typ        | Opis                |
| :------------- | :--------- | :------------------ |
| `Id`           | `Guid`     | Id filmu            |
| `Title`        | `string`   | Nazwa filmu         |
| `Duration`     | `TimeSpan` | Czas trwania filmu  | 
| `PosterUrl`    | `string`   | Url plakatu         |
| `Director`     | `string`   | Reżyser             |
| `Cast`         | `string`   | Obsada              |
| `Description`  | `string`   | Opis filmu          |
| `Rating`       | `double`   | Ocena filmu (0-10)  |
| `CategoryId`   | `Guid`     | Id kategorii        |

**Screening** 
| Parametr        | Typ               | Opis               |
| :-------------- | :---------------- | :----------------- |
| `Id`            | `Guid`            | Id seansu          |
| `MovieId`       | `Guid`            | Id filmu           |
| `StartDateTime` | `DateTimeOffset`  | Data rozpoczęcia   |
| `EndDateTime`   | `DateTimeOffset`  | Data zakończenia   |

**Reservation** 
| Parametr       | Typ      | Opis                    |
| :------------- | :------- | :---------------------- |
| `Id`           | `Guid`   | Id rezerwacji           |
| `Email`        | `string` | Email klienta           |
| `PhoneNumber`  | `string` | Numer telefonu klienta  |
| `Status`       | `enum`   | Status                  |
| `ScreeningId`  | `Guid`   | Id seansu               |

**ReservatedSeat** 
| Parametr         | Typ    | Opis                     |
| :--------------- | :----- | :----------------------- |
| `Id`             | `Guid` | Id rezerwacji siedzenia  |
| `SeatId`         | `Guid` | Id siedzenia             |
| `ReservationId`  | `Guid` | Id rezerwacji            |

**User**
| Parametr       | Typ        | Opis                                 |
| :------------- | :--------- | :----------------------------------- |
| `Id`           | `Guid`     | Id użytkownika                       |
| `IsAdmin`      | `Boolean`  | Czy użytkownik jest administratorem  |
| `Email`        | `string`   | Email użytkownika                    |
| `FirstName`    | `string`   | Imię użytkownika                     |
| `LastName`     | `string`   | Nazwisko użytkownika                 |
| `PasswordHash` | `string`   | Zabezpieczone hasło użytkownika      |
