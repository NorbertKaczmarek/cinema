using AutoMapper;
using cinema.api.Helpers;
using cinema.api.Models;
using cinema.api.Models.Admin;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace cinema.api.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
[ApiExplorerSettings(GroupName = "Admin")]
public class UsersAdminController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public UsersAdminController(CinemaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public PageResult<UserDto> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context
            .Users
            .Where(
                u => query.Phrase == null ||
                (
                    u.Email.ToLower().Contains(query.Phrase.ToLower()) ||
                    u.FirstName.ToLower().Contains(query.Phrase.ToLower()) ||
                    u.LastName.ToLower().Contains(query.Phrase.ToLower())
                )
            );

        var totalCount = baseQuery.Count();

        List<User> result;

        if (query.Size == 0)
        {
            result = baseQuery.ToList();
        }
        else
        {
            result = baseQuery
                .Skip(query.Size * query.Page)
                .Take(query.Size)
                .ToList();
        }

        var resultDto = _mapper.Map<List<UserDto>>(result);
        return new PageResult<UserDto>(resultDto, totalCount, query.Size);
    }

    [HttpGet("{id}")]
    public UserDto Get(Guid id)
    {
        var user = getById(id);
        if (user == null) return null!;

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    private User getById(Guid id)
    {
        return _context.Users.FirstOrDefault(m => m.Id == id)!;
    }

    private User? getUserByEmail(string email)
    {
        var user = _context
            .Users
            .FirstOrDefault(u => u.Email == email);

        return user;
    }

    [HttpPost]
    public ActionResult Post([FromBody] UserCreateDto dto)
    {
        if (getUserByEmail(dto.Email) != null) return BadRequest("User already exists.");

        string password = generateAndSendNewPassword(dto.Email);
        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(password);

        var newUser = new User()
        {
            IsAdmin = false,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Salt = saltText,
            SaltedHashedPassword = saltedHashedPassword,
        };
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost("{id}/resetPassword")]
    public void ResetUserPassword(Guid id)
    {
        // TODO check if sender is admin
        var user = getById(id);

        string password = generateAndSendNewPassword(user.Email);
        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(password);

        user.Salt = saltText;
        user.SaltedHashedPassword = saltedHashedPassword;
        _context.SaveChanges();
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] UserUpdateDto dto)
    {
        var existingUser = getById(id);
        if (existingUser == null) return NotFound("User not found.");

        var result = SalterAndHasher.CheckPassword(dto.Password, existingUser.Salt, existingUser.SaltedHashedPassword);
        if (result == false) return BadRequest("Incorrect password.");

        existingUser.FirstName = dto.FirstName ?? existingUser.FirstName;
        existingUser.LastName = dto.LastName ?? existingUser.LastName;

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(dto.ConfirmNewPassword)) return BadRequest("Confirm new password is empty.");

            if (dto.NewPassword != dto.ConfirmNewPassword) return BadRequest("Passwords do not match.");

            (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(dto.NewPassword);
            existingUser.Salt = saltText;
            existingUser.SaltedHashedPassword = saltedHashedPassword;
        }

        _context.SaveChanges();

        var userDto = _mapper.Map<UserDto>(existingUser);
        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var user = getById(id);
        if (user == null) return;

        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    private string generateAndSendNewPassword(string email)
    {
        string password = generateRandomPassword(8, includeSpecialChars: false);
        sendEmailWithPassword(email, password);
        return password;
    }

    private void sendEmailWithPassword(string email, string password)
    {
        // TODO
    }

    private static string generateRandomPassword(
        int length = 12,
        bool includeUppercase = true,
        bool includeLowercase = true,
        bool includeNumbers = true,
        bool includeSpecialChars = true)
    {
        if (length <= 0)
            throw new ArgumentException("Password length must be greater than 0.");

        const string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string numberChars = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>?/";

        StringBuilder characterPool = new StringBuilder();

        if (includeUppercase) characterPool.Append(upperCaseChars);
        if (includeLowercase) characterPool.Append(lowerCaseChars);
        if (includeNumbers) characterPool.Append(numberChars);
        if (includeSpecialChars) characterPool.Append(specialChars);

        if (characterPool.Length == 0)
            throw new ArgumentException("At least one character set must be selected.");

        Random random = new Random();
        StringBuilder password = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(characterPool.Length);
            password.Append(characterPool[randomIndex]);
        }

        return password.ToString();
    }
}
