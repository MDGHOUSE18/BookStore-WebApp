import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';
import { LoginSignupComponent } from '../login-signup/login-signup.component';
import { WishlistService } from 'src/app/Services/wishlistService/wishlist.service';

@Component({
  selector: 'app-wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.scss'],
})
export class WishlistComponent implements OnInit {
  subsription!: Subscription;
  token: string | null = null;
  wishListItems: any = [];

  constructor(
    private dataService: DataService,
    private dialog: MatDialog,
    private wishlistService: WishlistService
  ) {}

  ngOnInit(): void {
    this.subsription = this.dataService.AccessToken.subscribe(
      (result) => (this.token = result)
    );
    this.getWishlistItems();
  }
  openDialog() {
    const dialogRef = this.dialog.open(LoginSignupComponent, {});
    dialogRef.afterClosed();
  }

  getWishlistItems() {
    this.wishlistService.getAllWishlist().subscribe(
      (response) => {
        console.log(response);
        this.wishListItems = response.data;
        this.dataService.setWishListData(this.wishListItems)
      },
      (error) => {
        console.log('Error fetching wishlist items');
      }
    );
  }

  deleteWishList(wishListId: any) {
    console.log(wishListId);
    
    this.wishlistService.removeBookFromWishlist(wishListId).subscribe(
      (response) => {
        console.log(response);
        this.wishListItems = this.wishListItems.filter(
          (item: any) => item.wishListId !== wishListId
        );
      },
      (error) => {
        console.log('Error fetching wishlist items');
      }
    );
  }
}
