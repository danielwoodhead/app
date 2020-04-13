﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace MyHealth.Extensions.Cryptography
{
    public static class HashExtensions
    {
        public static string Sha256(this string input)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        public static byte[] Sha256(this byte[] input)
        {
            if (input == null)
            {
                return null;
            }

            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(input);
            }
        }
    }
}
