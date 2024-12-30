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
}
