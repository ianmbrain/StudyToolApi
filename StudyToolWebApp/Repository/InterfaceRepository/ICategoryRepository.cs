﻿using StudyToolWebApp.Models;

namespace StudyToolWebApp.Repository.InterfaceRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Card> GetCardsByCategory(int CategoryId);
        bool CategoryExists(int id);
    }
}