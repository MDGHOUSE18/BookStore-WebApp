import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private baseUrl: string = 'https://localhost:7223/api/Address';
  
    constructor(private http: HttpService) {}
  
    private getHeaders(): HttpHeaders {
      const token = localStorage.getItem('token');
      let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
      if (token) {
        headers = headers.append('Authorization', `Bearer ${token}`);
      }
      return headers;
    }

    createAddress(reqData: any) {
      debugger
      return this.http.postService(this.baseUrl, reqData, true, { headers: this.getHeaders() });
    }
  
    getAddresses() {
      return this.http.getService(this.baseUrl, { headers: this.getHeaders() });
    }
  
    getAddressById(addressId: string) {
      return this.http.getService(`${this.baseUrl}/${addressId}`, { headers: this.getHeaders() });
    }
  
    deleteAddress(addressId: string) {
      return this.http.deleteService(`${this.baseUrl}/${addressId}`, { headers: this.getHeaders() });
    }
  
    updateAddress( data: any) {
      return this.http.updateService(this.baseUrl, data, true, { headers: this.getHeaders() });
    }
  
}
