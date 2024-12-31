import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private userName = new BehaviorSubject<string | null>(
    localStorage.getItem('name') ?? null
  );
  private accessToken = new BehaviorSubject<string | null>(
    localStorage.getItem('token') ?? null
  );
  private userEmail = new BehaviorSubject<string | null>(
    localStorage.getItem('email') ?? null
  );

  UserName = this.userName.asObservable();
  AccessToken = this.accessToken.asObservable();
  UserEmail = this.userEmail.asObservable();

  private wishList = new BehaviorSubject([]);
  WishList = this.wishList.asObservable();

  constructor() {}

  setUsername(data: string): void {
    this.userName.next(data);
  }

  setAccessToken(token: string): void {
    this.accessToken.next(token);
  }

  setUserEmail(email: string): void {
    this.userEmail.next(email);
  }

  clearUserData(): void {
    this.userName.next(null);
    this.accessToken.next(null);
    this.userEmail.next(null);
  }

  setWishListData(data: any) {
    this.wishList.next(data);
  }
  
}