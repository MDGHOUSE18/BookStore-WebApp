import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'search'
})
export class SearchPipe implements PipeTransform {

  transform(value: any, filterBook: string):any[] {

    if (filterBook ==='') {
      return value;
    }

    const books = [];
    for(const book of value){
      if (book.title.toLowerCase().includes(filterBook)) {
        books.push(book)
      }
    }
    return books;
  }

}
