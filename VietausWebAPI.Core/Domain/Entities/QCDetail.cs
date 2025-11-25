using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class Qcdetail
{
    public Guid Id { get; set; }

    public string? BatchExternalId { get; set; }

    public Guid? BatchId { get; set; }

    public string MachineExternalId { get; set; } = null!;

    public virtual ProductInspection? Batch { get; set; }
}
