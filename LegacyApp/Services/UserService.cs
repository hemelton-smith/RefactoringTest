using LegacyApp.Constants;
using LegacyApp.Interfaces;
using LegacyApp.Models;
using LegacyApp.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LegacyApp.Services
{
    public class UserService : IUserService
    {
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname) || !email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            var clientRepository = new ClientRepository(null);
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            DoCreditCheck(user, client.Name);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserRepository.AddUser(user);

            return true;
        }

        private static void DoCreditCheck(User user, string clientName)
        {
            user.HasCreditLimit = clientName != ClientType.VeryImportantClient;

            if (clientName == ClientType.VeryImportantClient)
                return;

            using (var userCreditService = new UserCreditServiceClient())
            {
                var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                creditLimit = clientName == ClientType.ImportantClient ? (creditLimit * 2) : creditLimit;
                user.CreditLimit = creditLimit;
            }
        }
    }
}

