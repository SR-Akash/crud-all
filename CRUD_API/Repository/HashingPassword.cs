using CRUD_API.DbContexts;
using CRUD_API.Helper.AES_AdvanceEncryptionStandard;
using CRUD_API.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_API.Repository
{
    public class HashingPassword : IHashingPassword
    {
        private readonly DbContextCom _dbContext;
        public readonly AesModel _model;

        public HashingPassword(DbContextCom dbContext, AesModel model)
        {
            _dbContext = dbContext;
            _model = model;
        }


        static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // Adjust the size based on your security requirements
                rng.GetBytes(salt);
                return salt;
            }
        }

        static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Concatenate the salt and hashed password for storage
                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }


        public async Task<string> CreateUser(UserDTO create)
        {
            string password = create.ConfirmPassword;

            byte[] saltBytes = GenerateSalt();
            // Hash the password with the salt
            string hashedPassword = HashPassword(password, saltBytes);
            // Convert the byte array to a Base64-encoded string
            string base64Salt = Convert.ToBase64String(saltBytes);

            // Store the base64Salt in your database

            // When retrieving from the database, convert it back to a byte array
            byte[] retrievedSaltBytes = Convert.FromBase64String(base64Salt);

            var user = new Models.Usertest
            {
                ConfirmPassword = hashedPassword,
                Email = "",
                IsActive = true,
                LastActiondatetime = DateTime.Now,
                Mobile = create.MobileNo,
                Password = base64Salt,
                UserName = create.UserName,
                Salt = retrievedSaltBytes
            };
            await _dbContext.Usertests.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return "User added successfully";
        }
        public async Task<string> UserVerify(UserDTO verify)
        {

            // In a real scenario, you would retrieve these values from your database
            var user = _dbContext.Usertests.Where(x => x.Mobile == verify.MobileNo).Select(x => x).FirstOrDefault();

            string storedHashedPassword = user.ConfirmPassword;// "hashed_password_from_database";
            //string storedSalt = user.Salt; //"salt_from_database";
            byte[] storedSaltBytes = user.Salt;
            string enteredPassword = verify.ConfirmPassword; //"user_entered_password";

            // Convert the stored salt and entered password to byte arrays
            // byte[] storedSaltBytes = Convert.FromBase64String(user.Salt);
            byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);

            // Concatenate entered password and stored salt
            byte[] saltedPassword = new byte[enteredPasswordBytes.Length + storedSaltBytes.Length];
            Buffer.BlockCopy(enteredPasswordBytes, 0, saltedPassword, 0, enteredPasswordBytes.Length);
            Buffer.BlockCopy(storedSaltBytes, 0, saltedPassword, enteredPasswordBytes.Length, storedSaltBytes.Length);

            // Hash the concatenated value
            string enteredPasswordHash = HashPassword(enteredPassword, storedSaltBytes);

            // Compare the entered password hash with the stored hash
            if (enteredPasswordHash == storedHashedPassword)
            {
                return "Password is correct.";
            }
            else
            {
                return "Password is incorrect.";
            }
        }
    }
}
