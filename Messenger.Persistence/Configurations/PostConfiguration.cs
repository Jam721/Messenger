using Messenger.Core.Models;
using Messenger.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne<UserEntity>(p => p.User) 
            .WithMany(u => u.Posts);
    }
}