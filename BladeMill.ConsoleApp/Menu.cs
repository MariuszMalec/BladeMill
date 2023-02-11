using BladeMill.BLL.DAL;
using BladeMill.BLL.DatatBaseAcess;
using BladeMill.BLL.Repositories;
using BladeMill.BLL.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace BladeMill.ConsoleApp
{
    public class Menu
    {
        private static readonly char _indicator = '►';
        private bool _isWorking = true;
        private bool _isDataSourceSelected;

        private readonly List<string> _dataSourceList = new()
        {
            { _indicator + " Uzytkownicy BladeMilla" },
            { "  Zamknij aplikację" }
        };

        private readonly List<string> _menuList = new()
        {
            { _indicator + " Wyświetl klasyfikację użytkowników" },
            { "  Dodaj nowego użytkownika" },
            { "  Edytuj wybranego użytkownika" },
            { "  Wybierz ponownie źródło danych" },
            { "  Zamknij aplikację" }
        };

        private readonly IBaseRepository<UserDto> Users;

        public void StartProgram()
        {
            while (_isWorking)
            {
                Console.Clear();

                if (!_isDataSourceSelected)
                {
                    Console.WriteLine("+++ WITAJ W APLIKACJI +++\n");
                    SelectDataSource();
                    _isDataSourceSelected = true;
                }
                else
                {
                    SelectActionFromMenu();
                }
            }
        }

        private void SelectActionFromMenu()
        {
            Console.WriteLine("MENU:\n");

            switch (SelectPositionFromMenu(_menuList))
            {
                case 0: // Wyswietl klasyfikacje uzytkowników
                    Console.Clear();
                    Console.WriteLine(_menuList[0] + "\n");

                    var userService = new UserServiceWithoutDatabase();
                    var users = userService.GetAll();
                    users.ForEach(u => Console.WriteLine($"{u.ToString()}"));

                    ConnectToMsSqldataBase();

                    //PrintInfo.UserClassification(_usersRepository);

                    BackToMenu();
                    break;
                case 1: // Dodaj nowego uzytkownika
                    Console.Clear();
                    Console.WriteLine(_menuList[1] + "\n");

                    BackToMenu();
                    break;
                case 2: // Edytuj wybranego uzytkownika
                    Console.Clear();
                    Console.WriteLine(_menuList[2] + "\n");

                    BackToMenu();
                    break;
                case 3: // Wybierz ponownie zródlo danych
                    Console.Clear();
                    _isDataSourceSelected = false;
                    break;
                case 4: // Zamknij aplikacje
                    Console.Clear();
                    _isWorking = false;
                    break;
            }
        }

        private void ConnectToMsSqldataBase()
        {
            var connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Uzytkownicy;Trusted_Connection=True;MultipleActiveResultSets=True;");            

            //var connection = new SqliteConnection("Data Source=C:\\temp\\Databases\\MainAppDb.db");
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            //var connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Uzytkownicy;Trusted_Connection=True;MultipleActiveResultSets=True;");

            connection.Open();

            try
            {
                var connectionExist = IsConnectionMsql(connection);
                //var connectionExist = IsConnectionSqlite(connection);

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;

                using (SqlCommand command = new SqlCommand("SELECT TOP 2 * FROM Uzytkownicy", connection))


                //using (SqlDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        Console.WriteLine("{0} {1} {2}",
                //            reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                //    }
                //}
            
                using (var context = new ApplicationDbContext(options))
                {
                    //context.Database.EnsureCreated();
                    //init data

                    var user = new UserDto() { Created = DateTime.Now , Id=10,FirstName="Gal", LastName="Anonim",FullName="Eangle",Sso=123123123};

                    context.Uzytkownicy.Add(user);

                    context.SaveChangesAsync();

                    var user2 = context.Uzytkownicy.FindAsync(10).Result;

                    Console.WriteLine(user2.LastName);

                }

            }
            finally
            {
                connection.Close();
            }
        }
        private bool IsConnectionMsql(SqlConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                return false;
            }
            return true;
        }
        private bool IsConnectionSqlite(SqliteConnection connection)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                return false;
            }
            return true;
        }

        private void SelectDataSource()
        {
            Console.WriteLine("Wybierz źródło danych dla aplikacji:\n");

            var index = SelectPositionFromMenu(_dataSourceList);

            switch (index)
            {
                case 0: // Plik
                    Console.Clear();
                    try
                    {
                        //_usersRepository.AddFileDataToRepository();
                        //_placesRepository.AddFileDataToRepository();
                    }
                    catch (FileNotFoundException)
                    {
                        var errorMessage = "[ERROR] Nie znaleziono pliku! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }
                    catch (IOException)
                    {
                        var errorMessage = "[ERROR] Błąd odczytu pliku! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }
                    catch (ArgumentNullException)
                    {
                        var errorMessage = "[ERROR] Plik jest pusty! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }

                    break;
                case 1: //API
                    Console.Clear();
                    try
                    {
                        //_usersRepository.AddFileDataToRepository();
                    }
                    catch (FileNotFoundException)
                    {
                        var errorMessage = "[ERROR] Nie znaleziono pliku! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }
                    catch (IOException)
                    {
                        var errorMessage = "[ERROR] Błąd odczytu pliku! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }
                    catch (ArgumentNullException)
                    {
                        var errorMessage = "[ERROR] Plik jest pusty! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }

                    try
                    {
                        //_placesRepository.AddApiDataToRepository(
                        //    ReadDataFromConsole.GetLatitudeFromConsole(),
                        //    ReadDataFromConsole.GetLongitudeFromConsole(),
                        //    ReadDataFromConsole.GetRadiusFromConsole(),
                        //    ReadDataFromConsole.GetApiKeyFromConsole()
                        //);
                    }
                    catch (Exception)
                    {
                        var errorMessage = "[ERROR] Błąd komunikacji z API! Czy chcesz wybrać inne źródło danych?";
                        //HandleDataSourceError(index, errorMessage);
                    }

                    break;
                case 2: // Zamknij aplikacje
                    _isWorking = false;
                    break;
            }
        }
        private int SelectPositionFromMenu(List<string> menu)
        {
            var index = 0;

            while (true)
            {
                Console.SetCursorPosition(0, 4);

                PrintMenuPositions(menu);
                PrintMoveLegend();

                var move = Console.ReadKey(true).Key;

                if (move == ConsoleKey.DownArrow && index < menu.Count - 1)
                {
                    menu[index] = menu[index].Replace(_indicator, ' ');
                    index++;
                    menu[index] = _indicator + menu[index].Substring(1);
                }
                else if (move == ConsoleKey.UpArrow && index > 0)
                {
                    menu[index] = menu[index].Replace(_indicator, ' ');
                    index--;
                    menu[index] = _indicator + menu[index].Substring(1);
                }
                else if (move == ConsoleKey.Enter)
                {
                    menu[index] = menu[index].Replace(_indicator, ' ');
                    menu[0] = _indicator + menu[0].Substring(1);
                    return index;
                }
            }
        }

        private void PrintMenuPositions(List<string> menu)
        {
            foreach (var position in menu)
            {
                if (position.Contains("Zamknij aplikację") ||
                    position.Contains("Wybierz ponownie źródło danych"))
                {
                    Console.WriteLine();
                }

                if (position.Contains(_indicator))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\r{position}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"\r{position}");
                }
            }
        }

        private void BackToMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n\n '{ConsoleKey.Enter.ToString().ToUpper()}' - powrót do MENU");
            Console.ResetColor();
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }

        private void PrintMoveLegend()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                $"\n\n '{ConsoleKey.UpArrow}' - w górę || '{ConsoleKey.DownArrow}' - w dół || '{ConsoleKey.Enter.ToString().ToUpper()}' - wybierz ");
            Console.ResetColor();
        }
    }
}