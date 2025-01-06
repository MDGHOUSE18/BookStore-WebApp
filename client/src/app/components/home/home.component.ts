import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/Services/booksService/books.service';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { CartService } from 'src/app/Services/cartService/cart.service';
import { DataService } from 'src/app/Services/dataService/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  Books: any[] = [];
  displayedBooks: any[] = [];
  pageSize: number = 12;
  pageIndex: number = 0;
  totalBooks: number = 0;
  cartListItem: any[] = [];
  subscription: Subscription = new Subscription();
  token: string | null = null;
  filterBooks:any;

  constructor(
    private booksService: BooksService,
    private router: Router,
    private cartService: CartService,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.initializeSubscriptions();
    this.getBooks();
    this.getCartItems();
    this.dataService.incomingData.subscribe(
      (response :any)=>{
        this.filterBooks = response;
      }
    )
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  initializeSubscriptions(): void {
    this.subscription.add(
      this.dataService.AccessToken.subscribe((result) => (this.token = result))
    );
  }

  getBooks(): void {
    this.booksService.getAllBooks()?.subscribe(
      (response: any) => {
        this.Books = response?.data || [];
        this.totalBooks = this.Books.length;
        this.updateDisplayedBooks();
      },
      (error) => {
        console.error('Error while fetching books:', error);
      }
    );
  }

  getCartItems(): void {
    if (this.token) {
      this.cartService.getCarts().subscribe(
        (response: any) => {
          // console.log('Cart List Items:', response?.data);
          this.dataService.setCartData(response?.data || []);
        },
        (error) => {
          console.error('Error while fetching cart items:', error);
        }
      );
    }
  }

  updateDisplayedBooks(): void {
    const startIndex = this.pageIndex * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedBooks = this.Books.slice(startIndex, endIndex);
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updateDisplayedBooks();
  }

  navigateToBook(book: any): void {
    this.router.navigate(['/book'], { state: { book } });
  }
}
