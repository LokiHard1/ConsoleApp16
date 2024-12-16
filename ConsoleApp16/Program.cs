using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

class BankAccount
{
    public int AccountNumber { get; private set; }
    private string pinHash;
    private float balance;

    private static int GenerateAccountNumber()
    {
        Random random = new Random();
        return random.Next(100000, 999999);
    }

    private static string HashPin(string pin)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pin));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool ValidatePin(string pin)
    {
        return pin.Length == 4 && int.TryParse(pin, out _);
    }

    public BankAccount(string pin)
    {
        if (!ValidatePin(pin))
        {
            throw new ArgumentException("Пин код 4 цифры ");
        }
        this.AccountNumber = GenerateAccountNumber();
        this.pinHash = HashPin(pin);
        this.balance = 0;
    }


    public float GetBalance()
    {
        return this.balance;
    }

    public bool IsPinValid(string pin)
    {
        return HashPin(pin) == this.pinHash;
    }
}

class Program
{
    static List<BankAccount> accounts = new List<BankAccount>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить новый счет");
            Console.WriteLine("2. Показать все счета");
            Console.WriteLine("3. Показать баланс счета");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddNewAccount();
                    break;
                case "2":
                    ShowAllAccounts();
                    break;
                case "3":
                    ShowAccountBalance();
                    break;
            }
        }
    }

    static void AddNewAccount()
    {
        Console.Write("Введите пин код ");
        string pin = Console.ReadLine();
        try
        {
            BankAccount newAccount = new BankAccount(pin);
            accounts.Add(newAccount);
            Console.WriteLine($"Новый счет добавлен. Номер счета: {newAccount.AccountNumber}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void ShowAllAccounts()
    {
        Console.WriteLine("Список счетов:");
        foreach (var account in accounts)
        {
            Console.WriteLine(account.AccountNumber);
        }
    }

    static void ShowAccountBalance()
    {
        Console.Write("Введите номер счета: ");
        if (int.TryParse(Console.ReadLine(), out int accountNumber))
        {
            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                Console.WriteLine($"Баланс счета {accountNumber}: {account.GetBalance()}");
            }
        }
    }
}