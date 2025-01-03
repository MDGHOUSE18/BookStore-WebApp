import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/Services/booksService/books.service';
import { ActivatedRoute } from '@angular/router';
import { WishlistService } from 'src/app/Services/wishlistService/wishlist.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DataService } from 'src/app/Services/dataService/data.service';
import { Subscription } from 'rxjs';
import { CartService } from 'src/app/Services/cartService/cart.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss'],
})
export class BookComponent implements OnInit {
  book: any = {}; // Default object to avoid undefined errors
  bookId: number = 0;
  bookQuantity: number = 0;
  isWishlisted: boolean = false;
  wishListItem: any[] = [];
  isCartlisted: boolean = false;
  cartItems: any[] = [];
  subscription: Subscription = new Subscription();
  token: string | null = null;

  constructor(
    private bookService: BooksService,
    private route: ActivatedRoute,
    private wishlistService: WishlistService,
    private snackBar: MatSnackBar,
    private dataService: DataService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.initializeData();
    this.loadBookDetails();
    this.loadCartAndWishlist();
  }

  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks
    this.subscription.unsubscribe();
  }

  initializeData(): void {
    this.subscription.add(
      this.dataService.AccessToken.subscribe((result) => (this.token = result))
    );
    this.subscription.add(
      this.dataService.CartItems.subscribe((result) => (this.cartItems = result || []))
    );
  }

  loadBookDetails(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.bookId = +id;
      this.getBook();
    } else {
      console.error('Book ID not found in route.');
    }
  }

  loadCartAndWishlist(): void {
    if (this.token) {
      this.isCartListedOrNot();
      this.wishlistService.getAllWishlist().subscribe(
        (response) => {
          this.wishListItem = response?.data || [];
          const listItem = this.wishListItem.find((item: any) => item.bookId == this.bookId);
          this.isWishlisted = !!listItem;
        },
        (error) => console.error('Error fetching wishlist:', error)
      );
    } else {
      console.warn('Token not available, skipping cart and wishlist load.');
    }
  }

  getBook(): void {
    this.bookService.getBook(this.bookId).subscribe(
      (response: any) => {
        this.book = response?.data || {};
        if (!this.book.title) {
          console.warn('Book data is empty or undefined.');
        }
      },
      (error) => {
        console.error('Error fetching book:', error);
      }
    );
  }

  isCartListedOrNot(): void {
    const listItem = this.cartItems.find((item: any) => item.bookId == this.bookId);
    if (listItem) {
      this.bookQuantity = listItem.cartQuantity || 0;
      this.isCartlisted = true;
    }
  }

  addToCart(): void {
    const reqData = {
      bookId: this.bookId,
      quantity: 1,
    };
    this.cartService.createCart(reqData).subscribe(
      () => {
        this.snackBar.open('Book added to cart successfully', 'Close', { duration: 2000 });
        this.dataService.addToCart(this.book);
        this.bookQuantity = 1;
        this.isCartlisted = true;
      },
      (error) => console.error('Error adding to cart:', error)
    );
  }

  increaseQuantity(): void {
    if (this.bookQuantity < this.book.stockQuantity && this.bookQuantity < 10) {
      this.bookQuantity++;
      this.updateCartItem(1);
      this.dataService.updateCartItemQuantity(this.bookId, this.bookQuantity);
    } else {
      this.snackBar.open('You reached the maximum quantity for cart', 'Close', { duration: 2000 });
    }
  }

  decreaseQuantity(): void {
    if (this.bookQuantity > 1) {
      this.bookQuantity--;
      this.updateCartItem(-1);
      this.dataService.updateCartItemQuantity(this.bookId, this.bookQuantity);
    } else {
      this.deleteCartItem();
    }
  }

  updateCartItem(change: number): void {
    const reqData = {
      bookId: this.bookId,
      newQuantity: change,
    };
    this.cartService.updateCart(reqData).subscribe(
      () => console.log('Cart updated successfully'),
      (error) => console.error('Error updating cart:', error)
    );
  }

  addToWishlist(): void {
    this.wishlistService.addBookToWishlist(this.bookId.toString()).subscribe(
      () => {
        this.snackBar.open(`${this.book.title} added to wishlist`, 'Close', { duration: 4000 });
        this.isWishlisted = true;
      },
      (error) => console.error('Error adding book to wishlist:', error)
    );
  }

  deleteCartItem(): void {
    this.cartService.deleteCart(this.bookId.toString()).subscribe(
      (response: any) => {
        this.isCartlisted = false;
        this.snackBar.open(response.message, 'Close', { duration: 2000 });
      },
      (error) => console.error('Error deleting cart item:', error)
    );
  }
}
