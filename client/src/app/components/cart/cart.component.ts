import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginSignupComponent } from '../login-signup/login-signup.component';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';
import { CartService } from 'src/app/Services/cartService/cart.service';
import { AddressService } from 'src/app/Services/addressService/address.service';
import { OrdersService } from 'src/app/Services/orderService/orders.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
})
export class CartComponent {
  subsription!: Subscription;
  token: string | null = null;
  cartItems: any = [];
  addAddress: boolean = false;
  btn: boolean = true;
  addresses: any = [];
  selectedAddress: any;
  isPlacedOrder: boolean = false;
  isAddressSelected: boolean = false;
  newAddress: any = {
    addressId: '',
    fullName: '',
    phone: '',
    addressType: '',
    address: '',
    city: '',
    state: '',
  };

  typeToNumber: { [key: string]: number } = {
    Home: 1,
    Work: 2,
    Other: 3,
  };

  constructor(
    private dialog: MatDialog,
    private dataService: DataService,
    private cartService: CartService,
    private addressService: AddressService,
    private orderService: OrdersService,
    private router:Router,
    private snackBar:MatSnackBar
  ) {}

  ngOnInit(): void {
    this.subsription = this.dataService.AccessToken.subscribe(
      (result) => (this.token = result)
    );
    this.dataService.UserName.subscribe((result) => {
      this.newAddress.fullName = result;
    });
    this.dataService.UserPhone.subscribe((result) => {
      this.newAddress.phone = result;
    });
    this.getCartItems();
    this.getAddresses();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(LoginSignupComponent, {});
    dialogRef.afterClosed();
  }

  getCartItems(): void {
    this.cartService.getCarts().subscribe(
      (response) => {
        if (response.data) {
          this.cartItems = response.data;
        }
        this.dataService.setCartData(response.data);
      },
      (error) => {
        console.error('Error fetching wishlist items');
      }
    );
  }

  deleteCartItem(bookId: any): void {
    this.cartService.deleteCart(bookId).subscribe((response) => {
      this.cartItems = this.cartItems.filter(
        (item: any) => item.bookId !== bookId
      );
      this.dataService.removeFromCart(bookId);
    });
  }

  increaseQuantity(item: any): void {
    if (item.cartQuantity < item.stockQuantity && item.cartQuantity < 10) {
      item.cartQuantity++;
      this.updateCartItem(item.bookId, +1);
    }
  }

  decreaseQuantity(item: any): void {
    if (item.cartQuantity > 1) {
      item.cartQuantity--;
      this.updateCartItem(item.bookId, -1);
    }
  }

  updateCartItem(id: any, num: number): void {
    let reqData = {
      bookId: id,
      newQuantity: num,
    };
    this.cartService.updateCart(reqData).subscribe(
     
      (error) => {
        console.error('Error updating cart');
      }
    );
  }

  onPlaceOrder(): void {
    this.isPlacedOrder = !this.isPlacedOrder;
  }

  getAddresses(): void {
    this.addressService.getAddresses().subscribe(
      (response: any) => {
        this.addresses = response.data;
      },
      (error) => {
        console.error('Error fetching addresses', error);
      }
    );
  }

  onOpenAddAddress(): void {
    this.addAddress = !this.addAddress;
  }

  addNewAddress(formData: any): void {
    let reqData = {
      typeId: this.typeToNumber[formData.addressType],
      address: formData.address,
      city: formData.city,
      state: formData.state,
    };
    
    this.addressService.createAddress(reqData).subscribe(
      (response) => {
        this.getAddresses();
        this.addAddress = false;
      },
      (error) => {
        console.error('Error adding address', error);
      }
    );
  }

  onOpenUpdateAddress(address: any): void {
    this.addAddress = !this.addAddress;
    this.btn = !this.btn;
    this.newAddress.addressType = address.typeOfAddress;
    this.newAddress.address = address.address;
    this.newAddress.city = address.city;
    this.newAddress.state = address.state;
    this.newAddress.addressId = address.addressId;
  }

  updateAddress(formData: any): void {
    const addressId = formData.addressId;
    const addressType = this.typeToNumber[formData.addressType];
    let reqData = {
      addressId: formData.addressId,
      typeId: addressType,
      address: formData.address,
      city: formData.city,
      state: formData.state,
    };
    this.addressService.updateAddress(reqData).subscribe(
      (response) => {
        this.getAddresses();
      },
      (error) => {
        console.error('Error updating address', error);
      }
    );
    this.addAddress = !this.addAddress;
    this.btn = !this.btn;
    this.newAddress.addressType = '';
    this.newAddress.address = '';
    this.newAddress.city = '';
    this.newAddress.state = '';
    this.newAddress.addressId = '';
  }

  onContinue() {
    if (this.selectedAddress) {
      this.isAddressSelected = true;
      this.onOpenUpdateAddress(this.selectedAddress);
    } else {
      this.snackBar.open('No address selected', 'Close', { duration: 2000 });
    }
  }

  placeOrder(): void {
    if (this.selectedAddress) {
      this.orderService.createOrder(this.selectedAddress.addressId).subscribe(
        (response: any) => {
          this.router.navigate(["/success"])
        },
        (error: any) => {
          console.error('Unable to create order', error);
        }
      );
    } else {
      this.snackBar.open('No address selected', 'Close', { duration: 2000 });
    }
  }
}
