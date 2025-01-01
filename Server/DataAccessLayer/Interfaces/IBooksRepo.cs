﻿using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IBooksRepo
    {
        Task<BookDTO> AddBookAsync(AddBookDTO bookDTO);
        Task<bool> DeleteBookAsync(int bookId);
        Task<List<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(int bookId);
        Task<BookDTO> UpdateBookAsync(int bookId, AddBookDTO bookDTO);
        Task<bool> UpdateBookImageAsync(int bookId, byte[] imageData);
    }
}