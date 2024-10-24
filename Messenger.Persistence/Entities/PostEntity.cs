﻿using Messenger.Core.Models;

namespace Messenger.Persistence.Entities;

public class PostEntity
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }

    public UserEntity User { get; set; } = new();
}