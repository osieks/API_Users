# API_Users
Projekt ten zapewnia API wraz z bazą danych, które umożliwiają rejestrację użytkowników, 
wyświetlanie listy zarejestrowanych użytkowników oraz pobieranie danych wybranego użytkownika na podstawie adresu e-mail i hasła. 
Dane są przechowywane w bazie danych, która zostanie zainicjowana na początku działania aplikacji, jeśli jeszcze nie istnieje. 
Swagger został użyty do udokumentowania API.

Moduł obsługujący rejestrację konta

    Opis zadania:

    Przygotuj API wraz z warstwą bazy danych, udostępniające trzy metody:

     Metodę rejestrującą użytkownika realizującą:
    - Walidację pól rejestracji
    a. Pól obowiązkowych: Imię, Nazwisko, Hasło, PESEL, E-mail, Telefon
    b. Opcjonalnych: Wiek, średnie zużycie prądu z ostatnich 3 miesięcy w kWh (z dokładnością do trzech miejsc po przecinku
    - Obsługę błędów
    - Bezpieczny zapis użytkownika do bazy (szyfrowanie hasła)
    Metodę dostarczającą listę zarejestrowanych użytkowników
    Metodę pobierającą dane nt. wybranego użytkownika na podstawie E mail+Hasło

    Dane powinny być zapisywane do bazy danych, w razie braku bazy powinna być inicjalizowana na starcie aplikacji. Rozwiązanie powinno być wyposażone w Swaggera. 
