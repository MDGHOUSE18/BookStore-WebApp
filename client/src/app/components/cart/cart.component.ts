import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginSignupComponent } from '../login-signup/login-signup.component';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';
import { CartService } from 'src/app/Services/cartService/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
})
export class CartComponent {
  subsription!: Subscription;
  token: string | null = null;
  cartItems:any=[]
  // quantity:any=0

  constructor(
    private dialog: MatDialog,
    private dataService: DataService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.subsription = this.dataService.AccessToken.subscribe(
      (result) => (this.token = result)
    );
    this.getCartItems();
  }

  openDialog() {
    const dialogRef = this.dialog.open(LoginSignupComponent, {});
    dialogRef.afterClosed();
  }
  getCartItems() {
    this.cartService.getCarts().subscribe(
      (response) => {
        console.log(response);
        this.cartItems = response.data;
        // this.dataService.setWishListData(this.wishListItems)
      },
      (error) => {
        console.log('Error fetching wishlist items');
      }
    );
  }

  deleteCartItem(cartId:any){
    this.cartService.deleteCart(cartId).subscribe((response) => {
      this.cartItems = this.cartItems.filter(
        (item:any) => item.cartId !== cartId
      );
    })
  }

  // Increment item quantity
  increaseQuantity(item: any) {
    if (item.cartQuantity < item.stockQuantity && item.cartQuantity < 10) {
      item.cartQuantity++;
      this.updateCartItem(item.bookId,+1);
    }
  }

  // Decrement item quantity
  decreaseQuantity(item: any) {
    if (item.cartQuantity > 1) {
      item.cartQuantity--;
      this.updateCartItem(item.bookId,-1);
    }
  }

  // Update the cart item in the backend
  updateCartItem(id:any,num:number) {
    let reqData = {
      bookId:id,
      newQuantity : num
    }
    this.cartService.updateCart(reqData).subscribe(
      (response) => {
        console.log('Cart updated successfully');
        console.log(response.message);
        
      },
      (error) => {
        console.log('Error updating cart');
      }
    );
  }
}
