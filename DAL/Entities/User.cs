using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string AdressStreet { get; set; }
        [Required]
        public int AdressNumber { get; set; }
        [Required]
        public string AdressCity { get; set; }
        [Required]
        public int AdressPostalCode { get; set; }
    }
}
