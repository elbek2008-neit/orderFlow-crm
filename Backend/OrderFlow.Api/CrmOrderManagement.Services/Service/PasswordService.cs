using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Core.Interfaces;
using System.Security.Cryptography;

namespace CrmOrderManagement.Services.Service
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 32; //256 bites
        private const int HashSize = 32; // 256 bites
        private const int iterations = 100000; // Кол иттерации для PBKDF2

        public string HashPassword(string password) { 
        
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password can not be empty", nameof(password));

            // Генерируем Соль
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            // Хешируем Пароль с солью
            using var pdkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var hash = pdkdf2.GetBytes(HashSize);

            // Объеденяем соль и хеш
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Вощзвращаем в формате Base64 
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashPassword) {

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashPassword))
                return false;

            try 
            {
                // Декодируем сохраненный хеш
                var hashBytes = Convert.FromBase64String(hashPassword);

                // Извлекаем Соль
                var salt = new Byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Занаво
                // Хешируем введенный пароль с той же солью
                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                var hash = pbkdf2.GetBytes(HashSize);

                // Сравниваем хеши
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }

                return true;
            }
            catch
            { 
                return false;
            }
        }

        public string GeneratedRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var password = new StringBuilder();

            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            foreach (byte b in randomBytes)
            {
                password.Append(validChars[b % validChars.Length]);
            }

            return password.ToString();
        }
    }
}
