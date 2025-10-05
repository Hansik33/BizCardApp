using System;

namespace BizCardApp.Models;

public class BusinessCard
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }
}