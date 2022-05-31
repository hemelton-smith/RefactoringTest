using LegacyApp.Interfaces;
using LegacyApp.Repositories;
using LegacyApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LegacyApp.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ProveAddUser(args);
        }

        public static void ProveAddUser(string[] args)
        {
            /*
			 *	DO NOT CHANGE THIS FILE AT ALL
        	*/

            var userService = new UserService();
            var addResult = userService.AddUser("Joe", "Soap", "joe.soap@gmail.com", new DateTime(1993, 1, 1), 1);
            Console.WriteLine("Adding Joe Soap was " + (addResult ? "successful" : "unsuccessful"));
        }
    }
}
