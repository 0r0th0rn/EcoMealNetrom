namespace EcoMeal.API.Models;

public class UserStatsDTO
{
    public int TotalOrders { get; set; }
    public decimal TotalMoneySaved { get; set; }
    public double TotalWeightSaved { get; set; }
}