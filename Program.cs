using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace console_json
{
    class Program
    {
        static Dictionary<string, int> accounts = new Dictionary<string, int>();
        
        static void Main(string[] args)
        {
            Console.WriteLine(RestoreUsers());

            //Adding First items to the dictionary
            if (accounts.Count == 0) {
                accounts.Add("Admin", 1645);
                accounts.Add("User", 2345);
            }

            //Console.WriteLine(accounts["Admin"]);

            Console.WriteLine(ChangeUser("Admin", 8888));            
            Console.WriteLine(BackupUsers(accounts));

            //Console.WriteLine(accounts["Admin"]);
        }

        static string ChangeUser(string username, int password) {

            if (accounts.ContainsKey(username)) {
                accounts[username] = password;
                return $"Details for [ {username} ] have been updated.";
            } else {
                return "User does not exist, nothing has been changed.";
            }
        }

        static string BackupUsers(Dictionary<string, int> users) {

            var backup = JsonConvert.SerializeObject(users);
            var date = DateTime.Now.Ticks;

            try {
                Directory.CreateDirectory("./Backup/");
                File.WriteAllText($"./Backup/backup-{date}.json", backup);
            } catch (IOException ex) {
                return $"An error occured - {ex}";
            }

            return "Backup completed successfully";
        }

        static string RestoreUsers() {

            try {
                var path = $"./Backup/backup-{NewestFile()}.json";
                
                var data = File.ReadAllText(path);

                //Console.WriteLine($"Selected File => {path}");

                accounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);

                return "Data Restored completed successfully";

            } catch (IOException ex) {
                return $"An error occured - {ex}";
            }
        }

        static long NewestFile() 
        {
            var files = Directory.GetFiles("./Backup/");
            List<Int64> temp = new List<Int64>();  // empty array

            foreach (string file in files) {

                var x = file.Remove(0,16);
                var mystr = x.Substring(0, x.Length-5);
                temp.Add(Int64.Parse(mystr));
            }

            //Console.WriteLine(string.Join(",", temp));

            var arr = temp.ToArray();
            var max = arr.Max(); 

            return max;
        }
    }
}
