using AutoMapper;
using cinema.api.Helpers.EmailSender;
using cinema.api.Models;
using cinema.api.Models.Admin;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace cinema.api.Controllers.Public;

[ApiController]
[Route("api/user/orders")]
[ApiExplorerSettings(GroupName = "User")]
public class OrdersController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly EmailOptions _emailOptions;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;

    private readonly Random _random = new Random();

    public OrdersController(CinemaDbContext context, EmailOptions emailOptions, IEmailSender emailSender, IMapper mapper)
    {
        _context = context;
        _emailOptions = emailOptions;
        _emailSender = emailSender;
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult Post([FromBody] OrderCreateDto dto)
    {
        if (dto == null || dto.Email is null || dto.Email.Trim() == "")
            return BadRequest("Invalid order data.");
        
        if (dto.SeatIds.Count() == 0) return BadRequest("No seats selected");

        var screening = _context.Screenings.FirstOrDefault(s => s.Id == dto.ScreeningId);
        if (screening == null) return BadRequest("Invalid screening ID.");

        var requestedSeats = _context.Seats.Where(s => dto.SeatIds.Contains(s.Id)).ToList();
        if (requestedSeats.Count != dto.SeatIds.Count)
            return BadRequest("One or more seat IDs are invalid.");

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
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber ?? "",
            Status = OrderStatus.Pending,
            Screening = screening,
            Seats = requestedSeats,
        };

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        sendTicketViaEmail(newOrder.Id);

        var newOrderDto = _mapper.Map<OrderDto>(newOrder);

        return Created($"/api/admin/orders/{newOrder.Id}", newOrderDto);
    }

    private string sendTicketViaEmail(Guid id)
    {
        var order = _context
            .Orders
            .Include(o => o.Seats)
            .Include(o => o.Screening)
            .ThenInclude(s => s!.Movie)
            .ThenInclude(m => m!.Category)
            .FirstOrDefault(o => o.Id == id)!;

        var senderInfo = new SenderInfo
        {
            Email = _emailOptions.Email,
            DisplayName = _emailOptions.DisplayName,
            AppPassword = _emailOptions.AppPassword,
            SmtpClientHost = _emailOptions.SmtpClientHost,
            SmtpClientPort = _emailOptions.SmtpClientPort,
        };

        string code = _random.Next(1000, 10000).ToString();

        var ticketInfo = new TicketInfo
        {
            MovieName = order.Screening!.Movie!.Title,
            Date = order.Screening!.StartDateTime.ToString("yyyy-MM-dd"),
            Time = order.Screening!.StartDateTime.ToString("HH:mm"),
            SeatNumbers = string.Join(", ", order.Seats!.Select(s => $"{s.Row}{s.Number}")),
            WebsiteUrl = _emailOptions.WebsiteUrl,
            Code = code
        };

        _emailSender.sendEmailAsync(senderInfo, order.Email, ticketInfo);

        return code;
    }
}
