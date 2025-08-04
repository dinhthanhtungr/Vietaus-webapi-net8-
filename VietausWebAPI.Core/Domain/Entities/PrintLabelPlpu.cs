using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PrintLabelPlpu
{
    public int Id { get; set; }

    public string? NameProduct { get; set; }

    public string? ProductionCode { get; set; }

    public string? BatchNo { get; set; }

    public string? Dentity { get; set; }

    public string? Bagging { get; set; }

    public int? ExpiryDate { get; set; }

    public string? WarningInfo { get; set; }

    public int? LogoImages { get; set; }

    public string? Qrco { get; set; }
}
