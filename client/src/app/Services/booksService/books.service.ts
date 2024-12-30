import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

  constructor(private http:HttpService) { }

  baseUrl:string ="https://localhost:7223/api/"

  getAllBooks(){
    return this.http.getService(this.baseUrl+"Books")
  }
  getBook(id:number){
    return this.http.getService(this.baseUrl+"Books/"+id)
  }
}
