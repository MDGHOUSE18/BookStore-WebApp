using BusinessLayer.Interfaces;
using Common.DTO;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class BooksBL : IBooksBL
    {
        private IBooksRepo _bookRepo;

        public BooksBL(IBooksRepo bookRepo)
        {
            this._bookRepo = bookRepo;
        }

        public async Task<BookDTO> AddBookAsync(AddBookDTO bookDTO)
        {
            return await _bookRepo.AddBookAsync(bookDTO);
        }

        public async Task<BookDTO> GetBookByIdAsync(int bookId)
        {
            return await _bookRepo.GetBookByIdAsync(bookId);
        }

        public async Task<BookDTO> UpdateBookAsync(int bookId, AddBookDTO bookDTO)
        {
            return await _bookRepo.UpdateBookAsync(bookId, bookDTO);
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            return await _bookRepo.DeleteBookAsync(bookId);
        }

        public async Task<List<BookDTO>> GetAllBooksAsync()
        {
            return await _bookRepo.GetAllBooksAsync();
        }

        public async Task<bool> UpdateBookImageAsync(int bookId, byte[] imageData)
        {
            return await _bookRepo.UpdateBookImageAsync(bookId, imageData);
        }
    }
}
