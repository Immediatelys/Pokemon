﻿using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);
        bool Save();
    }
}