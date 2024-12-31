import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private baseUrl: string = 'https://localhost:7223/api/Carts';

  constructor(private http: HttpService) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }
    return headers;
  }

  createCart(reqData: any) {
    return this.http.postService(this.baseUrl, reqData, true, { headers: this.getHeaders() });
  }

  getCarts() {
    return this.http.getService(this.baseUrl, { headers: this.getHeaders() });
  }

  deleteCart(cartId: string) {
    return this.http.deleteService(`${this.baseUrl}/${cartId}`, { headers: this.getHeaders() });
  }

  clearCart() {
    return this.http.deleteService(`${this.baseUrl}/ClearCart`, { headers: this.getHeaders() });
  }
  updateCart(data:any){
    return this.http.updateService(this.baseUrl,data,true,{ headers: this.getHeaders() })
  }
}
