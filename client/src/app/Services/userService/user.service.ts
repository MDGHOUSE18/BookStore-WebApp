import { Injectable } from '@angular/core';
import { HttpService } from '../http/http.service';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpService) {}

  baseUrl: string = 'https://localhost:7223/api/Users';

  private headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }
    return headers;
  }
  register(reqData: any) {
    return this.http.postService(`${this.baseUrl}/register`, reqData, false, { headers: this.headers });
  }

  login(reqData: any) {
    return this.http.postService(`${this.baseUrl}/login`, reqData, false, { headers: this.headers });
  }

  forgotPassword(reqData: any) {
    return this.http.postService(`${this.baseUrl}/forgotPassword`, reqData, false, { headers: this.headers });
  }

  getUserProfile(){
    return this.http.getService(this.baseUrl, { headers: this.getHeaders()})
  }
}
