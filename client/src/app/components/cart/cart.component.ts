import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginSignupComponent } from '../login-signup/login-signup.component';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';
import { CartService } from 'src/app/Services/cartService/cart.service';
import { AddressService } from 'src/app/Services/addressService/address.service';
import { state } from '@angular/animations';

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
  // quantity:any=0
  // Example data for addresses
  addresses: any = [];
  selectedAddress: any;
  isPlacedOrder:boolean=false;
  isAddressSelected:boolean=false;
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
    private addressService: AddressService
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

  openDialog() {
    const dialogRef = this.dialog.open(LoginSignupComponent, {});
    dialogRef.afterClosed();
  }
  getCartItems() {
    this.cartService.getCarts().subscribe(
      (response) => {
        // console.log(response);
        this.cartItems = response.data;
        // this.dataService.setWishListData(this.wishListItems)
        this.dataService.setCartData(response.data);
      },
      (error) => {
        console.log('Error fetching wishlist items');
      }
    );
  }

  deleteCartItem(bookId: any) {
    this.cartService.deleteCart(bookId).subscribe((response) => {
      this.cartItems = this.cartItems.filter(
        (item: any) => item.bookId !== bookId
      );
      this.dataService.removeFromCart(bookId);
    });
  }

  // Increment item quantity
  increaseQuantity(item: any) {
    if (item.cartQuantity < item.stockQuantity && item.cartQuantity < 10) {
      item.cartQuantity++;
      this.updateCartItem(item.bookId, +1);
    }
  }

  // Decrement item quantity
  decreaseQuantity(item: any) {
    if (item.cartQuantity > 1) {
      item.cartQuantity--;
      this.updateCartItem(item.bookId, -1);
    }
  }

  // Update the cart item in the backend
  updateCartItem(id: any, num: number) {
    let reqData = {
      bookId: id,
      newQuantity: num,
    };
    this.cartService.updateCart(reqData).subscribe(
      (response) => {
        // console.log('Cart updated successfully');
        console.log(response.message);
      },
      (error) => {
        console.log('Error updating cart');
      }
    );
  }

  onPlaceOrder(){
    this.isPlacedOrder =!this.isPlacedOrder
  }

  //Address

  // Fetch all addresses from the backend
  getAddresses() {
    this.addressService.getAddresses().subscribe(
      (response: any) => {
        this.addresses = response.data;
      },
      (error) => {
        console.error('Error fetching addresses', error);
      }
    );
  }

  // Open/Close the address form
  onOpenAddAddress() {
    this.addAddress = !this.addAddress;
  }

  // Add a new address
  addNewAddress(formData: any) {
    let reqData = {
      typeId: formData.addressType,
      address: formData.address,
      city: formData.city,
      state: formData.state,
    };
    // console.log(reqData)
    this.addressService.createAddress(reqData).subscribe(
      (response) => {
        // Address added successfully, refetch addresses
        this.getAddresses();
        this.addAddress = false;
        console.log(response);
      },
      (error) => {
        console.error('Error adding address', error);
      }
    );
  }

  onOpenUpdateAddress(address: any) {
    this.addAddress = !this.addAddress;
    this.btn = !this.btn;
    this.newAddress.addressType = address.typeOfAddress;
    this.newAddress.address = address.address;
    this.newAddress.city = address.city;
    this.newAddress.state = address.state;
    this.newAddress.addressId = address.addressId;

    // console.log('Address: ' + address.addressId);
    // console.log('new Address : ', this.newAddress);
  }
  // Update an existing address
  updateAddress(formData: any) {
    const addressId = formData.addressId;
    const addressType = this.typeToNumber[formData.addressType]
    let reqData = {
      addressId: formData.addressId,
      typeId: addressType,
      address: formData.address,
      city: formData.city,
      state: formData.state,
    };
    console.log(reqData);
    this.addressService.updateAddress(reqData).subscribe(
      (response) => {
        // Address updated successfully
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
    // console.log(this.newAddress);
  }

  onContinue() {
    if (this.selectedAddress) {
      this.isAddressSelected=true;
      this.onOpenUpdateAddress(this.selectedAddress)
      console.log('Selected Address:', this.selectedAddress);
    } else {
      console.log('No address selected');
    }
  }

  // Remove an address
  // removeAddress(addressId: string) {
  //   this.addressService.deleteAddress(addressId).subscribe(
  //     (response) => {
  //       // Address deleted successfully, refetch addresses
  //       this.getAddresses();
  //     },
  //     (error) => {
  //       console.error('Error removing address', error);
  //     }
  //   );
  // }
}
