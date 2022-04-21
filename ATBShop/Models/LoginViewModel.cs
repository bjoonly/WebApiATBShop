namespace ATBShop.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// User email
        /// </summary>
        /// <example>email@gmail.com</example>
        public string Email { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        /// <example>12345</example>
        public string Password { get; set; }
    }
}
