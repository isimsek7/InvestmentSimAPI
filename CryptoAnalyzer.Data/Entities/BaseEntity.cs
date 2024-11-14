﻿using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Data.Entities
{
	public class BaseEntity
	{
		public int Id { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

		[JsonIgnore]
		public bool IsDeleted { get; set; }
	}

    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
		where TEntity:BaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{

			builder.HasQueryFilter(x => x.IsDeleted == false);
			builder.Property(x => x.ModifiedDate).IsRequired(false);
		}

	}
}

