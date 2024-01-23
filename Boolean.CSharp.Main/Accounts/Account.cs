﻿using Boolean.CSharp.Main.Classes;
using Boolean.CSharp.Main.enums;
using Boolean.CSharp.Main.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boolean.CSharp.Main.Accounts
{
    public abstract class Account
    {
        private ICustomer _customer;
        private Branch _branch;

        public Branch Branch { get => _branch; }

        private List<ITransaction> _transactions = new List<ITransaction>();
        public List<ITransaction> Transactions { get => _transactions; set => _transactions = value; }

        public Account(ICustomer customer, Branch branch)
        {
            this._customer = customer;
            this._branch = branch;
        }

        public decimal Deposit(decimal amount)
        {
            ITransaction newTransaction = new Transaction(amount, GetBalance(), TransactionType.Credit);
            Transactions.Add(newTransaction);
            
            return newTransaction.NewBalance;
        }

        public decimal Withdraw(decimal amount)
        {
            ITransaction newTransaction = new Transaction(amount, GetBalance(), TransactionType.Debit);
            Transactions.Add(newTransaction);

            return newTransaction.NewBalance;
        }

        public decimal GetBalance() 
        {
             return (Transactions.Count > 0 ? Transactions.Last().NewBalance : 0);
        }

        public string GenerateBankStatement()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{"Date",10} || {"Credit",10} || {"Debit",10} || {"Balance",10}");
            foreach (ITransaction transaction in _transactions.OrderByDescending(t => t.Date).Where(t => t.Status == TransactionStatus.Approved))
            {
                stringBuilder.AppendLine(
                    $"{transaction.Date.ToShortDateString(),10}" +
                    $" || {(transaction.Type == TransactionType.Credit ? transaction.Amount : 0),10} " +
                    $"|| {(transaction.Type == TransactionType.Debit ? transaction.Amount : 0),10} || " +
                    $"{transaction.NewBalance,10} ");
            }
            Console.WriteLine("THIS IS THE METHOD:");
            Console.WriteLine( stringBuilder.ToString() ); //Visual 
            return stringBuilder.ToString();
        }
    }
}
