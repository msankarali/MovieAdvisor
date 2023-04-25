using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class MovieAdvisorDbContextInitializer
    {
        private readonly MovieAdvisorDbContext _movieAdvisorDbContext;

        public MovieAdvisorDbContextInitializer(MovieAdvisorDbContext movieAdvisorDbContext)
        {
            _movieAdvisorDbContext = movieAdvisorDbContext;
        }

        public async Task StartMigration()
        {
            try
            {
                if (_movieAdvisorDbContext.Database.IsSqlServer())
                {
                    await _movieAdvisorDbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Seed()
        {
            if (!_movieAdvisorDbContext.Set<Movie>().Any())
            {
                await _movieAdvisorDbContext.Users.AddRangeAsync(new User[]
                {
                    new User
                    {
                        Id = 1,
                        FirstName = "Muharrem Servet",
                        LastName = "Ankaralı",
                        Email = "mservetankarali@gmail.com",
                        PasswordHash = new byte[] {1,2,3,4,5},
                        PasswordSalt = new byte[] {1,2,3,4,5}
                    },
                    new User
                    {
                        Id = 2,
                        FirstName = "Ali",
                        LastName = "Yıldırım",
                        Email = "ali.yildirim@example.com",
                        PasswordHash = new byte[] {1,2,3,4,5},
                        PasswordSalt = new byte[] {1,2,3,4,5}
                    },
                    new User
                    {
                        Id = 3,
                        FirstName = "Gizem",
                        LastName = "Tekin",
                        Email = "gizem.tekin@example.com",
                        PasswordHash = new byte[] {1,2,3,4,5},
                        PasswordSalt = new byte[] {1,2,3,4,5}
                    },
                    new User
                    {
                        Id = 4,
                        FirstName = "Burak",
                        LastName = "Aydın",
                        Email = "burak.aydin@example.com",
                        PasswordHash = new byte[] {1,2,3,4,5},
                        PasswordSalt = new byte[] {1,2,3,4,5}
                    },
                    new User
                    {
                        Id = 5,
                        FirstName = "Esra",
                        LastName = "Çalışkan",
                        Email = "esra.caliskan@example.com",
                        PasswordHash = new byte[] {1,2,3,4,5},
                        PasswordSalt = new byte[] {1,2,3,4,5}
                    }
                });

                await _movieAdvisorDbContext.Set<Movie>().AddRangeAsync(new Movie[]
                {
                    new Movie
                    {
                        Id = 1,
                        Title = "The Godfather",
                        Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                        ReleaseDate = new DateTime(1972, 3, 24),
                        Ratings = new List<Rating>
                        {
                            new Rating { Id = 1, Comment = "A true classic of the gangster genre", Score = 9, UserId = 1},
                            new Rating { Id = 2, Comment = "Al Pacino's acting is phenomenal", Score = 10, UserId = 2},
                            new Rating { Id = 3, Comment = "Brilliant cinematography and direction", Score = 9, UserId = 3},
                            new Rating { Id = 4, Comment = "A bit slow-paced, but still a masterpiece", Score = 8, UserId = 4}
                        }
                    },
                    new Movie
                    {
                        Id = 2,
                        Title = "The Shawshank Redemption",
                        Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                        ReleaseDate = new DateTime(1994, 10, 14),
                        Ratings = new List<Rating>
                        {
                            new Rating { Id = 5, Comment = "One of the best movies of all time", Score = 10, UserId = 1},
                            new Rating { Id = 6, Comment = "Acting is top-notch", Score = 9, UserId = 3},
                            new Rating { Id = 7, Comment = "Storyline was predictable but still good", Score = 7, UserId = 5},
                            new Rating { Id = 8, Comment = "A classic that everyone needs to see", Score = 8, UserId = 2}
                        }
                    },
                    new Movie
                    {
                        Id = 3,
                        Title = "Interstellar",
                        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                        ReleaseDate = new DateTime(2014, 11, 7),
                        Ratings = new List<Rating>
                        {
                            new Rating { Id = 9, Comment = "Mind-blowing visuals and special effects", Score = 9, UserId = 4},
                            new Rating { Id = 10, Comment = "Hans Zimmer's score is amazing", Score = 10, UserId = 2},
                            new Rating { Id = 11, Comment = "The storyline is a bit convoluted and hard to follow", Score = 6, UserId = 3},
                            new Rating { Id = 12, Comment = "Overall a great sci-fi film", Score = 8, UserId = 5}
                        }
                    },
                    new Movie
                    {
                        Id = 4,
                        Title = "The Dark Knight",
                        Description = "Batman, Gordon and Harvey Dent are forced to deal with the chaos unleashed by a criminal mastermind known only as the Joker, as he drives each of them to their limits.",
                        ReleaseDate = new DateTime(2008, 7, 18),
                        Ratings = new List<Rating>
                        {
                            new Rating { Id = 13, Comment = "Heath Ledger's Joker was iconic", Score = 10, UserId = 2},
                            new Rating { Id = 14, Comment = "More of a crime-thriller than a superhero movie", Score = 9, UserId = 1},
                            new Rating { Id = 15, Comment = "Too long, with a muddled narrative", Score = 5, UserId = 3},
                            new Rating { Id = 16, Comment = "Still one of the best comic book movies out there", Score = 8, UserId = 4}
                        }
                    },
                    new Movie
                    {
                        Id = 5,
                        Title = "Inception",
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        ReleaseDate = new DateTime(2010, 7, 16),
                        Ratings = new List<Rating>
                        {
                            new Rating { Id = 17, Comment = "The visuals and action scenes are stunning", Score = 9, UserId = 3},
                            new Rating { Id = 18, Comment = "The pacing is a bit slow in places", Score = 7, UserId = 5},
                            new Rating { Id = 19, Comment = "A complex and challenging film, but worth it", Score = 8, UserId = 2},
                            new Rating { Id = 20, Comment = "The ending could have been better", Score = 6, UserId = 1}
                        }
                    },
                    new Movie
                    {
                        Id = 6,
                        Title = "Hunger Games",
                        Description = "A group of villagers who survive to win the match for their lives",
                        ReleaseDate = new DateTime(2005, 7, 2)
                    }
                });

                await _movieAdvisorDbContext.SaveChangesAsync();
            }
        }
    }
}
