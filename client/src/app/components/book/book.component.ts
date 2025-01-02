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
  book: any;
  bookId: number = 0;
  bookQuantity: number = 0;
  isWishlisted: boolean = false;
  wishListItem: any[] = [];
  isCartlisted: boolean = false;
  cartListItem: any[] = [];
  subscription!: Subscription;
  token:string|null = null;
  cartItems:any=[]

  constructor(
    private bookService: BooksService,
    private route: ActivatedRoute,
    private wishlistService: WishlistService,
    private snackBar: MatSnackBar,
    private dataService: DataService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.subscription=this.dataService.AccessToken.subscribe((result)=>this.token=result)
    this.subscription=this.dataService.CartItems.subscribe((result)=>this.cartItems=result)
    // console.log(this.cartItems)
    this.isCartListedOrNot()
    this.wishlistService.getAllWishlist().subscribe((response) => {
      this.wishListItem = response.data;
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        const listItem = this.wishListItem.find(
          (item: any) => item.bookId == id
        );

        this.isWishlisted = !!listItem;
      }
    });
    

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.bookId = +id;
      this.getBook();
    }
  }

  isCartListedOrNot(){
    const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        const listItem = this.cartItems.find(
          (item: any) => item.bookId == id
        );
        this.bookQuantity = listItem.cartQuantity
        this.isCartlisted = !!listItem;
      }
  }
  
  getBook() {
    this.bookService.getBook(this.bookId).subscribe(
      (response: any) => {
        this.book = response.data;
        // console.log(response);
      },
      (error) => {
        console.error('Error fetching book:', error);
      }
    );
  }
  addToCart() {
    let reqData = {
      bookId: this.bookId,
      quantity: 1,
    };
    // console.log(reqData);

    this.cartService.createCart(reqData).subscribe((response: any) => {
      // console.log(response.message);
      this.snackBar.open('Book added to cart successfully', 'Close', {
        duration: 2000,
      });
      
      this.dataService.addToCart(this.book)
      this.bookQuantity = 1;
      this.isCartlisted = true;
      // this.dataService.setCartData=
    });
  }
  
  increaseQuantity() {
    if (this.bookQuantity < this.book.stockQuantity && this.bookQuantity < 10) {
      this.bookQuantity++;
      this.updateCartItem(this.bookId, +1);
      this.dataService.updateCartItemQuantity(this.bookId,this.bookQuantity)
    }else{
      this.snackBar.open("you reached maximum quantity for cart", 'Close', {
        duration: 2000,
      });
    }
  }

  decreaseQuantity() {
    if (this.bookQuantity > 1) {
      this.bookQuantity--;
      this.updateCartItem(this.bookId, -1);
      this.dataService.updateCartItemQuantity(this.bookId,this.bookQuantity)
    }else{
      this.deleteCartItem()
    }
  }

  updateCartItem(id: any, num: number) {
    // console.log('bookId ' + id + '  num : ' + num);

    let reqData = {
      bookId: id,
      newQuantity: num,
    };
    this.cartService.updateCart(reqData).subscribe(
      (response) => {
        console.log('Cart updated successfully');
        // console.log(response.message);
      },
      (error) => {
        console.log('Error updating cart');
      }
    );
  }
  addToWishlist() {
    this.wishlistService.addBookToWishlist(this.bookId.toString()).subscribe(
      (response) => {
        this.snackBar.open(
          this.book.title + 'Book added to wishlist',
          'Close',
          { duration: 4000 }
        );
        // console.log('Book added to wishlist:', response);
        this.isWishlisted = true;
      },
      (error) => {
        console.error('Error adding book to wishlist:', error);
      }
    );
  }
  deleteCartItem(){
    this.cartService.deleteCart(this.bookId.toString()).subscribe((response:any) => {
      this.isCartlisted = !this.isCartlisted;
      this.snackBar.open(response.message, 'Close', {
        duration: 2000,
      });
    })
  }
}
