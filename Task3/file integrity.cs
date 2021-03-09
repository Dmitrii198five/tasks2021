using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace hashcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0]; // <path to the input file>
            string pathToCheck = args[1]; // <path to the directory containing the files to check>

            List<Task> tasks = taskParcer(inputFile);

            foreach (Task task in tasks) {
                checkTask(task, pathToCheck);
            }

        }

        public static void checkTask(Task task, string pathToCheck)
        {
            string fullPath = pathToCheck + Path.DirectorySeparatorChar + task.fileName;

            if (!File.Exists(fullPath)) { Console.WriteLine(task.fileName +  " NOT FOUND");return; }

            string hashResult = "";

            switch (task.hashType)
            {
                case "md5":
                    hashResult = CalculateMD5(fullPath);
                    break;
                case "sha1":
                    hashResult = CalculateSHA1(fullPath);
                    break;
                default://"SHA256":
                    hashResult = CalculateSHA256(fullPath);
                    break;
            }

            if (string.Equals(hashResult, task.hashToCheck, StringComparison.OrdinalIgnoreCase)) { Console.WriteLine(task.fileName + " OK"); return; }
            else { Console.WriteLine(task.fileName + " FAIL"); return; }
        }

        private static string CalculateMD5(string filename)
        {
            string result;

            using (MD5 mD5 = MD5.Create())
            {
                using (FileStream fileStream = File.OpenRead(filename))
                {
                    byte[] numArray = mD5.ComputeHash(fileStream);

                    result = BitConverter.ToString(numArray).Replace("-", String.Empty);
                }

                // Return the hexadecimal string.
                return result;
            }
        }

        private static string CalculateSHA1(string filename)
        {
            string result = "";

            using (SHA1 sha1 = SHA1.Create())
            {
                using (FileStream fileStream = File.OpenRead(filename))
                {
                    byte[] numArray = sha1.ComputeHash(fileStream);
                    
                    result = BitConverter.ToString(numArray).Replace("-", String.Empty);
                }

                // Return the hexadecimal string.
                return result;
            }
        }

        private static string CalculateSHA256(string filename)
        {
            string result = "";

            using (SHA256 mySHA256 = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filename))
                {
                    byte[] numArray = mySHA256.ComputeHash(fileStream);

                    result = BitConverter.ToString(numArray).Replace("-", String.Empty);
                }

                // Return the hexadecimal string.
                return result;
            }
        }

        private static List<Task> taskParcer(string pathUnparsedTask) 
        {
            List<Task> tasks = new List<Task>();

            string line;
            using (var f = new StreamReader(pathUnparsedTask, Encoding.Default))
            {
                while ((line = f.ReadLine()) != null)
                {
                    string[] splitLine = line.ToString().Split(' ');

                    tasks.Add(new Task(splitLine[0], splitLine[1], splitLine[2]));
                }
            }

            return tasks;
        }

    }

    class Task 
    {
        public string fileName;
        public string hashType;
        public string hashToCheck;

        public Task(string FileName, string HashType, string HashToCheck)
        {
            fileName = FileName;
            hashType = HashType;
            hashToCheck = HashToCheck;
        }
    }
}
