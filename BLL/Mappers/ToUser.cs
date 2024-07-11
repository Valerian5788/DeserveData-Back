using DAL.Entities;
using DAL.Forms.Users;


namespace BLL.Mappers
{
    public static class ToUser
    {
        public static User CreateToUser(CreateUserForm form)
        {
            return new User
            {
                Email = form.Email,
                BirthDate = form.BirthDate,
                Name = form.Name,
                FirstName = form.FirstName,
                Password = form.Password,
                AdressStreet = form.AdressStreet,
                AdressNumber = form.AdressNumber,
                AdressCity = form.AdressCity,
                AdressPostalCode = form.AdressPostalCode
            };
        }
    }
}
