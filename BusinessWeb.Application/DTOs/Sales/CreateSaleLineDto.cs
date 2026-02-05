namespace BusinessWeb.Application.DTOs.Sales;

public class CreateSaleLineDto
{
    public Guid ProductId { get; set; }
    public Guid ProductPackageId { get; set; }

    /// <summary>Package birlikda miqdor (masalan 2 box)</summary>
    public decimal Quantity { get; set; }

    /// <summary>Sale paytida kiritiladigan narx (1 package uchun)</summary>
    public decimal UnitPrice { get; set; }
}
