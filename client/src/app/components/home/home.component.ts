import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/Services/booksService/books.service';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  Books: any; // Array of all books
  displayedBooks: any[] = []; // Books to display on the current page
  pageSize = 12; // Number of books per page
  pageIndex = 0; // Current page index
  totalBooks = 0; // Total number of books

  constructor(
    private booksService: BooksService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getBooks(); // Fetch books on component initialization
  }

  getBooks() {
    this.booksService.getAllBooks()?.subscribe(
      (response: any) => {
        // console.log(response);
        this.Books = response.data; // Assign fetched books
        this.totalBooks = this.Books.length; // Set total number of books
        this.updateDisplayedBooks(); // Display books for the first page
      },
      (error) => {
        console.log('Error while fetching books');
      }
    );
  }

  updateDisplayedBooks() {
    const startIndex = this.pageIndex * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedBooks = this.Books.slice(startIndex, endIndex);
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updateDisplayedBooks(); // Update displayed books for the new page
  }

  // navigateToBook(book: any) {
  //   this.router.navigate(['/book'], { state: { book } });
  // }
}
