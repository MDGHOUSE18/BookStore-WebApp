import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/Services/booksService/books.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {
  book: any;
  bookId: number = 0;
  bookQuantity:number=0;

  constructor(private bookService: BooksService, private route: ActivatedRoute) {}

  ngOnInit(): void {
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
}
