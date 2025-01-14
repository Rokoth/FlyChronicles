﻿using System;

namespace FlyChronicles.DB.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTimeOffset VersionDate { get; set; }
        Guid VersionId { get; set; }
    }
}
