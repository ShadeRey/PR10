using System;

namespace PR10.Models;

public class Order
{
    public int Id { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset DeliveryDate { get; set; }
    public int PickUpPoint { get; set; }
    public int Client { get; set; }
    public int ReceiveCode { get; set; }
    public int OrderStatus { get; set; }
}