// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Testing.xunit;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Microsoft.AspNetCore.Identity.EntityFrameworkCore.Test
{
    public class CustomPocoTest
    {

        public class User<TKey> where TKey : IEquatable<TKey>
        {
            public TKey Id { get; set; }
            public string UserName { get; set; }
        }

        public class CustomDbContext<TKey> : DbContext where TKey : IEquatable<TKey>
        {
            public CustomDbContext(DbContextOptions options) : base(options)
            { }

            public DbSet<User<TKey>> Users { get; set; }

        }

        [ConditionalFact]
        [FrameworkSkipCondition(RuntimeFrameworks.Mono)]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public async Task CanUpdateNameGuid()
        {
            using (var db = new CustomDbContext<Guid>(
                new DbContextOptionsBuilder().UseSqlite($"DataSource=D{Guid.NewGuid()}.db").Options))
            {
                db.Database.EnsureCreated();

                var oldName = Guid.NewGuid().ToString();
                var user = new User<Guid> { UserName = oldName, Id = Guid.NewGuid() };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                var newName = Guid.NewGuid().ToString();
                user.UserName = newName;
                await db.SaveChangesAsync();
                Assert.Null(db.Users.SingleOrDefault(u => u.UserName == oldName));
                Assert.Equal(user, db.Users.Single(u => u.UserName == newName));

                db.Database.EnsureDeleted();
            }
        }

        [ConditionalFact]
        [FrameworkSkipCondition(RuntimeFrameworks.Mono)]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public async Task CanUpdateNameString()
        {
            using (var db = new CustomDbContext<string>(
                new DbContextOptionsBuilder().UseSqlite($"DataSource=D{Guid.NewGuid()}.db").Options))
            {
                db.Database.EnsureCreated();

                var oldName = Guid.NewGuid().ToString();
                var user = new User<string> { UserName = oldName, Id = Guid.NewGuid().ToString() };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                var newName = Guid.NewGuid().ToString();
                user.UserName = newName;
                await db.SaveChangesAsync();
                Assert.Null(db.Users.SingleOrDefault(u => u.UserName == oldName));
                Assert.Equal(user, db.Users.Single(u => u.UserName == newName));

                db.Database.EnsureDeleted();
            }
        }

        [ConditionalFact]
        [FrameworkSkipCondition(RuntimeFrameworks.Mono)]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public async Task CanCreateUserInt()
        {
            using (var db = new CustomDbContext<int>(
                new DbContextOptionsBuilder().UseSqlite($"DataSource=D{Guid.NewGuid()}.db").Options))
            {
                db.Database.EnsureCreated();

                var user = new User<int>();
                db.Users.Add(user);
                await db.SaveChangesAsync();
                user.UserName = "Boo";
                await db.SaveChangesAsync();
                var fetch = db.Users.First(u => u.UserName == "Boo");
                Assert.Equal(user, fetch);

                db.Database.EnsureDeleted();
            }
        }

        [ConditionalFact]
        [FrameworkSkipCondition(RuntimeFrameworks.Mono)]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public async Task CanCreateUserIntViaSet()
        {
            using (var db = new CustomDbContext<int>(
                new DbContextOptionsBuilder().UseSqlite($"DataSource=D{Guid.NewGuid()}.db").Options))
            {
                db.Database.EnsureCreated();

                var user = new User<int>();
                var users = db.Set<User<int>>();
                users.Add(user);
                await db.SaveChangesAsync();
                user.UserName = "Boo";
                await db.SaveChangesAsync();
                var fetch = users.First(u => u.UserName == "Boo");
                Assert.Equal(user, fetch);

                db.Database.EnsureDeleted();
            }
        }

        [ConditionalFact]
        [FrameworkSkipCondition(RuntimeFrameworks.Mono)]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public async Task CanUpdateNameInt()
        {
            using (var db = new CustomDbContext<int>(
                new DbContextOptionsBuilder().UseSqlite($"DataSource=D{Guid.NewGuid()}.db").Options))
            {
                db.Database.EnsureCreated();

                var oldName = Guid.NewGuid().ToString();
                var user = new User<int> { UserName = oldName };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                var newName = Guid.NewGuid().ToString();
                user.UserName = newName;
                await db.SaveChangesAsync();
                Assert.Null(db.Users.SingleOrDefault(u => u.UserName == oldName));
                Assert.Equal(user, db.Users.Single(u => u.UserName == newName));

                db.Database.EnsureDeleted();
            }
        }

        [ConditionalFact]
        [FrameworkSkipCondition(RuntimeFrameworks.Mono)]
        [OSSkipCondition(OperatingSystems.Linux)]
        [OSSkipCondition(OperatingSystems.MacOSX)]
        public async Task CanUpdateNameIntWithSet()
        {
            using (var db = new CustomDbContext<int>(
                new DbContextOptionsBuilder().UseSqlite($"DataSource=D{Guid.NewGuid()}.db").Options))
            {
                db.Database.EnsureCreated();

                var oldName = Guid.NewGuid().ToString();
                var user = new User<int> { UserName = oldName };
                db.Set<User<int>>().Add(user);
                await db.SaveChangesAsync();
                var newName = Guid.NewGuid().ToString();
                user.UserName = newName;
                await db.SaveChangesAsync();
                Assert.Null(db.Set<User<int>>().SingleOrDefault(u => u.UserName == oldName));
                Assert.Equal(user, db.Set<User<int>>().Single(u => u.UserName == newName));

                db.Database.EnsureDeleted();
            }
        }
    }
}
