namespace BizCardApp.Resources;

public static class AppStrings
{
    public static class Dialogs
    {
        public const string TitleWarning = "⚠️ Ostrzeżenie";
        public const string TitleSuccess = "✅ Sukces";
        public const string TitleError = "❌ Błąd";
        public const string TitleInfo = "ℹ️ Informacja";

        public const string Info =
                "Wizytownik\r\n" +
                "Wersja: 1.0.0\r\n\r\n" +
                "Autor: Hansik33\r\n" +
                "Repozytorium: github.com/Hansik33/BizCardApp\r\n\r\n" +
                "Aplikacja do zarządzania wizytówkami biznesowymi.";

        public const string UnableToConnectDatabase =
                "Nie można połączyć się z bazą danych!\r\n" +
                "Sprawdź plik konfiguracyjny (appsettings.json) i uruchom ponownie aplikację.\r\n" +
                "Jeśli problem nadal występuje, zaimportuj ponownie plik skryptu SQL (BizCardApp.sql) bazy danych.";

        public static class BusinessCard
        {
            public const string AddSuccess = "Wizytówka została pomyślnie dodana.";
            public const string UpdateSuccess = "Wizytówka została pomyślnie zaktualizowana.";
            public const string DeleteConfirmation = "Czy na pewno chcesz usunąć tę wizytówkę?";
            public const string DeleteSuccess = "Wizytówka została pomyślnie usunięta.";

            public const string ClosingWithoutSavingConfirmation =
                "Wprowadzone zmiany nie zostały zapisane.\r\n" +
                "Czy na pewno chcesz zamknąć okno bez zapisywania?";

            public static class Required
            {
                public static class FirstName
                {
                    public const string Empty = "Imię nie może być puste!";
                    public const string TooShort = "Imię jest za krótkie!";
                    public const string TooLong = "Imię jest za długie!";
                    public const string InvalidCharacters = "Imię zawiera niedozwolone znaki!";
                }

                public static class LastName
                {
                    public const string Empty = "Nazwisko nie może być puste!";
                    public const string TooShort = "Nazwisko jest za krótkie!";
                    public const string TooLong = "Nazwisko jest za długie!";
                    public const string InvalidCharacters = "Nazwisko zawiera niedozwolone znaki!";
                }

                public static class FullName
                {
                    public const string NotUnique = "Wizytówka z takim imieniem i nazwiskiem już istnieje!";
                }
            }

            public static class Optional
            {
                public static class Company
                {
                    public const string TooShort = "Nazwa firmy jest za krótka!";
                    public const string TooLong = "Nazwa firmy jest za długa!";
                    public const string InvalidCharacters = "Nazwa firmy zawiera niedozwolone znaki!";
                }

                public static class JobTitle
                {
                    public const string TooShort = "Stanowisko jest za krótkie!";
                    public const string TooLong = "Stanowisko jest za długie!";
                    public const string InvalidCharacters = "Stanowisko zawiera niedozwolone znaki!";
                }

                public static class Phone
                {
                    public const string TooShort = "Numer telefonu jest za krótki!";
                    public const string TooLong = "Numer telefonu jest za długi!";
                    public const string InvalidCharacters = "Numer telefonu zawiera niedozwolone znaki!";
                    public const string InvalidFormat = "Numer telefonu ma nieprawidłowy format!";
                }

                public static class Email
                {
                    public const string TooLong = "Adres e-mail jest za długi!";
                    public const string InvalidCharacters = "Adres e-mail zawiera niedozwolone znaki!";
                    public const string InvalidFormat = "Adres e-mail ma nieprawidłowy format!";
                }

                public static class Address
                {
                    public const string TooShort = "Adres jest za krótki!";
                    public const string TooLong = "Adres jest za długi!";
                    public const string InvalidCharacters = "Adres zawiera niedozwolone znaki!";
                    public const string InvalidFormat = "Adres ma nieprawidłowy format!";
                }
            }
        }
    }
}