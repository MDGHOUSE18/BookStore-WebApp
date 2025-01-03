import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  constructor(private http: HttpService) {}

  baseUrl: string = 'https://localhost:7223/api/Orders';

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }
    return headers;
  }

  createOrder(addressId: string) {
    const url = `${this.baseUrl}/${addressId}`;
    return this.http.postService(url, null, true, { headers: this.getHeaders() });
  }

  updateOrder(orderId: string, orderData: any) {
    const url = `${this.baseUrl}/${orderId}`;
    return this.http.updateService(url, orderData, true, { headers: this.getHeaders() });
  }

  getOrder(orderId: string) {
    const url = `${this.baseUrl}/${orderId}`;
    return this.http.getService(url, { headers: this.getHeaders() });
  }

  getAllOrders() {
    const url = this.baseUrl;
    return this.http.getService(url, { headers: this.getHeaders() });
  }
  
}
