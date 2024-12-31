import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/Services/booksService/books.service';
import { ActivatedRoute } from '@angular/router';
import { WishlistService } from 'src/app/Services/wishlistService/wishlist.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DataService } from 'src/app/Services/dataService/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {
  book: any;
  bookId: number = 0;
  bookQuantity:number=0;
  isWishlisted:boolean=true;
  wishListItem:any[]=[]
  subscription!:Subscription

  constructor(
    private bookService: BooksService, 
    private route: ActivatedRoute,
    private wishlistService : WishlistService,
    private snackBar: MatSnackBar,
    private dataService : DataService
  ) {}

  ngOnInit(): void {
    this.wishlistService.getAllWishlist().subscribe((response)=>{
      this.wishListItem = response.data;
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        const listItem = this.wishListItem.find((item: any) => item.bookId == id);
        this.isWishlisted = !!listItem; 
      }
    })
    // this.subscription = this.dataService.WishList.subscribe((result) => {
    //   this.wishListItem = result;
    //   const id = this.route.snapshot.paramMap.get('id');
    //   if (id) {
    //     const listItem = this.wishListItem.find((item: any) => item.bookId == id);
    //     this.isWishlisted = !!listItem; // Set true if found, false otherwise
    //   }
    // });
  
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.bookId = +id;
      this.getBook();
    }
  }

  getBook() {
    this.bookService.getBook(this.bookId).subscribe(
      (response: any) => {
        this.book = response.data;
        console.log(response)
      },
      (error) => {
        console.error('Error fetching book:', error);
      }
    );
  }
  toggleCart() {
    // if (this.bookQuantity === 0) {
    //   // Add the whole book object with a quantity of 1
    //   this.cartService.addToCart({ ...this.bookDetails, quantity: 1 });
    // } else {
    //   // Add the book to cart with an updated quantity
    //   this.cartService.addToCart({ ...this.bookDetails, quantity: 1 });
    // }
    // this.bookQuantity = this.cartService.getBookQuantity(this.bookDetails.id);
  }

  decreaseQuantity() {
    // if (this.bookQuantity > 1) {
    //   this.cartService.addToCart({ ...this.bookDetails, quantity: -1 });
    //   this.bookQuantity = this.cartService.getBookQuantity(this.bookDetails.id);
    //   console.log(this.cartService.getBookQuantity(this.bookDetails.id));
    // } else {
    //   this.cartService.removeFromCart(this.bookDetails.id);
    //   this.bookQuantity = 0;
    //   console.log(this.cartService.getBookQuantity(this.bookDetails.id));
    // }
  }

  increaseQuantity() {
    // this.cartService.addToCart({ ...this.bookDetails, quantity: 1 });
    // this.bookQuantity = this.cartService.getBookQuantity(this.bookDetails.id);
    // console.log(this.cartService.getBookQuantity(this.bookDetails.id));
  }
  addToWishlist(){
    this.wishlistService.addBookToWishlist(this.bookId.toString()).subscribe(
      (response) => {
        this.snackBar.open(this.book.title+'Book added to wishlist', 'Close', { duration: 4000 });
        console.log('Book added to wishlist:', response);
        this.isWishlisted = true;
        // this.getWishlist(); // Refresh wishlist
      },
      (error) => {
        console.error('Error adding book to wishlist:', error);
      }
    );
  }
}
