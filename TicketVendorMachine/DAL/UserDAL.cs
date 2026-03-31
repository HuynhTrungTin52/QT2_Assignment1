using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserDAL
    {
        private readonly TicketContext _context;

        public UserDAL()
        {
            _context = new TicketContext();
        }
        public bool InsertUser(User user)
        {
            
            
                _context.Users.Add(user);
                int rowsAffected = _context.SaveChanges();
                return rowsAffected > 0;
            
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
    
            
                return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            
        }
        public bool EmailExists(string email)
        {

            
                return _context.Users.Any(u => u.Email == email);
            
        }

    }
}
