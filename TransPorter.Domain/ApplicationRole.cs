﻿using Microsoft.AspNetCore.Identity;
using TransPorter.Shared.Interfaces;

namespace TransPorter.Domain;

public class ApplicationRole : IdentityRole<Guid>, IAuditableEntity
{
    public Guid CreatedUserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
