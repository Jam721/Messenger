﻿using System.ComponentModel.DataAnnotations;

namespace Messenger.Contracts;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserName { get; set; }
}