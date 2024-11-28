﻿namespace DiscountSystem.Application.Users.Queries;

public class DiscountDTO
{
    public Guid Id { get; set; }
    public string DiscountName { get; set; }
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}