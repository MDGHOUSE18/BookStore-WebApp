import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/Services/booksService/books.service';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { CartService } from 'src/app/Services/cartService/cart.service';
import { DataService } from 'src/app/Services/dataService/data.service';

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
  cartListItem:any=[]
  

  constructor(
    private booksService: BooksService,
    private router: Router,
    private cartService : CartService,
    private dataService:DataService
  ) {}

  ngOnInit(): void {
    this.getBooks(); 
    this.getCartItems()
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
  getCartItems(){
    this.cartService.getCarts().subscribe((response) => {
      // this.cartListItem = response.data;
      // // const id = this.route.snapshot.paramMap.get('id');
      // if (id) {
      //   const listItem = this.cartListItem.find(
      //     (item: any) => item.bookId == id
      //   );
        this.dataService.setCartData(response.data)
        console.log('Cart List Item:', response.data);
        // console.log('Is Cart Listed:', this.isCartlisted);
      // }
    });
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
