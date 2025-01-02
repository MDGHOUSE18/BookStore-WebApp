import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-book-card',
  templateUrl: './book-card.component.html',
  styleUrls: ['./book-card.component.scss']
})
export class BookCardComponent  implements OnInit{
  
  @Input() book:any={};
  BooksImagePath:string=''
  BooksImages:string[]=[
    '../../../assets/books/book.jpeg',
    '../../../assets/books/book1.jpeg',
    '../../../assets/books/book2.jpeg',
    '../../../assets/books/book3.jpeg',
    '../../../assets/books/book4.jpeg',
    '../../../assets/books/book5.jpeg',
    '../../../assets/books/book6.jpg  ',
    '../../../assets/books/book7.jpeg',
    '../../../assets/books/book8.jpeg',
    '../../../assets/books/book9.jpeg',
    '../../../assets/books/book10.jpg',
    '../../../assets/books/book11.jpeg',


  ]
  ngOnInit(): void {
    this.getRandomIndex()
  }
  getRandomIndex(){
    const index = Math.floor(Math.random()*this.BooksImages.length)
    this.BooksImagePath = this.BooksImages[index]
  }
}
