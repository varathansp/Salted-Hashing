using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SaltedHashing
{
    class AccountManager
    {
        private HashingManager _HashManager;
        public AccountManager()
        {
            _HashManager = new HashingManager();
        }
        public bool Register(string userName, string password)
        {
            try
            {
                byte[] salt = _HashManager.GetSalt();
                byte[] hashValue = _HashManager.GetHashValue(password, salt);
                string savingPassword = _HashManager.CombineAndConvert(salt, hashValue);
                using (SQL002DBEntities entities = new SQL002DBEntities())
                {
                    entities.RegisterUserIdentity(userName, savingPassword);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Login(string userid, string password)
        {
            try
            {
                /* Fetch the stored value */
                SQL002DBEntities DBContext = new SQL002DBEntities();
                UserIdentity user = DBContext.UserIdentities.Where(u => u.UserId == userid).SingleOrDefault();
                /* Extract the bytes */
                byte[] hashBytes = Convert.FromBase64String(user.Password);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                        //throw new UnauthorizedAccessException();
                    }
                   
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
