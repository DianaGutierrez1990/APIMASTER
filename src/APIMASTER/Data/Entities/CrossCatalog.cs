namespace APIMASTER.Data.Entities;

public class CrossCatalog
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public bool? Active { get; set; }
}

public class CrossCustomerDairyBarnSilo
{
    public int Id_Customer_Dairy_Barn_Silo { get; set; }
    public string? Customer_Name { get; set; }
    public string? Dairy_Name { get; set; }
    public string? Barn_Name { get; set; }
    public string? Silo_Name { get; set; }
}

public class CrossTruck
{
    public int Id_Truck { get; set; }
    public string? Truck_Number { get; set; }
    public string? Truck_Name { get; set; }
    public bool? Active { get; set; }
}

public class CrossDriver
{
    public int Id_Driver { get; set; }
    public string? Driver_Name { get; set; }
    public string? License_Number { get; set; }
    public bool? Active { get; set; }
}
