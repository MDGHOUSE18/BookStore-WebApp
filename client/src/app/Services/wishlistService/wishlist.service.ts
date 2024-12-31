import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {

  constructor(private http: HttpService) {}
  
    baseUrl: string = 'https://localhost:7223/api/WishLists';
  
    private getHeaders(): HttpHeaders {
      const token = localStorage.getItem('token');
      let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
      if (token) {
        headers = headers.append('Authorization', `Bearer ${token}`);
      }
      return headers;
    }

    addBookToWishlist(bookId: string) {
      return this.http.postService(`${this.baseUrl}/${bookId}`, null, true, { headers: this.getHeaders() });
    }
  
    getAllWishlist() {
      return this.http.getService(`${this.baseUrl}`, { headers: this.getHeaders() });
    }
  
    removeBookFromWishlist(wishListId:any) {
      return this.http.deleteService(`${this.baseUrl}/${wishListId}`, { headers: this.getHeaders() });
    }
  
    removeAllFromWishlist() {
      return this.http.deleteService(`${this.baseUrl}/RemoveAll`, { headers: this.getHeaders() });
    }

}
