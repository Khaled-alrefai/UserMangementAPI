using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static Dictionary<int, User> _users = new Dictionary<int, User>();
        private static HashSet<string> _emails = new HashSet<string>();
        private static int _nextId = 1;

        static UserController()
        {
            var defaultUser = new User
            {
                Id = _nextId++,
                Name = "Default User",
                Email = "default@example.com"
            };

            _users[defaultUser.Id] = defaultUser;
            _emails.Add(defaultUser.Email);
        }

        // GET: api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_users.Values);
        }



        // GET: api/user/
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            if (!_users.TryGetValue(id, out var user))
            {
                return NotFound($"No user found with ID {id}.");
            }
            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_emails.Contains(user.Email))
            {
                return Conflict("User with this email already exists.");
            }

            user.Id = _nextId++;
            _users[user.Id] = user;
            _emails.Add(user.Email);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);

        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (!_users.TryGetValue(id, out var user))
            {
                return NotFound($"No user found with ID {id}.");
            }

            // إذا تم تغيير الإيميل، نتحقق أولاً
            if (user.Email != updatedUser.Email)
            {
                if (_emails.Contains(updatedUser.Email))
                {
                    return Conflict("Another user with this email already exists.");
                }

                _emails.Remove(user.Email);
                _emails.Add(updatedUser.Email);
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            return NoContent();
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!_users.TryGetValue(id, out var user))
            {
                return NotFound($"No user found with ID {id}.");
            }

            _users.Remove(id);
            _emails.Remove(user.Email);

            return NoContent();
        }
    }

}
