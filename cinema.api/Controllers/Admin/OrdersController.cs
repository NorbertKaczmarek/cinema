using AutoMapper;
using cinema.api.Helpers.EmailSender;
using cinema.api.Models;
using cinema.api.Models.Admin;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers.Admin;

[ApiController]
[Route("api/admin/orders")]
[ApiExplorerSettings(GroupName = "Admin")]
public class OrdersController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly EmailOptions _emailOptions;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;

    public OrdersController(CinemaDbContext context, EmailOptions emailOptions, IEmailSender emailSender, IMapper mapper)
    {
        _context = context;
        _emailOptions = emailOptions;
        _emailSender = emailSender;
        _mapper = mapper;
    }

    [HttpGet]
    public PageResult<OrderDto> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context
            .Orders
            .Include(o => o.Seats)
            .Include(o => o.Screening)
            .ThenInclude(s => s!.Movie)
            .ThenInclude(m => m!.Category)
            .Where(
                o => query.Phrase == null ||
                (
                    o.Email.ToLower().Contains(query.Phrase.ToLower())
                )
            );

        var totalCount = baseQuery.Count();

        List<Order> result;

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

        var resultDto = _mapper.Map<List<OrderDto>>(result);

        return new PageResult<OrderDto>(resultDto, totalCount, query.Size);
    }

    [HttpGet("{id}")]
    public OrderDto Get(Guid id)
    {
        var order = getById(id);
        if (order is null) return null!; //NotFound("Order with that id was not found.");
        var orderDto = _mapper.Map<OrderDto>(order);
        return orderDto;
    }

    [HttpGet("{id}/email")]
    public OrderDto SentTestEmail(Guid id)
    {
        var order = getById(id);

        var senderInfo = new SenderInfo
        {
            Email = _emailOptions.Email,
            DisplayName = _emailOptions.DisplayName,
            AppPassword = _emailOptions.AppPassword,
            SmtpClientHost = _emailOptions.SmtpClientHost,
            SmtpClientPort = _emailOptions.SmtpClientPort,
        };

        var ticketInfo = new TicketInfo
        {
            MovieName = order.Screening!.Movie!.Title,
            Date = order.Screening!.StartDateTime.ToString("yyyy-MM-dd"),
            Time = order.Screening!.StartDateTime.ToString("HH:mm"),
            SeatNumbers = string.Join(", ", order.Seats!.Select(s => $"{s.Row}{s.Number}")),
            WebsiteUrl = _emailOptions.WebsiteUrl,
            Code = "0000"  // TODO unique code
        };

        _emailSender.sendEmailAsync(senderInfo, senderInfo.Email, ticketInfo);  // TODO order.Email

        return _mapper.Map<OrderDto>(order);
    }

    private Order getById(Guid id)
    {
        return _context
            .Orders
            .Include(o => o.Seats)
            .Include(o => o.Screening)
            .ThenInclude(s => s!.Movie)
            .ThenInclude(m => m!.Category)
            .FirstOrDefault(o => o.Id == id)!;
    }

    [HttpPost]
    public ActionResult Post([FromBody] OrderCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid order data.");

        var screening = _context.Screenings.FirstOrDefault(s => s.Id == dto.ScreeningId);
        if (screening == null) return BadRequest("Invalid screening ID.");

        var requestedSeats = _context.Seats.Where(s => dto.SeatIds.Contains(s.Id)).ToList();
        if (requestedSeats.Count != dto.SeatIds.Count) return BadRequest("One or more seat IDs are invalid.");

        var takenSeatIds = _context.Orders
            .Where(o => o.ScreeningId == dto.ScreeningId)
            .SelectMany(o => o.Seats!)
            .Select(s => s.Id)
            .ToHashSet();

        var takenSeats = requestedSeats.Where(seat => takenSeatIds.Contains(seat.Id)).ToList();
        if (takenSeats.Any())
        {
            var takenSeatNumbers = string.Join(", ", takenSeats.Select(s => $"{s.Row}{s.Number}"));
            return BadRequest($"The following seats are already taken: {takenSeatNumbers}");
        }

        var newOrder = new Order
        {
            Email = dto.Email ?? "",
            PhoneNumber = dto.PhoneNumber ?? "",
            Status = Enum.TryParse(dto.Status, true, out OrderStatus parsedStatus)
                ? parsedStatus
                : throw new ArgumentException($"Invalid order status: {dto.Status}"),
            Screening = screening,
            Seats = requestedSeats,
        };

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        var newOrderDto = _mapper.Map<OrderDto>(newOrder);

        return Created($"/api/admin/orders/{newOrder.Id}", newOrderDto);
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] OrderUpdateDto dto)
    {
        if (dto == null) return BadRequest("Invalid order data.");

        var existingOrder = getById(id);
        if (existingOrder == null) return NotFound($"Order with id {id} not found.");

        if (dto.ScreeningId != null)
        {
            var newScreening = _context.Screenings.FirstOrDefault(s => s.Id == dto.ScreeningId);
            if (newScreening == null) return BadRequest("Invalid screening ID.");
            existingOrder.Screening = newScreening;
        }

        if (dto.SeatIds != null)
        {
            var newSeats = _context.Seats.Where(s => dto.SeatIds.Contains(s.Id)).ToList();
            if (newSeats.Count != dto.SeatIds.Count) return BadRequest("One or more seat IDs are invalid.");
            existingOrder.Seats = newSeats;
        }

        existingOrder.Email = dto.Email ?? existingOrder.Email;
        existingOrder.PhoneNumber = dto.PhoneNumber ?? existingOrder.PhoneNumber;
        existingOrder.Status = Enum.TryParse(dto.Status, true, out OrderStatus parsedStatus)
                ? parsedStatus
                : throw new ArgumentException($"Invalid order status: {dto.Status}");

        _context.SaveChanges();

        return Ok(_mapper.Map<OrderDto>(existingOrder));
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var order = getById(id);
        if (order == null) return;

        _context.Orders.Remove(order);
        _context.SaveChanges();
    }
}
