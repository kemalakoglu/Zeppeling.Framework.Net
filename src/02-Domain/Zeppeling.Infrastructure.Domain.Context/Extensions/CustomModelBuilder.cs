using System;
using System.Collections.Generic;
using System.Text;
using Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes;
using Microsoft.EntityFrameworkCore;

namespace Zeppeling.Infrastructure.Domain.Context.Extensions
{
    public static class CustomModelBuilder
    {
        public  static void BuildModels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RC>(r =>
            {
                r.HasKey(pr => pr.Id);
                r.ToTable("ResponseCodes");
                r.Property(prop => prop.Name).HasMaxLength(100);
            });
        }
    }
}
